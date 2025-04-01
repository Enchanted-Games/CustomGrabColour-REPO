using HarmonyLib;
using UnityEngine;
using CustomGrabColour;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;


class PhysGrabberPatches
{
    [HarmonyPatch(typeof(PhysGrabber))]
    [HarmonyPatch("ColorStateSetColor")]
    class PhysGrabber_ColorState_Patch
    {
        static AccessTools.FieldRef<PhysGrabber, int> prevColorStateRef =
            AccessTools.FieldRefAccess<PhysGrabber, int>("prevColorState");
        static AccessTools.FieldRef<PhysGrabber, List<GameObject>> physGrabPointVisualGridObjectsRef =
            AccessTools.FieldRefAccess<PhysGrabber, List<GameObject>>("physGrabPointVisualGridObjects");

        static void Prefix(PhysGrabber __instance, ref Color mainColor, ref Color emissionColor)
        {
            int currentColourState = prevColorStateRef(__instance);

            if (mainColor == null || emissionColor == null)
            {
                return;
            }

            CustomGrabBeamColour grabBeamColour = __instance.playerAvatar.gameObject.GetComponent<CustomGrabBeamColour>();
            if (!grabBeamColour)
            {
                Plugin.LogMessageIfDebug("Player has no custom beam colour");
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
                return;
            }

            Color customColour;
            if(grabBeamSettings.matchSkin)
            {
                customColour = grabBeamColour.BodyMaterial.GetColor(Shader.PropertyToID("_AlbedoColor"));
                if (customColour == null)
                {
                    customColour = grabBeamSettings.colour;
                }
                customColour.a = grabBeamSettings.colour.a;
            }
            else
            {
                customColour = grabBeamSettings.colour;
            }

            mainColor.r = customColour.r / 1.7f;
            mainColor.g = customColour.g / 1.7f; // TODO: probably find a better way to fix this
            mainColor.b = customColour.b / 1.7f;
            mainColor.a = customColour.a;
            //emissionColor.r = customColour.r / 1.7f;
            //emissionColor.g = customColour.g / 1.7f;
            //emissionColor.b = customColour.b / 1.7f;
            //emissionColor.a = 0.1f;
            emissionColor = Color.black;
            emissionColor.a = 0;

            Plugin.LogMessageIfDebug("Set player beam to: (" + mainColor.r + ", " + mainColor.g + ", " + mainColor.b + ", " + mainColor.a + "). colour state is " + currentColourState);

            List<GameObject> physGrabPointVisualGridObjects = physGrabPointVisualGridObjectsRef(__instance);

            for (int i = 0; i < physGrabPointVisualGridObjects.Count; i++)
            {
                Material gridMeshMaterial = physGrabPointVisualGridObjects[i].GetComponent<MeshRenderer>().material;
                if(gridMeshMaterial)
                {
                    Plugin.LogMessageIfDebug("Set grid mesh to: (" + mainColor.r + ", " + mainColor.g + ", " + mainColor.b + ", " + mainColor.a + ")");

                    gridMeshMaterial.color = customColour;
                    gridMeshMaterial.SetColor("_EmissionColor", customColour);
                }
            }

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