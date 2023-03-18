using System;
using UnityEngine;
using KKAPI.Chara;
using KKAPI;
using KKAPI.MainGame;
using KKAPI.Maker;

namespace KKTriangleInfo
{
	class KKTICharaController : CharaCustomFunctionController
	{
		public const int KKTICOLLLAYER = 14;
		private static Raycaster caster;

		public static EventHandler UpdateCollidersEvent;

		KKTICollider headColl = null;
		KKTICollider tongueColl = null;
		KKTICollider bodyColl = null;
		KKTIHairColliders hairColl = null;
		KKTIClothingColliders[] clothColls = new KKTIClothingColliders[9];
		KKTIAccessoryColliders accColl = null;

		protected override void OnCardBeingSaved(GameMode curGameMode)
		{
			SetExtendedData(null);
		}

		protected override void Awake()
		{
			UpdateCollidersEvent += UpdateColliders;
			GameAPI.StartH += MakeAllColliders;
			GameAPI.EndH += Cleanup;
			MakerAPI.MakerBaseLoaded += MakeAllColliders;
			MakerAPI.MakerExiting += Cleanup;
			Hooks.ReloadEvent += MakeBodyColliders;

			base.Awake();
		}

		private static void MakeRaycaster()
		{
			Camera.main.cullingMask += 1 << KKTICOLLLAYER;
			caster = Camera.main.gameObject.AddComponent<Raycaster>();
		}

		private void MakeBodyColliders(object sender, EventArgs e)
		{
			headColl = KKTICollider.Make(Array.Find(ChaControl.objHead.GetComponentsInChildren<SkinnedMeshRenderer>(true), x => x.name == "cf_O_face"), "KKTI_Head");
			tongueColl = KKTICollider.Make(Array.Find(ChaControl.objHead.GetComponentsInChildren<SkinnedMeshRenderer>(true), x => x.name == "o_tang"), "KKTI_Tongue");
			bodyColl = KKTICollider.Make(Array.Find(ChaControl.objBody.GetComponentsInChildren<SkinnedMeshRenderer>(true), x => x.name == "o_body_a"), "KKTI_Body");
			hairColl = KKTIHairColliders.Make(ChaControl);
		}

		private void MakeAllColliders(object sender, EventArgs e)
		{
			MakeBodyColliders(sender, e);
			for (int i = 0; i < clothColls.Length; ++i)
				clothColls[i] = KKTIClothingColliders.Make(ChaControl, (ChaFileDefine.ClothesKind)i);
			accColl = KKTIAccessoryColliders.Make(ChaControl);

			if (caster == null)
				MakeRaycaster();
		}

		public void Cleanup(object sender, EventArgs e)
		{
			UpdateCollidersEvent -= UpdateColliders;
			GameAPI.StartH -= MakeAllColliders;
			GameAPI.EndH -= Cleanup;
			MakerAPI.MakerBaseLoaded -= MakeAllColliders;
			MakerAPI.MakerExiting -= Cleanup;
			Hooks.ReloadEvent -= MakeBodyColliders;
			if (caster != null)
				Destroy(caster);
		}

		private void UpdateColliders(object sender, EventArgs e)
		{
			headColl.UpdateCollider();
			tongueColl.UpdateCollider();
			bodyColl.UpdateCollider();
			hairColl.UpdateCollider();
			foreach (KKTIClothingColliders coll in clothColls)
				coll.UpdateCollider();
			accColl.UpdateCollider();
		}
	}
}
