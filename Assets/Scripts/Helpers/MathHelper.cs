using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class MathHelper
{
    public static int GetOffset(int value, int favoriteValue, int resilienceValue)
    {
        if (favoriteValue + resilienceValue <= value && favoriteValue - resilienceValue >= value) return 0;
        if (favoriteValue + resilienceValue <= value) return value - (favoriteValue + resilienceValue);
        else return (favoriteValue - resilienceValue) - value;
    }

    public static byte GetReversed(byte num)
    {
        return (byte)(byte.MaxValue - num);
    }

    public static float GetMin(this float num, float min)
    {
        if (num < min) return min;
        return num;
    }

    public static int GetMin(this int num, int min)
    {
        if (num < min) return min;
        return num;
    }

    public static float GetMinMax(this float num, float min, float max)
    {
        if (float.IsNaN(num)) return min;
        if (num > max) return max;
        if (num < min) return min;
        return num;
    }

    public static int GetMinMax(this int num, int min, int max)
    {
        if (num > max) return max;
        if (num < min) return min;
        return num;
    }

    public static byte GetMinMax(this byte num, byte min, byte max)
    {
        if (num > max) return max;
        if (num < min) return min;
        return num;
    }

    public static float GetRounded(this float num)
    {
        return (float)Math.Round(num, 2);
    }
}

