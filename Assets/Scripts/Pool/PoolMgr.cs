using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

//同步方法，不需要回掉处理事件
public class PoolMgr : UnitySingleton<PoolMgr>
{
    private SpawnPool pool;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        Destroy(gameObject);
    }

    void CreatePool(string poolName)
    {
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
    }


    void CreatePoolPrefab(string aName, Transform prefab)
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
            prefabPool.cullDelay = 5;
            pool.CreatePrefabPool(prefabPool);
            pool.prefabPools[aName] = prefabPool;
        }
    }



    Transform SpawnerEntity(Transform prefab, Transform parent, Vector3 v)
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
        if (!PoolManager.Pools.ContainsKey(poolName))
        {
            CreatePool(poolName);
        }

        if (!pool.prefabPools.ContainsKey(avatarName))
        {
            GameObject prefab = ResourcesMgr.Instance.LoadResource<GameObject>(rType, avatarName);
            if (null == prefab)
            {
                Debuger.LogError(avatarName + " is NULL!");
                return null;
            }
            CreatePoolPrefab(avatarName, prefab.transform);
        }
        if (pool.prefabPools.ContainsKey(avatarName))
        {
            Transform tran = SpawnerEntity(pool.prefabPools[avatarName].prefab, parent, v);
            return tran;
        }
        return null;
    }

    public void Despawner(Transform inst)
    {
        if (null != this.pool && null != inst)
        {
            if (this.pool.IsSpawned(inst))
            {
                this.pool.Despawn(inst);
            }
        }
    }

    public void DespawnerAll()
    {
        if (null != this.pool)
        {
            this.pool.DespawnAll();
        }
    }
}
