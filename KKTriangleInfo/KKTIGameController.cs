
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using KKAPI;
using KKAPI.MainGame;
using UnityEngine;

namespace KKTriangleInfo
{
	class KKTIGameController : GameCustomFunctionController
	{
		private Raycaster caster;

		//We just need a way to create one and exactly one raycaster
		protected override void OnStartH(HSceneProc inProc, bool inIsFreeH)
		{
			Camera.main.cullingMask += 1 << KKTICharaController.KKTICOLLLAYER;
			caster = Camera.main.gameObject.AddComponent<Raycaster>();
		}
	}
}
