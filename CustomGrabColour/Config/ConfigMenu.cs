using CustomGrabColour;
using MenuLib;
using MenuLib.MonoBehaviors;
using System;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

class ConfigMenu
{
    private static Image neutralGrabColourPreviewImage;
    private static GameObject neutralGrabColourPreviewParent;

    private static float horizontalPos = 70f;

    public static void Init()
    {
        MenuAPI.AddElementToMainMenu(parent =>
        {
            // add button to open colour config screen
            var repoButton = MenuAPI.CreateREPOButton("Change Grab Colour", () => OpenPopup(), parent, localPosition: new Vector2(28.3f, 350.0f));
        });
    }

    private static void OpenPopup()
    {
        // build colour config screen
        var changeGrabColourPage = MenuAPI.CreateREPOPopupPage("Change Grab Beam Colour", REPOPopupPage.PresetSide.Left, false, pageDimmerVisibility: true, spacing: 1.5f);

        // colour previews
        changeGrabColourPage.AddElement(parent =>
        {
            var disclaimerLabel = MenuAPI.CreateREPOLabel("Preview may not be 100% accurate ", parent, new Vector2(425f, 155f));
            disclaimerLabel.transform.localScale = new Vector2(0.5f, 0.5f);
            if(!neutralGrabColourPreviewParent)
            {
                CreateBeamPreviewColourRectangle(parent, new Vector2(515f, 225f), out neutralGrabColourPreviewParent, out neutralGrabColourPreviewImage);
            }
        });

        // neutral grab beam colours
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var neutralBeamLabel = MenuAPI.CreateREPOLabel("Neutral Beam Colour", parent, new Vector2(0, getVerticalOffsetForScrollChildren(0)));
            neutralBeamLabel.transform.localScale = new Vector2(0.5f, 0.5f);
            return neutralBeamLabel.rectTransform;
        });
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var redSlider = CreateColourSlider(
                "Red",
                "",
                f =>
                {
                    CustomGrabBeamColour.LocalNeutralColour.r = f;
                    Color col = neutralGrabColourPreviewImage.color;
                    col.r = f;
                    neutralGrabColourPreviewImage.color = col;
                },
                CustomGrabBeamColour.LocalNeutralColour.r,
                1,
                parent,
                new Vector2(0, getVerticalOffsetForScrollChildren(1))
            );
            return redSlider.rectTransform;
        });
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var greenSlider = CreateColourSlider(
                "Green",
                "",
                f =>
                {
                    CustomGrabBeamColour.LocalNeutralColour.g = f;
                    Color col = neutralGrabColourPreviewImage.color;
                    col.g = f;
                    neutralGrabColourPreviewImage.color = col;
                },
                CustomGrabBeamColour.LocalNeutralColour.g,
                1,
                parent,
                new Vector2(0, getVerticalOffsetForScrollChildren(2))
            );
            return greenSlider.rectTransform;
        });
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var blueSlider = CreateColourSlider(
                "Blue",
                "",
                f =>
                {
                    CustomGrabBeamColour.LocalNeutralColour.b = f;
                    Color col = neutralGrabColourPreviewImage.color;
                    col.b = f;
                    neutralGrabColourPreviewImage.color = col;
                },
                CustomGrabBeamColour.LocalNeutralColour.b,
                1,
                parent,
                new Vector2(0, getVerticalOffsetForScrollChildren(3))
            );
            return blueSlider.rectTransform;
        });
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var opacitySlider = CreateColourSlider(
                "Opacity",
                "",
                f =>
                {
                    CustomGrabBeamColour.LocalNeutralColour.a = f;
                    Color col = neutralGrabColourPreviewImage.color;
                    col.a = f;
                    neutralGrabColourPreviewImage.color = col;
                },
                CustomGrabBeamColour.LocalNeutralColour.a,
                CustomGrabColourConfig.MaxOpacity,
                parent,
                new Vector2(0, getVerticalOffsetForScrollChildren(4))
            );
            return opacitySlider.rectTransform;
        });
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var matchSkinToggle = MenuAPI.CreateREPOToggle(
                "Match Skin Colour",
                (val) => { CustomGrabBeamColour.LocalNeutralColour.matchSkin = val; },
                parent,
                new Vector2(0, getVerticalOffsetForScrollChildren(5)),
                "Yes",
                "No",
                CustomGrabBeamColour.LocalNeutralColour.matchSkin
            );
            return matchSkinToggle.rectTransform;
        });

        // close and reset buttons
        changeGrabColourPage.AddElement(parent =>
        {
            var closeButton = MenuAPI.CreateREPOButton("Done", () => {
                changeGrabColourPage.ClosePage(true);
                CustomGrabBeamColour.UpdateBeamColourForAllBeams();
                CustomGrabBeamColour.SaveLocalColoursToConfig();
            },
                parent,
                new Vector2(horizontalPos, 30f)
            );

            var resetButton = MenuAPI.CreateREPOButton("Reset", () => {
                changeGrabColourPage.ClosePage(true);
                CustomGrabBeamColour.ResetBeamColours();
                CustomGrabBeamColour.UpdateBeamColourForAllBeams();
                CustomGrabBeamColour.SaveLocalColoursToConfig();
            },
                parent,
                new Vector2(590f, 30f)
            );
            resetButton.overrideButtonSize = resetButton.GetLabelSize() / 2;
            resetButton.transform.localScale = new Vector2(0.5f, 0.5f);
        });

        // setup colour previews and open page
        SetupPreviewRectangleColours(CustomGrabBeamColour.LocalNeutralColour.colour, CustomGrabBeamColour.LocalHealingColour.colour, CustomGrabBeamColour.LocalRotatingColour.colour);
        changeGrabColourPage.OpenPage(false);
    }

    private static void SetupPreviewRectangleColours(Color neutralColour, Color healingColour, Color rotatingColour)
    {
        neutralGrabColourPreviewImage.color = neutralColour;
    }

    private static void CreateBeamPreviewColourRectangle(Transform parent, Vector2 position, out GameObject colourPreviewParent, out Image colourPreviewImage)
    {
        colourPreviewParent = new GameObject();
        colourPreviewParent.name = "Grab Beam Colour Preview Rectangle Parent";
        colourPreviewParent.transform.SetParent(parent);

        Canvas canvas = colourPreviewParent.AddComponent<Canvas>();
        canvas.transform.SetParent(colourPreviewParent.gameObject.transform.parent);

        GameObject imageGameObject = new GameObject();
        imageGameObject.transform.SetParent(canvas.gameObject.transform.parent);

        colourPreviewImage = imageGameObject.AddComponent<Image>();
        colourPreviewImage.name = "Grab Beam Colour Preview Rectangle Image";
        colourPreviewImage.transform.SetParent(imageGameObject.gameObject.transform.parent);

        colourPreviewImage.color = new Color(1.0F, 0.0F, 0.0F);
        colourPreviewImage.gameObject.transform.position = position;
    }

    public static REPOSlider CreateColourSlider(string name, string desc, Action<float> onChange, float initial, float max, Transform parent, Vector2 localPosition)
    {
        return MenuAPI.CreateREPOSlider(
            name,
            desc,
            onChange,
            parent,
            localPosition,
            0,
            max,
            2,
            initial,
            "",
            "",
            REPOSlider.BarBehavior.UpdateWithValue
        );
    }

    private static float getVerticalOffsetForScrollChildren(int index)
    {
        return 40 * index;
    }

    private static float getVerticalPos(int offset)
    {
        int index = -Math.Abs(0 - offset);
        index += 3;
        return 180.0f + (30 * index);
    }
}