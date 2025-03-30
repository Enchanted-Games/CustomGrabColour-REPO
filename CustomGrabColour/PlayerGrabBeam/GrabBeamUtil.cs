class GrabBeamUtil
{
    public static void TrySendBeamColourUpdate(PlayerAvatar player)
    {
        // if player has custom beam colour update it
        if (player.GetComponent<CustomGrabBeamColour>()?.currentBeamColour != null)
        {
            CustomGrabBeamColour.UpdateBeamColour();
        }
    }
}