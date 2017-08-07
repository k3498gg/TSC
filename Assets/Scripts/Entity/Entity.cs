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

    private bool isRecoverEnergy = false;

    private int m_heroId;

    private StateType m_state = StateType.NONE;

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

    public StateType State
    {
        get
        {
            return m_state;
        }

        set
        {
            m_state = value;
        }
    }

    public int HeroId
    {
        get
        {
            return m_heroId;
        }

        set
        {
            m_heroId = value;
        }
    }

    public void InitEntity(int heroId)
    {
        Debuger.LogError("CreateEntity 主角模型");
        HeroId = heroId;
        GameObject go = SpawnerGO(heroId);
        if (null == go)
        {
            Debuger.LogError("角色模型不存在Id:" + heroId);
            return;
        }
        InitCharactor(go);
        HeroInfo heroInfo = InfoMgr<HeroInfo>.Instance.GetInfo(heroId);
        InitEntityAttribute(heroInfo);
    }

    GameObject SpawnerGO(int heroId)
    {
        HeroInfo heroInfo = InfoMgr<HeroInfo>.Instance.GetInfo(heroId);
        GameObject go = ResourcesMgr.Instance.Spawner(heroInfo.model, ResourceType.RESOURCE_ENTITY, transform);// ResourcesMgr.Instance.Instantiate(prefab);
        if (null == go)
        {
            Debuger.LogError("角色模型不存在Id:" + heroId);
        }
        return null;
    }

    //初始化主角
    /*public*/ void InitCharactor(GameObject go)
    {
        if (null == go)
        {
            return;
        }
        RoleModel = go.transform;
        RoleModel.parent = CacheModel;
        RoleModel.localScale = Vector3.one;
        RoleModel.localPosition = Vector3.zero;
        RoleModel.localRotation = Quaternion.identity;
        GameMgr.Instance.ARPGAnimatController.Init(RoleModel);
        GameMgr.Instance.CameraController.SetTarget(CacheModel);
    }

    /*public*/ void InitEntityAttribute(HeroInfo info)
    {
        ConstInfo speedConstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_SPEED);
        Attribute.BaseSpeed = float.Parse(speedConstInfo.data) / AppConst.factor;
        Attribute.Speed = Attribute.BaseSpeed;
        ConstInfo rebornconstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_REBORN);
        Attribute.RebornTime = int.Parse(rebornconstInfo.data) / AppConst.factor;
        ConstInfo atkDisconstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_ATK_RADIO);
        Attribute.Atkdis = int.Parse(atkDisconstInfo.data) / AppConst.factor;
        ConstInfo maxPhyconstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_MAX_PHY);
        Attribute.MaxPhy = int.Parse(maxPhyconstInfo.data) / AppConst.factor;
        Attribute.CurPhy = Attribute.MaxPhy;
        ConstInfo perPhyconstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_COST_PHY_SPEED);
        Attribute.CostPhySpeed = int.Parse(perPhyconstInfo.data) / AppConst.factor;

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

    //特效加载
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
            ParticleMgr.Instance.Despawner(ResourceType.RESOURCE_PARTICLE, CacheSkillParticleTran);
            CacheSkillParticleTran = null;
        }
    }

    void DespawnerAccelerateParticle()
    {
        if (null != CacheAccelParticleTran)
        {
            ParticleMgr.Instance.Despawner(ResourceType.RESOURCE_PARTICLE,CacheAccelParticleTran);
            CacheAccelParticleTran = null;
        }
    }

    //取消上一次的道具状态
    void CancelLastState(StateType lastStaty)
    {
        switch (lastStaty)
        {
            case StateType.STATE_MARK:
                DesMark();
                break;
            case StateType.STATE_MAGNET:
                break;
            case StateType.STATE_TRANSFERGATE:
                break;
            case StateType.STATE_SPEED:
                break;
            case StateType.STATE_PROTECT:
                break;
        }
    }
    

    public void UpdateState(StateType stateType,ItemInfo item)
    {
        if(this.State == stateType)
        {
            
            return;
        }
        CancelLastState(State);
        this.State = stateType;
        switch (stateType)
        {
            case StateType.STATE_MARK:
                MarkUpdate();
                break;
            case StateType.STATE_MAGNET:
                MagnetUpdate();
                break;
            case StateType.STATE_TRANSFERGATE:
                TransferUpdate();
                break;
            case StateType.STATE_SPEED:
                SpeedUpdate();
                break;
            case StateType.STATE_PROTECT:
                ProtectUpdate();
                break;
        }
    }

    //取消变身
    void DesMark()
    {

    }


    void MarkUpdate()
    {
        Debug.LogError("变身效果");
        HeroInfo heroInfo = InfoMgr<HeroInfo>.Instance.GetInfo(4);
        GameObject go = ResourcesMgr.Instance.Spawner(heroInfo.model, ResourceType.RESOURCE_ENTITY, CacheModel);// ResourcesMgr.Instance.Instantiate(prefab);
        if (null == go)
        {
            return;
        }
        ResourcesMgr.Instance.Despawner(ResourceType.RESOURCE_ENTITY, RoleModel);
        InitCharactor(go);
    }

    //吸鐵石
    void MagnetUpdate()
    {
        Debug.LogError("吸铁石");
    }

    void TransferUpdate()
    {
        Debug.LogError("传送门");
    }

    void SpeedUpdate()
    {
        Debug.LogError("速度变化");
    }

    void ProtectUpdate()
    {
        Debug.LogError("保护时间");
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

        foreach (KeyValuePair<int, DropItemInfo> kv in TSCData.Instance.DropItemDic)
        {
            if(kv.Value.IsLock)
            {
                continue;
            }
            if (Util.PtInCircleArea(kv.Value.Cache, CacheModel, Attribute.Atkdis))
            {
                Debug.LogError("Eat:" + kv.Key + " " + kv.Value.ItemId　+" "+(ItemType) kv.Value.InfoId);
                kv.Value.FlyToEntity(this);
            }
        }
    }
}
