using BepInEx;
using HarmonyLib;
using BepInEx.Logging;
using CustomGrabColour.Config;

namespace CustomGrabColour
{
    public static class PluginInfo
    {
        public const string PluginID = "CustomGrabColour";
        public const string PluginName = "CustomGrabColour";
        public const string PluginVersion = "2.3.0";
        public const string PluginGuid = "games.enchanted.CustomGrabColour";
    }

    [BepInPlugin(PluginInfo.PluginGuid, PluginInfo.PluginName, PluginInfo.PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }

        private ManualLogSource _pluginLogger;

        public static void LogMessageIfDebug(object message)
        {
            if(CustomGrabColourConfig.EnableDebugLogs.Value)
            {
                Instance._pluginLogger.LogMessage("CustomGrabColour Debug: " + message);
            }
        }
        public static void LogMessage(object message)
        {
            Instance._pluginLogger.LogMessage("CustomGrabColour: " + message);
        }
        public static void LogError(object message)
        {
            Instance._pluginLogger.LogError("CustomGrabColour Debug: " + message);
        }
        public static void LogErrorIfDebug(object message)
        {
            if (CustomGrabColourConfig.EnableDebugLogs.Value)
            {
                Instance._pluginLogger.LogError("CustomGrabColour: " + message);
            }
        }
        public static void LogWarning(object message)
        {
            Instance._pluginLogger.LogWarning("CustomGrabColour: " + message);
        }

        private void Awake()
        {
            Instance = this;

            _pluginLogger = Logger;

            _pluginLogger.LogInfo($"Loading plugin {PluginInfo.PluginName}! ({PluginInfo.PluginGuid})");

            CustomGrabColourConfig.Init(Config);

            // Apply Harmony patches (if any exist)
            Harmony harmony = new Harmony(PluginInfo.PluginGuid);
            harmony.PatchAll();

            ConfigMenu.Init();

            // Plugin startup logic
            _pluginLogger.LogInfo($"Loading finished for {PluginInfo.PluginName}! ({PluginInfo.PluginGuid})");
        }
    }
}
