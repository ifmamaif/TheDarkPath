using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityWrapper
{
    public static GameObject InstantiateGameObject(string name = "GameObject")
    {
        return Dispatcher.RunOnUIThread(() =>
       {
           return new GameObject(name);
       });
    }

    public static GameObject[,] InstantiateGameObject(int x, int y)
    {
        return Dispatcher.RunOnUIThread(() =>
        {
            return new GameObject[x, y];
        });
    }

    public static void RunOnUIThread(Action func)
    {
        Dispatcher.RunOnUIThread(() =>
       {
           func();
       });
    }
}