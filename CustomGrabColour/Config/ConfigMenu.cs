using MenuLib;
using MenuLib.MonoBehaviors;
using System;
using CustomGrabColour.PlayerGrabBeam;
using UnityEngine;
using UnityEngine.UI;

namespace CustomGrabColour.Config;

internal static class ConfigMenu
{
    private static Image _neutralGrabColourPreviewImage;
    private static GameObject _neutralGrabColourPreviewParent;

    private static Image _rotatingGrabColourPreviewImage;
    private static GameObject _rotatingGrabColourPreviewParent;

    private static Image _healingGrabColourPreviewImage;
    private static GameObject _healingGrabColourPreviewParent;

    private const float HorizontalPos = 70f;

    public static void Init()
    {
        MenuAPI.AddElementToEscapeMenu(parent =>
        {
            // add button to open colour config screen
            MenuAPI.CreateREPOButton("Change Grab Colour", OpenPopup, parent, localPosition: new Vector2(28.3f, 350.0f));
        });
        MenuAPI.AddElementToColorMenu(parent =>
        {
            // add button to open colour config screen
            MenuAPI.CreateREPOButton("Change Grab Colour", OpenPopup, parent, localPosition: new Vector2(28.3f, 350.0f));
        });
        
        if (CustomGrabColourConfig.DebugAddButtonToMainMenu.Value)
        {
            MenuAPI.AddElementToMainMenu(parent =>
            {
                // add button to open colour config screen
                MenuAPI.CreateREPOButton("Change Grab Colour", OpenPopup, parent, localPosition: new Vector2(28.3f, 350.0f));
            });
        }
    }

    private static void OpenPopup()
    {
        // build colour config screen
        var changeGrabColourPage = MenuAPI.CreateREPOPopupPage("Change Grab Beam Colour", REPOPopupPage.PresetSide.Left, false, pageDimmerVisibility: true, spacing: 1.5f);

        // colour previews
        changeGrabColourPage.AddElement(parent =>
        {
            // neutral
            float neutralYPos = GetVerticalPos(0);
            var neutralLabel = MenuAPI.CreateREPOLabel("Neutral Colour Preview", parent, new Vector2(452f, neutralYPos - 60));
            neutralLabel.transform.localScale = new Vector2(0.5f, 0.5f);
            if (!_neutralGrabColourPreviewParent)
            {
                CreateBeamPreviewColourRectangle(parent, new Vector2(515f, neutralYPos), out _neutralGrabColourPreviewParent, out _neutralGrabColourPreviewImage);
            }

            // rotating
            float rotatingYPos = GetVerticalPos(1);
            var rotatingLabel = MenuAPI.CreateREPOLabel("Rotating Colour Preview", parent, new Vector2(452f, rotatingYPos - 60));
            rotatingLabel.transform.localScale = new Vector2(0.5f, 0.5f);
            if (!_rotatingGrabColourPreviewParent)
            {
                CreateBeamPreviewColourRectangle(parent, new Vector2(515f, rotatingYPos), out _rotatingGrabColourPreviewParent, out _rotatingGrabColourPreviewImage);
            }

            // healing
            float healingYPos = GetVerticalPos(2);
            var healingLabel = MenuAPI.CreateREPOLabel("Healing Colour Preview", parent, new Vector2(452f, healingYPos - 60));
            healingLabel.transform.localScale = new Vector2(0.5f, 0.5f);
            if (!_healingGrabColourPreviewParent)
            {
                CreateBeamPreviewColourRectangle(parent, new Vector2(515f, healingYPos), out _healingGrabColourPreviewParent, out _healingGrabColourPreviewImage);
            }
        });

        // neutral grab beam colours
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var beamLabel = MenuAPI.CreateREPOLabel("Neutral Beam Colour", parent, new Vector2(0, GetVerticalOffsetForScrollChildren(0)));
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
                    if (CustomGrabBeamColour.LocalNeutralColour.MatchSkin) return;
                    Color col = _neutralGrabColourPreviewImage.color;
                    col.r = f;
                    _neutralGrabColourPreviewImage.color = col;
                },
                CustomGrabBeamColour.LocalNeutralColour.r,
                1,
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(1))
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
                    if (CustomGrabBeamColour.LocalNeutralColour.MatchSkin) return;
                    Color col = _neutralGrabColourPreviewImage.color;
                    col.g = f;
                    _neutralGrabColourPreviewImage.color = col;
                },
                CustomGrabBeamColour.LocalNeutralColour.g,
                1,
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(2))
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
                    if (CustomGrabBeamColour.LocalNeutralColour.MatchSkin) return;
                    Color col = _neutralGrabColourPreviewImage.color;
                    col.b = f;
                    _neutralGrabColourPreviewImage.color = col;
                },
                CustomGrabBeamColour.LocalNeutralColour.b,
                1,
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(3))
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
                    Color col = _neutralGrabColourPreviewImage.color;
                    col.a = f;
                    _neutralGrabColourPreviewImage.color = col;
                },
                CustomGrabBeamColour.LocalNeutralColour.a,
                CustomGrabColourConfig.MaxOpacity,
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(4))
            );
            return opacitySlider.rectTransform;
        });
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var matchSkinToggle = MenuAPI.CreateREPOToggle(
                "Match Skin Colour",
                (val) =>
                {
                    CustomGrabBeamColour.LocalNeutralColour.MatchSkin = val;
                    HandleNeutralSkinMatchChange();
                },
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(5)),
                "Yes",
                "No",
                CustomGrabBeamColour.LocalNeutralColour.MatchSkin
            );
            return matchSkinToggle.rectTransform;
        });

        // rotating grab beam colours
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var beamLabel = MenuAPI.CreateREPOLabel("Rotating Beam Colour", parent, new Vector2(0, GetVerticalOffsetForScrollChildren(6)));
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
                    if (CustomGrabBeamColour.LocalRotatingColour.MatchSkin) return;
                    Color col = _rotatingGrabColourPreviewImage.color;
                    col.r = f;
                    _rotatingGrabColourPreviewImage.color = col;
                    HandleRotatingSkinMatchChange();
                },
                CustomGrabBeamColour.LocalRotatingColour.r,
                1,
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(7))
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
                    if (CustomGrabBeamColour.LocalRotatingColour.MatchSkin) return;
                    Color col = _rotatingGrabColourPreviewImage.color;
                    col.g = f;
                    _rotatingGrabColourPreviewImage.color = col;
                    HandleRotatingSkinMatchChange();
                },
                CustomGrabBeamColour.LocalRotatingColour.g,
                1,
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(8))
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
                    if (CustomGrabBeamColour.LocalRotatingColour.MatchSkin) return;
                    Color col = _rotatingGrabColourPreviewImage.color;
                    col.b = f;
                    _rotatingGrabColourPreviewImage.color = col;
                    HandleRotatingSkinMatchChange();
                },
                CustomGrabBeamColour.LocalRotatingColour.b,
                1,
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(9))
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
                    Color col = _rotatingGrabColourPreviewImage.color;
                    col.a = f;
                    _rotatingGrabColourPreviewImage.color = col;
                    HandleRotatingSkinMatchChange();
                },
                CustomGrabBeamColour.LocalRotatingColour.a,
                CustomGrabColourConfig.MaxOpacity,
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(10))
            );
            return opacitySlider.rectTransform;
        });
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var matchSkinToggle = MenuAPI.CreateREPOToggle(
                "Match Skin Colour",
                (val) =>
                {
                    CustomGrabBeamColour.LocalRotatingColour.MatchSkin = val;
                    HandleRotatingSkinMatchChange();
                },
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(11)),
                "Yes",
                "No",
                CustomGrabBeamColour.LocalRotatingColour.MatchSkin
            );
            return matchSkinToggle.rectTransform;
        });

        // healing grab beam colours
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var beamLabel = MenuAPI.CreateREPOLabel("Healing Beam Colour", parent, new Vector2(0, GetVerticalOffsetForScrollChildren(6)));
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
                    if (CustomGrabBeamColour.LocalHealingColour.MatchSkin) return;
                    Color col = _healingGrabColourPreviewImage.color;
                    col.r = f;
                    _healingGrabColourPreviewImage.color = col;
                    HandleHealingSkinMatchChange();
                },
                CustomGrabBeamColour.LocalHealingColour.r,
                1,
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(7))
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
                    if (CustomGrabBeamColour.LocalHealingColour.MatchSkin) return;
                    Color col = _healingGrabColourPreviewImage.color;
                    col.g = f;
                    _healingGrabColourPreviewImage.color = col;
                    HandleHealingSkinMatchChange();
                },
                CustomGrabBeamColour.LocalHealingColour.g,
                1,
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(8))
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
                    if (CustomGrabBeamColour.LocalHealingColour.MatchSkin) return;
                    Color col = _healingGrabColourPreviewImage.color;
                    col.b = f;
                    _healingGrabColourPreviewImage.color = col;
                    HandleHealingSkinMatchChange();
                },
                CustomGrabBeamColour.LocalHealingColour.b,
                1,
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(9))
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
                    Color col = _healingGrabColourPreviewImage.color;
                    col.a = f;
                    _healingGrabColourPreviewImage.color = col;
                    HandleHealingSkinMatchChange();
                },
                CustomGrabBeamColour.LocalHealingColour.a,
                CustomGrabColourConfig.MaxOpacity,
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(10))
            );
            return opacitySlider.rectTransform;
        });
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var matchSkinToggle = MenuAPI.CreateREPOToggle(
                "Match Skin Colour",
                (val) =>
                {
                    CustomGrabBeamColour.LocalHealingColour.MatchSkin = val;
                    HandleHealingSkinMatchChange();
                },
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(11)),
                "Yes",
                "No",
                CustomGrabBeamColour.LocalHealingColour.MatchSkin
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
                new Vector2(HorizontalPos, 30f)
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
        SetupPreviewRectangleColours(CustomGrabBeamColour.LocalNeutralColour.Colour, CustomGrabBeamColour.LocalHealingColour.Colour, CustomGrabBeamColour.LocalRotatingColour.Colour);
        changeGrabColourPage.OpenPage(false);
    }


    private static void SetupPreviewRectangleColours(Color neutralColour, Color healingColour, Color rotatingColour)
    {
        _neutralGrabColourPreviewImage.color = neutralColour;
        _healingGrabColourPreviewImage.color = healingColour;
        _rotatingGrabColourPreviewImage.color = rotatingColour;
        HandleNeutralSkinMatchChange();
        HandleRotatingSkinMatchChange();
        HandleHealingSkinMatchChange();
    }

    private static void HandleNeutralSkinMatchChange()
    {
        _neutralGrabColourPreviewImage.color = CustomGrabBeamColour.LocalNeutralColour.MatchSkin ? CustomGrabBeamColour.GetLocalBodyColour(Color.black) : CustomGrabBeamColour.LocalNeutralColour.Colour;
        Color col = _neutralGrabColourPreviewImage.color;
        col.a = CustomGrabBeamColour.LocalNeutralColour.Colour.a;
        _neutralGrabColourPreviewImage.color = col;
    }
    private static void HandleRotatingSkinMatchChange()
    {
        _rotatingGrabColourPreviewImage.color = CustomGrabBeamColour.LocalRotatingColour.MatchSkin ? CustomGrabBeamColour.GetLocalBodyColour(Color.black) : CustomGrabBeamColour.LocalRotatingColour.Colour;
        Color col = _rotatingGrabColourPreviewImage.color;
        col.a = CustomGrabBeamColour.LocalRotatingColour.Colour.a;
        _rotatingGrabColourPreviewImage.color = col;
    }
    private static void HandleHealingSkinMatchChange()
    {
        _healingGrabColourPreviewImage.color = CustomGrabBeamColour.LocalHealingColour.MatchSkin ? CustomGrabBeamColour.GetLocalBodyColour(Color.black) : CustomGrabBeamColour.LocalHealingColour.Colour;
        Color col = _healingGrabColourPreviewImage.color;
        col.a = CustomGrabBeamColour.LocalHealingColour.Colour.a;
        _healingGrabColourPreviewImage.color = col;
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
        colourPreviewImage.gameObject.transform.localScale = new Vector3(0.85f, 0.85f, 1f);
    }

    private static REPOSlider CreateColourSlider(string name, string desc, Action<float> onChange, float initial, float max, Transform parent, Vector2 localPosition)
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
            initial
        );
    }

    private static float GetVerticalOffsetForScrollChildren(int index)
    {
        return 40 * index;
    }

    private static float GetVerticalPos(int offset)
    {
        int index = -Math.Abs(0 - offset);
        index += 3;
        // first number is offset from bottom, second number is spacing between elements
        return -12f + (110 * index);
    }
}