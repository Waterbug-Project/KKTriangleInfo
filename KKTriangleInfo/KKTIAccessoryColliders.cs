using System.Collections.Generic;
using UnityEngine;

namespace KKTriangleInfo
{
	//Holds all of the individual collections of colliders used by all the accessories of a particular outfit.
	class KKTIAccessoryColliders : MonoBehaviour
	{
		ChaControl cha;
		List<KKTIAccCollider> accColls;

		public static KKTIAccessoryColliders MakeKKTIAccessoryColliders(ChaControl inCha)
		{
			GameObject newObj = new GameObject();

			KKTIAccessoryColliders output = newObj.AddComponent<KKTIAccessoryColliders>();
			output.cha = inCha;
			output.accColls = new List<KKTIAccCollider>();
			output.LoadNewAccessories();

			return output;
		}

		protected void LoadNewAccessories()
		{
			if (accColls != null)
			{
				foreach (KKTIAccCollider coll in accColls)
					Destroy(coll.gameObject);
				accColls.Clear();
			}

			int i = -1;
			GameObject[] accArray = cha.objAccessory;
			while (++i < accArray.Length)
				if (accArray[i] != null)
				{
					KKTIAccCollider newColl = KKTIAccCollider.MakeKKTIAccCollider(accArray[i], LoadNewAccessories, i.ToString());
					accColls.Add(newColl);
				}
		}

		public void SetVisible(bool inVisible)
		{
			foreach (KKTIAccCollider coll in accColls)
				coll.gameObject.GetComponent<KKTIAccCollider>().SetVisible(inVisible);
		}

		public void ToggleVisible()
		{
			foreach (KKTIAccCollider coll in accColls)
				coll.gameObject.GetComponent<KKTIAccCollider>().ToggleVisible();
		}

		public void UpdateCollider()
		{
			foreach (KKTIAccCollider coll in accColls)
				coll.UpdateCollider();
		}

		public void OnDestroy()
		{
			if (accColls != null)
				foreach (KKTIAccCollider coll in accColls)
					Destroy(coll.gameObject);
		}
	}
}

