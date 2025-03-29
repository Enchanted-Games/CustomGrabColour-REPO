using HarmonyLib;

class PlayerAvatarPatches
{
    [HarmonyPatch(typeof(PlayerAvatar))]
    [HarmonyPatch("Awake")]
    class PlayerAvatar_Awake_Patch
    {
        public static void Postfix(PlayerAvatar __instance)
        {
            // add custom grab beam colour component
            CustomGrabBeamColour moddedColorPlayerAvatar = __instance.gameObject.AddComponent<CustomGrabBeamColour>();
        }
    }

    [HarmonyPatch(typeof(PlayerAvatar))]
    [HarmonyPatch("PlayerAvatarSetColor")]
    class PlayerAvatar_PlayerAvatarSetColor_Patch
    {
        public static void Postfix(PlayerAvatar __instance, int colorIndex)
        {
            // if player has custom beam colour update it
            if (__instance.GetComponent<CustomGrabBeamColour>()?.currentBeamColour != null)
            {
                CustomGrabBeamColour.UpdateBeamColour();
            }
        }
    }
}