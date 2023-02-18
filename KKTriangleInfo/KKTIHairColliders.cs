﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KKTriangleInfo
{
	//Holds all of the colliders used by the character's hair. Basically just an array that hides its colliders based on visibility.
	class KKTIHairColliders : MonoBehaviour
	{
		List<KKTICollider> hairColls;
		List<SkinnedMeshRenderer> rends;
		List<MeshFilter> filts;

		public static KKTIHairColliders MakeKKTIHairColliders(ChaControl inCha)
		{
			GameObject newObj = new GameObject();
			KKTIHairColliders output = newObj.AddComponent<KKTIHairColliders>();
			output.hairColls = new List<KKTICollider>();
			output.rends = new List<SkinnedMeshRenderer>();
			output.filts = new List<MeshFilter>();
			GameObject[] hairIter = inCha.objHair;
			foreach (GameObject go in hairIter)
			{
				output.rends.AddRange(go.GetComponentsInChildren<SkinnedMeshRenderer>(true));
				output.filts.AddRange(go.GetComponentsInChildren<MeshFilter>(true));
			}

			foreach (SkinnedMeshRenderer smr in output.rends)
				output.hairColls.Add(KKTICollider.MakeKKTIColliderObj(smr, "KKTI_Hair_Part"));

			foreach (MeshFilter filt in output.filts)
				output.hairColls.Add(KKTICollider.MakeKKTIColliderObj(filt, "KKTI_Hair_Part"));

			return output;
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

