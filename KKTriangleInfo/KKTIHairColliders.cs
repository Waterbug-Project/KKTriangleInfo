using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KKTriangleInfo
{
	//Holds all of the colliders used by the character's hair. Basically just an array that hides its colliders based on visibility.
	class KKTIHairColliders : MonoBehaviour
	{
		List<KKTICollider> hairColls;
		List<SkinnedMeshRenderer> rends;

		public static KKTIHairColliders MakeKKTIHairColliders(ChaControl inCha)
		{
			GameObject newObj = new GameObject();
			KKTIHairColliders output = newObj.AddComponent<KKTIHairColliders>();
			output.hairColls = new List<KKTICollider>();
			output.rends = new List<SkinnedMeshRenderer>();
			GameObject[] hairIter = inCha.objHair;
			foreach (GameObject go in hairIter)
				foreach (SkinnedMeshRenderer smr in go.GetComponentsInChildren<SkinnedMeshRenderer>(true))
				{
					output.rends.Add(smr);
					KKTICollider newColl = KKTICollider.MakeKKTIColliderObj(smr, "KKTI_Hair_Part");
					output.hairColls.Add(newColl);
				}

			return output;
		}

		public void Update()
		{
			SetCollidersByVisibility();
		}

		private void SetCollidersByVisibility()
		{
			for (int i = 0; i < hairColls.Count; ++i)
				hairColls[i].gameObject.SetActive(rends[i].isVisible);
		}
		
		public void SetVisible(bool inVisible)
		{
			foreach (KKTICollider coll in hairColls)
				coll.SetVisible(inVisible);
		}
		
		public void ToggleVisible()
		{
			foreach (KKTICollider coll in hairColls)
				coll.ToggleVisible();
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

