using UnityEngine;

public static class Timer
{
    static float startTime;
    public static void StartTimer() => startTime = Time.time;
    public static float GetTime() => Time.time - startTime;
}
