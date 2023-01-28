
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
	class KKTriangleInfoController : GameCustomFunctionController
	{
		protected override void OnStartH(HSceneProc inProc, bool inIsFreeH)
		{
			UnityEngine.Debug.Log("[KKTriangleInfo] Hello World!");
		}
	}
}
