using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMgr : Singleton<ParticleMgr>
{
    public GameObject Spawner(string name, Vector3 v, Transform parent)
    {
        Transform inst = PoolMgr.Instance.SpawnerEntity(name, v, ResourceType.RESOURCE_PARTICLE, parent);
        if (null != inst)
        {
            return inst.gameObject;
        }
        Debuger.LogError("Particle is Null : " + name);
        return null;
    }

    public GameObject Spawner(int effId, Vector3 v, Transform parent)
    {
        ParticleInfo particleInfo = InfoMgr<ParticleInfo>.Instance.GetInfo(effId);
        if (null != particleInfo)
        {
            return Spawner(particleInfo.data, v, parent);
        }
        Debuger.LogError("Particle is Null : " + effId);
        return null;
    }

    public void Despawner(Transform inst)
    {
        PoolMgr.Instance.Despawner(inst);
    }
}
