using System;
using UnityEngine;

namespace CustomGrabColour.PlayerGrabBeam;

public struct GrabBeamColourSettings(Color colour, bool matchSkin, GrabBeamColourSettings.BeamType currentBeamType)
{
    public Color Colour = colour;
    public bool MatchSkin = matchSkin;
    public readonly BeamType CurrentBeamType = currentBeamType;

    public static object[] ToRPCBuffer(GrabBeamColourSettings beamColour)
    {
        Color colour = beamColour.Colour;
        return [colour.r, colour.g, colour.b, colour.a, beamColour.MatchSkin, (byte)beamColour.CurrentBeamType];
    }

    public static GrabBeamColourSettings FromRPCBuffer(object[] rpcBuffer)
    {
        if (
            rpcBuffer[0] is not float ||
            rpcBuffer[1] is not float ||
            rpcBuffer[2] is not float ||
            rpcBuffer[3] is not float ||
            rpcBuffer[4] is not bool ||
            rpcBuffer[5] is not byte
        )
        {
            throw new ArgumentException("FromRPCBuffer recieved incorrect parameters");
        }
        Color colour = new Color((float)rpcBuffer[0], (float)rpcBuffer[1], (float)rpcBuffer[2], (float)rpcBuffer[3]);

        return new GrabBeamColourSettings(colour, (bool)rpcBuffer[4], (BeamType)rpcBuffer[5]);
    }
    public float r
    {
        get => Colour.r;
        set => Colour.r = value;
    }
    public float g
    {
        get => Colour.g;
        set => Colour.g = value;
    }
    public float b
    {
        get => Colour.b;
        set => Colour.b = value;
    }
    public float a
    {
        get => Colour.a;
        set => Colour.a = value;
    }

    public enum BeamType : byte
    {
        Neutral = 0,
        Heal,
        Rotate,
        Climb,
    }
}