using System.Collections.Generic;
using UnityEngine;

namespace KKTriangleInfo
{
	//A collider for raycasts to intersect. The collision mesh updates in real time, if based on a SkinnedMeshRenderer.
	[DefaultExecutionOrder(10000000)]		//Updates colliders after all bones have moved into place
	class KKTICollider : MonoBehaviour
	{
		private SkinnedMeshRenderer meshSource;
		private MeshCollider coll;

		public Mesh accessMesh;
		public List<Vector3> accessVerts;
		
		public static KKTICollider Make(SkinnedMeshRenderer inSource, string inName = "KKTICollider")
		{
			Mesh mesh = new Mesh();
			inSource.BakeMesh(mesh);
			mesh.RecalculateBounds();
			mesh.name = inSource.name;

			return MakeInternal(inSource.transform, mesh, inName, inSource);
		}

		public static KKTICollider Make(MeshFilter inSource, string inName = "KKTICollider")
		{
			return MakeInternal(inSource.transform, inSource.sharedMesh, inName);
		}

		private static KKTICollider MakeInternal(Transform inTransf, Mesh inMesh, string inName,
			SkinnedMeshRenderer inSource = null)
		{
			GameObject newObj = new GameObject();
			newObj.AddComponent<Rigidbody>().isKinematic = true;
			
			KKTICollider output = newObj.AddComponent<KKTICollider>();
			output.meshSource = inSource;
			output.coll = newObj.AddComponent<MeshCollider>();

			//Initialize public mesh
			output.accessMesh = new Mesh();
			output.accessMesh.MarkDynamic();
			output.accessMesh.name = inMesh.name;
			output.accessVerts = new List<Vector3>();
			if (output.meshSource != null)
			{
				output.UpdatePublics();
				output.coll.sharedMesh = output.accessMesh;
			}
			else
			{
				output.accessMesh = inMesh;
				output.accessMesh.GetVertices(output.accessVerts);
				output.coll.sharedMesh = inMesh;
				output.coll.sharedMesh.RecalculateBounds();
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
			if (meshSource != null)
				UpdatePublics();
		}

		public void UpdateCollider()
		{
			if (isActiveAndEnabled)
			{
				//accessMesh.MarkDynamic();
				//coll.sharedMesh = null;
				//coll.sharedMesh = new Mesh();
				//coll.sharedMesh.MarkDynamic();
				//accessMesh.RecalculateBounds();
				coll.sharedMesh = accessMesh;
				//coll.sharedMesh.RecalculateBounds();
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
	}
}

