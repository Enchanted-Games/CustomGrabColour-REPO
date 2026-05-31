using System;
using System.Reflection;
using CustomGrabColour.Config;
using Photon.Pun;
using UnityEngine;
using static CustomGrabColour.PlayerGrabBeam.GrabBeamColourSettings;

namespace CustomGrabColour.PlayerGrabBeam;

// handles local and other players grab beam colours
public class  CustomGrabBeamColour : MonoBehaviour, IPunObservable
{
    internal static GrabBeamColourSettings LocalNeutralColour;
    internal static GrabBeamColourSettings LocalHealingColour;
    internal static GrabBeamColourSettings LocalRotatingColour;
    internal static GrabBeamColourSettings LocalClimbingColour;
    
    internal GrabBeamColourSettings CurrentNeutralColour = new(CustomGrabColourConfig.NeutralDefaultColour, false, BeamType.Neutral);
    internal GrabBeamColourSettings CurrentHealingColour = new(CustomGrabColourConfig.HealingDefaultColour, false, BeamType.Heal);
    internal GrabBeamColourSettings CurrentRotatingColour = new(CustomGrabColourConfig.RotatingDefaultColour, false, BeamType.Rotate);
    internal GrabBeamColourSettings CurrentClimbingColour = new(CustomGrabColourConfig.ClimbingDefaultColour, false, BeamType.Climb);
    internal bool SentInitialColourUpdate = false;

    public PlayerAvatar player;

    public static GrabBeamColourSettings LocalBeamColour
    {
        set
        {
            switch (value.CurrentBeamType)
            {
                case BeamType.Neutral:
                {
                    LocalNeutralColour = value; break;
                }
                case BeamType.Heal:
                {
                    LocalHealingColour = value; break;
                }
                case BeamType.Rotate:
                {
                    LocalRotatingColour = value; break;
                }
                case BeamType.Climb:
                {
                    LocalClimbingColour = value; break;
                }
            }
        }
        get => throw new NotImplementedException("Tried to get field LocalBeamColour, call GetLocalSettingsForBeamType instead");
    }
    public static GrabBeamColourSettings GetLocalSettingsForBeamType(BeamType beamType)
    {
        switch (beamType)
        {
            case BeamType.Heal:
            {
                return LocalHealingColour;
            }
            case BeamType.Rotate:
            {
                return LocalRotatingColour;
            }
            case BeamType.Climb:
            {
                return LocalClimbingColour;
            }
            default:
            {
                return LocalNeutralColour;
            }
        }
    }

    public GrabBeamColourSettings CurrentBeamColour
    {
        set
        {
            switch (value.CurrentBeamType)
            {
                case BeamType.Neutral:
                {
                    CurrentNeutralColour = value; break;
                }
                case BeamType.Heal:
                {
                    CurrentHealingColour = value; break;
                }
                case BeamType.Rotate:
                {
                    CurrentRotatingColour = value; break;
                }
                case BeamType.Climb:
                {
                    CurrentClimbingColour = value; break;
                }
            }
        }
        get => throw new NotImplementedException("Tried to get field CurrentBeamColour, call GetSettingsForBeamType instead");
    }
    public GrabBeamColourSettings GetSettingsForBeamType(BeamType beamType)
    {
        switch (beamType)
        {
            case BeamType.Rotate:
            {
                return CurrentRotatingColour;
            }
            case BeamType.Heal:
            {
                return CurrentHealingColour;
            }
            case BeamType.Climb:
            {
                return CurrentClimbingColour;
            }
            default:
            {
                return CurrentNeutralColour;
            }
        }
    }

    void Awake()
    {
        player = gameObject.GetComponent<PlayerAvatar>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new NotImplementedException();
    }

    public static void SaveLocalColoursToConfig()
    {
        CustomGrabColourConfig.SaveColour(LocalNeutralColour);
        CustomGrabColourConfig.SaveColour(LocalRotatingColour);
        CustomGrabColourConfig.SaveColour(LocalHealingColour);
        CustomGrabColourConfig.SaveColour(LocalClimbingColour);
    }

    public static void ResetBeamColours()
    {
        LocalNeutralColour = new GrabBeamColourSettings(CustomGrabColourConfig.NeutralDefaultColour, (bool)CustomGrabColourConfig.NeutralGrabBeam.MatchSkin.DefaultValue, BeamType.Neutral);
        LocalRotatingColour = new GrabBeamColourSettings(CustomGrabColourConfig.RotatingDefaultColour, (bool)CustomGrabColourConfig.RotatingGrabBeam.MatchSkin.DefaultValue, BeamType.Rotate);
        LocalHealingColour = new GrabBeamColourSettings(CustomGrabColourConfig.HealingDefaultColour, (bool)CustomGrabColourConfig.HealingGrabBeam.MatchSkin.DefaultValue, BeamType.Heal);
        LocalClimbingColour = new GrabBeamColourSettings(CustomGrabColourConfig.ClimbingDefaultColour, (bool)CustomGrabColourConfig.ClimbingGrabBeam.MatchSkin.DefaultValue, BeamType.Climb);
        UpdateBeamColourForAllBeams();
    }

    public static void UpdateBeamColour(GrabBeamColourSettings newColour)
    {
        newColour.a = Mathf.Clamp(newColour.a, 0f, CustomGrabColourConfig.MaxOpacity);
        LocalBeamColour = newColour;
        UpdateBeamColour(newColour.CurrentBeamType);
    }
    public static void UpdateBeamColourForAllBeams()
    {
        foreach (BeamType beamType in Enum.GetValues(typeof(BeamType)))
        {
            UpdateBeamColour(beamType);
        }
    }

    public static void UpdateBeamColour(BeamType beamType)
    {
        GrabBeamColourSettings settings = GetLocalSettingsForBeamType(beamType);

        if (GameManager.Multiplayer())
        {
            PlayerAvatar.instance.photonView.RPC("SetBeamColourRPC", RpcTarget.AllBuffered, ToRPCBuffer(settings));
        } else
        {
            PlayerAvatar.instance.GetComponent<CustomGrabBeamColour>().SetBeamColourRPC(ToRPCBuffer(settings));
        }
    }

    [PunRPC]
    public void SetBeamColourRPC(object[] beamColourParts)
    {
        GrabBeamColourSettings newBeamColour = FromRPCBuffer(beamColourParts);
        Plugin.LogMessageIfDebug("SetBeamColourRPC called with values: r:" + newBeamColour.r + ", g:" + newBeamColour.g + ", b:" + newBeamColour.b + ", a:" + newBeamColour.a + ", matchSkin:" + newBeamColour.MatchSkin + ", beamType:" + newBeamColour.CurrentBeamType);

        newBeamColour.a = Mathf.Clamp(newBeamColour.a, 0f, CustomGrabColourConfig.MaxOpacity);

        CurrentBeamColour = newBeamColour;

        if (newBeamColour.CurrentBeamType != BeamType.Neutral) return;

        // invoke ColorStates method to make sure the beam colour updates properly
        Type physGrabberType = typeof(PhysGrabber);

        try
        {
            FieldInfo colorStatesField = physGrabberType.GetField(nameof(PhysGrabber.prevColorState), BindingFlags.Instance | BindingFlags.NonPublic);
            if (colorStatesField != null) colorStatesField.SetValue(player.physGrabber, -1);
        }
        catch (Exception e)
        {
            Plugin.LogErrorIfDebug("Error while setting field 'prevColorState', this is probably harmless.\n" + e);
        }

        try
        {
            MethodInfo colorStatesInfo = physGrabberType.GetMethod(nameof(PhysGrabber.ColorStates), BindingFlags.Instance | BindingFlags.NonPublic);
            if (colorStatesInfo != null) colorStatesInfo.Invoke(player.physGrabber, null);
        }
        catch (Exception e)
        {
            Plugin.LogErrorIfDebug("Error while calling method 'ColorStates', this is probably harmless.\n" + e);
        }
    }
    
    private static Color GetGrabberCosmeticMaterial(PlayerCosmetics cosmetics, Color fallback)
    {
        int colourId = cosmetics.colorsEquipped[(int) SemiFunc.CosmeticType.GrabberMesh];
        if (colourId < 0 || colourId >= MetaManager.instance.colors.Count)
        {
            Plugin.LogWarning($"[CustomGrabBeamColour] colourId {colourId} somehow out of range 0..{MetaManager.instance.colors.Count - 1}");
            return fallback;
        }
        return MetaManager.instance.colors[colourId].color;
    }

    // gets the body colour of the player this beam belongs to, will return the fallback colour if no body colour is found
    public Color GetGrabberCosmeticColour(Color fallbackColour)
    {
        return GetGrabberCosmeticMaterial(player.playerCosmetics, fallbackColour);
    }

    public static Color GetLocalGrabberCosmeticColour(Color fallbackColour)
    {
        return GetGrabberCosmeticMaterial(PlayerAvatar.instance.playerCosmetics, fallbackColour);
    }
}