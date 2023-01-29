using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using KKAPI;
using KKAPI.Chara;
using KKAPI.MainGame;

namespace KKTriangleInfo
{
	[BepInPlugin(GUID, PluginName, Version)]
	// Tell BepInEx that this plugin needs KKAPI of at least the specified version.
	// If not found, this plugi will not be loaded and a warning will be shown.
	[BepInDependency(KoikatuAPI.GUID, KoikatuAPI.VersionConst)]
	public class ExamplePlugin : BaseUnityPlugin
	{
		public const string PluginName = "KKTriangleInfo";
		public const string GUID = "org.bepinex.plugins.kktriangleinfo";
		public const string Version = "0.0.0.0";

		internal static new ManualLogSource Logger;

		private ConfigEntry<bool> _exampleConfigEntry;

		private void Awake()
		{
			Logger = base.Logger;

			_exampleConfigEntry = Config.Bind("General", "Enable this plugin", true, "If false, this plugin will do nothing");

			if (_exampleConfigEntry.Value)
			{
				GameAPI.RegisterExtraBehaviour<KKTIGameController>(GUID);
				CharacterApi.RegisterExtraBehaviour<KKTICharaController>(GUID);
			}
		}
	}
}
