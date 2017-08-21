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


    void CreatePoolPrefab(SpawnPool pool, string aName, Transform prefab, ResourceType t)
    {
        if (null == prefab)
        {
            return;
        }

        if (null == pool.GetPrefabPool(prefab))
        {
            PrefabPool prefabPool = new PrefabPool(prefab);
            prefabPool.preloadAmount = 0;
            prefabPool.cullDespawned = Util.GetDespawnFlag(t);
            prefabPool.cullAbove = 5;
            prefabPool.cullDelay = Util.GetDespawnTime(t);
            pool.CreatePrefabPool(prefabPool);
            pool.prefabPools[aName] = prefabPool;
        }
    }

    public Transform GetPoolRoot(ResourceType t)
    {
        string poolName = Util.GetPoolName(t);
        if (PoolManager.Pools.ContainsKey(poolName))
        {
            SpawnPool pool = PoolManager.Pools[poolName];
            return pool.group;
        }
        Debuger.LogError("Error...pool group");
        return null;
    }

    SpawnPool GetSpawnPool(ResourceType t)
    {
        string poolName = Util.GetPoolName(t);
        if (PoolManager.Pools.ContainsKey(poolName))
        {
            SpawnPool pool = PoolManager.Pools[poolName];
            return pool;
        }
        Debuger.LogError("Error...pool group");
        return null;
    }

    public Transform SpawnerEntity(ResourceType type, Transform prefab, Transform parent, Vector3 v)
    {
        SpawnPool pool = GetSpawnPool(type);
        if (null != pool)
        {
            return SpawnerEntity(pool, prefab, parent, v);
        }
        return null;
    }

    Transform SpawnerEntity(SpawnPool pool, Transform prefab, Transform parent, Vector3 v)
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
            CreatePoolPrefab(pool, avatarName, prefab.transform, rType);
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
            Transform t = GetPoolRoot(type);
            SpawnPool pool = PoolManager.Pools[poolName];
            while (pool._spawned.Count > 0)
            {
                pool._spawned[0].parent = t;
                pool.Despawn(pool._spawned[0]);
            }
            //foreach(Transform inst in pool._spawned)
            //{
            //    pool.Despawn(inst);
            //    inst.parent = t;
            //}
        }
    }

    public void Despawner(ResourceType type, Transform inst)
    {
        string poolName = Util.GetPoolName(type);
        if (PoolManager.Pools.ContainsKey(poolName))
        {
            SpawnPool pool = PoolManager.Pools[poolName];
            Despawner(pool, inst);
        }
    }

    void Despawner(SpawnPool pool, Transform inst)
    {
        if (null != pool && null != inst)
        {
            if (pool.IsSpawned(inst))
            {
                pool.Despawn(inst);
                inst.parent = pool.group;
            }
        }
    }

    //按顺序回收资源
    public void DespawnerAll()
    {
        Despawner(ResourceType.RESOURCE_PARTICLE);
        Despawner(ResourceType.RESOURCE_ANIMATOR);
        Despawner(ResourceType.RESOURCE_OBSTACLE);
        Despawner(ResourceType.RESOURCE_ITEM);
        Despawner(ResourceType.RESOURCE_ENTITY);
        Despawner(ResourceType.RESOURCE_NET);
    }
}
