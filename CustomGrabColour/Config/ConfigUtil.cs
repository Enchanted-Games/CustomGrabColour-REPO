using CustomGrabColour;
using UnityEngine;

class ConfigUtil
{
    public static string ColorToString(Color color)
    {
        return color.r + ", " + color.g + ", " + color.b + ", " + color.a;
    }

    public static Color StringToColor(string color, Color defaultColour)
    {
        // parse r,g,b string
        string[] splitString = color.Split(',');
        if (splitString.Length == 3)
        {
            float[] elements = new float[3];
            for (int i = 0; i < splitString.Length; i++)
            {
                try
                {
                    elements.SetValue(float.Parse(splitString[i].Trim()), i);
                } catch
                {
                    return defaultColour;
                }
            }
            return new Color(elements[0], elements[1], elements[2], CustomGrabColourConfig.DefaultOpacity);
        }
        // parse rgba string
        else if (splitString.Length == 4)
            {
                float[] elements = new float[4];
                for (int i = 0; i < splitString.Length; i++)
                {
                    try
                    {
                        elements.SetValue(float.Parse(splitString[i].Trim()), i);
                    }
                    catch
                    {
                        return defaultColour;
                    }
                }
                return new Color(elements[0], elements[1], elements[2], elements[3]);
            }
            else
        {
            return defaultColour;
        }
    }
}