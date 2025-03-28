using BepInEx;
using HarmonyLib;
using BepInEx.Logging;
using UnityEngine;

namespace CustomGrabColour
{
    public static class PluginInfo
    {
        public const string PLUGIN_ID = "CustomGrabColour";
        public const string PLUGIN_NAME = "CustomGrabColour";
        public const string PLUGIN_VERSION = "0.0.2";
        public const string PLUGIN_GUID = "games.enchanted.CustomGrabColour";
    }

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }

        public ManualLogSource PluginLogger;

        public static void LogMessageIfDebug(object message)
        {
            if(CustomGrabColourConfig.enableDebugLogs.Value)
            {
                Instance.PluginLogger.LogMessage("CustomGrabColour Debug: " + message);
            }
        }
        public static void LogMessage(object message)
        {
            Instance.PluginLogger.LogMessage("CustomGrabColour: " + message);
        }
        public static void LogError(object message)
        {
            Instance.PluginLogger.LogError("CustomGrabColour: " + message);
        }

        private void Awake()
        {
            PluginLogger.LogInfo($"Loading plugin {PluginInfo.PLUGIN_NAME}! ({PluginInfo.PLUGIN_GUID})");
            Instance = this;

            PluginLogger = Logger;

            CustomGrabColourConfig.Init(Config);
            CustomGrabBeamColour.LocalColour = ConfigUtil.StringToColor(CustomGrabColourConfig.neutralGrabBeamColour.Value, new Color(1f, 0.1856f, 0f, 1f));

            // Apply Harmony patches (if any exist)
            Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

            // Plugin startup logic
            PluginLogger.LogInfo($"Loading finished for {PluginInfo.PLUGIN_NAME}! ({PluginInfo.PLUGIN_GUID})");
        }
    }
}
