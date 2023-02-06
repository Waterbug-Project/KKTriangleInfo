using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KKTriangleInfo
{
	//A collider for raycasts to intersect. The collision mesh updates in real time, if based on a SkinnedMeshRenderer.
	[DefaultExecutionOrder(10000000)]
	class KKTICollider : MonoBehaviour
	{
		private Transform baseTransf;
		private SkinnedMeshRenderer meshSource;
		private MeshCollider coll;

		public Mesh accessMesh;
		public List<Vector3> accessVerts;

		private MeshRenderer debugRend;
		private MeshFilter debugFilt;
		private Mesh debugCheckMesh = null;

		//Surely there must be a better way of getting all this information in.
		public static KKTICollider MakeKKTIColliderObj(SkinnedMeshRenderer inSource, string inName = "KKTICollider",
			ChaFileDefine.ClothesKind inKind = ChaFileDefine.ClothesKind.top)
		{
			Mesh mesh = new Mesh();
			inSource.BakeMesh(mesh);
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();

			return MakeKKTICollInternal(inSource.transform, mesh, inName, inSource, inKind);
		}

		public static KKTICollider MakeKKTIColliderObj(MeshFilter inSource, string inName = "KKTICollider",
			ChaFileDefine.ClothesKind inKind = ChaFileDefine.ClothesKind.top)
		{
			Mesh mesh = new Mesh();
			AssignMesh(inSource.sharedMesh, mesh);

			return MakeKKTICollInternal(inSource.transform, mesh, inName, null, inKind);
		}

		private static KKTICollider MakeKKTICollInternal(Transform inTransf, Mesh inMesh, string inName,
			SkinnedMeshRenderer inSource = null, ChaFileDefine.ClothesKind inKind = ChaFileDefine.ClothesKind.top)
		{
			GameObject newObj = new GameObject();
			KKTICollider output = newObj.AddComponent<KKTICollider>();

			newObj.AddComponent<Rigidbody>().isKinematic = true;

			//DEBUG - add togglable visibility
			GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			output.debugFilt = newObj.AddComponent<MeshFilter>();
			output.debugFilt.mesh = new Mesh();
			AssignMesh(inMesh, output.debugFilt.mesh);
			output.debugRend = newObj.AddComponent<MeshRenderer>();
			output.debugRend.material = temp.GetComponent<MeshRenderer>().material;
			output.debugRend.enabled = false;
			Destroy(temp);

			MeshCollider outColl = newObj.AddComponent<MeshCollider>();

			output.baseTransf = inTransf;
			output.meshSource = inSource;
			output.coll = outColl;

			//Initialize public mesh
			output.accessMesh = new Mesh();
			output.accessMesh.MarkDynamic();
			output.accessVerts = new List<Vector3>();
			if (output.meshSource != null)
				output.UpdatePublics();
			else
			{
				output.accessMesh = inMesh;
				output.accessMesh.RecalculateBounds();
				output.accessMesh.RecalculateNormals();
				output.accessMesh.GetVertices(output.accessVerts);
				outColl.sharedMesh = inMesh;
				outColl.sharedMesh.RecalculateBounds();
				outColl.sharedMesh.RecalculateNormals();
			}
			if (output.meshSource != null)
			{
				output.debugCheckMesh = new Mesh();
				output.meshSource.BakeMesh(output.debugCheckMesh);
				output.debugCheckMesh.RecalculateBounds();
			}

			newObj.transform.position = inTransf.position;
			newObj.transform.rotation = inTransf.rotation;
			newObj.transform.parent = inTransf;
			if (inSource == null)
				newObj.transform.localScale = Vector3.one;
			newObj.name = inName;
			newObj.layer = KKTICharaController.KKTICOLLLAYER;
			return output;
		}

		void LateUpdate()
		{
			//Originally caught edge cases - now allows colliders based on MeshFilters to avoid redundant calculations.
			if (meshSource != null)
				UpdatePublics();
		}

		private void ClearCollider()
		{
			coll.sharedMesh = null;
		}

		public void UpdateCollider()
		{
			if (isActiveAndEnabled)
			{
				accessMesh.MarkDynamic();
				coll.sharedMesh = null;
				coll.sharedMesh = new Mesh();
				coll.sharedMesh.MarkDynamic();
				accessMesh.RecalculateBounds();
				coll.sharedMesh = accessMesh;
				coll.sharedMesh.RecalculateBounds();
				coll.sharedMesh.RecalculateNormals();
			}
		}

		//Only call for colliders based on SkinnedMeshRenderers
		public void UpdatePublics()
		{
			meshSource.BakeMesh(accessMesh);
			accessMesh.RecalculateBounds();
			//Retreive vertices of the mesh once per frame
			//All of the particles stuck to/colliding with this mesh will reference the retrieved list, instead of retrieving it once per frame per particle
			accessMesh.GetVertices(accessVerts);
		}

		public void SetVisible(bool inVisible)
		{
			if (inVisible)
			{
				debugFilt.mesh.vertices = coll.sharedMesh.vertices;
				debugFilt.mesh.triangles = coll.sharedMesh.triangles;
			}

			debugRend.enabled = inVisible;
		}

		public void ToggleVisible()
		{
			SetVisible(!debugRend.enabled);
		}

		public void ManualRefresh()
		{
			Mesh tempMesh = new Mesh();
			meshSource.BakeMesh(tempMesh);
			coll.sharedMesh = null;
			coll.sharedMesh = tempMesh;
			coll.sharedMesh.vertices = tempMesh.vertices;
			coll.sharedMesh.uv = tempMesh.uv;
			coll.sharedMesh.triangles = tempMesh.triangles;
			coll.sharedMesh.RecalculateBounds();
			coll.sharedMesh.RecalculateNormals();
			coll.sharedMesh.name = "KKTI_Body_Mesh_" + coll.sharedMesh.vertices[0].x;
		}

		private static void AssignMesh(Mesh source, Mesh dest)
		{
			dest.Clear();
			source.MarkDynamic();
			source.RecalculateBounds();
			source.RecalculateNormals();
			dest.vertices = source.vertices;
			dest.uv = source.uv;
			dest.triangles = source.triangles;
			dest = source;
		}
	}
}

