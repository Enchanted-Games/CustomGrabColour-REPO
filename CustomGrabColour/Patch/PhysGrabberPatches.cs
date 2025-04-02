using HarmonyLib;
using UnityEngine;
using CustomGrabColour;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;


class PhysGrabberPatches
{
    static AccessTools.FieldRef<PhysGrabber, List<GameObject>> physGrabPointVisualGridObjectsRef =
        AccessTools.FieldRefAccess<PhysGrabber, List<GameObject>>("physGrabPointVisualGridObjects");

    [HarmonyPatch(typeof(PhysGrabber))]
    [HarmonyPatch("ColorStateSetColor")]
    class PhysGrabber_ColorState_Patch
    {
        static AccessTools.FieldRef<PhysGrabber, int> prevColorStateRef =
            AccessTools.FieldRefAccess<PhysGrabber, int>("prevColorState");

        static void Prefix(PhysGrabber __instance, ref Color mainColor, ref Color emissionColor)
        {
            int currentColourState = prevColorStateRef(__instance);

            if (mainColor == null || emissionColor == null)
            {
                ResetRotateBeamGridsColour(__instance);
                return;
            }

            CustomGrabBeamColour grabBeamColour = __instance.playerAvatar.gameObject.GetComponent<CustomGrabBeamColour>();
            if (!grabBeamColour)
            {
                Plugin.LogMessageIfDebug("Player has no custom beam colour");
                ResetRotateBeamGridsColour(__instance);
                return;
            }

            GrabBeamColourSettings grabBeamSettings;
            if (currentColourState == 0)
            {
                grabBeamSettings = grabBeamColour.currentNeutralColour;
            }
            else if (currentColourState == 1)
            {
                grabBeamSettings = grabBeamColour.currentHealingColour;
            }
            else if (currentColourState == 2)
            {
                grabBeamSettings = grabBeamColour.currentRotatingColour;
            }
            else
            {
                ResetRotateBeamGridsColour(__instance);
                return;
            }

            Color customColour;
            if(grabBeamSettings.matchSkin)
            {
                customColour = grabBeamColour.GetBodyColour(grabBeamSettings.colour);
                customColour.a = grabBeamSettings.colour.a;
            }
            else
            {
                customColour = grabBeamSettings.colour;
            }

            mainColor.r = customColour.r / 3.5f; // TODO: probably find a better way to fix this
            mainColor.g = customColour.g / 3.5f;
            mainColor.b = customColour.b / 3.5f;
            mainColor.a = customColour.a;
            emissionColor.r = customColour.r / 3.5f;
            emissionColor.g = customColour.g / 3.5f;
            emissionColor.b = customColour.b / 3.5f;
            emissionColor.a = 0.05f;

            Plugin.LogMessageIfDebug("Set player beam to: (" + mainColor.r + ", " + mainColor.g + ", " + mainColor.b + ", " + mainColor.a + "). colour state is " + currentColourState);

            SetRotateBeamGridsColour(__instance, customColour);

            return;
        }
    }
    internal static void ResetRotateBeamGridsColour(PhysGrabber __instance)
    {
        SetRotateBeamGridsColour(__instance, CustomGrabColourConfig.RotatingDefaultColour);
    }
    internal static void SetRotateBeamGridsColour(PhysGrabber __instance, Color gridColour)
    {
        List<GameObject> physGrabPointVisualGridObjects = physGrabPointVisualGridObjectsRef(__instance);

        for (int i = 0; i < physGrabPointVisualGridObjects.Count; i++)
        {
            Material gridMeshMaterial = physGrabPointVisualGridObjects[i].GetComponent<MeshRenderer>().material;
            if (gridMeshMaterial)
            {
                Plugin.LogMessageIfDebug("Set grid mesh to: (" + gridColour.r + ", " + gridColour.g + ", " + gridColour.b + ", " + gridColour.a + ")");

                gridMeshMaterial.color = gridColour;
                gridMeshMaterial.SetColor("_EmissionColor", gridColour);
            }
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
            GrabBeamUtil.TrySendBeamColourUpdateForAllBeams(__instance.playerAvatar);
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
            GrabBeamUtil.TrySendBeamColourUpdateForAllBeams(__instance.playerAvatar);
        }
    }
}