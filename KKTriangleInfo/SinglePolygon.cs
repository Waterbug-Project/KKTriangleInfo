using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KKTriangleInfo
{
	class SinglePolygon : MonoBehaviour
	{
		Transform baseTransf;
		Mesh tempMesh;
		MeshFilter meshFilter;
		MeshRenderer meshRend;
		List<GameObject> debugObjs;
		KKTICollider hitColl;
		Vector3[] verts;
		int[] vertInds;

		public static SinglePolygon MakeSinglePolygonObj(GameObject inObj, int inTriInd)
		{
			GameObject newObj = new GameObject();
			SinglePolygon output = newObj.AddComponent<SinglePolygon>();

			output.meshFilter = newObj.AddComponent<MeshFilter>();
			output.meshRend = newObj.AddComponent<MeshRenderer>();
			//Clumsy code to obtain material
			GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			output.meshRend.material = temp.GetComponent<MeshRenderer>().material;
			Destroy(temp);
			output.debugObjs = new List<GameObject>();
			output.baseTransf = inObj.transform;

			output.hitColl = inObj.GetComponent<KKTICollider>();
			int[] tempTris = output.hitColl.accessMesh.triangles;
			output.vertInds = new int[3];
			output.verts = new Vector3[3];
			for (int i = 0; i < 3; ++i)
			{
				output.vertInds[i] = tempTris[inTriInd * 3 + i];
				output.verts[i] = inObj.transform.TransformPoint(output.hitColl.accessVerts[output.vertInds[i]]);
			}

			output.tempMesh = new Mesh();
			output.meshFilter.mesh = output.tempMesh;
			output.tempMesh.vertices = output.verts;
			output.tempMesh.uv = new Vector2[] { Vector2.zero, new Vector2(1f, 0f), Vector2.one };      //We don't use this. May or may not display the upper-right triangle of a square image.
			output.tempMesh.triangles = new int[] { 0, 1, 2 };
			for (int i = 0; i < 3; ++i)
			{
				output.debugObjs.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere));
				output.debugObjs[i].transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
				output.debugObjs[i].transform.position = output.tempMesh.vertices[i];
			}

			return output;
		}

		void Update()
		{
			if (hitColl != null)
			{
				for (int i = 0; i < 3; ++i)
					verts[i] = baseTransf.TransformPoint(hitColl.accessVerts[vertInds[i]]);

				//tempMesh = meshFilter.mesh;
				meshFilter.mesh = tempMesh;
				tempMesh.vertices = verts;
				for (int i = 0; i < 3; ++i)
					debugObjs[i].transform.position = tempMesh.vertices[i];
			}
		}

		void OnDestroy()
		{
			foreach (GameObject go in debugObjs)
				Destroy(go);
			Destroy(gameObject);
		}
	}
}
