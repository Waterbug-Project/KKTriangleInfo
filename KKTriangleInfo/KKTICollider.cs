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

		//Surely there must be a better way of getting all this information in.
		public static KKTICollider MakeKKTIColliderObj(SkinnedMeshRenderer inSource, string inName = "KKTICollider",
			ChaFileDefine.ClothesKind inKind = ChaFileDefine.ClothesKind.top)
		{
			Mesh mesh = new Mesh();
			inSource.BakeMesh(mesh);
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			mesh.name = inSource.name;

			return MakeKKTICollInternal(inSource.transform, mesh, inName, inSource, inKind);
		}

		public static KKTICollider MakeKKTIColliderObj(MeshFilter inSource, string inName = "KKTICollider",
			ChaFileDefine.ClothesKind inKind = ChaFileDefine.ClothesKind.top)
		{
			return MakeKKTICollInternal(inSource.transform, inSource.sharedMesh, inName, null, inKind);
		}

		private static KKTICollider MakeKKTICollInternal(Transform inTransf, Mesh inMesh, string inName,
			SkinnedMeshRenderer inSource = null, ChaFileDefine.ClothesKind inKind = ChaFileDefine.ClothesKind.top)
		{
			GameObject newObj = new GameObject();
			KKTICollider output = newObj.AddComponent<KKTICollider>();

			newObj.AddComponent<Rigidbody>().isKinematic = true;

			//DEBUG - add togglable visibility
			output.debugFilt = newObj.AddComponent<MeshFilter>();
			output.debugFilt.mesh = new Mesh();
			output.debugFilt.mesh.vertices = inMesh.vertices;
			output.debugFilt.mesh.uv = inMesh.uv;
			output.debugFilt.mesh.triangles = inMesh.triangles;
			output.debugRend = newObj.AddComponent<MeshRenderer>();
			output.debugRend.material = new Material(Shader.Find("Hidden/Internal-Colored"));
			output.debugRend.enabled = false;

			output.baseTransf = inTransf;
			output.meshSource = inSource;
			output.coll = newObj.AddComponent<MeshCollider>();

			//Initialize public mesh
			output.accessMesh = new Mesh();
			output.accessMesh.MarkDynamic();
			output.accessMesh.name = inMesh.name;
			output.accessVerts = new List<Vector3>();
			if (output.meshSource != null)
				output.UpdatePublics();
			else
			{
				output.accessMesh = inMesh;
				output.accessMesh.RecalculateBounds();
				output.accessMesh.RecalculateNormals();
				output.accessMesh.GetVertices(output.accessVerts);
				output.coll.sharedMesh = inMesh;
				output.coll.sharedMesh.RecalculateBounds();
				output.coll.sharedMesh.RecalculateNormals();
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
	}
}

