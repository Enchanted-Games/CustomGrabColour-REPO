using BepInEx;
using BepInEx.Configuration;
using CustomGrabColour;
using System.IO;
using UnityEngine;

class CustomGrabColourConfig
{
    public static string ConfigFileName = "CustomGrabColour.es3";

    public static ConfigEntry<string> neutralGrabBeamColour;

    public static void Init(ConfigFile config)
    {
        neutralGrabBeamColour = config.Bind(
            "General",
            "NeutralGrabBeamColour",
            ConfigUtil.ColorToString(new Color(1f, 0.1856f, 0f, 1f)),
            "The default colour for the grab beam when holding an item (when not rotating or healing a player)"
        );
    }

    public static void SaveColour(Color colour)
    {
        neutralGrabBeamColour.Value = ConfigUtil.ColorToString(colour);
        neutralGrabBeamColour.SetSerializedValue(ConfigUtil.ColorToString(colour));
    }
}