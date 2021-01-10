using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class MathGeoHelper
{ 
    public static Quaternion LookRotation(Vector2 lookFrom, Vector2 lookTo)
    {
        var diff = lookTo - lookFrom;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    public static bool IsInCircle(Vector2 point, Vector2 circleCenter, float radius)
    {
        var x = Math.Pow(point.x - circleCenter.x, 2);
        var y = Math.Pow(point.y - circleCenter.y, 2);
        var d = Math.Sqrt(x + y);
        return d < radius;
    }

    public static bool IsInCircle(XYKey point, XYKey circleCenter, float radius)
    {
        return IsInCircle(
            new Vector2(point.X, point.Y),
            new Vector2(circleCenter.X, circleCenter.Y),
            radius);
    }

    public static bool IsInCircleEdge(XYZKey point, XYZKey circleCenter, float radius)
    {
        var distance = Vector3HexHelper.Distance(point, circleCenter);
        UnityEngine.Debug.Log(new { distance, radius });
        return distance == (int)radius;
    }

    public static bool IsInCircle(XYZKey point, XYZKey circleCenter, float radius)
    {
        var distance = Vector3HexHelper.Distance(point, circleCenter);
        return distance < radius;
    }
}

