using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using KKAPI;
using KKAPI.Chara;
using KKAPI.MainGame;
using UnityEngine;

namespace KKTriangleInfo
{
	[BepInPlugin(GUID, PluginName, Version)]
	// Tell BepInEx that this plugin needs KKAPI of at least the specified version.
	// If not found, this plugi will not be loaded and a warning will be shown.
	[BepInDependency(KoikatuAPI.GUID, KoikatuAPI.VersionConst)]
	public class KKTriangleInfo : BaseUnityPlugin
	{
		public const string PluginName = "KK_TriangleInfo";
		public const string GUID = "waterbug.plugins.kktriangleinfo";
		public const string Version = "0.0.0.0";

		public static Color SELECTCOLOR;
		public static char CASTKEY;

		internal static new ManualLogSource Logger;
		
		private ConfigEntry<float> redVal;
		private ConfigEntry<float> greenVal;
		private ConfigEntry<float> blueVal;
		private ConfigEntry<char> castKey;

		private void Awake()
		{
			Logger = base.Logger;

			redVal = Config.Bind("General", "Selection Red Value", 0.0f, "Red color value for selection triangle, between 0 and 1");
			greenVal = Config.Bind("General", "Selection Green Value", 1.0f, "Green color value for selection triangle, between 0 and 1");
			blueVal = Config.Bind("General", "Selection Blue Value", 0.0f, "Blue color value for selection triangle, between 0 and 1");
			SELECTCOLOR = new Color(Mathf.Clamp(redVal.Value, 0f, 1f), Mathf.Clamp(greenVal.Value, 0f, 1f), Mathf.Clamp(blueVal.Value, 0f, 1f));

			//castKey = Config.Bind("General", "Selection Hotkey", 't', "Key to press to select the polygon underneath the cursor");
			CASTKEY = 't';//castKey.Value;

			GameAPI.RegisterExtraBehaviour<KKTIGameController>(GUID);
			CharacterApi.RegisterExtraBehaviour<KKTICharaController>(GUID);
		}
	}
}
