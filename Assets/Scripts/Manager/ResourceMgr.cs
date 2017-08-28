using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesMgr : Singleton<ResourcesMgr>
{
    public T LoadResource<T>(ResourceType type, string name) where T : UnityEngine.Object
    {
        string path = Util.GetPrefabPath(type) + name;
        return (T)Resources.Load(path);
    }

    //public Transform Spawner(ResourceType t,Transform prefab, Transform parent, Vector3 v)
    //{
    //    return PoolMgr.Instance.SpawnerEntity(t,prefab, parent, v);
    //}

    public GameObject Spawner(string aName, Vector3 v, ResourceType mtype, Transform parent)
    {
        Transform tran = PoolMgr.Instance.SpawnerEntity(aName, v, mtype, parent);
        if (null == tran)
        {
            return null;
        }
        return tran.gameObject;
    }


    public GameObject Spawner(string aName, ResourceType mtype, Transform parent)
    {
        Transform tran = PoolMgr.Instance.SpawnerEntity(aName, Vector3.zero, mtype, parent);
        if (null == tran)
        {
            return null;
        }
        return tran.gameObject;
    }

    public void Despawner(ResourceType type,Transform inst)
    {
        PoolMgr.Instance.Despawner(type, inst);
    }
}
