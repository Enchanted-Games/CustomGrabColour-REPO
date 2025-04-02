
using CustomGrabColour;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static GrabBeamColourSettings;

// handles local and other players grab beam colours
public class CustomGrabBeamColour : MonoBehaviour, IPunObservable
{
    internal static GrabBeamColourSettings LocalNeutralColour;
    internal static GrabBeamColourSettings LocalHealingColour;
    internal static GrabBeamColourSettings LocalRotatingColour;

    internal GrabBeamColourSettings currentNeutralColour;
    internal GrabBeamColourSettings currentHealingColour;
    internal GrabBeamColourSettings currentRotatingColour;

    public PlayerAvatar player;
    internal Material bodyMaterialInternal;
    public Material BodyMaterial
    {
        set { bodyMaterialInternal = value; }
        get {
            if (bodyMaterialInternal == null)
            {
                try
                {
                    FieldInfo grabBeamActiveField = player.playerHealth.GetType().GetField("bodyMaterial", BindingFlags.Instance | BindingFlags.NonPublic);
                    bodyMaterialInternal = (Material)grabBeamActiveField.GetValue(player.playerHealth);
                }
                catch (Exception)
                {
                    Plugin.LogMessageIfDebug("Failed to get value of PlayerHealth bodyMaterial field");
                    return null;
                }
                return bodyMaterialInternal;
            } else
            {
                return bodyMaterialInternal;
            }
        }
    }

    public static GrabBeamColourSettings LocalBeamColour
    {
        set
        {
            switch (value.beamType)
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
            }
        }
        get { throw new NotImplementedException("Tried to get field LocalBeamColour, call GetLocalSettingsForBeamType instead"); }
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
            switch (value.beamType)
            {
                case BeamType.Neutral:
                    {
                        currentNeutralColour = value; break;
                    }
                case BeamType.Heal:
                    {
                        currentHealingColour = value; break;
                    }
                case BeamType.Rotate:
                    {
                        currentRotatingColour = value; break;
                    }
            }
        }
        get { throw new NotImplementedException("Tried to get field CurrentBeamColour, call GetSettingsForBeamType instead"); }
    }
    public GrabBeamColourSettings GetSettingsForBeamType(BeamType beamType)
    {
        switch (beamType)
        {
            case BeamType.Heal:
                {
                    return currentNeutralColour;
                }
            case BeamType.Rotate:
                {
                    return currentHealingColour;
                }
            default:
                {
                    return currentRotatingColour;
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
        CustomGrabColourConfig.SaveColour(LocalHealingColour);
        CustomGrabColourConfig.SaveColour(LocalRotatingColour);
    }

    public static void ResetBeamColours()
    {
        LocalNeutralColour = new GrabBeamColourSettings(CustomGrabColourConfig.NeutralDefaultColour, (bool)CustomGrabColourConfig.neutralGrabBeam.matchSkin.DefaultValue, BeamType.Neutral);
        LocalHealingColour = new GrabBeamColourSettings(CustomGrabColourConfig.HealingDefaultColour, (bool)CustomGrabColourConfig.healingGrabBeam.matchSkin.DefaultValue, BeamType.Heal);
        LocalRotatingColour = new GrabBeamColourSettings(CustomGrabColourConfig.RotatingDefaultColour, (bool)CustomGrabColourConfig.rotatingGrabBeam.matchSkin.DefaultValue, BeamType.Rotate);
        UpdateBeamColourForAllBeams();
    }

    public static void UpdateBeamColour(GrabBeamColourSettings newColour)
    {
        newColour.a = Mathf.Clamp(newColour.a, 0f, CustomGrabColourConfig.MaxOpacity);
        LocalBeamColour = newColour;
        UpdateBeamColour(newColour.beamType);
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
        Plugin.LogMessageIfDebug("SetBeamColourRPC called with values: r:" + newBeamColour.r + ", g:" + newBeamColour.g + ", b:" + newBeamColour.b + ", a:" + newBeamColour.a + ", matchSkin:" + newBeamColour.matchSkin + ", beamType:" + newBeamColour.beamType);

        newBeamColour.a = Mathf.Clamp(newBeamColour.a, 0f, CustomGrabColourConfig.MaxOpacity);

        CurrentBeamColour = newBeamColour;

        // invoke ColorStates method to make sure the beam colour updates properly
        Type physGrabberType = player.physGrabber.GetType();

        try
        {
            FieldInfo colorStatesField = physGrabberType.GetField("prevColorState", BindingFlags.Instance | BindingFlags.NonPublic);
            colorStatesField.SetValue(player.physGrabber, -1);
        }
        catch (Exception e)
        {
            Plugin.LogErrorIfDebug("Error while setting field 'prevColorState', player beam colour might not update properly.\n" + e);
        }

        try
        {
            MethodInfo colorStatesInfo = physGrabberType.GetMethod("ColorStates", BindingFlags.Instance | BindingFlags.NonPublic);
            colorStatesInfo.Invoke(player.physGrabber, null);
        }
        catch (Exception e)
        {
            Plugin.LogErrorIfDebug("Error while calling method 'ColorStates', player beam colour might not update properly.\n" + e);
        }
    }

    // gets the body colour of the player this beam belongs to, will return the fallback colour if no body colour is found
    public Color GetBodyColour(Color fallbackColour)
    {
        Color bodyColour = BodyMaterial.GetColor(Shader.PropertyToID("_AlbedoColor"));
        if (bodyColour == null)
        {
            return fallbackColour;
        }
        return bodyColour;
    }

    public static Color GetLocalBodyColour(Color fallbackColour)
    {
        Color bodyColour;
        try
        {
            FieldInfo grabBeamActiveField = PlayerAvatar.instance.playerHealth.GetType().GetField("bodyMaterial", BindingFlags.Instance | BindingFlags.NonPublic);
            Material bodyMat = (Material)grabBeamActiveField.GetValue(PlayerAvatar.instance.playerHealth);
            bodyColour = bodyMat.GetColor(Shader.PropertyToID("_AlbedoColor"));
        }
        catch (Exception)
        {
            Plugin.LogMessageIfDebug("Failed to get value of PlayerHealth bodyMaterial field in GetLocalBodyColour");
            return fallbackColour;
        }
        if (bodyColour == null)
        {
            return fallbackColour;
        }
        return bodyColour;
    }
}