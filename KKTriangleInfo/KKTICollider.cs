using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KKTriangleInfo
{
	//A collider for raycasts to intersect. The collision mesh updates in real time, if based on a SkinnedMeshRenderer.
	class KKTICollider : MonoBehaviour
	{
		private Transform baseTransf;
		private SkinnedMeshRenderer meshSource;
		private MeshCollider coll;

		public Mesh accessMesh;

		private MeshRenderer debugRend;
		private MeshFilter debugFilt;
		private Mesh debugCheckMesh = null;

		private ChaControl cha;

		//Surely there must be a better way of getting all this information in.
		public static KKTICollider MakeKKTIColliderObj(SkinnedMeshRenderer inSource, string inName = "KKTICollider",
			ChaControl inCha = null, ChaFileDefine.ClothesKind inKind = ChaFileDefine.ClothesKind.top)
		{
			Mesh mesh = new Mesh();
			inSource.BakeMesh(mesh);
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();

			return MakeKKTICollInternal(inSource.transform, mesh, inName, inSource, inCha, inKind);
		}

		public static KKTICollider MakeKKTIColliderObj(MeshFilter inSource, string inName = "KKTICollider",
			ChaControl inCha = null, ChaFileDefine.ClothesKind inKind = ChaFileDefine.ClothesKind.top)
		{
			Mesh mesh = new Mesh();
			AssignMesh(inSource.sharedMesh, mesh);

			return MakeKKTICollInternal(inSource.transform, mesh, inName, null, inCha, inKind);
		}

		private static KKTICollider MakeKKTICollInternal(Transform inTransf, Mesh inMesh, string inName,
			SkinnedMeshRenderer inSource = null, ChaControl inCha = null, ChaFileDefine.ClothesKind inKind = ChaFileDefine.ClothesKind.top)
		{
			GameObject newObj = new GameObject();

			newObj.AddComponent<Rigidbody>().isKinematic = true;

			//DEBUG - add togglable visibility
			GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			newObj.AddComponent<MeshFilter>();
			MeshFilter outFilt = newObj.GetComponent<MeshFilter>();
			outFilt.mesh = new Mesh();
			AssignMesh(inMesh, outFilt.mesh);
			MeshRenderer outRend = newObj.AddComponent<MeshRenderer>();
			outRend.material = temp.GetComponent<MeshRenderer>().material;
			outRend.enabled = false;
			Destroy(temp);

			MeshCollider outColl = newObj.AddComponent<MeshCollider>();

			KKTICollider output = newObj.AddComponent<KKTICollider>();
			output.baseTransf = inTransf;
			output.meshSource = inSource;
			output.coll = outColl;

			//Initialize public mesh
			output.accessMesh = new Mesh();
			output.accessMesh.MarkDynamic();
			if (output.meshSource != null)
				output.UpdatePublics();
			else
			{
				output.accessMesh = inMesh;
				output.accessMesh.RecalculateBounds();
				output.accessMesh.RecalculateNormals();
				outColl.sharedMesh = inMesh;
				outColl.sharedMesh.RecalculateBounds();
				outColl.sharedMesh.RecalculateNormals();
			}
			output.debugFilt = outFilt;
			output.debugRend = outRend;
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
		}

		public void SetVisible(bool inVisible)
		{
			if (inVisible)
			{
				debugFilt.mesh.vertices = coll.sharedMesh.vertices;
				debugFilt.mesh.triangles = coll.sharedMesh.triangles;
			}

			GetComponent<MeshRenderer>().enabled = inVisible;
		}

		public void ToggleVisible()
		{
			SetVisible(!GetComponent<MeshRenderer>().enabled);
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

