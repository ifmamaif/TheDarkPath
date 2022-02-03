using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class Utils
{
    public static void MessureTime(Action func, string nameFunc = "Call function duration")
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        func();
        stopwatch.Stop();
        long milliseconds = stopwatch.ElapsedMilliseconds;
        UnityEngine.Debug.Log(nameFunc + " time: " + milliseconds + " ms");
    }

    public static T MessureTime<T>(Func<T> func, string nameFunc = "Call function duration")
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        T rezult = func();
        stopwatch.Stop();
        long milliseconds = stopwatch.ElapsedMilliseconds;
        UnityEngine.Debug.Log(nameFunc + " time: " + milliseconds + " ms");
        return rezult;
    }

    public static float Lerp(float a0, float a1, float weight)
    {
        return a0 + weight * (a1 - a0);
    }
}