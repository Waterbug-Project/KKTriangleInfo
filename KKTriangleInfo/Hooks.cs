using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KKTriangleInfo
{
	static class Hooks
	{
		public static EventHandler ChangeClothesEvent;
		public static EventHandler ChangeAccessoryEvent;

		[HarmonyPostfix, HarmonyPatch(typeof(ChaControl), "ChangeClothes", typeof(bool))]
		public static void ChangeClothesHook()
		{
			ChangeClothesEvent?.Invoke(null, null);
		}

		[HarmonyPostfix, HarmonyPatch(typeof(ChaControl), "ChangeAccessory", typeof(bool))]
		public static void ChangeAccessoryHook()
		{
			ChangeAccessoryEvent?.Invoke(null, null);
		}
	}
}
