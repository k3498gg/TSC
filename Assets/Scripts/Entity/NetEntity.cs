using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetEntity : IEntity
{
    private int id;
    //角色属性
    private EntityAttribute attribute;
    private Transform cacheAccelParticleTran; //加速特效
    private Transform cacheSkillParticleTran; //技能特效
    private Transform cacheParticleParent; //模型特效父节点
    //子节点，角色模型
    private Transform roleModel;
    //当前节点模型Entity
    private Transform cacheModel;

    private ARPGAnimatorController m_arpgAnimatContorller;
    private CharacterController m_characterController;

    private int m_heroId;

    private bool init = false;

    private OccpType occupation = OccpType.NONE;

    private StateType m_state = StateType.NONE;

    private CollisionType collision = CollisionType.NONE;

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
                if (null != CacheModel)
                {
                    cacheParticleParent = CacheModel.Find("p");
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

    public OccpType Occupation
    {
        get
        {
            return occupation;
        }

        set
        {
            occupation = value;
        }
    }

    public ARPGAnimatorController ArpgAnimatContorller
    {
        get
        {
            if(null == m_arpgAnimatContorller)
            {
                m_arpgAnimatContorller = CacheModel.GetComponent<ARPGAnimatorController>();
            }
            return m_arpgAnimatContorller;
        }

        set
        {
            m_arpgAnimatContorller = value;
        }
    }

    public CharacterController CharacterController
    {
        get
        {
            if(null == m_characterController)
            {
                m_characterController = CacheModel.GetComponent<CharacterController>();
            }
            return m_characterController;
        }

        set
        {
            m_characterController = value;
        }
    }

    public bool IsAlive
    {
        get
        {
            return Attribute.Hp > 0;
        }
    }

    public int Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public void InitEntity(OccpType occp, int heroId)
    {
        ChangeOccp(occp, heroId);
        HeroInfo heroInfo = InfoMgr<HeroInfo>.Instance.GetInfo(heroId);
        InitEntityAttribute(heroInfo);
        init = true;
    }

    GameObject SpawnerGO(int heroId, Vector3 v)
    {
        HeroInfo heroInfo = InfoMgr<HeroInfo>.Instance.GetInfo(heroId);
        GameObject go = ResourcesMgr.Instance.Spawner(heroInfo.model, v, ResourceType.RESOURCE_ENTITY, transform);// ResourcesMgr.Instance.Instantiate(prefab);
        return go;
    }

    void ChangeOccp(OccpType occp, int heroId)
    {
        Occupation = occp;
        HeroId = heroId;
        HeroInfo heroInfo = InfoMgr<HeroInfo>.Instance.GetInfo(heroId);
        GameObject go = ResourcesMgr.Instance.Spawner(heroInfo.model, ResourceType.RESOURCE_ENTITY, CacheModel);// ResourcesMgr.Instance.Instantiate(prefab);
        if (null == go)
        {
            return;
        }
        ResourcesMgr.Instance.Despawner(ResourceType.RESOURCE_ENTITY, RoleModel);
        InitCharactor(go);
    }

    //初始化主角
    void InitCharactor(GameObject go)
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
        ArpgAnimatContorller = CacheModel.GetComponent<ARPGAnimatorController>();
        ArpgAnimatContorller.Init(RoleModel);
        //GameMgr.Instance.CameraController.SetTarget(CacheModel);
    }

    void InitEntityAttribute(HeroInfo info)
    {
        ConstInfo speedConstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_SPEED);
        Attribute.BaseSpeed = float.Parse(speedConstInfo.data) / AppConst.factor;
        Attribute.Speed = Attribute.BaseSpeed;
        ConstInfo rebornconstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_REBORN);
        Attribute.RebornTime = int.Parse(rebornconstInfo.data) / AppConst.factor;
        ConstInfo atkDisconstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_ATK_RADIO);
        Attribute.Basedis = int.Parse(atkDisconstInfo.data) / AppConst.factor;
        Attribute.Atkdis = Attribute.Basedis;
        ConstInfo maxPhyconstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_MAX_PHY);
        Attribute.MaxPhy = int.Parse(maxPhyconstInfo.data) / AppConst.factor;
        Attribute.CurPhy = Attribute.MaxPhy;
        ConstInfo perPhyconstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_COST_PHY_SPEED);
        Attribute.CostPhySpeed = int.Parse(perPhyconstInfo.data) / AppConst.factor;
        OccupationInfo occpInfo = InfoMgr<OccupationInfo>.Instance.GetInfo(info.occupationId);
        Attribute.Skills = occpInfo.skillId;
        Attribute.Score = 0;
        Attribute.Level = 0;
        Attribute.Hp = 100;
        Attribute.MaxHp = 100;
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
            ParticleMgr.Instance.Despawner(ResourceType.RESOURCE_PARTICLE, CacheAccelParticleTran);
            CacheAccelParticleTran = null;
        }
    }

    //积分变换，体力变换
    public void EnergyUpdate(ItemEffectInfo effect)
    {
        Attribute.Score = Attribute.Score + effect.param1;
        Attribute.CurPhy = Attribute.CurPhy+ effect.param2;
        if (Attribute.CurPhy > Attribute.MaxPhy)
        {
            Attribute.CurPhy = Attribute.MaxPhy;
        }
        Attribute.Level = GetCurrentLevel();
    }

    //等级变化
    public int GetCurrentLevel()
    {
        LevelInfo info = InfoMgr<LevelInfo>.Instance.GetInfo(Attribute.Level);
        Dictionary<int, LevelInfo> level = InfoMgr<LevelInfo>.Instance.Dict;
        int lev = 0;
        foreach (KeyValuePair<int,LevelInfo> kv in level)
        {
           if(Attribute.Score >= kv.Value.score)
            {
                lev = Mathf.Max(lev, kv.Value.id);
            }else
            {
                break;
            }
        }
        Debuger.LogError("Level:"+ lev);
        return lev;
    }

    public void UpdateState(StateType stateType, ItemInfo item, ItemEffectInfo effect)
    {
        this.State = stateType;
        switch (stateType)
        {
            case StateType.STATE_MARK:
                //TurnedBody(item);
                break;
            case StateType.STATE_MAGNET:
                Magnet(item, effect);
                break;
            case StateType.STATE_TRANSFERGATE:
                Transfer();
                break;
            case StateType.STATE_SPEED:
                ChangeSpeed(item, effect);
                break;
            case StateType.STATE_PROTECT:
                Protect();
                break;
        }
    }

    //取消变身
    void BackBody(Timer.TimerData data)
    {
        Debug.LogError("返回变身效果");
        HeroInfo heroInfo = InfoMgr<HeroInfo>.Instance.GetInfo(HeroId);
        GameObject go = ResourcesMgr.Instance.Spawner(heroInfo.model, ResourceType.RESOURCE_ENTITY, CacheModel);// ResourcesMgr.Instance.Instantiate(prefab);
        if (null == go)
        {
            return;
        }
        ResourcesMgr.Instance.Despawner(ResourceType.RESOURCE_ENTITY, RoleModel);
        InitCharactor(go);
    }


    void TurnedBody(ItemInfo item)
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
        Timer.Instance.AddTimer(item.timer, 1, true, BackBody);
    }

    //吸鐵石
    void Magnet(ItemInfo item, ItemEffectInfo effect)
    {
        Debug.LogError("吸铁石");
        float rate = (float)effect.param1 / AppConst.factor;
        this.Attribute.Atkdis = this.Attribute.Basedis * rate;
    }

    void Transfer()
    {
        Debug.LogError("传送门");
    }

    void ChangeSpeed(ItemInfo item, ItemEffectInfo effect)
    {
        Debug.LogError("速度变化");
        //加速道具 停止使用加速功能
        this.Attribute.Speed = Attribute.BaseSpeed * effect.param1 / AppConst.factor;
        Timer.Instance.AddTimer(item.timer, 1, true, BackSpeed);
    }

    void BackSpeed(Timer.TimerData data)
    {
        //发送事件 可以使用加速功能
        this.Attribute.Speed = Attribute.BaseSpeed;
    }

    void Protect()
    {
        Debug.LogError("保护时间");
    }

    void DetectItem()
    {
        foreach (KeyValuePair<int, DropItemInfo> kv in TSCData.Instance.DropItemDic)
        {
            if (kv.Value.IsLock)
            {
                continue;
            }
            if (Util.PtInCircleArea(kv.Value.Cache, CacheModel, Attribute.Atkdis))
            {
                kv.Value.FlyToEntity(this);
            }
        }
    }

    void DetectObstacle()
    {
        if (null != ArpgAnimatContorller)
        {
            if (ArpgAnimatContorller.Skill == 1)
            {
                foreach (KeyValuePair<int, ObstacleEntity> kv in TSCData.Instance.ObstacleDic)
                {
                    if (Util.PtInRectArea(CacheModel, kv.Value.Cache, kv.Value.Width + Attribute.Atkdis, kv.Value.Height + Attribute.Atkdis))
                    {
                        StopSkill(CollisionType.Collision_OBSTACLE);
                    }
                }
            }
        }
    }

    public void StopSkill(CollisionType type)
    {
        collision = type;
        Timer.Instance.RemoveTimer(TimerWalkInstantHandler);
        TimerWalkInstantHandler(null);
        collision = CollisionType.NONE;
    }


    void TimerWalkInstantHandler(Timer.TimerData data)
    {
        Attribute.Speed = Attribute.BaseSpeed;
        ArpgAnimatContorller.Skill = 0;
        DespawnerParticle(EffectType.WALKINSTANT);

        switch (collision)
        {
            case CollisionType.Collision_NET:
                Debug.LogError("碰到玩家了");
                break;
            case CollisionType.Collision_ITEM:
                Debug.LogError("碰到道具了");
                break;
            case CollisionType.Collision_OBSTACLE:
                Debug.LogError("碰到障碍物了");
                break;
            case CollisionType.NONE:
                Debug.LogError("自己停止了");
                OccpType occp = Util.GetNextOccp(occupation);
                int id = Util.GetHeroIdByOccp(occp, HeroId);
                ChangeOccp(occp, id);
                break;
        }
    }

    public void StopAccelerate()
    {
        Attribute.Speed = GameMgr.Instance.MainEntity.Attribute.BaseSpeed;
        DespawnerParticle(EffectType.ACCELERATE);
    }

    //加速
    public void Accelerate(SkillInfo skill, EffectInfo effectInfo)
    {
        //加速停止恢复能量条
        Attribute.Speed = Attribute.BaseSpeed * effectInfo.param / AppConst.factor;
        SpawnerParticle(skill.particleID, EffectType.ACCELERATE, CacheParticleParent.position, CacheParticleParent);
    }

    //冲锋
    public void Walkinstant(SkillInfo skill, EffectInfo effectInfo)
    {
        ArpgAnimatContorller.Skill = 1;
        Attribute.Speed = Attribute.BaseSpeed * effectInfo.param / AppConst.factor;
        SpawnerParticle(skill.particleID, EffectType.WALKINSTANT, CacheParticleParent.position, CacheParticleParent);
        Timer.Instance.AddTimer((float)effectInfo.keeptime / AppConst.factor, 1, true, TimerWalkInstantHandler);
    }

    public void BeHit()
    {
        Attribute.Hp = 0;
    }

    //先用物理引擎去检测碰撞,效率低再优化
    float mTotalTime = 0;
    void Update()
    {
        if (!init)
        {
            return;
        }
        mTotalTime += Time.deltaTime;
        if (mTotalTime <= 0.2f)
        {
            return;
        }

        mTotalTime = 0;

        DetectItem();

        DetectObstacle();
    }
}
