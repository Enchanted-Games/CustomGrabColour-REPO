using UnityEngine;
using static GrabBeamColour;

public struct GrabBeamColour(Color colour, bool matchSkin, BeamType beamType)
{
    public Color colour = colour;
    public bool matchSkin = matchSkin;
    public BeamType beamType = beamType;

    public static object[] ToRPCBuffer(GrabBeamColour beamColour)
    {
        Color colour = beamColour.colour;
        return [colour.r, colour.g, colour.b, colour.a, beamColour.matchSkin, beamColour.beamType];
    }

    public static GrabBeamColour FromRPCBuffer(object[] rpcBuffer)
    {
        return null;
    }

    public enum BeamType
    {
        Neutral,
        Heal,
        Rotate,
    }
}