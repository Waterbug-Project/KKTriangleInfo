﻿using System;
using UnityEngine;

namespace KKTriangleInfo
{
	//All of the colliders needed for one piece of clothing (e.g. top, bottom, socks...)
	//This includes the colliders for various states of undress - colliders are enabled and disabled as needed
	class KKTIClothingColliders : MonoBehaviour
	{
		ChaControl cha;
		ChaFileDefine.ClothesKind kind;
		KKTICollider[] colls;
		SkinnedMeshRenderer[] rends;
		GameObject baseObj;

		public static KKTIClothingColliders Make(ChaControl inCha, ChaFileDefine.ClothesKind inKind, int inLayer = KKTICharaController.KKTICOLLLAYER)
		{
			GameObject newObj = new GameObject();
			newObj.layer = inLayer;
			KKTIClothingColliders output = newObj.AddComponent<KKTIClothingColliders>();
			output.cha = inCha;
			if (!output.cha.IsClothesStateKind((int)inKind))
				UnityEngine.Debug.LogError("[KKTriangleInfo]\tClothesKind " + inKind + " is not valid for the given character!");
			output.kind = inKind;
			output.name = "KKTI_Clothing_Colliders_" + inKind.ToString();
			output.LoadClothingMeshes();
			//It would be nice to put this in a function, but I can't figure out how to pass in the handler.
			switch (inKind)
			{
				case ChaFileDefine.ClothesKind.top:			Hooks.ChangeClothesTopEvent += output.ChangeClothesHandler;	break;
				case ChaFileDefine.ClothesKind.bot:			Hooks.ChangeClothesBotEvent += output.ChangeClothesHandler;	break;
				case ChaFileDefine.ClothesKind.bra:			Hooks.ChangeClothesBraEvent += output.ChangeClothesHandler;	break;
				case ChaFileDefine.ClothesKind.shorts:		Hooks.ChangeClothesShortsEvent += output.ChangeClothesHandler;	break;
				case ChaFileDefine.ClothesKind.gloves:		Hooks.ChangeClothesGlovesEvent += output.ChangeClothesHandler;	break;
				case ChaFileDefine.ClothesKind.panst:		Hooks.ChangeClothesPanstEvent += output.ChangeClothesHandler;	break;
				case ChaFileDefine.ClothesKind.socks:		Hooks.ChangeClothesSocksEvent += output.ChangeClothesHandler;	break;
				case ChaFileDefine.ClothesKind.shoes_inner:	Hooks.ChangeClothesShoesEvent += output.ChangeClothesHandler;	break;
				case ChaFileDefine.ClothesKind.shoes_outer:	Hooks.ChangeClothesShoesEvent += output.ChangeClothesHandler;	break;
			}

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
			if (colls != null)
				for (int i = 0; i < colls.Length; ++i)
					if (colls[i] != null)
						Destroy(colls[i].gameObject);
			LoadClothingMeshes();
		}

		private void LoadClothingMeshes()
		{
			baseObj = cha.objClothes[(int)kind];
			//Only try to load the colliders for this piece of clothing if the new outfit actually includes clothing within this clothing slot
			if (baseObj != null)
			{
				//We're assuming that no clothes use MeshRenderers here.
				rends = baseObj.GetComponentsInChildren<SkinnedMeshRenderer>(true);
				colls = new KKTICollider[rends.Length];
				for (int i = 0; i < colls.Length; ++i)
					colls[i] = KKTICollider.Make(rends[i], "KKTI_Clothes_Coll_" + rends[i].name);
			}
			else
				colls = null;
		}

		public void UpdateCollider()
		{
			if (colls != null)
				foreach (KKTICollider coll in colls)
					coll.UpdateCollider();
		}

		public void OnDestroy()
		{
			switch (kind)
			{
				case ChaFileDefine.ClothesKind.top:			Hooks.ChangeClothesTopEvent -= ChangeClothesHandler; break;
				case ChaFileDefine.ClothesKind.bot:			Hooks.ChangeClothesBotEvent -= ChangeClothesHandler; break;
				case ChaFileDefine.ClothesKind.bra:			Hooks.ChangeClothesBraEvent -= ChangeClothesHandler; break;
				case ChaFileDefine.ClothesKind.shorts:		Hooks.ChangeClothesShortsEvent -= ChangeClothesHandler; break;
				case ChaFileDefine.ClothesKind.gloves:		Hooks.ChangeClothesGlovesEvent -= ChangeClothesHandler; break;
				case ChaFileDefine.ClothesKind.panst:		Hooks.ChangeClothesPanstEvent -= ChangeClothesHandler; break;
				case ChaFileDefine.ClothesKind.socks:		Hooks.ChangeClothesSocksEvent -= ChangeClothesHandler; break;
				case ChaFileDefine.ClothesKind.shoes_inner:	Hooks.ChangeClothesShoesEvent -= ChangeClothesHandler; break;
				case ChaFileDefine.ClothesKind.shoes_outer:	Hooks.ChangeClothesShoesEvent -= ChangeClothesHandler; break;
			}
			if (colls != null)
				for (int i = 0; i < colls.Length; ++i)
					Destroy(colls[i].gameObject);
		}
	}
}
