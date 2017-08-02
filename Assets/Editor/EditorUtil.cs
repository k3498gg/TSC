using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EditorUtil
{
    public static void Init<T>(string path) where T : IInfo, new()
    {
        string file = path + typeof(T).ToString().ToLower() + ".bin";
        if (!File.Exists(file))
        {
            Debug.LogError("file not exits:" + file);
            return;
        }
        InfoMgr<T>.Instance.Init(file);
    }

    public static void ClearConsole()
    {
        var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod.Invoke(null, null);
    }


}
