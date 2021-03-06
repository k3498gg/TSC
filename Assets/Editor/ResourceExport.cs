﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Text;
using UnityEditor.SceneManagement;

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


    #region 导出道具布局文件
    [MenuItem("Export/ExportItemMap")]
    static void ExportItemMap()
    {
        List<MapInfo> maps = new List<MapInfo>();
        for (int idx = 0; idx < EditorBuildSettings.scenes.Length; idx++)
        {
            EditorBuildSettingsScene s = EditorBuildSettings.scenes[idx];
            if (s.enabled)
            {
                //EditorSceneManager.OpenScene(s.path);
                MapInfo mapInfo = new MapInfo();
                mapInfo.Id = idx;

                GameObject ground = GameObject.FindGameObjectWithTag("ground");
                Vector3 scale = ground.transform.localScale;
                Vector3 bound = ground.GetComponent<MeshFilter>().sharedMesh.bounds.extents;
                mapInfo.Width = bound.x * scale.x;
                mapInfo.Height = bound.z * scale.z;

                if (null != mapInfo.ItemMapInfo)
                {
                    mapInfo.ItemMapInfo.Clear();
                }
                else
                {
                    mapInfo.ItemMapInfo = new List<ItemMapInfo>();
                }

                int index = 0;
                GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
                if (items.Length > 0)
                {
                    foreach (GameObject go in items)
                    {
                        Transform t = go.transform;
                        MapArea area = MapArea.NONE;
                        DropMapArea a = t.parent.GetComponent<DropMapArea>();
                        if (null != a)
                        {
                            area = a.mapArea;
                        }
                        Vector3 v = t.position;
                        float w = t.localScale.x * 0.5f;
                        float h = t.localScale.z * 0.5f;
                        ItemMapInfo m = new ItemMapInfo(ItemMapType.ITEM, area, v.x, v.z, w, h);
                        m.Index = index++;
                        mapInfo.ItemMapInfo.Add(m);
                    }
                }

                GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
                if (points.Length > 0)
                {
                    foreach (GameObject go in points)
                    {
                        Transform t = go.transform;
                        MapArea area = MapArea.NONE;
                        DropMapArea a = t.parent.GetComponent<DropMapArea>();
                        if (null != a)
                        {
                            area = a.mapArea;
                        }
                        Vector3 v = t.position;
                        float w = t.localScale.x * 0.5f;
                        float h = t.localScale.z * 0.5f;
                        ItemMapInfo m = new ItemMapInfo(ItemMapType.POINT, area, v.x, v.z, w, h);
                        m.Index = index++;
                        mapInfo.ItemMapInfo.Add(m);
                    }
                }

                GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
                if (obstacles.Length > 0)
                {
                    //foreach (GameObject go in obstacles)
                    for (int obi = 0; obi < obstacles.Length; obi++)
                    {
                        Transform t = obstacles[obi].transform;
                        ObstacleType otype = Util.GetComponent<ObstacleType>(obstacles[obi]);
                        Vector3 v = t.position;
                        Obstacle obs = new Obstacle();
                        obs.Id = obi+1;
                        obs.PosX = v.x;
                        obs.PosY = v.z;
                        obs.Poshh = v.y;
                        obs.Rotate = t.localRotation.y;
                        obs.Width = t.localScale.x;
                        obs.Height = t.localScale.z;
                        obs.Hh = t.localScale.y;
                        obs.ObsType = otype.obstacle_type;
                        mapInfo.Obstacles.Add(obs);
                    }
                }

                maps.Add(mapInfo);
            }
        }
        Util.Serialize<List<MapInfo>>(maps, AppConst.AppStreamingPath + "/" + AppConst.TextDir + "/map.bin");
        AssetDatabase.Refresh();
    }

    [MenuItem("Export/ReadItemMap")]
    static void ReadItemMap()
    {
        List<MapInfo> maps = Util.DeSerialize<List<MapInfo>>(AppConst.AppStreamingPath + "/" + AppConst.TextDir + "/map.bin");
        for (int i = 0; i < maps.Count; i++)
        {
            Debuger.LogError(maps[i].Id);
            Debuger.LogWarning("--------------------------");
            for (int j = 0; j < maps[i].ItemMapInfo.Count; j++)
            {
                Debuger.LogError(maps[i].ItemMapInfo[j].ToString());
            }

            for(int j = 0; j < maps[i].Obstacles.Count;j++)
            {
                Debuger.LogError(maps[i].Obstacles[j].ToString());
            }
        }
    }


    #endregion
}
