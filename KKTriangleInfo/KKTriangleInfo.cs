﻿using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using KKAPI;
using KKAPI.Chara;
using KKAPI.MainGame;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KKTriangleInfo
{
	[BepInPlugin(GUID, PluginName, Version)]
	[BepInDependency(KoikatuAPI.GUID, KoikatuAPI.VersionConst)]
	public class KKTriangleInfo : BaseUnityPlugin
	{
		public const string PluginName = "KK_TriangleInfo";
		public const string GUID = "waterbug.plugins.kktriangleinfo";
		public const string Version = "0.0.0.0";

		public static Color SELECTCOLOR;
		public static char CASTKEY;
		public static List<char> CASTKEYMODS;

		internal static new ManualLogSource Logger;
		
		private ConfigEntry<float> redVal;
		private ConfigEntry<float> greenVal;
		private ConfigEntry<float> blueVal;
		private ConfigEntry<KeyboardShortcut> castKey;

		private void Awake()
		{
			Logger = base.Logger;

			redVal = Config.Bind("General", "Selection Red Value", 0.0f, "Red color value for selection triangle, between 0 and 1");
			greenVal = Config.Bind("General", "Selection Green Value", 1.0f, "Green color value for selection triangle, between 0 and 1");
			blueVal = Config.Bind("General", "Selection Blue Value", 0.0f, "Blue color value for selection triangle, between 0 and 1");
			SELECTCOLOR = new Color(Mathf.Clamp(redVal.Value, 0f, 1f), Mathf.Clamp(greenVal.Value, 0f, 1f), Mathf.Clamp(blueVal.Value, 0f, 1f));

			CASTKEY = 't';
			//try
			//{
			//	castKey = Config.Bind("General", "Selection Hotkey", new KeyboardShortcut(KeyCode.T), "Key to press to select the polygon underneath the cursor");
			//	KeyboardShortcut configVal = castKey.Value;
			//	CASTKEY = (char)configVal.MainKey;

			//	CASTKEYMODS = new List<char>();
			//	IEnumerable<KeyCode> modKeys = configVal.Modifiers;
			//	foreach (char modKey in modKeys)
			//		CASTKEYMODS.Add(modKey);
			//}
			//catch (Exception ex)
			//{
			//	UnityEngine.Debug.Log("Found exception in KKTriangleInfo.Awake: " + ex.ToString() + "!");
			//}

			GameAPI.RegisterExtraBehaviour<KKTIGameController>(GUID);
			CharacterApi.RegisterExtraBehaviour<KKTICharaController>(GUID);
		}
	}
}
