
public static class ConvertExtensions
{
    public static byte ToByte(this float f)
    {
        if (f > byte.MaxValue) return byte.MaxValue;
        else if (f < byte.MinValue) return byte.MinValue;
        else return (byte)f;
    }
}

