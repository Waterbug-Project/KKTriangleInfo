using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
			GameAPI.StartH += MakeColliders;
			GameAPI.EndH += Cleanup;
			MakerAPI.MakerBaseLoaded += MakeColliders;
			MakerAPI.MakerExiting += Cleanup;
			Hooks.ReloadEvent += MakeColliders;

			base.Awake();
		}

		private static void MakeRaycaster()
		{
			Camera.main.cullingMask += 1 << KKTICOLLLAYER;
			caster = Camera.main.gameObject.AddComponent<Raycaster>();
		}

		private void MakeColliders(object sender, EventArgs e)
		{
			headColl = KKTICollider.MakeKKTIColliderObj(Array.Find(ChaControl.objHead.GetComponentsInChildren<SkinnedMeshRenderer>(true), x => x.name == "cf_O_face"), "KKTI_Head");
			tongueColl = KKTICollider.MakeKKTIColliderObj(Array.Find(ChaControl.objHead.GetComponentsInChildren<SkinnedMeshRenderer>(true), x => x.name == "o_tang"), "KKTI_Tongue");
			bodyColl = KKTICollider.MakeKKTIColliderObj(Array.Find(ChaControl.objBody.GetComponentsInChildren<SkinnedMeshRenderer>(true), x => x.name == "o_body_a"), "KKTI_Body");
			hairColl = KKTIHairColliders.MakeKKTIHairColliders(ChaControl);
			for (int i = 0; i < clothColls.Length; ++i)
				clothColls[i] = KKTIClothingColliders.MakeKKTIClothingColliders(ChaControl, (ChaFileDefine.ClothesKind)i);
			accColl = KKTIAccessoryColliders.MakeKKTIAccessoryColliders(ChaControl);

			if (caster == null)
				MakeRaycaster();
		}

		public void Cleanup(object sender, EventArgs e)
		{
			UpdateCollidersEvent -= UpdateColliders;
			GameAPI.StartH -= MakeColliders;
			GameAPI.EndH -= Cleanup;
			MakerAPI.MakerBaseLoaded -= MakeColliders;
			MakerAPI.MakerExiting -= Cleanup;
			Hooks.ReloadEvent -= MakeColliders;
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

		protected override void Update()
		{
			if (ChaControl.sex == 1)
			{
				if (Input.GetKeyDown("y"))
				{
					bodyColl.UpdateCollider();
					bodyColl.ToggleVisible();
				}
			}
		}
	}
}
