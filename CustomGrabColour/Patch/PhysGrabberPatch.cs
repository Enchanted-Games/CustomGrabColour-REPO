using HarmonyLib;
using UnityEngine;
using CustomGrabColour;
using Photon.Realtime;

[HarmonyPatch(typeof(PhysGrabber))]
[HarmonyPatch("ColorStateSetColor")]
class PhysGrabberPatch
{
    static AccessTools.FieldRef<PhysGrabber, int> prevColorStateRef =
        AccessTools.FieldRefAccess<PhysGrabber, int>("prevColorState");

    static void Prefix(PhysGrabber __instance, ref Color mainColor, ref Color emissionColor)
    {
        int currentColourState = prevColorStateRef(__instance);

        if (currentColourState != 0)
        {
            return;
        }

        CustomGrabBeamColour grabBeamColour = __instance.playerAvatar.gameObject.GetComponent<CustomGrabBeamColour>();
        if (!grabBeamColour)
        {
            Plugin.LogMessageIfDebug("Player has no custom beam colour");
            return;
        }

        Color customColour = grabBeamColour.currentBeamColour;

        mainColor.r = customColour.r;
        mainColor.g = customColour.g;
        mainColor.b = customColour.b;
        mainColor.a = customColour.a;
        emissionColor.r = customColour.r;
        emissionColor.g = customColour.g;
        emissionColor.b = customColour.b;
        Plugin.LogMessageIfDebug("Set player beam to: (" + mainColor.r + ", " + mainColor.g + ", " + mainColor.b + ", " + mainColor.a + "). colour state is " + currentColourState);
        return;
    }

    static void Postfix() { }
}