using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KKTriangleInfo
{
	//Hold all the colliders used by one accessory. One of these is needed per accessory in an outfit.
	class KKTIAccCollider : MonoBehaviour
	{
		public delegate void Del();

		GameObject acc;
		Del updateFunc;
		KKTICollider[] colls;
		string accID;
		bool isVisible;

		public static KKTIAccCollider MakeKKTIAccCollider(GameObject inAcc, Del inUpdateFunc, string inID = "X", int inLayer = KKTICharaController.KKTICOLLLAYER)
		{
			GameObject newObj = new GameObject();
			newObj.layer = inLayer;
			KKTIAccCollider output = newObj.AddComponent<KKTIAccCollider>();
			output.acc = inAcc;
			output.accID = inID;
			output.updateFunc = inUpdateFunc;
			output.ReloadAccessories();
			//Force colliders to become active/inactive
			output.isVisible = !inAcc.activeSelf;
			output.SetCollidersByVisibility();

			return output;
		}

		//Should we be calling Destroy for any of this?
		private void ReloadAccessories()
		{
			SkinnedMeshRenderer[] rends = acc.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			MeshFilter[] filts = acc.GetComponentsInChildren<MeshFilter>(true);

			colls = new KKTICollider[rends.Length + filts.Length];
			int accCollIter = 0;
			for (int i = 0; i < rends.Length; ++i, ++accCollIter)
				colls[accCollIter] = KKTICollider.MakeKKTIColliderObj(rends[i], "KKTI_Acc_Coll_SMR_" + accID + "_" + accCollIter);
			for (int i = 0; i < filts.Length; ++i, ++accCollIter)
				colls[accCollIter] = KKTICollider.MakeKKTIColliderObj(filts[i], "KKTI_Acc_Coll_MF_" + accID + "_" + accCollIter);
		}

		public void Update()
		{
			CheckAccessoryChange();
			SetCollidersByVisibility();
		}

		private void CheckAccessoryChange()
		{
			if (acc == null)
				updateFunc();
		}

		private void SetCollidersByVisibility()
		{
			if (isVisible != acc.activeSelf)
			{
				foreach (KKTICollider coll in colls)
					coll.gameObject.SetActive(acc.activeSelf);
				isVisible = acc.activeSelf;
			}
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

