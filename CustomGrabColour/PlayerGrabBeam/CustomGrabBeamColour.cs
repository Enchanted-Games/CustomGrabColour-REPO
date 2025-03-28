
using CustomGrabColour;
using Photon.Pun;
using System;
using System.Reflection;
using UnityEngine;

// handles local and other players grab beam colours
public class CustomGrabBeamColour : MonoBehaviour, IPunObservable
{
	public static Color LocalColour;
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

	public static void UpdateBeamColour(Color newColour)
	{
		LocalColour = newColour;
        UpdateBeamColour();
	}
	public static void UpdateBeamColour()
	{
		if (!GameManager.Multiplayer())
		{
			PlayerAvatar.instance.GetComponent<CustomGrabBeamColour>().SetBeamColourRPC(LocalColour.r, LocalColour.g, LocalColour.b, LocalColour.a);
		}
		else
		{
			PlayerAvatar.instance.photonView.RPC("SetBeamColourRPC", RpcTarget.AllBuffered, LocalColour.r, LocalColour.g, LocalColour.b, LocalColour.a);
		}

		CustomGrabColourConfig.SaveColour(LocalColour);
	}

	[PunRPC]
	public void SetBeamColourRPC(float r, float g, float b, float a)
    {
        Plugin.LogMessageIfDebug("SetBeamColourRPC called with values: r:" + r + ", g:" + g + " b:" + b + " a:" + a);
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
            Plugin.LogError("Error while setting field 'prevColorState', player beam colour might not update properly.\n" + e.Message);
        }

		try
		{
			MethodInfo colorStatesInfo = physGrabberType.GetMethod("ColorStates", BindingFlags.Instance | BindingFlags.NonPublic);
			colorStatesInfo.Invoke(player.physGrabber, null);
        }
        catch (Exception e)
        {
            Plugin.LogError("Error while calling method field 'ColorStates', player beam colour might not update properly.\n" + e.Message);
        }
    }
}