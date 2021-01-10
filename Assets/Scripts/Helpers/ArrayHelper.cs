using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class ArrayHelper
{
    public static List<T> ToList<T>(T[,] doubleArray)
    {
        var result = new List<T>();
        if (doubleArray != null)
        {
            for (int x = 0; x < doubleArray.GetLength(0); x++)
                for (int y = 0; y < doubleArray.GetLength(1); y++)
                    result.Add(doubleArray[x, y]);
        }
        return result;
    }
    public static List<T> ToList<T>(T[,,] doubleArray)
    {
        var result = new List<T>();
        if (doubleArray != null)
        {
            for (int x = 0; x < doubleArray.GetLength(0); x++)
                for (int y = 0; y < doubleArray.GetLength(1); y++)
                    for (int z = 0; z < doubleArray.GetLength(2); z++)
                        result.Add(doubleArray[x, y, z]);
        }
        return result;
    }

    public static T GetSafe<T>(this T[] array, int index)
    {
        if (index > array.TopIndex()) return default(T);

        return array[index];
    }

    public static IEnumerable<T> PickRandom<T>(this List<T> source, int maxCount)
    {
        if (source.Count > 0)
        {
            for (int i = 0; i < maxCount; i++)
            {
                yield return source[UnityEngine.Random.Range(0, source.Count)];
            }
        }
    }

    public static int TopIndex<T>(this List<T> list)
    {
        return list.Count - 1;
    }

    public static int TopIndex<T>(this T[] array)
    {
        return array.Length - 1;
    }

    public static IEnumerable<T> Traverse<T>(T item, Func<T, IEnumerable<T>> childSelector)
    {
        var stack = new Stack<T>();
        stack.Push(item);
        while (stack.Any())
        {
            var next = stack.Pop();
            yield return next;
            foreach (var child in childSelector(next))
                stack.Push(child);
        }
    }
}

