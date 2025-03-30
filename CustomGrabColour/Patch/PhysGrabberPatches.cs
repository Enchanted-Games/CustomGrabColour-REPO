using HarmonyLib;
using UnityEngine;
using CustomGrabColour;
using Photon.Realtime;
using System;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;


class PhysGrabberPatches
{
    [HarmonyPatch(typeof(PhysGrabber))]
    [HarmonyPatch("ColorStateSetColor")]
    class PhysGrabber_ColorState_Patch
    {
        static AccessTools.FieldRef<PhysGrabber, int> prevColorStateRef =
            AccessTools.FieldRefAccess<PhysGrabber, int>("prevColorState");

        static void Prefix(PhysGrabber __instance, ref Color mainColor, ref Color emissionColor)
        {
            int currentColourState = prevColorStateRef(__instance);

            if (currentColourState != 0 || mainColor == null || emissionColor == null)
            {
                return;
            }

            CustomGrabBeamColour grabBeamColour = __instance.playerAvatar.gameObject.GetComponent<CustomGrabBeamColour>();
            if (!grabBeamColour)
            {
                Plugin.LogMessageIfDebug("Player has no custom beam colour");
                return;
            }

            Color customColour;
            if (currentColourState == 0)
            {
                customColour = grabBeamColour.currentBeamColour;
            }
            else
            {
                return;
            }

            mainColor.r = customColour.r;
            mainColor.g = customColour.g;
            mainColor.b = customColour.b;
            mainColor.a = customColour.a;
            emissionColor.r = customColour.r;
            emissionColor.g = customColour.g;
            emissionColor.b = customColour.b;
            emissionColor.a = customColour.a;
            Plugin.LogMessageIfDebug("Set player beam to: (" + mainColor.r + ", " + mainColor.g + ", " + mainColor.b + ", " + mainColor.a + "). colour state is " + currentColourState);
            return;
        }
    }

    [HarmonyPatch(typeof(PhysGrabber))]
    [HarmonyPatch("ChangeBeamAlpha")]
    class PhysGrabber_ChangeBeamAlpha_Patch
    {
        static bool Prefix()
        {
            // cancel changing the beam alpha
            return false;
        }
    }

    [HarmonyPatch(typeof(PhysGrabber))]
    [HarmonyPatch("PhysGrabBeamActivateRPC")]
    class PhysGrabber_PhysGrabBeamActivateRPC_Patch
    {
        static void Postfix(PhysGrabber __instance)
        {
            // if player has custom beam colour update it when they activate their beam
            GrabBeamUtil.TrySendBeamColourUpdate(__instance.playerAvatar);
        }
    }

    [HarmonyPatch(typeof(PhysGrabber))]
    [HarmonyPatch("PhysGrabBeamActivate")]
    class PhysGrabber_PhysGrabBeamActivate_Patch
    {
        static async void Prefix(PhysGrabber __instance)
        {
            bool grabBeamActive = true;
            try
            {
                FieldInfo grabBeamActiveField = __instance.GetType().GetField("physGrabBeamActive", BindingFlags.Instance | BindingFlags.NonPublic);
                grabBeamActive = (bool)grabBeamActiveField.GetValue(__instance);
            }
            catch (Exception) {
                Plugin.LogMessageIfDebug("Failed to get value of PhysGrabber physGrabBeamActive field");
            }

            if (grabBeamActive) return;

            await Task.Delay(100);

            Plugin.LogMessageIfDebug("PhysGrabBeamActivate called");

            // if player has custom beam colour update it when they activate their beam
            GrabBeamUtil.TrySendBeamColourUpdate(__instance.playerAvatar);
        }
    }
}