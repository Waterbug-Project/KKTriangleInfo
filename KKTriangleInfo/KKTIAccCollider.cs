using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KKTriangleInfo
{
	//Hold all the colliders used by one accessory. One of these is needed per accessory in an outfit.
	class KKTIAccCollider : MonoBehaviour
	{
		GameObject acc;
		KKTICollider[] colls;
		string accID;

		public static KKTIAccCollider MakeKKTIAccCollider(GameObject inAcc, string inID = "X", int inLayer = KKTICharaController.KKTICOLLLAYER)
		{
			GameObject newObj = new GameObject();
			newObj.layer = inLayer;
			KKTIAccCollider output = newObj.AddComponent<KKTIAccCollider>();
			output.acc = inAcc;
			output.accID = inID;
			output.name = "KKTI_Acc_Coll_" + inID;

			SkinnedMeshRenderer[] rends = output.acc.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			MeshFilter[] filts = output.acc.GetComponentsInChildren<MeshFilter>(true);

			//DEBUG - rework to remove checks for KKTIAccCollider on parent GameObject in final build
			List<int> rendsInds = new List<int>();
			List<int> filtsInds = new List<int>();
			for (int i = 0; i < rends.Length || i < filts.Length; ++i)
			{
				if (i < rends.Length && rends[i].gameObject.GetComponent<KKTICollider>() == null)
					rendsInds.Add(i);
				if (i < filts.Length && filts[i].gameObject.GetComponent<KKTICollider>() == null)
					filtsInds.Add(i);
			}
			output.colls = new KKTICollider[rendsInds.Count + filtsInds.Count];
			int accCollIter = 0;
			foreach (int ind in rendsInds)
			{
				output.colls[accCollIter] = KKTICollider.MakeKKTIColliderObj(rends[ind], "KKTI_Acc_Coll_SMR_" + inID + "_" + accCollIter);
				accCollIter++;
			}
			foreach (int ind in filtsInds)
			{
				output.colls[accCollIter] = KKTICollider.MakeKKTIColliderObj(filts[ind], "KKTI_Acc_Coll_MF_" + inID + "_" + accCollIter);
				accCollIter++;
			}

			return output;
		}

		public void SetVisible(bool inVisible)
		{
			foreach (KKTICollider coll in colls)
				coll.gameObject.GetComponent<KKTICollider>().SetVisible(inVisible);
		}

		public void ToggleVisible()
		{
			foreach (KKTICollider coll in colls)
				coll.gameObject.GetComponent<KKTICollider>().ToggleVisible();
		}

		public void UpdateCollider()
		{
			foreach (KKTICollider coll in colls)
				coll.UpdateCollider();
		}

		public void OnDestroy()
		{
			if (colls != null)
				foreach (KKTICollider coll in colls)
					Destroy(coll.gameObject);
		}
	}
}

