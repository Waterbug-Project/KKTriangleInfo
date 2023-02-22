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
		bool isVisible;

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
			output.colls = new KKTICollider[rends.Length + filts.Length];
			int accCollIter = 0;
			for (int i = 0; i < rends.Length; ++i, ++accCollIter)
				output.colls[accCollIter] = KKTICollider.MakeKKTIColliderObj(rends[i], "KKTI_Acc_Coll_SMR_" + inID + "_" + accCollIter);
			for (int i = 0; i < filts.Length; ++i, ++accCollIter)
				output.colls[accCollIter] = KKTICollider.MakeKKTIColliderObj(filts[i], "KKTI_Acc_Coll_MF_" + inID + "_" + accCollIter);

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

