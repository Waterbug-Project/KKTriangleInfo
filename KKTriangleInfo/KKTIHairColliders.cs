using System.Collections.Generic;
using UnityEngine;

namespace KKTriangleInfo
{
	//Holds all of the colliders used by the character's hair. Basically just an array that hides its colliders based on visibility.
	class KKTIHairColliders : MonoBehaviour
	{
		List<KKTICollider> hairColls;

		public static KKTIHairColliders Make(ChaControl inCha)
		{
			GameObject newObj = new GameObject();
			KKTIHairColliders output = newObj.AddComponent<KKTIHairColliders>();
			output.hairColls = new List<KKTICollider>();
			output.name = "KKTI_Hair_Colliders";
			GameObject[] hairIter = inCha.objHair;

			List<SkinnedMeshRenderer> rends = new List<SkinnedMeshRenderer>();
			List<MeshFilter> filts = new List<MeshFilter>();
			foreach (GameObject go in hairIter)
			{
				rends.AddRange(go.GetComponentsInChildren<SkinnedMeshRenderer>(true));
				filts.AddRange(go.GetComponentsInChildren<MeshFilter>(true));
			}
			foreach (SkinnedMeshRenderer smr in rends)
				output.hairColls.Add(KKTICollider.Make(smr, "KKTI_Hair_Part"));
			foreach (MeshFilter filt in filts)
				output.hairColls.Add(KKTICollider.Make(filt, "KKTI_Hair_Part"));

			return output;
		}
		
		public void UpdateCollider()
		{
			foreach (KKTICollider coll in hairColls)
				coll.UpdateCollider();
		}

		public void OnDestroy()
		{
			if (hairColls != null)
				foreach (KKTICollider coll in hairColls)
					Destroy(coll.gameObject);
		}
	}
}

