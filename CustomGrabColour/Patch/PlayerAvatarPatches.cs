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
            __instance.gameObject.AddComponent<CustomGrabBeamColour>();
        }
    }

    [HarmonyPatch(typeof(PlayerAvatar))]
    [HarmonyPatch("PlayerAvatarSetColor")]
    class PlayerAvatar_PlayerAvatarSetColor_Patch
    {
        public static void Postfix(PlayerAvatar __instance, int colorIndex)
        {
            GrabBeamUtil.TrySendBeamColourUpdate(__instance);
        }
    }
}