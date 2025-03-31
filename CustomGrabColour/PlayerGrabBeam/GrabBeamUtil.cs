using System;

class GrabBeamUtil
{
    public static void TrySendBeamColourUpdate(PlayerAvatar player, GrabBeamColourSettings.BeamType beamType)
    {
        // if player has custom beam colour update it
        CustomGrabBeamColour customGrabBeamColourComponent = player.GetComponent<CustomGrabBeamColour>();
        if (customGrabBeamColourComponent != null)
        {
            CustomGrabBeamColour.UpdateBeamColour(beamType);
        }
    }

    public static void TrySendBeamColourUpdateForAllBeams(PlayerAvatar player)
    {
        CustomGrabBeamColour customGrabBeamColourComponent = player.GetComponent<CustomGrabBeamColour>();
        foreach (GrabBeamColourSettings.BeamType beamType in Enum.GetValues(typeof(GrabBeamColourSettings.BeamType)))
        {
            TrySendBeamColourUpdate(player, beamType);
        }
    }
}