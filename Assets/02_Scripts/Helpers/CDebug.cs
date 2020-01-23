using UnityEngine;

public static class CDebug
{
    public static void Log(string message)
    {
        Debug.Log($"[{Time.realtimeSinceStartup}] {message}");
    }
    public static void LogWarning(string message)
    {
        Debug.LogWarning($"[{Time.realtimeSinceStartup}] <color=yellow>{message}</color>");
    }
    public static void LogError(string message)
    {
        Debug.LogError($"[{Time.realtimeSinceStartup}] <color=red>{message}</color>");
    }

    public static void Log(this Component component, string message)
    {
        Debug.Log($"[{Time.realtimeSinceStartup}] <color=blue>[{component.GetType().FullName}]</color> {message}");
    }
    public static void LogWarning(this Component component, string message)
    {
        Debug.LogWarning($"[{Time.realtimeSinceStartup}] <color=blue>[{component.GetType().FullName}]</color> <color=yellow>{message}</color>");
    }
    public static void LogError(this Component component, string message)
    {
        Debug.LogError($"[{Time.realtimeSinceStartup}] <color=blue>[{component.GetType().FullName}]</color> <color=red>{message}</color>");
    }

    public static void Log(this IDebugComponent component, string message)
    {
        Debug.Log($"[{Time.realtimeSinceStartup}] <color=blue>[{component.ComponentName}]</color> {message}");
    }
    public static void LogWarning(this IDebugComponent component, string message)
    {
        Debug.LogWarning($"[{Time.realtimeSinceStartup}] <color=blue>[{component.ComponentName}]</color> <color=yellow>{message}</color>");
    }
    public static void LogError(this IDebugComponent component, string message)
    {
        Debug.LogError($"[{Time.realtimeSinceStartup}] <color=blue>[{component.ComponentName}]</color> <color=red>{message}</color>");
    }
}