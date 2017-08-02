using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Text;

public class ResourceExport
{
    #region 配置表下载到C盘
    [MenuItem("Export/ExportBinInfo")]
    static void ClearVersion()
    {
        ClearPlayerPrefs();
        ExportBinInfo();
    }

    static void ExportBinInfo()
    {
        string[] files = Directory.GetFiles(AppConst.AppStreamingPath + "/" + AppConst.TextDir, "*.bin", SearchOption.AllDirectories);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < files.Length; i++)
        {
            string file = files[i].Replace(@"\", @"/");
            if (File.Exists(file))
            {
                FileInfo info = new FileInfo(file);
                sb.Append(info.Name).Append("|").Append(info.Length);
                if (i < files.Length - 1)
                {
                    sb.AppendLine();
                }
            }
        }
        File.WriteAllText(AppConst.AppStreamingPath + "/file.txt", sb.ToString());
    }

    static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
    #endregion


    #region 导出道具显示布局

    static void ExportItemMap()
    {
        foreach(EditorBuildSettingsScene s in EditorBuildSettings.scenes)
        {
            if(s.enabled)
            {
                GameObject.FindGameObjectsWithTag("Item");
            }
        }
    }

    #endregion
}
