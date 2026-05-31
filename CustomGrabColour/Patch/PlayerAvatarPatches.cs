using CustomGrabColour.PlayerGrabBeam;
using HarmonyLib;

namespace CustomGrabColour.Patch;

[HarmonyPatch(typeof(PlayerAvatar))]
internal abstract class PlayerAvatarPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerAvatar.Awake))]
    public static void AwakePostfix(PlayerAvatar __instance)
    {
        // add grab beam colour component if not already present
        if(__instance.GetComponent<CustomGrabBeamColour>() != null) return;
        __instance.gameObject.AddComponent<CustomGrabBeamColour>();
    }
}