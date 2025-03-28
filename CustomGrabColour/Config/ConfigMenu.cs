using CustomGrabColour;
using MenuLib;
using MenuLib.MonoBehaviors;
using MenuLib.Structs;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem.PlaybackState;

class ConfigMenu
{
    public static void Init()
    {
        MenuAPI.AddElementToEscapeMenu(parent =>
        {
            //int amountOfChildren = parent.GetChildCount();
            //for (int i = 0; i < amountOfChildren; i++)
            //{
            //    Plugin.LogMessage("Child [" + i + "]: " + parent.GetChild(i).name);
            //}
            var repoButton = MenuAPI.CreateREPOButton("Change Grab Colour", () => BuildAndOpenPopup(), parent, localPosition: new Vector2(28.3f, 350.0f));
        });
    }

    private static void BuildAndOpenPopup()
    {
        var changeGrabColourPage = MenuAPI.CreateREPOPopupPage("Change Grab Beam Colour", REPOPopupPage.PresetSide.Left, false, pageDimmerVisibility: true, spacing: 1.5f);

        changeGrabColourPage.AddElement(parent =>
        {
            var redSlider = createColourSlider(
                "Red",
                "",
                f => CustomGrabBeamColour.LocalColour.r = f,
                CustomGrabBeamColour.LocalColour.r,
                parent,
                3
            );
            var greenSlider = createColourSlider(
                "Green",
                "",
                f => CustomGrabBeamColour.LocalColour.g = f,
                CustomGrabBeamColour.LocalColour.g,
                parent,
                2
            );
            var blueSlider = createColourSlider(
                "Blue",
                "",
                f => CustomGrabBeamColour.LocalColour.b = f,
                CustomGrabBeamColour.LocalColour.b,
                parent,
                1
            );
            var opacitySlider = createColourSlider(
                "Opacity",
                "",
                f => CustomGrabBeamColour.LocalColour.a = f,
                CustomGrabBeamColour.LocalColour.a,
                parent,
                0
            );

            var closeButton = MenuAPI.CreateREPOButton("Done", () => {
                    changeGrabColourPage.ClosePage(true);
                    CustomGrabBeamColour.UpdateBeamColour();
                },
                parent,
                new Vector2(70f, 30f)
            );
        });

        changeGrabColourPage.OpenPage(false);
    }

    public static REPOSlider createColourSlider(string name, string desc, Action<float> onChange, float initial, Transform parent, int offset)
    {
        return MenuAPI.CreateREPOSlider(
            name,
            desc,
            onChange,
            parent,
            new Vector2(60f, 165.0f + (35 * offset)),
            0,
            1,
            2,
            initial,
            "",
            "",
            REPOSlider.BarBehavior.UpdateWithValue
        );
    }
}