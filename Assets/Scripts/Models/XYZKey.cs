using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public struct XYZKey
{
    [SyncVar]
    public int X;

    [SyncVar]
    public int Y;

    [SyncVar]
    public int Z;

    public XYZKey(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    public XYZKey(float x, float y, float z)
    {
        X = (int)x;
        Y = (int)y;
        Z = (int)z;
    }
    public XYZKey(Vector3 vec)
    {
        X = (int)vec.x;
        Y = (int)vec.y;
        Z = (int)vec.z;
    }

    public Vector3 GetVector()
    {
        return new Vector3(X, Y, Z);
    }

    public static bool operator ==(XYZKey xy1, XYZKey xy2)
    {
        return xy1.Equals(xy2);
    }

    public static bool operator !=(XYZKey xy1, XYZKey xy2)
    {
        return !xy1.Equals(xy2);
    }

    public override bool Equals(object obj)
    {
        if (obj is XYZKey)
        {
            var xy = (XYZKey)obj;
            return this.X == xy.X && this.Y == xy.Y && this.Z == xy.Z;
        }
        return false;
    }

    public override int GetHashCode()
    {
        var hashCode = 1861411795;
        hashCode = hashCode * -1521134295 + X.GetHashCode();
        hashCode = hashCode * -1521134295 + Y.GetHashCode();
        hashCode = hashCode * -1521134295 + Z.GetHashCode();
        return hashCode;
    }

    public override string ToString()
    {
        return X + " " + Y + " " + Z;
    }
}
