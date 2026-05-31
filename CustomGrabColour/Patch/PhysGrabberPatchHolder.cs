using HarmonyLib;
using UnityEngine;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using CustomGrabColour.Config;
using CustomGrabColour.PlayerGrabBeam;

namespace CustomGrabColour.Patch;

internal abstract class PhysGrabberPatchHolder
{
    private static readonly AccessTools.FieldRef<PhysGrabber, List<GameObject>> PhysGrabPointVisualGridObjectsRef =
        AccessTools.FieldRefAccess<PhysGrabber, List<GameObject>>(nameof(PhysGrabber.physGrabPointVisualGridObjects));
    private static readonly AccessTools.FieldRef<PhysGrabber, int> PrevColorStateRef =
        AccessTools.FieldRefAccess<PhysGrabber, int>(nameof(PhysGrabber.prevColorState));

    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    [HarmonyPatch(typeof(PhysGrabber))]
    internal abstract class PhysGrabberPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(PhysGrabber.ColorStateSetColor))]
        static void ColorStateSetColorPrefix(PhysGrabber __instance, ref Color mainColor, ref Color emissionColor)
        {
            int currentColourState = PrevColorStateRef(__instance);

            CustomGrabBeamColour grabBeamColour = GetCurrentGrabBeamColour(__instance);
            if (!grabBeamColour)
            {
                Plugin.LogMessageIfDebug("Player has no custom beam colour");
                ResetRotateBeamGridsColour(__instance);
                return;
            }

            GrabBeamColourSettings grabBeamSettings;
            if (currentColourState == (int) PhysGrabber.ColorState.Orange)
            {
                grabBeamSettings = grabBeamColour.CurrentNeutralColour;
            }
            else if (currentColourState == (int) PhysGrabber.ColorState.Green)
            {
                grabBeamSettings = grabBeamColour.CurrentHealingColour;
            }
            else if (currentColourState == (int) PhysGrabber.ColorState.Purple)
            {
                grabBeamSettings = grabBeamColour.CurrentRotatingColour;
            }
            else if (currentColourState == (int) PhysGrabber.ColorState.Blue)
            {
                grabBeamSettings = grabBeamColour.CurrentClimbingColour;
            }
            else
            {
                ResetRotateBeamGridsColour(__instance);
                return;
            }

            Color customColour;
            if(grabBeamSettings.MatchSkin)
            {
                customColour = grabBeamColour.GetGrabberCosmeticColour(grabBeamSettings.Colour);
                customColour.a = grabBeamSettings.Colour.a;
            }
            else
            {
                customColour = grabBeamSettings.Colour;
            }

            mainColor.r = customColour.r / 3.5f; // TODO: probably find a better way to fix this
            mainColor.g = customColour.g / 4f;
            mainColor.b = customColour.b / 3.5f;
            mainColor.a = customColour.a;
            emissionColor.r = customColour.r / 3.5f;
            emissionColor.g = customColour.g / 4f;
            emissionColor.b = customColour.b / 3.5f;
            emissionColor.a = 0.3f;

            Plugin.LogMessageIfDebug($"Set player '{__instance.playerAvatar.playerName}' beam to: (" + mainColor.r + ", " + mainColor.g + ", " + mainColor.b + ", " + mainColor.a + "). colour state is " + currentColourState);

            SetRotateBeamGridsColour(__instance, customColour);
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(nameof(PhysGrabber.ChangeBeamAlpha))]
        static bool CancelAlphaChange()
        {
            return false;
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(nameof(PhysGrabber.PhysGrabBeamActivate))]
        static void PhysGrabBeamActivatePrefix(PhysGrabber __instance)
        {
            CustomGrabBeamColour grabBeamColour = GetCurrentGrabBeamColour(__instance);
            if(grabBeamColour.SentInitialColourUpdate) return;
            
            bool grabBeamActive;
            try
            {
                FieldInfo? grabBeamActiveField = __instance.GetType().GetField(nameof(PhysGrabber.physGrabBeamActive), BindingFlags.Instance | BindingFlags.NonPublic);
                if (grabBeamActiveField != null)
                {
                    grabBeamActive = (bool) grabBeamActiveField.GetValue(__instance);
                }
                else
                {
                    grabBeamActive = true;
                }
            }
            catch (Exception) {
                Plugin.LogMessageIfDebug("Failed to get value of PhysGrabber physGrabBeamActive field");
                grabBeamActive = true;
            }
    
            if (grabBeamActive) return;
    
            // await Task.Delay(100);
    
            Plugin.LogMessageIfDebug("PhysGrabBeamActivate called");
    
            // if player has custom beam colour update it when they activate their beam
            GrabBeamUtil.TrySendBeamColourUpdateForAllBeams(__instance.playerAvatar);
            grabBeamColour.SentInitialColourUpdate = true;
        }
    }

    private static CustomGrabBeamColour GetCurrentGrabBeamColour(PhysGrabber grabber)
    {
        return grabber.playerAvatar.GetComponent<CustomGrabBeamColour>();
    }

    private static void ResetRotateBeamGridsColour(PhysGrabber __instance)
    {
        SetRotateBeamGridsColour(__instance, CustomGrabColourConfig.RotatingDefaultColour);
    }

    private static void SetRotateBeamGridsColour(PhysGrabber __instance, Color gridColour)
    {
        List<GameObject> physGrabPointVisualGridObjects = PhysGrabPointVisualGridObjectsRef(__instance);

        foreach (var gridMeshObject in physGrabPointVisualGridObjects)
        {
            Material gridMeshMaterial = gridMeshObject.GetComponent<MeshRenderer>().material;
            if (!gridMeshMaterial) continue;
            
            Color col = new Color(
                gridColour.r / 3.5f,
                gridColour.g / 4f,
                gridColour.b / 3.5f,
                gridColour.a
            );
            gridMeshMaterial.color = col;

            Color emission = new Color(
                gridColour.r / 3.5f,
                gridColour.g / 4f,
                gridColour.b / 3.5f,
                0.3f
            );
            gridMeshMaterial.SetColor(EmissionColor, emission);

            Plugin.LogMessageIfDebug("Set grid mesh to: (" + gridMeshMaterial.color.r + ", " + gridMeshMaterial.color.g + ", " + gridMeshMaterial.color.b + ", " + gridMeshMaterial.color.a + ")");
        }
    }
}