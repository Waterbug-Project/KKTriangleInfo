using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KKTriangleInfo
{
	static class Hooks
	{
		public static EventHandler ChangeClothesEvent;
		public static EventHandler ChangeAccessoryEvent;

		//Code taken from Anon11 on the KK Discord
		[HarmonyPostfix, HarmonyPatch(typeof(ChaControl), nameof(ChaControl.ChangeClothesAsync), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(bool), typeof(bool))]
		private static void ChangeClothesAsyncPostfix(ChaControl __instance, ref IEnumerator __result)
		{
			var original = __result;
			__result = new[] { original, Postfix() }.GetEnumerator();

			IEnumerator Postfix()
			{
				ChangeClothesEvent?.Invoke(null, null);
				yield break;
			}
		}

		[HarmonyPostfix, HarmonyPatch(typeof(ChaControl), "ChangeAccessory", typeof(bool))]
		public static void ChangeAccessoryHook()
		{
			ChangeAccessoryEvent?.Invoke(null, null);
		}
	}
}
