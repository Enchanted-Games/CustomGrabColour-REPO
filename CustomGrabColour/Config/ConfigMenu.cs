using CustomGrabColour;
using MenuLib;
using MenuLib.MonoBehaviors;
using System;
using UnityEngine;
using UnityEngine.UI;

class ConfigMenu
{
    private static Image neutralGrabColourPreviewImage;
    private static GameObject neutralGrabColourPreviewParentObject;

    public static void Init()
    {
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

        changeGrabColourPage.AddElement(parent =>
        {
            CreateBeamPreviewColourRectangle(parent);
            SetBeamPreviewColourRectangleColour(CustomGrabBeamColour.LocalColour);

            var redSlider = CreateColourSlider(
                "Red",
                "",
                f => {
                    CustomGrabBeamColour.LocalColour.r = f;
                    Color col = neutralGrabColourPreviewImage.color;
                    col.r = f;
                    neutralGrabColourPreviewImage.color = col;
                    neutralGrabColourPreviewParentObject.gameObject.transform.localPosition = new Vector2(515f, 225f + f);
                },
                CustomGrabBeamColour.LocalColour.r,
                1,
                parent,
                3
            );
            var greenSlider = CreateColourSlider(
                "Green",
                "",
                f => {
                    CustomGrabBeamColour.LocalColour.g = f;
                    Color col = neutralGrabColourPreviewImage.color;
                    col.g = f;
                    neutralGrabColourPreviewImage.color = col;
                },
                CustomGrabBeamColour.LocalColour.g,
                1,
                parent,
                2
            );
            var blueSlider = CreateColourSlider(
                "Blue",
                "",
                f => {
                    CustomGrabBeamColour.LocalColour.b = f;
                    Color col = neutralGrabColourPreviewImage.color;
                    col.b = f;
                    neutralGrabColourPreviewImage.color = col;
                },
                CustomGrabBeamColour.LocalColour.b,
                1,
                parent,
                1
            );
            var opacitySlider = CreateColourSlider(
                "Opacity",
                "",
                f => {
                    CustomGrabBeamColour.LocalColour.a = f;
                    Color col = neutralGrabColourPreviewImage.color;
                    col.a = f;
                    neutralGrabColourPreviewImage.color = col;
                },
                CustomGrabBeamColour.LocalColour.a,
                CustomGrabColourConfig.MaxOpacity,
                parent,
                0
            );

            var disclaimerLabel = MenuAPI.CreateREPOLabel("Preview may not be 100% accurate ", parent, new Vector2(430f, 155f));
            disclaimerLabel.transform.localScale = new Vector2(0.5f, 0.5f);

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

    private static void SetBeamPreviewColourRectangleColour(Color colour)
    {
        neutralGrabColourPreviewImage.color = colour;
    }

    private static void CreateBeamPreviewColourRectangle(Transform parent)
    {
        if(!neutralGrabColourPreviewParentObject)
        {
            neutralGrabColourPreviewParentObject = new GameObject();
        }
        neutralGrabColourPreviewParentObject.name = "Grab Beam Colour Preview Rectangle Parent";
        neutralGrabColourPreviewParentObject.transform.SetParent(parent);

        Canvas canvas = neutralGrabColourPreviewParentObject.AddComponent<Canvas>();
        canvas.transform.SetParent(neutralGrabColourPreviewParentObject.gameObject.transform.parent);

        GameObject imageGameObject = new GameObject();
        imageGameObject.transform.SetParent(canvas.gameObject.transform.parent);

        Image image = imageGameObject.AddComponent<Image>();
        image.transform.SetParent(imageGameObject.gameObject.transform.parent);

        image.color = new Color(1.0F, 0.0F, 0.0F);
        neutralGrabColourPreviewImage = image;
        neutralGrabColourPreviewImage.gameObject.transform.position = new Vector2(515f, 225f);
    }

    public static REPOSlider CreateColourSlider(string name, string desc, Action<float> onChange, float initial, float max, Transform parent, int offset)
    {
        return MenuAPI.CreateREPOSlider(
            name,
            desc,
            onChange,
            parent,
            new Vector2(60f, 165.0f + (35 * offset)),
            0,
            max,
            2,
            initial,
            "",
            "",
            REPOSlider.BarBehavior.UpdateWithValue
        );
    }
}