using System;
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
        if (
            rpcBuffer[0] is not float ||
            rpcBuffer[1] is not float ||
            rpcBuffer[2] is not float ||
            rpcBuffer[3] is not float ||
            rpcBuffer[4] is not bool ||
            rpcBuffer[5] is not BeamType
        )
        {
            throw new ArgumentException("FromRPCBuffer recieved incorrect parameters");
        }
        Color colour = new Color((float)rpcBuffer[0], (float)rpcBuffer[1], (float)rpcBuffer[2], (float)rpcBuffer[3]);

        return new GrabBeamColour(colour, (bool)rpcBuffer[4], (BeamType)rpcBuffer[5]);
    }
    public float r
    {
        get { return colour.r; }
        set { colour.r = value; }
    }
    public float g
    {
        get { return colour.g; }
        set { colour.g = value; }
    }
    public float b
    {
        get { return colour.b; }
        set { colour.b = value; }
    }
    public float a
    {
        get { return colour.a; }
        set { colour.a = value; }
    }

    public enum BeamType
    {
        Neutral,
        Heal,
        Rotate,
    }
}