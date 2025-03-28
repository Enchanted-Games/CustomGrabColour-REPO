using BepInEx;
using BepInEx.Configuration;
using CustomGrabColour;
using System.IO;
using UnityEngine;

class CustomGrabColourConfig
{
    public static readonly float DefaultOpacity = 0.15f;
    public static readonly float MaxOpacity = 0.5f;
    public static string ConfigFileName = "CustomGrabColour.es3";

    public static ConfigEntry<string> neutralGrabBeamColour;
    public static ConfigEntry<bool> enableDebugLogs;

    public static void Init(ConfigFile config)
    {
        neutralGrabBeamColour = config.Bind(
            "General",
            "NeutralGrabBeamColour",
            ConfigUtil.ColorToString(new Color(1f, 0.1856f, 0f, DefaultOpacity)),
            "The default colour for the grab beam when holding an item (when not rotating or healing a player)"
        );

        enableDebugLogs = config.Bind(
            "Debug",
            "EnableDebugLogs",
            false,
            "Outputs additional debugging information to the log"
        );
    }

    public static void SaveColour(Color colour)
    {
        Plugin.LogMessageIfDebug("Saving colour to config file: " + colour);
        colour.a = Mathf.Clamp(colour.a, 0f, MaxOpacity);
        neutralGrabBeamColour.Value = ConfigUtil.ColorToString(colour);
        neutralGrabBeamColour.SetSerializedValue(ConfigUtil.ColorToString(colour));
    }
}