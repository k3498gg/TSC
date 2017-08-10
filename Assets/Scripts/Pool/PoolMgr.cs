using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

//同步方法，不需要回掉处理事件
public class PoolMgr : UnitySingleton<PoolMgr>
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        Destroy(gameObject);
    }

    SpawnPool CreatePool(string poolName)
    {
        SpawnPool pool = null;
        if (PoolManager.Pools.ContainsKey(poolName))
        {
            pool = PoolManager.Pools[poolName];
        }
        else
        {
            pool = PoolManager.Pools.Create(poolName);
            pool.group.parent = transform;
            pool.group.localPosition = Vector3.zero;
            pool.group.localRotation = Quaternion.identity;
        }
        return pool;
    }


    void CreatePoolPrefab(SpawnPool pool, string aName, Transform prefab)
    {
        if (null == prefab)
        {
            return;
        }

        if (null == pool.GetPrefabPool(prefab))
        {
            PrefabPool prefabPool = new PrefabPool(prefab);
            prefabPool.preloadAmount = 0;
            prefabPool.cullDespawned = true;
            prefabPool.cullAbove = 1;
            prefabPool.cullDelay = 1;
            pool.CreatePrefabPool(prefabPool);
            pool.prefabPools[aName] = prefabPool;
        }
    }



    public Transform SpawnerEntity(SpawnPool pool, Transform prefab, Transform parent, Vector3 v)
    {
        if (null != prefab && null != parent)
        {
            return pool.Spawn(prefab, v, Quaternion.identity, parent);
        }
        return null;
    }

    public Transform SpawnerEntity(string avatarName, Vector3 v, ResourceType rType, Transform parent)
    {
        string poolName = Util.GetPoolName(rType);
        SpawnPool pool = CreatePool(poolName);
        if (null == pool)
        {
            Debuger.LogError("init pool failed:" + poolName);
            return null;
        }

        if (!pool.prefabPools.ContainsKey(avatarName))
        {
            GameObject prefab = ResourcesMgr.Instance.LoadResource<GameObject>(rType, avatarName);
            if (null == prefab)
            {
                Debuger.LogError(avatarName + " is NULL!");
                return null;
            }
            CreatePoolPrefab(pool, avatarName, prefab.transform);
        }
        if (pool.prefabPools.ContainsKey(avatarName))
        {
            Transform tran = SpawnerEntity(pool, pool.prefabPools[avatarName].prefab, parent, v);
            return tran;
        }
        return null;
    }

    public void Despawner(ResourceType type)
    {
        string poolName = Util.GetPoolName(type);
        if (PoolManager.Pools.ContainsKey(poolName))
        {
            SpawnPool pool = PoolManager.Pools[poolName];
            pool.DespawnAll();
        }
    }

    public void Despawner(ResourceType type,Transform inst)
    {
        string poolName = Util.GetPoolName(type);
        if (PoolManager.Pools.ContainsKey(poolName))
        {
            SpawnPool pool = PoolManager.Pools[poolName];
            Despawner(pool, inst);
        }
    }

    public void Despawner(SpawnPool pool, Transform inst)
    {
        if (null != pool && null != inst)
        {
            if (pool.IsSpawned(inst))
            {
                pool.Despawn(inst);
            }
        }
    }

    public void DespawnerAll()
    {
       
        foreach(KeyValuePair<string, SpawnPool> kv in PoolManager.Pools)
        {
            if (null != kv.Value)
            {
                kv.Value.DespawnAll();
            }
        }
    }
}
