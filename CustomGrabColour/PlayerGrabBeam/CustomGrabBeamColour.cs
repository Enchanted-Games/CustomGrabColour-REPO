
using CustomGrabColour;
using Photon.Pun;
using System;
using System.Reflection;
using UnityEngine;

// handles local and other players grab beam colours
public class CustomGrabBeamColour : MonoBehaviour, IPunObservable
{
	public static Color LocalColour = new Color(1f, 0.1856f, 0f);
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
		Plugin.Instance.PluginLogger.LogInfo(LocalColour);
		if (!GameManager.Multiplayer())
		{
			PlayerAvatar.instance.GetComponent<CustomGrabBeamColour>().SetBeamColourRPC(LocalColour.r, LocalColour.g, LocalColour.b);
		}
		else
		{
			PlayerAvatar.instance.photonView.RPC("SetBeamColourRPC", RpcTarget.AllBuffered, LocalColour.r, LocalColour.g, LocalColour.b);
		}

		ES3Settings es3Settings = new ES3Settings("ModSettingsData.es3", [ES3.Location.File]);
		ES3.Save<string>("PlayerBodyColorR", LocalColour.r.ToString(), es3Settings);
		ES3.Save<string>("PlayerBodyColorG", LocalColour.g.ToString(), es3Settings);
		ES3.Save<string>("PlayerBodyColorB", LocalColour.b.ToString(), es3Settings);
	}

	[PunRPC]
	public void SetBeamColourRPC(float r, float g, float b)
    {
        Plugin.Instance.PluginLogger.LogInfo("colour from RPC: " + r + ", " + g + ", " + b);
        currentBeamColour = new Color(r, g, b);

		// invoke ColorStates method to make sure the beam colour updates properly
        Type physGrabberType = player.physGrabber.GetType();

		try
		{
			FieldInfo colorStatesField = physGrabberType.GetField("prevColorState", BindingFlags.Instance | BindingFlags.NonPublic);
			colorStatesField.SetValue(player.physGrabber, -1);
		}
		catch (Exception e)
		{
            Plugin.Instance.PluginLogger.LogError("Error while setting field 'prevColorState', player beam colour might not update properly.\n" + e.Message);
        }

		try
		{
			MethodInfo colorStatesInfo = physGrabberType.GetMethod("ColorStates", BindingFlags.Instance | BindingFlags.NonPublic);
			colorStatesInfo.Invoke(player.physGrabber, null);
        }
        catch (Exception e)
        {
            Plugin.Instance.PluginLogger.LogError("Error while calling method field 'ColorStates', player beam colour might not update properly.\n" + e.Message);
        }
    }
}