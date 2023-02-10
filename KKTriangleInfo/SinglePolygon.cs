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
		KKTICollider hitColl;
		Vector3[] verts;
		int[] vertInds;

		public static SinglePolygon MakeSinglePolygonObj(GameObject inObj, int inTriInd)
		{
			GameObject newObj = new GameObject();
			SinglePolygon output = newObj.AddComponent<SinglePolygon>();

			output.meshFilter = newObj.AddComponent<MeshFilter>();
			output.meshRend = newObj.AddComponent<MeshRenderer>();
			output.meshRend.material = new Material(Shader.Find("Shader Forge/main_item"));
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
			}
		}

		void OnDestroy()
		{
			Destroy(gameObject);
		}
	}
}
