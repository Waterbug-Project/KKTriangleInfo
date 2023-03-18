using UnityEngine;

namespace KKTriangleInfo
{
	//Hold all the colliders used by one accessory. One of these is needed per accessory in an outfit.
	class KKTIAccCollider : MonoBehaviour
	{
		GameObject acc;
		KKTICollider[] colls;

		public static KKTIAccCollider Make(GameObject inAcc, string inID = "X", int inLayer = KKTICharaController.KKTICOLLLAYER)
		{
			GameObject newObj = new GameObject();
			newObj.layer = inLayer;
			KKTIAccCollider output = newObj.AddComponent<KKTIAccCollider>();
			output.acc = inAcc;
			output.name = "KKTI_Acc_Coll_" + inID;

			SkinnedMeshRenderer[] rends = output.acc.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			MeshFilter[] filts = output.acc.GetComponentsInChildren<MeshFilter>(true);

			output.colls = new KKTICollider[rends.Length + filts.Length];
			int accCollIter = 0;
			while (accCollIter < rends.Length)
			{
				output.colls[accCollIter] = KKTICollider.Make(rends[accCollIter], "KKTI_Acc_Coll_SMR_" + inID + "_" + accCollIter);
				accCollIter++;
			}
			for (int i = 0; i < filts.Length; ++i)
			{
				output.colls[accCollIter] = KKTICollider.Make(filts[i], "KKTI_Acc_Coll_MF_" + inID + "_" + accCollIter);
				accCollIter++;
			}

			return output;
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

