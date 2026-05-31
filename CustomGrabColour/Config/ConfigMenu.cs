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

    private static Image _climbingGrabColourPreviewImage;
    private static GameObject _climbingGrabColourPreviewParent;

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
            var row1Y = GetVerticalPos(0);
            var row2Y = GetVerticalPos(1);
            const float col1X = 455f;
            const float col2X = 605f;
            const float labelYOffset = 60f;
            var labelScale = new Vector2(0.5f, 0.5f);
            
            // neutral
            var neutralLabel = MenuAPI.CreateREPOLabel("Neutral Colour Preview", parent, new Vector2(col1X, row1Y - labelYOffset));
            neutralLabel.transform.localScale = labelScale;
            neutralLabel.transform.SetPositionAndRotation(new Vector3(col1X - (neutralLabel.labelTMP.GetPreferredWidth() / 4), neutralLabel.transform.position.y, neutralLabel.transform.position.z), neutralLabel.transform.rotation);
            if (!_neutralGrabColourPreviewParent)
            {
                CreateBeamPreviewColourRectangle(parent, new Vector2(col1X, row1Y), out _neutralGrabColourPreviewParent, out _neutralGrabColourPreviewImage);
            }

            // rotating
            var rotatingLabel = MenuAPI.CreateREPOLabel("Rotating Colour Preview", parent, new Vector2(col1X, row2Y - labelYOffset));
            rotatingLabel.transform.localScale = labelScale;
            rotatingLabel.transform.SetPositionAndRotation(new Vector3(col1X - (rotatingLabel.labelTMP.GetPreferredWidth() / 4), rotatingLabel.transform.position.y, rotatingLabel.transform.position.z), rotatingLabel.transform.rotation);
            if (!_rotatingGrabColourPreviewParent)
            {
                CreateBeamPreviewColourRectangle(parent, new Vector2(col1X, row2Y), out _rotatingGrabColourPreviewParent, out _rotatingGrabColourPreviewImage);
            }

            // healing
            var healingLabel = MenuAPI.CreateREPOLabel("Healing Colour Preview", parent, new Vector2(col2X, row1Y - labelYOffset));
            healingLabel.transform.localScale = labelScale;
            healingLabel.transform.SetPositionAndRotation(new Vector3(col2X - (healingLabel.labelTMP.GetPreferredWidth() / 4), healingLabel.transform.position.y, healingLabel.transform.position.z), healingLabel.transform.rotation);
            if (!_healingGrabColourPreviewParent)
            {
                CreateBeamPreviewColourRectangle(parent, new Vector2(col2X, row1Y), out _healingGrabColourPreviewParent, out _healingGrabColourPreviewImage);
            }

            // climbing
            var climbingLabel = MenuAPI.CreateREPOLabel("Climbing Colour Preview", parent, new Vector2(col2X, row2Y - labelYOffset));
            climbingLabel.transform.localScale = labelScale;
            climbingLabel.transform.SetPositionAndRotation(new Vector3(col2X - (climbingLabel.labelTMP.GetPreferredWidth() / 4), climbingLabel.transform.position.y, climbingLabel.transform.position.z), climbingLabel.transform.rotation);
            if (!_climbingGrabColourPreviewParent)
            {
                CreateBeamPreviewColourRectangle(parent, new Vector2(col2X, row2Y), out _climbingGrabColourPreviewParent, out _climbingGrabColourPreviewImage);
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
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(3))
            );
            return blueSlider.rectTransform;
        });
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var opacitySlider = CreateAlphaSlider(
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
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(4))
            );
            return opacitySlider.rectTransform;
        });
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var matchSkinToggle = MenuAPI.CreateREPOToggle(
                "Match Grabber Cosmetic",
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
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(9))
            );
            return blueSlider.rectTransform;
        });
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var opacitySlider = CreateAlphaSlider(
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
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(10))
            );
            return opacitySlider.rectTransform;
        });
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var matchSkinToggle = MenuAPI.CreateREPOToggle(
                "Match Grabber Cosmetic",
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
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(9))
            );
            return blueSlider.rectTransform;
        });
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var opacitySlider = CreateAlphaSlider(
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
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(10))
            );
            return opacitySlider.rectTransform;
        });
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var matchSkinToggle = MenuAPI.CreateREPOToggle(
                "Match Grabber Cosmetic",
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
        
        
        // climbing grab beam colours
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var beamLabel = MenuAPI.CreateREPOLabel("Climbing Beam Colour", parent, new Vector2(0, GetVerticalOffsetForScrollChildren(6)));
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
                    CustomGrabBeamColour.LocalClimbingColour.r = f;
                    if (CustomGrabBeamColour.LocalClimbingColour.MatchSkin) return;
                    Color col = _climbingGrabColourPreviewImage.color;
                    col.r = f;
                    _climbingGrabColourPreviewImage.color = col;
                    HandleClimbingSkinMatchChange();
                },
                CustomGrabBeamColour.LocalClimbingColour.r,
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
                    CustomGrabBeamColour.LocalClimbingColour.g = f;
                    if (CustomGrabBeamColour.LocalClimbingColour.MatchSkin) return;
                    Color col = _climbingGrabColourPreviewImage.color;
                    col.g = f;
                    _climbingGrabColourPreviewImage.color = col;
                    HandleClimbingSkinMatchChange();
                },
                CustomGrabBeamColour.LocalClimbingColour.g,
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
                    CustomGrabBeamColour.LocalClimbingColour.b = f;
                    if (CustomGrabBeamColour.LocalClimbingColour.MatchSkin) return;
                    Color col = _climbingGrabColourPreviewImage.color;
                    col.b = f;
                    _climbingGrabColourPreviewImage.color = col;
                    HandleClimbingSkinMatchChange();
                },
                CustomGrabBeamColour.LocalClimbingColour.b,
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(9))
            );
            return blueSlider.rectTransform;
        });
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var opacitySlider = CreateAlphaSlider(
                "Opacity",
                "",
                f =>
                {
                    CustomGrabBeamColour.LocalClimbingColour.a = f;
                    Color col = _climbingGrabColourPreviewImage.color;
                    col.a = f;
                    _climbingGrabColourPreviewImage.color = col;
                    HandleClimbingSkinMatchChange();
                },
                CustomGrabBeamColour.LocalClimbingColour.a,
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(10))
            );
            return opacitySlider.rectTransform;
        });
        changeGrabColourPage.AddElementToScrollView(parent =>
        {
            var matchSkinToggle = MenuAPI.CreateREPOToggle(
                "Match Grabber Cosmetic",
                (val) =>
                {
                    CustomGrabBeamColour.LocalClimbingColour.MatchSkin = val;
                    HandleClimbingSkinMatchChange();
                },
                parent,
                new Vector2(0, GetVerticalOffsetForScrollChildren(11)),
                "Yes",
                "No",
                CustomGrabBeamColour.LocalClimbingColour.MatchSkin
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
        SetupPreviewRectangleColours(CustomGrabBeamColour.LocalNeutralColour.Colour, CustomGrabBeamColour.LocalRotatingColour.Colour, CustomGrabBeamColour.LocalHealingColour.Colour, CustomGrabBeamColour.LocalClimbingColour.Colour);
        changeGrabColourPage.OpenPage(false);
    }


    private static void SetupPreviewRectangleColours(Color neutralColour, Color rotatingColour, Color healingColour, Color climbingColour)
    {
        _neutralGrabColourPreviewImage.color = neutralColour;
        _rotatingGrabColourPreviewImage.color = rotatingColour;
        _healingGrabColourPreviewImage.color = healingColour;
        _climbingGrabColourPreviewImage.color = climbingColour;
        HandleNeutralSkinMatchChange();
        HandleRotatingSkinMatchChange();
        HandleHealingSkinMatchChange();
    }

    private static void HandleNeutralSkinMatchChange()
    {
        _neutralGrabColourPreviewImage.color = CustomGrabBeamColour.LocalNeutralColour.MatchSkin ? CustomGrabBeamColour.GetLocalGrabberCosmeticColour(CustomGrabColourConfig.NeutralDefaultColour) : CustomGrabBeamColour.LocalNeutralColour.Colour;
        Color col = _neutralGrabColourPreviewImage.color;
        col.a = CustomGrabBeamColour.LocalNeutralColour.Colour.a;
        _neutralGrabColourPreviewImage.color = col;
    }
    private static void HandleRotatingSkinMatchChange()
    {
        _rotatingGrabColourPreviewImage.color = CustomGrabBeamColour.LocalRotatingColour.MatchSkin ? CustomGrabBeamColour.GetLocalGrabberCosmeticColour(CustomGrabColourConfig.RotatingDefaultColour) : CustomGrabBeamColour.LocalRotatingColour.Colour;
        Color col = _rotatingGrabColourPreviewImage.color;
        col.a = CustomGrabBeamColour.LocalRotatingColour.Colour.a;
        _rotatingGrabColourPreviewImage.color = col;
    }
    private static void HandleHealingSkinMatchChange()
    {
        _healingGrabColourPreviewImage.color = CustomGrabBeamColour.LocalHealingColour.MatchSkin ? CustomGrabBeamColour.GetLocalGrabberCosmeticColour(CustomGrabColourConfig.HealingDefaultColour) : CustomGrabBeamColour.LocalHealingColour.Colour;
        Color col = _healingGrabColourPreviewImage.color;
        col.a = CustomGrabBeamColour.LocalHealingColour.Colour.a;
        _healingGrabColourPreviewImage.color = col;
    }
    private static void HandleClimbingSkinMatchChange()
    {
        _climbingGrabColourPreviewImage.color = CustomGrabBeamColour.LocalClimbingColour.MatchSkin ? CustomGrabBeamColour.GetLocalGrabberCosmeticColour(CustomGrabColourConfig.ClimbingDefaultColour) : CustomGrabBeamColour.LocalClimbingColour.Colour;
        Color col = _climbingGrabColourPreviewImage.color;
        col.a = CustomGrabBeamColour.LocalClimbingColour.Colour.a;
        _climbingGrabColourPreviewImage.color = col;
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

    private static REPOSlider CreateColourSlider(string name, string desc, Action<float> onChange, float initial, Transform parent, Vector2 localPosition)
    {
        return MenuAPI.CreateREPOSlider(
            name,
            desc,
            onChange,
            parent,
            localPosition,
            0,
            1,
            2,
            initial
        );
    }
    
    private static REPOSlider CreateAlphaSlider(string name, string desc, Action<float> onChange, float initial, Transform parent, Vector2 localPosition)
    {
        return MenuAPI.CreateREPOSlider(
            name,
            desc,
            onChange,
            parent,
            localPosition,
            CustomGrabColourConfig.MinOpacity,
            CustomGrabColourConfig.MaxOpacity,
            2,
            initial
        );
    }
    
    private static REPOSlider CreateSlider(string name, string desc, Action<float> onChange, float initial, float min, float max, Transform parent, Vector2 localPosition)
    {
        return MenuAPI.CreateREPOSlider(
            name,
            desc,
            onChange,
            parent,
            localPosition,
            min,
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
        return -160f + (150 * index);
    }
}