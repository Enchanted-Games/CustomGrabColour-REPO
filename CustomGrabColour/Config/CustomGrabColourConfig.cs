using System.Collections.Generic;
using BepInEx.Configuration;
using CustomGrabColour.PlayerGrabBeam;
using UnityEngine;

namespace CustomGrabColour.Config;

public static class CustomGrabColourConfig
{
    private static readonly Dictionary<GrabBeamColourSettings.BeamType, BeamConfigEntries> BeamTypeToConfigEntries = [];

    public struct BeamConfigEntries(ConfigEntry<string> beamColour, ConfigEntry<bool> matchSkin)
    {
        public readonly ConfigEntry<string> BeamColour = beamColour;
        public readonly ConfigEntry<bool> MatchSkin = matchSkin;
    }

    public const float DefaultOpacity = 0.2f;
    public const float MinOpacity = 0.1f;
    public const float MaxOpacity = 0.7f;
    public static readonly Color NeutralDefaultColour = new(1f, 0.58f, 0.19f, 0.35f);
    public static readonly Color HealingDefaultColour = new(0.17f, 1f, 0.17f, DefaultOpacity);
    public static readonly Color RotatingDefaultColour = new(0.65f, 0.06f, 0.8f, DefaultOpacity);
    public static readonly Color ClimbingDefaultColour = new(0.0f, 0.7f, 1f, DefaultOpacity);

    public static BeamConfigEntries NeutralGrabBeam;
    public static BeamConfigEntries HealingGrabBeam;
    public static BeamConfigEntries RotatingGrabBeam;
    public static BeamConfigEntries ClimbingGrabBeam;

    public static ConfigEntry<bool> EnableDebugLogs;
    public static ConfigEntry<bool> DebugAddButtonToMainMenu;

    private const string ColourNotes = "\nStored as R,G,B,A values in 0-1 range";

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
            "Should the neutral grab beam match the colour of your grabber cosmetic?"
        );
        NeutralGrabBeam = new BeamConfigEntries(neutralGrabBeamColour, neutralGrabBeamMatchSkin);
        BeamTypeToConfigEntries.Add(GrabBeamColourSettings.BeamType.Neutral, NeutralGrabBeam);


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
            "Should the rotating grab beam match the colour of your grabber cosmetic?"
        );
        RotatingGrabBeam = new BeamConfigEntries(rotatingGrabBeamColour, rotatingGrabBeamMatchSkin);
        BeamTypeToConfigEntries.Add(GrabBeamColourSettings.BeamType.Rotate, RotatingGrabBeam);


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
            "Should the healing grab beam match the colour of your grabber cosmetic?"
        );
        HealingGrabBeam = new BeamConfigEntries(healingGrabBeamColour, healingGrabBeamMatchSkin);
        BeamTypeToConfigEntries.Add(GrabBeamColourSettings.BeamType.Heal, HealingGrabBeam);
        
        
        ConfigEntry<string> climbingGrabBeamColour = config.Bind(
            "General",
            "ClimbingGrabBeamColour",
            ConfigUtil.ColorToString(ClimbingDefaultColour),
            "The colour of the grab beam when climbing with the tumble climb upgrade." + ColourNotes
        );
        ConfigEntry<bool> climbingGrabBeamMatchSkin = config.Bind(
            "General",
            "ClimbingGrabBeamMatchSkin",
            false,
            "Should the climbing grab beam match the colour of your grabber cosmetic?"
        );
        ClimbingGrabBeam = new BeamConfigEntries(climbingGrabBeamColour, climbingGrabBeamMatchSkin);
        BeamTypeToConfigEntries.Add(GrabBeamColourSettings.BeamType.Climb, ClimbingGrabBeam);


        EnableDebugLogs = config.Bind(
            "Debug",
            "EnableDebugLogs",
            false,
            "Outputs additional debugging information to the log"
        );
        
        DebugAddButtonToMainMenu = config.Bind(
            "Debug",
            "AddConfigToMainMenu",
            false,
            "Adds the config menu button to the main menu"
        );

        LoadValuesFromConfig();
    }

    private static void LoadValuesFromConfig()
    {
        // load neutral colour
        Color neutralColourFromConfig = ConfigUtil.StringToColor(NeutralGrabBeam.BeamColour.Value, NeutralDefaultColour);
        neutralColourFromConfig.a = ClampOpacity(neutralColourFromConfig.a);
        CustomGrabBeamColour.LocalNeutralColour = new GrabBeamColourSettings(neutralColourFromConfig, NeutralGrabBeam.MatchSkin.Value, GrabBeamColourSettings.BeamType.Neutral);

        // load rotating colour
        Color rotatingColourFromConfig = ConfigUtil.StringToColor(RotatingGrabBeam.BeamColour.Value, RotatingDefaultColour);
        rotatingColourFromConfig.a = ClampOpacity(rotatingColourFromConfig.a);
        CustomGrabBeamColour.LocalRotatingColour = new GrabBeamColourSettings(rotatingColourFromConfig, RotatingGrabBeam.MatchSkin.Value, GrabBeamColourSettings.BeamType.Rotate);

        // load healing colour
        Color healingColourFromConfig = ConfigUtil.StringToColor(HealingGrabBeam.BeamColour.Value, HealingDefaultColour);
        healingColourFromConfig.a = ClampOpacity(healingColourFromConfig.a);
        CustomGrabBeamColour.LocalHealingColour = new GrabBeamColourSettings(healingColourFromConfig, HealingGrabBeam.MatchSkin.Value, GrabBeamColourSettings.BeamType.Heal);

        // load climbing colour
        Color climbingColourFromConfig = ConfigUtil.StringToColor(ClimbingGrabBeam.BeamColour.Value, ClimbingDefaultColour);
        climbingColourFromConfig.a = ClampOpacity(climbingColourFromConfig.a);
        CustomGrabBeamColour.LocalClimbingColour = new GrabBeamColourSettings(climbingColourFromConfig, ClimbingGrabBeam.MatchSkin.Value, GrabBeamColourSettings.BeamType.Climb);
    }

    public static void SaveColour(GrabBeamColourSettings beamColourSettings)
    {
        Plugin.LogMessageIfDebug("Saving colour to config file: " + beamColourSettings);
        beamColourSettings.a = ClampOpacity(beamColourSettings.a);

        bool colourConfigValueExists = BeamTypeToConfigEntries.TryGetValue(beamColourSettings.CurrentBeamType, out BeamConfigEntries configEntries);
        if (!colourConfigValueExists)
        {
            Plugin.LogWarning("Unable to save colour value for beam type: " + beamColourSettings.CurrentBeamType);
        }

        configEntries.BeamColour.Value = ConfigUtil.ColorToString(beamColourSettings.Colour);
        configEntries.MatchSkin.Value = beamColourSettings.MatchSkin;
    }

    public static float ClampOpacity(float opacity)
    {
        return Mathf.Clamp(opacity, MinOpacity, MaxOpacity);
    }
}