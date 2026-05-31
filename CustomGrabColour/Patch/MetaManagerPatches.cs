using CustomGrabColour.PlayerGrabBeam;
using HarmonyLib;

namespace CustomGrabColour.Patch;

[HarmonyPatch(typeof(MetaManager))]
internal abstract class MetaManagerPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(MetaManager.CosmeticColorSet))]
    public static void CosmeticColorSetPostfix(MetaManager __instance, int _index, int _colorID)
    {
        if(_index != (int) SemiFunc.CosmeticType.GrabberMesh) return;
        if(PlayerAvatar.instance == null) return;
        GrabBeamUtil.TrySendBeamColourUpdateForAllBeams(PlayerAvatar.instance);
    }
}