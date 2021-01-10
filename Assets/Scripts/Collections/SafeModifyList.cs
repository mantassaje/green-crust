using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class SafeModifyList<T> : IEnumerable<T>
{
    private List<T> _list = new List<T>();
    private List<T> _listToAdd = new List<T>();
    private List<T> _listToRemove = new List<T>();

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

    public SafeModifyList()
    {
        Refersh();
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
        _listToAdd.Add(item);
    }

    public void Remove(T item)
    {
        _listToRemove.Add(item);
    }

    public T GetSafe(int index)
    {
        if (_list.Count() == 0) return default(T);
        index = index.GetMinMax(0, _list.Count() - 1);
        return _list[index];
    }

    /*public T GetSafe(float index)
    {
        return GetSafe()
    }*/

    public void Refersh()
    {
        _list.AddRange(_listToAdd);
        _listToRemove.ForEach(v => _list.Remove(v));
        _listToAdd.Clear();
        _listToRemove.Clear();
    }
}

