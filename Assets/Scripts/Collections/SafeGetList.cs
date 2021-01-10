using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class SafeGetList<T> : IEnumerable<T>
{
    private List<T> _list = new List<T>();

    public int Count
    {
        get
        {
            return _list.Count;
        }
    }

    public T this[int index]
    {
        get { return _list[index]; }
    }

    public SafeGetList()
    {
    }

    public SafeGetList(List<T> source)
    {
        _list = source.ToList();
    }

    public SafeGetList(params T[] source)
    {
        _list = source.ToList();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        _list.Add(item);
    }

    public void Remove(T item)
    {
        _list.Add(item);
    }

    public T GetSafe(int index)
    {
        if (_list.Count() == 0) return default(T);
        index = index.GetMinMax(0, _list.Count() - 1);
        return _list[index];
    }
}

