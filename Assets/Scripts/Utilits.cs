using System;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    private static bool _isApplicationQuitting;

    public static T instance
    {
        get
        {
            if (_isApplicationQuitting)
                return null;

            if (_instance != null)
                return _instance;

            _instance = FindObjectOfType<T>();
            if (_instance != null)
                return _instance;

            var obj = new GameObject { name = $"[{typeof(T).Name}]" };
            _instance = obj.AddComponent<T>();
            return _instance;
        }
    }

    public virtual void Touch() { }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError(_instance + " was destroyed. Another instance exist.");
        }
    }

    protected virtual void OnApplicationQuit()
    {
        _isApplicationQuitting = true;
    }
}
public static class EnumHelper
{
    public static List<T> GetValues<T>() where T : Enum
    {
        var objects = Enum.GetValues(typeof(T));
        var values = new List<T>(objects.Length);

        for (var i = 0; i < objects.Length; i++)
            values.Add((T)objects.GetValue(i));

        return values;
    }
    public static Dictionary<string,T> GetStringValuesPair<T>() where T : Enum
    {
        var objects = Enum.GetValues(typeof(T));
        var strValuePait = new Dictionary<string, T>();
        foreach(T value in objects)
        {
            strValuePait.Add(value.ToString(), value);
        }
        return strValuePait;
    }
}
