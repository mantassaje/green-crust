
using UnityEngine;
using System.Linq;

public static class GameObjectExtensions
{
    public static bool IsNull(this Transform mono)
    {
        return ReferenceEquals(mono, null);
    }

    public static bool IsNotNull(this Transform mono)
    {
        return !mono.IsNull();
    }

    public static bool IsNull(this GameObject mono)
    {
        return ReferenceEquals(mono, null);
    }

    public static bool IsNotNull(this GameObject mono)
    {
        return !mono.IsNull();
    }

    public static bool IsNull(this MonoBehaviour mono)
    {
        return ReferenceEquals(mono, null);
    }

    public static bool IsNotNull(this MonoBehaviour mono)
    {
        return !mono.IsNull();
    }
}

