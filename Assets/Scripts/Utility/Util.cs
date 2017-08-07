using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class Util
{
    // 网络可用
    public static bool NetAvailable
    {
        get
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }

    // 是否是无线
    public static bool IsWifi
    {
        get
        {
            return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        }
    }

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

    public static void InitMap(string file)
    {
        TSCData.Instance.DicMapInfo.Clear();
        List<MapInfo> mapInfos = Util.DeSerialize<List<MapInfo>>(file);
        for (int i = 0; i < mapInfos.Count; i++)
        {
            TSCData.Instance.DicMapInfo[mapInfos[i].Id] = mapInfos[i];
        }
    }

    public static void Copy(string source, string dest)
    {
        string[] files = Directory.GetFiles(source, "*.bin", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            string str = files[i].Remove(0, source.Length);
            string path = dest + "/" + str;
            string dir = Path.GetDirectoryName(path);
            Directory.CreateDirectory(dir);
            File.Copy(files[i], path, true);
        }
    }

    public static bool Equals(string str1, string str2, System.StringComparison comparison = System.StringComparison.Ordinal)
    {
        return str1.Equals(str2, comparison);
    }

    public static void AddChildToTarget(Transform target, Transform child)
    {
        child.parent = target;
        child.localScale = Vector3.one;
        child.localPosition = Vector3.zero;
        child.localEulerAngles = Vector3.zero;
        ChangeChildLayer(child, target.gameObject.layer);
    }

    public static void ChangeChildLayer(Transform t, int layer)
    {
        t.gameObject.layer = layer;
        for (int i = 0; i < t.childCount; ++i)
        {
            Transform child = t.GetChild(i);
            child.gameObject.layer = layer;
            ChangeChildLayer(child, layer);
        }
    }

    public static void SetActive(GameObject go, bool active)
    {
        if (null != go)
        {
            if (go.activeSelf != active)
            {
                go.SetActive(active);
            }
        }
    }

    public static T AddComponent<T>(GameObject go) where T : Component
    {
        if (null != go)
        {
            T t = go.GetComponent<T>();
            if (null == t)
            {
                t = go.AddComponent<T>();
            }
            return t;
        }
        return default(T);
    }

    public static T GetComponent<T>(GameObject go) where T : Component
    {
        if (null != go)
        {
            return go.GetComponent<T>();
        }
        return default(T);
    }

    public static bool PtInRectArea(Transform self, Transform target, float width, float height)
    {
        Vector3 right = target.right;
        right = new Vector3(right.x, 0, right.z);
        Vector3 forward = target.forward;
        forward = new Vector3(forward.x, 0, forward.z);
        Vector3 dir = self.position - target.position;
        if (Mathf.Abs(Vector3.Dot(dir, right)) < width * 0.5f)
        {
            if (Mathf.Abs(Vector3.Dot(dir, forward)) < height * 0.5f)
            {
                return true;
            }
        }
        return false;
    }

    public static bool PtInCircleArea(Transform self, Transform target, float radio)
    {
        Vector2 s_v = new Vector2(self.position.x, self.position.z);
        Vector2 t_v = new Vector2(target.position.x, target.position.z);
        return Vector2.SqrMagnitude(s_v - t_v) < radio * radio;
    }

    public static string GetPoolName(ResourceType type)
    {
        string name = string.Empty;
        switch (type)
        {
            case ResourceType.RESOURCE_UI:
                name = "UI";
                break;
            case ResourceType.RESOURCE_ENTITY:
                name = "Entity";
                break;
            case ResourceType.RESOURCE_PARTICLE:
                name = "Particle";
                break;
            case ResourceType.RESOURCE_ANIMATOR:
                name = "Animator";
                break;
            case ResourceType.RESOURCE_ITEM:
                name = "Item";
                break;
            case ResourceType.RESOURCE_OBSTACLE:
                name = "Obstacle";
                break;
        }
        return name;
    }


    public static string GetPrefabPath(ResourceType type)
    {
        string path = string.Empty;
        switch (type)
        {
            case ResourceType.RESOURCE_ANIMATOR:
                path = AppConst.AnimatorPrefabPath;
                break;
            case ResourceType.RESOURCE_ENTITY:
                path = AppConst.EntityPrefabPath;
                break;
            case ResourceType.RESOURCE_PARTICLE:
                path = AppConst.ParticlePrefabPath;
                break;
            case ResourceType.RESOURCE_UI:
                path = AppConst.UIPrefabPath;
                break;
            case ResourceType.RESOURCE_ITEM:
                path = AppConst.ItemPrefabPath;
                break;
            case ResourceType.RESOURCE_OBSTACLE:
                path = AppConst.ObstaclePrefabPath;
                break;
        }
        return path;
    }

    public static void Serialize<T>(T t, string path)
    {
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            formatter.Serialize(stream, t);
            stream.Flush();
            stream.Close();
        }
        catch (Exception e)
        {
            Debuger.LogError(e.Message);
        }
    }

    public static T DeSerialize<T>(string path)
    {
        if (!File.Exists(path))
        {
            return default(T);
        }

        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            T t = (T)formatter.Deserialize(stream);
            stream.Close();
            return t;
        }
        catch (Exception e)
        {
            Debuger.LogError(e.Message);
        }
        return default(T);
    }



}
