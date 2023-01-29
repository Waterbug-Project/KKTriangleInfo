using KKAPI.Chara;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KKTriangleInfo
{
	//All of the colliders needed for one piece of clothing (e.g. top, bottom, socks...)
	//This includes the colliders for various states of undress - colliders are enabled and disabled as needed
	class KKTIClothingColliders : MonoBehaviour
	{
		public static event EventHandler ClothesShiftEvent;

		ChaControl cha;
		ChaFileDefine.ClothesKind kind;
		KKTICollider[] wbColls;
		SkinnedMeshRenderer[] rends;
		byte state;
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
			output.LoadClothingMeshes();
			CharacterApi.CoordinateLoaded += output.CoordinateLoadedHandler;
			KKTIClothingColliders.ClothesShiftEvent += output.ClothesShiftHandler;

			return output;
		}

		public void CoordinateLoadedHandler(object sender, EventArgs e)
		{
			ReloadClothes();
		}

		//Called when the current article of clothing is changed for a different one
		private void ReloadClothes()
		{
			if (wbColls != null)
				for (int i = 0; i < wbColls.Length; ++i)
					Destroy(wbColls[i].gameObject);
			LoadClothingMeshes();
		}

		private void LoadClothingMeshes()
		{
			int kindAsInt = (int)kind;
			state = cha.fileStatus.clothesState[kindAsInt];
			baseObj = cha.objClothes[kindAsInt];
			//Only try to load the colliders for this piece of clothing if the new outfit actually includes clothing within this clothing slot
			if (baseObj != null)
			{
				//We're assuming that no clothes use MeshRenderers here.
				rends = baseObj.GetComponentsInChildren<SkinnedMeshRenderer>(true);
				wbColls = new KKTICollider[rends.Length];
				for (int i = 0; i < wbColls.Length; ++i)
					wbColls[i] = KKTICollider.MakeKKTIColliderObj(rends[i], "WB_Clothes_Coll_" + rends[i].name, cha, kind);
			}
		}

		public void LateUpdate()
		{
			//There's probably a better way of doing this involving hooks and events.
			CheckClothesChange();
		}

		//Would be nice if this class, as a whole, reacted more uniformly to changes in the underlying ChaControl...
		//For example: Where should "state" update? And shouldn't the class be more heavily structured around changes in such important underlying variables?
		public void ClothesShiftHandler(object sender, EventArgs e)
		{
			byte newState = cha.fileStatus.clothesState[(int)kind];
			if (state != newState)
				state = newState;
		}

		private void CheckClothesChange()
		{
			if (baseObj == null)
				ReloadClothes();
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

		//We don't actually use this for anything. What would this do that UpdateCollider doesn't?
		public void ManualRefresh()
		{
			if (wbColls != null)
				foreach (KKTICollider coll in wbColls)
					coll.ManualRefresh();
		}

		public void UpdateCollider()
		{
			if (wbColls != null)
				foreach (KKTICollider coll in wbColls)
					coll.UpdateCollider();
		}

		public void OnDestroy()
		{
			if (wbColls != null)
				for (int i = 0; i < wbColls.Length; ++i)
					Destroy(wbColls[i].gameObject);
		}
	}
}
