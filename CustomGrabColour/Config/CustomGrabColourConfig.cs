using BepInEx.Configuration;
using CustomGrabColour;
using System.Collections.Generic;
using UnityEngine;

class CustomGrabColourConfig
{
    public static Dictionary<GrabBeamColourSettings.BeamType, BeamConfigEntries> BeamTypeToConfigEntries = [];

    public struct BeamConfigEntries(ConfigEntry<string> beamColour, ConfigEntry<bool> matchSkin)
    {
        public ConfigEntry<string> beamColour = beamColour;
        public ConfigEntry<bool> matchSkin = matchSkin;
    }

    public static readonly float DefaultOpacity = 0.2f;
    public static readonly float MaxOpacity = 0.5f;
    public static readonly Color NeutralDefaultColour = new Color(1f, 0.58f, 0.19f, 0.35f);
    public static readonly Color HealingDefaultColour = new Color(0.17f, 1f, 0.17f, DefaultOpacity);
    public static readonly Color RotatingDefaultColour = new Color(0.65f, 0.06f, 0.8f, DefaultOpacity);

    public static BeamConfigEntries neutralGrabBeam;
    public static BeamConfigEntries healingGrabBeam;
    public static BeamConfigEntries rotatingGrabBeam;

    public static ConfigEntry<bool> enableDebugLogs;

    private static readonly string ColourNotes = "\nStored as R,G,B,A values in 0-1 range";

    public static void Init(ConfigFile config)
    {
        ConfigEntry<string> neutralGrabBeamColour = config.Bind(
            "General",
            "NeutralGrabBeamColour",
            ConfigUtil.ColorToString(NeutralDefaultColour),
            "The default colour of the grab beam when holding an item." + ColourNotes
        );
        ConfigEntry<bool> neutralGrabBeamMatchSkin = config.Bind(
            "General",
            "NeutralGrabBeamMatchSkin",
            true,
            "Should the neutral grab beam match the colour of your skin?"
        );
        neutralGrabBeam = new BeamConfigEntries(neutralGrabBeamColour, neutralGrabBeamMatchSkin);
        BeamTypeToConfigEntries.Add(GrabBeamColourSettings.BeamType.Neutral, neutralGrabBeam);


        ConfigEntry<string> rotatingGrabBeamColour = config.Bind(
            "General",
            "RotatingGrabBeamColour",
            ConfigUtil.ColorToString(RotatingDefaultColour),
            "The colour of the grab beam when rotating an item or monster." + ColourNotes
        );
        ConfigEntry<bool> rotatingGrabBeamMatchSkin = config.Bind(
            "General",
            "RotatingGrabBeamMatchSkin",
            false,
            "Should the rotating grab beam match the colour of your skin?"
        );
        rotatingGrabBeam = new BeamConfigEntries(rotatingGrabBeamColour, rotatingGrabBeamMatchSkin);
        BeamTypeToConfigEntries.Add(GrabBeamColourSettings.BeamType.Rotate, rotatingGrabBeam);


        ConfigEntry<string> healingGrabBeamColour = config.Bind(
            "General",
            "HealingGrabBeamColour",
            ConfigUtil.ColorToString(HealingDefaultColour),
            "The colour of the grab beam when healing another player." + ColourNotes
        );
        ConfigEntry<bool> healingGrabBeamMatchSkin = config.Bind(
            "General",
            "HealingGrabBeamMatchSkin",
            false,
            "Should the healing grab beam match the colour of your skin?"
        );
        healingGrabBeam = new BeamConfigEntries(healingGrabBeamColour, healingGrabBeamMatchSkin);
        BeamTypeToConfigEntries.Add(GrabBeamColourSettings.BeamType.Heal, healingGrabBeam);


        enableDebugLogs = config.Bind(
            "Debug",
            "EnableDebugLogs",
            false,
            "Outputs additional debugging information to the log"
        );

        LoadValuesFromConfig();
    }

    public static void LoadValuesFromConfig()
    {
        // load neutral colour
        Color neutralColourFromConfig = ConfigUtil.StringToColor(neutralGrabBeam.beamColour.Value, NeutralDefaultColour);
        neutralColourFromConfig.a = Mathf.Clamp(neutralColourFromConfig.a, 0f, MaxOpacity);
        CustomGrabBeamColour.LocalNeutralColour = new GrabBeamColourSettings(neutralColourFromConfig, neutralGrabBeam.matchSkin.Value, GrabBeamColourSettings.BeamType.Neutral);

        // load rotating colour
        Color rotatingColourFromConfig = ConfigUtil.StringToColor(rotatingGrabBeam.beamColour.Value, RotatingDefaultColour);
        rotatingColourFromConfig.a = Mathf.Clamp(rotatingColourFromConfig.a, 0f, MaxOpacity);
        CustomGrabBeamColour.LocalRotatingColour = new GrabBeamColourSettings(rotatingColourFromConfig, rotatingGrabBeam.matchSkin.Value, GrabBeamColourSettings.BeamType.Rotate);

        // load healing colour
        Color healingColourFromConfig = ConfigUtil.StringToColor(healingGrabBeam.beamColour.Value, HealingDefaultColour);
        healingColourFromConfig.a = Mathf.Clamp(healingColourFromConfig.a, 0f, MaxOpacity);
        CustomGrabBeamColour.LocalHealingColour = new GrabBeamColourSettings(healingColourFromConfig, healingGrabBeam.matchSkin.Value, GrabBeamColourSettings.BeamType.Heal);
    }

    public static void SaveColour(GrabBeamColourSettings beamColourSettings)
    {
        Plugin.LogMessageIfDebug("Saving colour to config file: " + beamColourSettings);
        beamColourSettings.a = Mathf.Clamp(beamColourSettings.a, 0f, MaxOpacity);

        bool colourConfigValueExists = BeamTypeToConfigEntries.TryGetValue(beamColourSettings.beamType, out BeamConfigEntries configEntries);
        if (!colourConfigValueExists)
        {
            Plugin.LogWarning("Unable to save colour value for beam type: " + beamColourSettings.beamType);
        }

        configEntries.beamColour.Value = ConfigUtil.ColorToString(beamColourSettings.colour);
        configEntries.matchSkin.Value = beamColourSettings.matchSkin;
    }
}