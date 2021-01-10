using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class EnumerableHelper
{
    public static T PickRandom<T>(this IList<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count())];
    }

    public static void Call<TSource>(this IEnumerable<TSource> source, Action<TSource> selector)
    {
        foreach (var item in source) selector(item);
    }

    public static bool ContainsAtLeastOne<TSource>(this IEnumerable<TSource> source, params TSource[] matches)
    {
        return source.Any(value => matches.Contains(value));
    }

    public static bool ContainsAll<TSource>(this IEnumerable<TSource> source, params TSource[] matches)
    {
        return matches.All(value => source.Contains(value));
    }
}
