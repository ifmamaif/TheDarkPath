using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;

public class Dispatcher : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (_instance == null)
        {
            _instance = new GameObject("Dispatcher").AddComponent<Dispatcher>();
            DontDestroyOnLoad(_instance.gameObject);
        }
    }

    static Dispatcher _instance;

    public static void RunOnUIThread(Action action)
    {
        lock(_instance)
        {
            action();
        }
    }

    public static T RunOnUIThread<T>(Func<T> action)
    {
        lock (_instance)
        {
            return action();
        }
    }
}