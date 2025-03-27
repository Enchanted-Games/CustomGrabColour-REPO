using System.IO;

using BepInEx;

using HarmonyLib;
using BepInEx.Logging;

namespace CustomGrabColour
{
    public static class PluginInfo
    {
        public const string PLUGIN_ID = "CustomGrabColour";
        public const string PLUGIN_NAME = "CustomGrabColour";
        public const string PLUGIN_VERSION = "0.0.1";
        public const string PLUGIN_GUID = "games.enchanted.CustomGrabColour";
    }

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }

        public ManualLogSource PluginLogger;

        private void Awake()
        {
            Instance = this;

            PluginLogger = Logger;

            // Apply Harmony patches (if any exist)
            Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

            // Plugin startup logic
            PluginLogger.LogInfo($"omg the mod is loading! {PluginInfo.PLUGIN_NAME} ({PluginInfo.PLUGIN_GUID})");
        }
    }
}
