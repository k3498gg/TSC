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
        Vector3 dir = new Vector3( self.position.x,0,self.position.z )- new Vector3( target.position.x,0,target.position.z);
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

    public static bool PtInRectArea(Vector3 self, Transform target, float width,float height)
    {
        Vector3 dir = new Vector3(self.x, 0, self.z) - new Vector3(target.position.x, 0, target.position.z) ;
        Vector3 right = target.right;
        right = new Vector3(right.x, 0, right.z);
        Vector3 forward = target.forward;
        forward = new Vector3(forward.x, 0, forward.z);
        if (Mathf.Abs(Vector3.Dot(dir, right)) < width * 0.5f)
        {
            if (Mathf.Abs(Vector3.Dot(dir, forward)) < height * 0.5f)
            {
                return true;
            }
        }
        return false;
    }

    public static int GetEntityLevelGap(int lev1,int lev2)
    {
        return lev1 - lev2;
    }


    public static float GetEntityDistance(Transform cache1,Transform cache2)
    {
        Vector2 v1 = new Vector2(cache1.position.x, cache1.position.z);
        Vector2 v2 = new Vector2(cache2.position.x, cache2.position.z);
        return Vector2.Distance(v1, v2);
        //return Mathf.Sqrt(Mathf.Pow((cache1.position.x - cache2.position.x), 2) + Mathf.Pow((cache1.position.y - cache2.position.y), 2));
    }

    public static bool GetDespawnFlag(ResourceType type)
    {
        bool despawn = false;
        switch (type)
        {
            case ResourceType.RESOURCE_UI:
                despawn = true;
                break;
            case ResourceType.RESOURCE_ENTITY:
                despawn = false;
                break;
            case ResourceType.RESOURCE_PARTICLE:
                despawn = true;
                break;
            case ResourceType.RESOURCE_ANIMATOR:
                despawn = true;
                break;
            case ResourceType.RESOURCE_ITEM:
                despawn = false;
                break;
            case ResourceType.RESOURCE_OBSTACLE:
                despawn = true;
                break;
            case ResourceType.RESOURCE_NET:
                despawn = false;
                break;
        }
        return despawn;
    }

    public static int GetDespawnTime(ResourceType type)
    {
        int time = 0;
        switch (type)
        {
            case ResourceType.RESOURCE_UI:
                time = 10;
                break;
            case ResourceType.RESOURCE_ENTITY:
                time = 10;
                break;
            case ResourceType.RESOURCE_PARTICLE:
                time = 5;
                break;
            case ResourceType.RESOURCE_ANIMATOR:
                time = 5;
                break;
            case ResourceType.RESOURCE_ITEM:
                time = 10;
                break;
            case ResourceType.RESOURCE_OBSTACLE:
                time = 5;
                break;
            case ResourceType.RESOURCE_NET:
                time = 20;
                break;
        }
        return time;
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
            case ResourceType.RESOURCE_NET:
                name = "Net";
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
            case ResourceType.RESOURCE_NET:
                path = AppConst.NetEntityPrefabPath;
                break;
            case ResourceType.RESOURCE_MGR:
                path = AppConst.MgrPrefabPath;
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

    public static StateType ConvertItemType(ItemType type)
    {
        StateType state = StateType.NONE;
        switch (type)
        {
            case ItemType.ITEM_MARK: //问号变身
                state = StateType.STATE_MARK;
                break;
            case ItemType.ITEM_MAGNET: //吸铁石
                state = StateType.STATE_MAGNET;
                break;
            case ItemType.ITEM_TRANSFERGATE: //传送门
                state = StateType.STATE_TRANSFERGATE;
                break;
            case ItemType.ITEM_SPEED://速度变化
                state = StateType.STATE_SPEED;
                break;
            case ItemType.ITEM_PROTECT: //保护罩
                state = StateType.STATE_PROTECT;
                break;
        }
        return state;
    }


    public static OccpType GetCanKillOccp(OccpType curType)
    {
        OccpType oc = OccpType.NONE;
        switch (curType)
        {
            case OccpType.Occp_TIGER:
                oc = OccpType.Occp_CHICK;
                break;
            case OccpType.Occp_STICK:
                oc = OccpType.Occp_TIGER;
                break;
            case OccpType.Occp_CHICK:
                oc = OccpType.Occp_STICK;
                break;
        }
        return oc;
    }

    public static bool IsSameOccp(OccpType occ1,OccpType occ2)
    {
        return occ1 == occ2;
    }

    public static OccpType GetNextOccp(OccpType curType)
    {
        OccpType oc = OccpType.NONE;
        switch (curType)
        {
            case OccpType.Occp_TIGER:
                oc = OccpType.Occp_STICK;
                break;
            case OccpType.Occp_STICK:
                oc = OccpType.Occp_CHICK;
                break;
            case OccpType.Occp_CHICK:
                oc = OccpType.Occp_TIGER;
                break;
        }
        return oc;
    }

    public static bool CanKillBody(OccpType type1,OccpType type2)
    {
        if(type1 == OccpType.Occp_TIGER)
        {
            if(type2 == OccpType.Occp_CHICK)
            {
                return true;
            }
        }else if (type1 == OccpType.Occp_STICK)
        {
            if (type2 == OccpType.Occp_TIGER)
            {
                return true;
            }
        }
        else if (type1 == OccpType.Occp_CHICK)
        {
            if (type2 == OccpType.Occp_STICK)
            {
                return true;
            }
        }

        return false;
    }

    public static int GetHeroIdByOccp(OccpType occp)
    {
        int id = 1;
        switch (occp)
        {
            case OccpType.Occp_TIGER:
                id = 1;
                break;
            case OccpType.Occp_STICK:
                id = 2;
                break;
            case OccpType.Occp_CHICK:
                id = 3;
                break;
        }
        return id;
    }

    public static int GetDropRareIndex()
    {
        if(AppConst.DropRareIndex == int.MaxValue)
        {
            AppConst.DropRareIndex = 0;
        }
        return AppConst.DropRareIndex++;
    }
}
