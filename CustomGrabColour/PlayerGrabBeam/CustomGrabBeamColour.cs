
using CustomGrabColour;
using Photon.Pun;
using System;
using System.Reflection;
using UnityEngine;

// handles local and other players grab beam colours
public class CustomGrabBeamColour : MonoBehaviour, IPunObservable
{
	public static GrabBeamColourSettings LocalColour;
    public Color currentBeamColour;
    public PlayerAvatar player;

    void Awake()
	{
		player = gameObject.GetComponent<PlayerAvatar>();
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		throw new NotImplementedException();
	}

	public static void SaveLocalColourToConfig()
	{
        CustomGrabColourConfig.SaveColour(LocalColour);
    }

    public static void ResetBeamColour()
	{
		LocalColour = new GrabBeamColourSettings(CustomGrabColourConfig.DefaultColor, false, GrabBeamColourSettings.BeamType.Neutral); // TODO: temp
		UpdateBeamColour();
	}

    public static void UpdateBeamColour(Color newColour)
    {
        newColour.a = Mathf.Clamp(newColour.a, 0f, CustomGrabColourConfig.MaxOpacity);
        LocalColour = new GrabBeamColourSettings(CustomGrabColourConfig.DefaultColor, false, GrabBeamColourSettings.BeamType.Neutral); // TODO: temp
        UpdateBeamColour();
	}
	public static void UpdateBeamColour()
    {
        if (GameManager.Multiplayer())
		{
			PlayerAvatar.instance.photonView.RPC("SetBeamColourRPC", RpcTarget.AllBuffered, GrabBeamColourSettings.ToRPCBuffer(LocalColour));
		} else
        {
            PlayerAvatar.instance.GetComponent<CustomGrabBeamColour>().SetBeamColourRPC(GrabBeamColourSettings.ToRPCBuffer(LocalColour));
        }
	}

	[PunRPC]
	public void SetBeamColourRPC(object[] beamColourParts)
    {
		GrabBeamColourSettings beamColour = GrabBeamColourSettings.FromRPCBuffer(beamColourParts);
		float r = beamColour.colour.r;
		float g = beamColour.colour.g;
		float b = beamColour.colour.b;
		float a = beamColour.colour.a;

        Plugin.LogMessageIfDebug("SetBeamColourRPC called with values: r:" + r + ", g:" + g + ", b:" + b + ", a:" + a + ", matchSkin:" + beamColour.matchSkin + ", beamType:" + beamColour.beamType);
		a = Mathf.Clamp(a, 0f, CustomGrabColourConfig.MaxOpacity);
        currentBeamColour = new Color(r, g, b, a);

		// invoke ColorStates method to make sure the beam colour updates properly
        Type physGrabberType = player.physGrabber.GetType();

		try
		{
			FieldInfo colorStatesField = physGrabberType.GetField("prevColorState", BindingFlags.Instance | BindingFlags.NonPublic);
 			colorStatesField.SetValue(player.physGrabber, -1);
		}
		catch (Exception e)
		{
            Plugin.LogError("Error while setting field 'prevColorState', player beam colour might not update properly.\n" + e);
        }

		try
		{
			MethodInfo colorStatesInfo = physGrabberType.GetMethod("ColorStates", BindingFlags.Instance | BindingFlags.NonPublic);
			colorStatesInfo.Invoke(player.physGrabber, null);
        }
        catch (Exception e)
        {
            Plugin.LogError("Error while calling method 'ColorStates', player beam colour might not update properly.\n" + e);
        }
    }
}