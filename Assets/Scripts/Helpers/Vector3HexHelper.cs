using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Vector3HexHelper
{
    public static int Distance(XYZKey hex1, XYZKey hex2)
    {
        var distance = Math.Abs(hex1.X - hex2.X) + Math.Abs(hex1.Y - hex2.Y) + Math.Abs(hex1.Z - hex2.Z);
        return distance / 2;
    }


    public static XYZKey GoTop(this XYZKey hexPos)
    {
        return new XYZKey(hexPos.X, hexPos.Y + 1, hexPos.Z - 1);
    }

    public static XYZKey GoBot(this XYZKey hexPos)
    {
        return new XYZKey(hexPos.X, hexPos.Y - 1, hexPos.Z + 1);
    }

    /// <summary>
    ///   /
    /// O 
    /// </summary>
    public static XYZKey GoTopRight(this XYZKey hexPos)
    {
        return new XYZKey(hexPos.X + 1, hexPos.Y, hexPos.Z - 1);
    }

    /// <summary>
    /// \
    ///   O 
    /// </summary>
    public static XYZKey GoTopLeft(this XYZKey hexPos)
    {
        return new XYZKey(hexPos.X - 1, hexPos.Y + 1, hexPos.Z);
    }

    /// <summary>
    /// O 
    ///   \   
    /// </summary>
    public static XYZKey GoBotRight(this XYZKey hexPos)
    {
        return new XYZKey(hexPos.X + 1, hexPos.Y - 1, hexPos.Z);
    }

    /// <summary>
    ///  O 
    /// /    
    /// </summary>
    public static XYZKey GoBotLeft(this XYZKey hexPos)
    {
        return new XYZKey(hexPos.X - 1, hexPos.Y, hexPos.Z + 1);
    }
}

