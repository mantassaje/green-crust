using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct XYKey
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public XYKey(int x, int y)
    {
        X = x;
        Y = y;
    }
    public XYKey(float x, float y)
    {
        X = (int)x;
        Y = (int)y;
    }

    public static bool operator ==(XYKey xy1, XYKey xy2)
    {
        return xy1.Equals(xy2);
    }

    public static bool operator !=(XYKey xy1, XYKey xy2)
    {
        return !xy1.Equals(xy2);
    }

    public override bool Equals(object obj)
    {
        if (obj is XYKey)
        {
            var xy = (XYKey)obj;
            return this.X == xy.X && this.Y == xy.Y;
        }
        return false;
    }

    public override int GetHashCode()
    {
        var hashCode = 1861411795;
        hashCode = hashCode * -1521134295 + X.GetHashCode();
        hashCode = hashCode * -1521134295 + Y.GetHashCode();
        return hashCode;
    }

    public override string ToString()
    {
        return X + " " + Y;
    }
}
