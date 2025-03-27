using HarmonyLib;
using UnityEngine;
using CustomGrabColour;

[HarmonyPatch(typeof(PhysGrabBeam))]
[HarmonyPatch("ScrollTexture")] // if possible use nameof() here
class Patch01
{
    // testing how changing line colour works
    static AccessTools.FieldRef<PhysGrabBeam, Vector2> scrollSpeedRef =
        AccessTools.FieldRefAccess<PhysGrabBeam, Vector2>("scrollSpeed");

    static AccessTools.FieldRef<PhysGrabBeam, Material> lineMaterialRef =
        AccessTools.FieldRefAccess<PhysGrabBeam, Material>("lineMaterial");

    static void Prefix(PhysGrabBeam __instance)
    {
        BepInEx.Logging.ManualLogSource pluginLogger = Plugin.Instance.PluginLogger;
        Material mat = lineMaterialRef(__instance);
        mat.color = Color.white;
        return;
    }

    static void Postfix() { }
}