
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

	public static void SaveLocalColourToConfig()
	{
        CustomGrabColourConfig.SaveColour(LocalColour);
    }

    public static void ResetBeamColour()
	{
		LocalColour = CustomGrabColourConfig.DefaultColor;
		UpdateBeamColour();
	}

    public static void UpdateBeamColour(Color newColour)
    {
        newColour.a = Mathf.Clamp(newColour.a, 0f, CustomGrabColourConfig.MaxOpacity);
        LocalColour = newColour;
        UpdateBeamColour();
	}
	public static void UpdateBeamColour()
    {
        if (GameManager.Multiplayer())
		{
			PlayerAvatar.instance.photonView.RPC("SetBeamColourRPC", RpcTarget.AllBuffered, LocalColour.r, LocalColour.g, LocalColour.b, LocalColour.a);
		} else
        {
            PlayerAvatar.instance.GetComponent<CustomGrabBeamColour>().SetBeamColourRPC(LocalColour.r, LocalColour.g, LocalColour.b, LocalColour.a);
        }
	}

	[PunRPC]
	public void SetBeamColourRPC(float r, float g, float b, float a)
    {
        Plugin.LogMessageIfDebug("SetBeamColourRPC called with values: r:" + r + ", g:" + g + " b:" + b + " a:" + a);
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