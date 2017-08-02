using UnityEngine;
using System.Collections;

public class Debuger
{
    static public void Log(object message)
    {
        Log(message, null);
    }
    static public void Log(object message, Object context)
    {
        if (AppConst.DebugMode)
        {
            Debug.Log(message, context);
        }
    }
    static public void LogError(object message)
    {
        LogError(message, null);
    }

    static public void LogError(object message, Object context)
    {
        if (AppConst.DebugMode)
        {
            Debug.LogError(message, context);
        }
    }
    static public void LogWarning(object message)
    {
        LogWarning(message, null);
    }

    static public void LogWarning(object message, Object context)
    {
        if (AppConst.DebugMode)
        {
            Debug.LogWarning(message, context);
        }
    }
}
