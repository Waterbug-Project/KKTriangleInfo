using System;
using UnityEngine;

namespace KKTriangleInfo
{
	//All of the colliders needed for one piece of clothing (e.g. top, bottom, socks...)
	//This includes the colliders for various states of undress - colliders are enabled and disabled as needed
	class KKTIClothingColliders : MonoBehaviour
	{
		ChaControl cha;
		ChaFileDefine.ClothesKind kind;
		KKTICollider[] wbColls;
		SkinnedMeshRenderer[] rends;
		GameObject baseObj;

		public static KKTIClothingColliders MakeKKTIClothingColliders(ChaControl inCha, ChaFileDefine.ClothesKind inKind, int inLayer = KKTICharaController.KKTICOLLLAYER)
		{
			GameObject newObj = new GameObject();
			newObj.layer = inLayer;
			KKTIClothingColliders output = newObj.AddComponent<KKTIClothingColliders>();
			output.cha = inCha;
			if (!output.cha.IsClothesStateKind((int)inKind))
				UnityEngine.Debug.LogError("[Tess]\tClothesKind " + inKind + " is not valid for the given character!");
			output.kind = inKind;
			output.name = "KKTI_Clothing_Colliders_" + inKind.ToString();
			output.LoadClothingMeshes();
			Hooks.ChangeClothesEvent += output.ChangeClothesHandler;

			return output;
		}

		public void ChangeClothesHandler(object sender, EventArgs e)
		{
			//Some clothing, like the school swimsuit, will change old clothing pieces to Delete_Reserve objects.
			if (baseObj == null || baseObj.name == "Delete_Reserve")
				ReloadClothes();
		}

		//Called when the current article of clothing is changed for a different one
		private void ReloadClothes()
		{
			if (wbColls != null)
				for (int i = 0; i < wbColls.Length; ++i)
					if (wbColls[i] != null)
						Destroy(wbColls[i].gameObject);
			LoadClothingMeshes();
		}

		private void LoadClothingMeshes()
		{
			int kindAsInt = (int)kind;
			baseObj = cha.objClothes[kindAsInt];
			//Only try to load the colliders for this piece of clothing if the new outfit actually includes clothing within this clothing slot
			if (baseObj != null)
			{
				//We're assuming that no clothes use MeshRenderers here.
				rends = baseObj.GetComponentsInChildren<SkinnedMeshRenderer>(true);
				wbColls = new KKTICollider[rends.Length];
				for (int i = 0; i < wbColls.Length; ++i)
					wbColls[i] = KKTICollider.MakeKKTIColliderObj(rends[i], "KKTI_Clothes_Coll_" + rends[i].name, kind);
			}
			else
				wbColls = null;
		}

		public void SetVisible(bool inVisible)
		{
			if (wbColls != null)
				for (int i = 0; i < wbColls.Length; ++i)
					if (rends[i].isVisible)
						wbColls[i].SetVisible(inVisible);
		}

		public void ToggleVisible()
		{
			if (wbColls != null)
				for (int i = 0; i < wbColls.Length; ++i)
					if (rends[i].isVisible)
						wbColls[i].ToggleVisible();
		}

		public void UpdateCollider()
		{
			if (wbColls != null)
				foreach (KKTICollider coll in wbColls)
					coll.UpdateCollider();
		}

		public void OnDestroy()
		{
			Hooks.ChangeClothesEvent -= ChangeClothesHandler;
			if (wbColls != null)
				for (int i = 0; i < wbColls.Length; ++i)
					Destroy(wbColls[i].gameObject);
		}
	}
}
