using System;
using System.Collections.Generic;
using UnityEngine;

public class TypeList
{
    private Dictionary<Type, object> _dic = new Dictionary<Type, object>();

    public void Add<T>(T obj)
        where T : MonoBehaviour
    {
        var t = typeof(T);
        if (!_dic.ContainsKey(t))
        {
            _dic[t] = new List<T>();
        }
        Get<T>().Add(obj);
    }

    public void Remove<T>(T obj)
        where T : MonoBehaviour
    {
        var t = typeof(T);
        if (_dic.ContainsKey(t))
        {
            Get<T>().Remove(obj);
        }
    }

    public List<T> Get<T>()
        where T : MonoBehaviour
    {
        var t = typeof(T);
        if (_dic.ContainsKey(t))
        {
            return (List<T>)_dic[t];
        }
        else return new List<T>();
    }
}

