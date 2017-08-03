using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : IEntity
{
    //角色属性
    private EntityAttribute attribute;
    private Transform cacheAccelParticleTran; //加速特效
    private Transform cacheSkillParticleTran; //技能特效
    private Transform cacheParticleParent; //模型特效父节点
    //子节点，角色模型
    private Transform roleModel;
    //当前节点模型Entity
    private Transform cacheModel;
    //当前人物强制移动
    private bool isForceMove = false;

    private bool isRecoverEnergy = false;

    public EntityAttribute Attribute
    {
        get
        {
            if (null == attribute)
            {
                attribute = new EntityAttribute();
            }
            return attribute;
        }
        set
        {
            attribute = value;
        }
    }

    public Transform RoleModel
    {
        get
        {
            return roleModel;
        }
        set
        {
            roleModel = value;
        }
    }

    //当前Entity脚本所在的transform
    public Transform CacheModel
    {
        get
        {
            if (null == cacheModel)
            {
                cacheModel = transform;
            }
            return cacheModel;
        }
        private set
        {
            cacheModel = value;
        }
    }

    public bool IsForceMove
    {
        get
        {
            return isForceMove;
        }

        set
        {
            isForceMove = value;
        }
    }

    public bool IsRecoverEnergy
    {
        get
        {
            return isRecoverEnergy;
        }

        set
        {
            isRecoverEnergy = value;
        }
    }

    public Transform CacheAccelParticleTran
    {
        get
        {
            return cacheAccelParticleTran;
        }

        set
        {
            cacheAccelParticleTran = value;
        }
    }

    public Transform CacheSkillParticleTran
    {
        get
        {
            return cacheSkillParticleTran;
        }

        set
        {
            cacheSkillParticleTran = value;
        }
    }

    public Transform CacheParticleParent
    {
        get
        {
            if (null == cacheParticleParent)
            {
                if (null != RoleModel)
                {
                    cacheParticleParent = RoleModel.Find("p");
                }
            }
            return cacheParticleParent;
        }

        set
        {
            cacheParticleParent = value;
        }
    }

    //初始化主角
    public void InitCharactor(GameObject go)
    {
        if (null == go)
        {
            return;
        }
        RoleModel = go.transform;
        RoleModel.parent = CacheModel;
        RoleModel.localScale = Vector3.one;
        GameMgr.Instance.ARPGAnimatController.Init(RoleModel);
        GameMgr.Instance.CameraController.SetTarget(CacheModel);
    }

    public void InitEntityAttribute(HeroInfo info)
    {
        ConstInfo speedConstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_SPEED);
        Attribute.BaseSpeed = float.Parse(speedConstInfo.data) / AppConst.factor;
        Attribute.Speed = Attribute.BaseSpeed;
        ConstInfo rebornconstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_REBORN);
        Attribute.RebornTime = uint.Parse(rebornconstInfo.data) / AppConst.factor;
        ConstInfo atkDisconstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_ATK_RADIO);
        Attribute.Atkdis = uint.Parse(atkDisconstInfo.data) / AppConst.factor;
        ConstInfo maxPhyconstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_MAX_PHY);
        Attribute.MaxPhy = uint.Parse(maxPhyconstInfo.data) / AppConst.factor;
        Attribute.CurPhy = Attribute.MaxPhy;
        ConstInfo perPhyconstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_COST_PHY_SPEED);
        Attribute.CostPhySpeed = uint.Parse(perPhyconstInfo.data) / AppConst.factor;

        OccupationInfo occpInfo = InfoMgr<OccupationInfo>.Instance.GetInfo(info.occupationId);
        Attribute.Skills = occpInfo.skillId;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debuger.LogError(" 进入了" + other.transform.name);
        if (other.CompareTag("Item"))
        {
            DropItemInfo drop = other.GetComponent<DropItemInfo>();
            if (null != drop)
            {
                drop.FlyToEntity(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debuger.LogError(" 离开了" + other.transform.name);
    }

    public void DespawnerParticle(EffectType type)
    {
        switch (type)
        {
            case EffectType.ACCELERATE:
                DespawnerAccelerateParticle();
                break;
            case EffectType.WALKINSTANT:
                DespawnerWalkInstanceParticle();
                break;
        }
    }

    public void SpawnerParticle(int id, EffectType type, Vector3 v, Transform parent)
    {
        DespawnerParticle(type);
        GameObject inst = ParticleMgr.Instance.Spawner(id, v, parent);
        if (null != inst)
        {
            switch (type)
            {
                case EffectType.ACCELERATE:
                    CacheAccelParticleTran = inst.transform;
                    break;
                case EffectType.WALKINSTANT:
                    CacheSkillParticleTran = inst.transform;
                    break;
            }
        }
    }

    void DespawnerWalkInstanceParticle()
    {
        if (null != CacheSkillParticleTran)
        {
            ParticleMgr.Instance.Despawner(CacheSkillParticleTran);
            CacheSkillParticleTran = null;
        }
    }

    void DespawnerAccelerateParticle()
    {
        if (null != CacheAccelParticleTran)
        {
            ParticleMgr.Instance.Despawner(CacheAccelParticleTran);
            CacheAccelParticleTran = null;
        }
    }

    //先用物理引擎去检测碰撞,效率低再优化
    float mTotalTime = 0;
    void Update()
    {
        mTotalTime += Time.deltaTime;
        if (mTotalTime <= 0.2f)
        {
            return;
        }

        mTotalTime = 0;

        foreach (KeyValuePair<int, DropItemInfo> kv in EntityMgr.Instance.DropItemDic)
        {
            if(kv.Value.IsLock)
            {
                continue;
            }
            if (Util.PtInCircleArea(kv.Value.Cache, CacheModel, Attribute.Atkdis))
            {
                Debug.LogError("Eat:" + kv.Key + " " + kv.Value.ItemId);
                kv.Value.FlyToEntity(this);
            }
        }
    }
}
