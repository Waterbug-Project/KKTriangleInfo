using HarmonyLib;
using System;
using System.Collections;

namespace KKTriangleInfo
{
	static class Hooks
	{
		//It would be nice to put these in an array that maps with ChaFileDefine.ClothesKind, but I can't figure out how.
		public static event EventHandler ChangeClothesTopEvent;
		public static event EventHandler ChangeClothesBotEvent;
		public static event EventHandler ChangeClothesBraEvent;
		public static event EventHandler ChangeClothesShortsEvent;
		public static event EventHandler ChangeClothesGlovesEvent;
		public static event EventHandler ChangeClothesPanstEvent;
		public static event EventHandler ChangeClothesSocksEvent;
		public static event EventHandler ChangeClothesShoesEvent;
		public static event EventHandler ReloadEvent;
		public static event EventHandler ChangeAccessoryEvent;

		private static int lastAccEvent = 0;

		//Code taken from Anon11 on the KK Discord
		[HarmonyPostfix, HarmonyPatch(typeof(ChaControl), nameof(ChaControl.ReloadAsync))]
		private static void ReloadAsyncPostfix(ChaControl __instance, ref IEnumerator __result)
		{
			var original = __result;
			__result = new[] { original, Postfix() }.GetEnumerator();

			IEnumerator Postfix()
			{
				ReloadEvent?.Invoke(null, null);
				yield break;
			}
		}

		[HarmonyPostfix, HarmonyPatch(typeof(ChaControl), nameof(ChaControl.ChangeAccessoryAsync), typeof(int), typeof(int), typeof(int), typeof(string), typeof(bool), typeof(bool))]
		private static void ChangeAccessoryAsyncPostfix(ChaControl __instance, ref IEnumerator __result)
		{
			var original = __result;
			__result = new[] { original, Postfix() }.GetEnumerator();

			IEnumerator Postfix()
			{
				//This hook is called about 20 times in a row in many cases - we only need to raise the event once per instance of that
				if (UnityEngine.Time.frameCount != lastAccEvent)
				{
					ChangeAccessoryEvent?.Invoke(null, null);
					lastAccEvent = UnityEngine.Time.frameCount;
				}
				yield break;
			}
		}

		[HarmonyPostfix, HarmonyPatch(typeof(ChaControl), nameof(ChaControl.ChangeClothesTopAsync))]
		private static void ChangeClothesTopAsyncPostfix(ChaControl __instance, ref IEnumerator __result)
		{
			var original = __result;
			__result = new[] { original, Postfix() }.GetEnumerator();

			IEnumerator Postfix()
			{
				ChangeClothesTopEvent?.Invoke(null, null);
				yield break;
			}
		}
		
		[HarmonyPostfix, HarmonyPatch(typeof(ChaControl), nameof(ChaControl.ChangeClothesBotAsync))]
		private static void ChangeClothesBotAsyncPostfix(ChaControl __instance, ref IEnumerator __result)
		{
			var original = __result;
			__result = new[] { original, Postfix() }.GetEnumerator();

			IEnumerator Postfix()
			{
				ChangeClothesBotEvent?.Invoke(null, null);
				yield break;
			}
		}

		[HarmonyPostfix, HarmonyPatch(typeof(ChaControl), nameof(ChaControl.ChangeClothesBraAsync))]
		private static void ChangeClothesBraAsyncPostfix(ChaControl __instance, ref IEnumerator __result)
		{
			var original = __result;
			__result = new[] { original, Postfix() }.GetEnumerator();

			IEnumerator Postfix()
			{
				ChangeClothesBraEvent?.Invoke(null, null);
				yield break;
			}
		}

		[HarmonyPostfix, HarmonyPatch(typeof(ChaControl), nameof(ChaControl.ChangeClothesShortsAsync))]
		private static void ChangeClothesShortsAsyncPostfix(ChaControl __instance, ref IEnumerator __result)
		{
			var original = __result;
			__result = new[] { original, Postfix() }.GetEnumerator();

			IEnumerator Postfix()
			{
				ChangeClothesShortsEvent?.Invoke(null, null);
				yield break;
			}
		}

		[HarmonyPostfix, HarmonyPatch(typeof(ChaControl), nameof(ChaControl.ChangeClothesGlovesAsync))]
		private static void ChangeClothesGlovesAsyncPostfix(ChaControl __instance, ref IEnumerator __result)
		{
			var original = __result;
			__result = new[] { original, Postfix() }.GetEnumerator();

			IEnumerator Postfix()
			{
				ChangeClothesGlovesEvent?.Invoke(null, null);
				yield break;
			}
		}

		[HarmonyPostfix, HarmonyPatch(typeof(ChaControl), nameof(ChaControl.ChangeClothesPanstAsync))]
		private static void ChangeClothesPanstAsyncPostfix(ChaControl __instance, ref IEnumerator __result)
		{
			var original = __result;
			__result = new[] { original, Postfix() }.GetEnumerator();

			IEnumerator Postfix()
			{
				ChangeClothesPanstEvent?.Invoke(null, null);
				yield break;
			}
		}

		[HarmonyPostfix, HarmonyPatch(typeof(ChaControl), nameof(ChaControl.ChangeClothesSocksAsync))]
		private static void ChangeClothesSocksAsyncPostfix(ChaControl __instance, ref IEnumerator __result)
		{
			var original = __result;
			__result = new[] { original, Postfix() }.GetEnumerator();

			IEnumerator Postfix()
			{
				ChangeClothesSocksEvent?.Invoke(null, null);
				yield break;
			}
		}

		[HarmonyPostfix, HarmonyPatch(typeof(ChaControl), nameof(ChaControl.ChangeClothesShoesAsync))]
		private static void ChangeClothesShoesAsyncPostfix(ChaControl __instance, ref IEnumerator __result)
		{
			var original = __result;
			__result = new[] { original, Postfix() }.GetEnumerator();

			IEnumerator Postfix()
			{
				ChangeClothesShoesEvent?.Invoke(null, null);
				yield break;
			}
		}
	}
}
