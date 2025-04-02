using MenuLib;
using MenuLib.MonoBehaviors;
using System;
using UnityEngine;
using UnityEngine.UI;

class ConfigMenu
{
    private static Image neutralGrabColourPreviewImage;
    private static GameObject neutralGrabColourPreviewParent;

    private static Image rotatingGrabColourPreviewImage;
    private static GameObject rotatingGrabColourPreviewParent;

    private static Image healingGrabColourPreviewImage;
    private static GameObject healingGrabColourPreviewParent;

    private static float horizontalPos = 70f;

    public static void Init()
    {
        MenuAPI.AddElementToMainMenu(parent =>
        {
            // add button to open colour config screen
            var repoButton = MenuAPI.CreateREPOButton("Change Grab Colour", () => OpenPopup(), parent, localPosition: new Vector2(28.3f, 350.0f));
        });
        MenuAPI.AddElementToEscapeMenu(parent =>
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
            // neutral
            float neutralYPos = getVerticalPos(0);
            var neutralLabel = MenuAPI.CreateREPOLabel("Neutral Colour Preview", parent, new Vector2(400f, neutralYPos - 100));
            neutralLabel.transform.localScale = new Vector2(0.5f, 0.5f);
            if(!neutralGrabColourPreviewParent)
            {
                CreateBeamPreviewColourRectangle(parent, new Vector2(515f, neutralYPos), out neutralGrabColourPreviewParent, out neutralGrabColourPreviewImage);
            }

            // rotating
            float rotatingYPos = getVerticalPos(1);
            var rotatingLabel = MenuAPI.CreateREPOLabel("Rotating Colour Preview", parent, new Vector2(400f, rotatingYPos - 100));
            rotatingLabel.transform.localScale = new Vector2(0.5f, 0.5f);
            if (!rotatingGrabColourPreviewParent)
            {
                CreateBeamPreviewColourRectangle(parent, new Vector2(515f, rotatingYPos), out rotatingGrabColourPreviewParent, out rotatingGrabColourPreviewImage);
            }

            // healing
            float healingYPos = getVerticalPos(2);
            var healingLabel = MenuAPI.CreateREPOLabel("Healing Colour Preview", parent, new Vector2(400f, healingYPos - 100));
            healingLabel.transform.localScale = new Vector2(0.5f, 0.5f);
            if (!healingGrabColourPreviewParent)
            {
                CreateBeamPreviewColourRectangle(parent, new Vector2(515f, healingYPos), out healingGrabColourPreviewParent, out healingGrabColourPreviewImage);
            }
        });

        // neutral grab beam colours
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var beamLabel = MenuAPI.CreateREPOLabel("Neutral Beam Colour", parent, new Vector2(0, getVerticalOffsetForScrollChildren(0)));
            beamLabel.transform.localScale = new Vector2(0.5f, 0.5f);
            return beamLabel.rectTransform;
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
                    HandleNeutralSettingsUpdate();
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
                    HandleNeutralSettingsUpdate();
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
                    HandleNeutralSettingsUpdate();
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
                    HandleNeutralSettingsUpdate();
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
                (val) =>
                {
                    CustomGrabBeamColour.LocalNeutralColour.matchSkin = val;
                    HandleNeutralSettingsUpdate();
                },
                parent,
                new Vector2(0, getVerticalOffsetForScrollChildren(5)),
                "Yes",
                "No",
                CustomGrabBeamColour.LocalNeutralColour.matchSkin
            );
            return matchSkinToggle.rectTransform;
        });

        // rotating grab beam colours
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var beamLabel = MenuAPI.CreateREPOLabel("Rotating Beam Colour", parent, new Vector2(0, getVerticalOffsetForScrollChildren(6)));
            beamLabel.transform.localScale = new Vector2(0.5f, 0.5f);
            return beamLabel.rectTransform;
        });
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var redSlider = CreateColourSlider(
                "Red",
                "",
                f =>
                {
                    CustomGrabBeamColour.LocalRotatingColour.r = f;
                    Color col = rotatingGrabColourPreviewImage.color;
                    col.r = f;
                    rotatingGrabColourPreviewImage.color = col;
                    HandleRotatingSettingsUpdate();
                },
                CustomGrabBeamColour.LocalRotatingColour.r,
                1,
                parent,
                new Vector2(0, getVerticalOffsetForScrollChildren(7))
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
                    CustomGrabBeamColour.LocalRotatingColour.g = f;
                    Color col = rotatingGrabColourPreviewImage.color;
                    col.g = f;
                    rotatingGrabColourPreviewImage.color = col;
                    HandleRotatingSettingsUpdate();
                },
                CustomGrabBeamColour.LocalRotatingColour.g,
                1,
                parent,
                new Vector2(0, getVerticalOffsetForScrollChildren(8))
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
                    CustomGrabBeamColour.LocalRotatingColour.b = f;
                    Color col = rotatingGrabColourPreviewImage.color;
                    col.b = f;
                    rotatingGrabColourPreviewImage.color = col;
                    HandleRotatingSettingsUpdate();
                },
                CustomGrabBeamColour.LocalRotatingColour.b,
                1,
                parent,
                new Vector2(0, getVerticalOffsetForScrollChildren(9))
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
                    CustomGrabBeamColour.LocalRotatingColour.a = f;
                    Color col = rotatingGrabColourPreviewImage.color;
                    col.a = f;
                    rotatingGrabColourPreviewImage.color = col;
                    HandleRotatingSettingsUpdate();
                },
                CustomGrabBeamColour.LocalRotatingColour.a,
                CustomGrabColourConfig.MaxOpacity,
                parent,
                new Vector2(0, getVerticalOffsetForScrollChildren(10))
            );
            return opacitySlider.rectTransform;
        });
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var matchSkinToggle = MenuAPI.CreateREPOToggle(
                "Match Skin Colour",
                (val) =>
                {
                    CustomGrabBeamColour.LocalRotatingColour.matchSkin = val;
                    HandleRotatingSettingsUpdate();
                },
                parent,
                new Vector2(0, getVerticalOffsetForScrollChildren(11)),
                "Yes",
                "No",
                CustomGrabBeamColour.LocalRotatingColour.matchSkin
            );
            return matchSkinToggle.rectTransform;
        });

        // healing grab beam colours
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var beamLabel = MenuAPI.CreateREPOLabel("Healing Beam Colour", parent, new Vector2(0, getVerticalOffsetForScrollChildren(6)));
            beamLabel.transform.localScale = new Vector2(0.5f, 0.5f);
            return beamLabel.rectTransform;
        });
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var redSlider = CreateColourSlider(
                "Red",
                "",
                f =>
                {
                    CustomGrabBeamColour.LocalHealingColour.r = f;
                    Color col = healingGrabColourPreviewImage.color;
                    col.r = f;
                    healingGrabColourPreviewImage.color = col;
                    HandleHealingSettingsUpdate();
                },
                CustomGrabBeamColour.LocalHealingColour.r,
                1,
                parent,
                new Vector2(0, getVerticalOffsetForScrollChildren(7))
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
                    CustomGrabBeamColour.LocalHealingColour.g = f;
                    Color col = healingGrabColourPreviewImage.color;
                    col.g = f;
                    healingGrabColourPreviewImage.color = col;
                    HandleHealingSettingsUpdate();
                },
                CustomGrabBeamColour.LocalHealingColour.g,
                1,
                parent,
                new Vector2(0, getVerticalOffsetForScrollChildren(8))
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
                    CustomGrabBeamColour.LocalHealingColour.b = f;
                    Color col = healingGrabColourPreviewImage.color;
                    col.b = f;
                    healingGrabColourPreviewImage.color = col;
                    HandleHealingSettingsUpdate();
                },
                CustomGrabBeamColour.LocalHealingColour.b,
                1,
                parent,
                new Vector2(0, getVerticalOffsetForScrollChildren(9))
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
                    CustomGrabBeamColour.LocalHealingColour.a = f;
                    Color col = healingGrabColourPreviewImage.color;
                    col.a = f;
                    healingGrabColourPreviewImage.color = col;
                    HandleHealingSettingsUpdate();
                },
                CustomGrabBeamColour.LocalHealingColour.a,
                CustomGrabColourConfig.MaxOpacity,
                parent,
                new Vector2(0, getVerticalOffsetForScrollChildren(10))
            );
            return opacitySlider.rectTransform;
        });
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var matchSkinToggle = MenuAPI.CreateREPOToggle(
                "Match Skin Colour",
                (val) =>
                {
                    CustomGrabBeamColour.LocalHealingColour.matchSkin = val;
                    HandleHealingSettingsUpdate();
                },
                parent,
                new Vector2(0, getVerticalOffsetForScrollChildren(11)),
                "Yes",
                "No",
                CustomGrabBeamColour.LocalHealingColour.matchSkin
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
        healingGrabColourPreviewImage.color = healingColour;
        rotatingGrabColourPreviewImage.color = rotatingColour;
    }

    private static void HandleNeutralSettingsUpdate()
    {
        // TODO: update preview rectangle colour and add get body colour method to CustomGrabBeamColour
    }
    private static void HandleRotatingSettingsUpdate()
    {
        // TODO: update preview rectangle colour and add get body colour method to CustomGrabBeamColour
    }
    private static void HandleHealingSettingsUpdate()
    {
        // TODO: update preview rectangle colour and add get body colour method to CustomGrabBeamColour
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
        return -30f + (120 * index);
    }
}