using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetEntity : IEntity
{
    private int id;
    private NPCControl m_npcControl;
    private EntityAttribute attribute;
    private Transform cacheAccelParticleTran; //加速特效
    private Transform cacheSkillParticleTran; //技能特效
    private Transform cacheParticleParent; //模型特效父节点
    //子节点，角色模型
    private Transform roleModel;
    //当前节点模型Entity
    private Transform cacheModel;

    private SkillInfo skill1;
    private EffectInfo effect1;
    private SkillInfo skill2;
    private EffectInfo effect2;

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
            if (null == m_arpgAnimatContorller)
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

    public CharacterController CharaController
    {
        get
        {
            if (null == m_characterController)
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

    public NPCControl NpcControl
    {
        get
        {
            if (null == m_npcControl)
            {
                m_npcControl = Util.AddComponent<NPCControl>(gameObject);
            }
            return m_npcControl;
        }

        set
        {
            m_npcControl = value;
        }
    }

    public void InitEntity(OccpType occp, int heroId)
    {
        ChangeOccp(occp, heroId);
        HeroInfo heroInfo = InfoMgr<HeroInfo>.Instance.GetInfo(heroId);
        InitEntityAttribute(heroInfo);
        InitSkill();
        Protect();
        init = true;
    }


    void InitSkill()
    {
        skill1 = InfoMgr<SkillInfo>.Instance.GetInfo(1);
        effect1 = InfoMgr<EffectInfo>.Instance.GetInfo(skill1.effectID);
        skill2 = InfoMgr<SkillInfo>.Instance.GetInfo(2);
        effect2 = InfoMgr<EffectInfo>.Instance.GetInfo(skill2.effectID);
    }

    GameObject SpawnerGO(int heroId, Vector3 v)
    {
        HeroInfo heroInfo = InfoMgr<HeroInfo>.Instance.GetInfo(heroId);
        GameObject go = ResourcesMgr.Instance.Spawner(heroInfo.model, v, ResourceType.RESOURCE_ENTITY, transform);// ResourcesMgr.Instance.Instantiate(prefab);
        return go;
    }

    void ChangeOccp(OccpType occp, int heroId)
    {
        HeroId = heroId;
        HeroInfo heroInfo = InfoMgr<HeroInfo>.Instance.GetInfo(heroId);
        GameObject go = ResourcesMgr.Instance.Spawner(heroInfo.model, ResourceType.RESOURCE_ENTITY, CacheModel);// ResourcesMgr.Instance.Instantiate(prefab);
        if (null == go)
        {
            return;
        }
        ResourcesMgr.Instance.Despawner(ResourceType.RESOURCE_ENTITY, RoleModel);
        Occupation = occp;
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
    }

    void InitEntityAttribute(HeroInfo info)
    {
        Attribute.BaseSpeed = AppConst.BaseSpeed;
        Attribute.Speed = Attribute.BaseSpeed;
        Attribute.RebornTime = AppConst.RebornTime;
        Attribute.Basedis = AppConst.Basedis;
        Attribute.Atkdis = Attribute.Basedis;
        Attribute.MaxPhy = AppConst.MaxPhy;
        Attribute.CurPhy = Attribute.MaxPhy;
        Attribute.CostPhySpeed = AppConst.CostPhySpeed;
        Attribute.MaxHp = AppConst.MaxHp;
        Attribute.Hp = Attribute.MaxHp;

        OccupationInfo occpInfo = InfoMgr<OccupationInfo>.Instance.GetInfo(info.occupationId);
        Attribute.Skills = occpInfo.skillId;
        Attribute.Score = 0;
        Attribute.Level = 0;
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
            CacheSkillParticleTran.parent = GameMgr.Instance.ParticleRoot;
            CacheSkillParticleTran = null;
        }
    }

    void DespawnerAccelerateParticle()
    {
        if (null != CacheAccelParticleTran)
        {
            ParticleMgr.Instance.Despawner(ResourceType.RESOURCE_PARTICLE, CacheAccelParticleTran);
            CacheAccelParticleTran.parent = GameMgr.Instance.ParticleRoot;
            CacheAccelParticleTran = null;
        }
    }

    //积分变换，体力变换
    public void EnergyUpdate(ItemEffectInfo effect)
    {
        Attribute.Score = Attribute.Score + effect.param1;
        Attribute.CurPhy = Attribute.CurPhy + effect.param2;
        if (Attribute.CurPhy > Attribute.MaxPhy)
        {
            Attribute.CurPhy = Attribute.MaxPhy;
        }
        Attribute.Level = GetCurrentLevel();
    }

    //等级变化
    public int GetCurrentLevel()
    {
        //LevelInfo info = InfoMgr<LevelInfo>.Instance.GetInfo(Attribute.Level);
        Dictionary<int, LevelInfo> level = InfoMgr<LevelInfo>.Instance.Dict;
        int lev = 0;
        foreach (KeyValuePair<int, LevelInfo> kv in level)
        {
            if (Attribute.Score >= kv.Value.score)
            {
                lev = Mathf.Max(lev, kv.Value.id);
            }
            else
            {
                break;
            }
        }
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
        State = StateType.STATE_PROTECT;
        Timer.Instance.AddTimer(3, 1, true, ProtectTimerOut);
    }

    void ProtectTimerOut(Timer.TimerData data)
    {
        RemoveProtect();
    }

    void RemoveProtect()
    {
        State = StateType.NONE;
        Timer.Instance.RemoveTimer(ProtectTimerOut);
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

    public void UpdateDir(Vector3 dir)
    {
        if (null != CacheModel)
        {
            CacheModel.forward = dir;
        }
    }

    public void SimpleMove(float speedfactor)
    {
        if (null != CharaController)
        {
            CharaController.SimpleMove(CacheModel.forward * Time.deltaTime * Attribute.BaseSpeed * speedfactor);
        }
    }

    //public void OnCollisionEnterThing(CollisionType collision)
    //{
    //    switch (collision)
    //    {
    //        case CollisionType.Collision_NET:
    //            Debug.LogError("碰到玩家了");
    //            break;
    //        case CollisionType.Collision_ITEM:
    //            Debug.LogError("碰到道具了");
    //            break;
    //        case CollisionType.Collision_OBSTACLE:
    //            Debug.LogError("碰到障碍物了");
    //            break;
    //        case CollisionType.NONE:
    //            Debug.LogError("自己停止了");
    //            SwitchOccp();
    //            break;
    //    }
    //}

    public void SwitchOccp()
    {
        if (IsAlive)
        {
            OccpType occp = Util.GetNextOccp(Occupation);
            int id = Util.GetHeroIdByOccp(occp);
            ChangeOccp(occp, id);
            Timer.Instance.AddTimer(1, 1, true, SwitchStateToWalkState);
        }
    }

    void SwitchStateToWalkState(Timer.TimerData data)
    {
        NpcControl.SetTransition(Transition.FreeWalk, this);
    }

    public void StopAccelerate()
    {
        ArpgAnimatContorller.Walk = false;
        Attribute.Speed = Attribute.BaseSpeed;
        DespawnerParticle(EffectType.ACCELERATE);
    }

    //加速
    public void Accelerate()
    {
        if (null == skill1 || null == effect1)
        {
            return;
        }
        //加速停止恢复能量条
        ArpgAnimatContorller.Walk = true;
        Attribute.Speed = Attribute.BaseSpeed * effect1.param / AppConst.factor;
        SpawnerParticle(skill1.particleID, EffectType.ACCELERATE, CacheParticleParent.position, CacheParticleParent);
    }

    public void StopSkill()
    {
        Attribute.Speed = Attribute.BaseSpeed;
        ArpgAnimatContorller.Skill = 0;
        DespawnerParticle(EffectType.WALKINSTANT);
    }

    //冲锋
    public void SkillEvent()
    {
        if (null == skill2 || null == effect2)
        {
            return;
        }

        ArpgAnimatContorller.Skill = 1;
        Attribute.Speed = Attribute.BaseSpeed * effect2.param / AppConst.factor;
        SpawnerParticle(skill2.particleID, EffectType.WALKINSTANT, CacheParticleParent.position, CacheParticleParent);
    }

    public float GetSkillTime()
    {
        if (null != effect2)
        {
            return (float)effect2.keeptime / AppConst.factor;
        }
        return AppConst.SkillMinTime;
    }


    public void BeKilled()
    {
        NpcControl.SetTransition(Transition.Dead, this);
    }

    public void EnterIdle()
    {
        ArpgAnimatContorller.Reset();
    }

    public void Dead()
    {
        Attribute.Hp = 0;
        ArpgAnimatContorller.Reset();
        ArpgAnimatContorller.Die = true;
        Timer.Instance.AddTimer(2, 1, true, RemoveBody);
    }

    public void Relive()
    {
        ArpgAnimatContorller.Reset();
        Protect();
    }

    void RemoveBody(Timer.TimerData data)
    {
        ResourcesMgr.Instance.Despawner(ResourceType.RESOURCE_ENTITY, RoleModel);
        ResourcesMgr.Instance.Despawner(ResourceType.RESOURCE_NET, CacheModel);
    }


    //停止逃跑到walk
    public void EndCurrentStateToWalk()
    {
        if (IsAlive)
        {
            NpcControl.SetTransition(Transition.FreeWalk, this);
        }
        else
        {
            Debug.LogError("角色已經死亡");
        }
    }


    void ResetTimeValue()
    {
        //lastFleeTime = 10;
        lastAcceTime = 10;
        lastSkillTime = 10;
        lastKillTime = 10;
        //chaseTime = 10;
    }

    //private float lastFleeTime = 10;
    private float lastAcceTime = 10;
    private float lastSkillTime = 10;
    private float chaseTime = 0; //追擊持續時間
    private float lastKillTime = 10;

    private void OnDisable()
    {
        ResetTimeValue();
        DespawnerAccelerateParticle();
        DespawnerWalkInstanceParticle();
    }


    //先用物理引擎去检测碰撞,效率低再优化
    float mTotalTime = 0;
    void Update()
    {
        if (!init)
        {
            return;
        }

        if (!GameMgr.Instance.IsEnterGame)
        {
            return;
        }

        if (!IsAlive)
        {
            return;
        }

        //DetectEntity();
        lastAcceTime += Time.deltaTime;
        lastSkillTime += Time.deltaTime;
        lastKillTime += Time.deltaTime;
        chaseTime += Time.deltaTime;
        mTotalTime += Time.deltaTime;
        if (mTotalTime <= 0.1f)
        {
            return;
        }

        mTotalTime = 0;

        DetectItem();

        //DetectObstacle();
    }


    private Transform lockCache;
    public Transform LockCache
    {
        private set
        {
            lockCache = value;
        }

        get
        {
            return lockCache;
        }
    }


    //查找身邊最近的玩家
    public void ChaseTarget()
    {
        if (lastKillTime < AppConst.KillInterval)
        {
            return;
        }
        if (!IsAlive)
        {
            return;
        }

        LockCache = null;
        float distance = 0;
        if (GameMgr.Instance.MainEntity.IsAlive)
        {
            if (Util.CanKillBody(Occupation, GameMgr.Instance.MainEntity.Occupation))
            {
                if (Util.GetEntityLevelGap(Attribute.Level, GameMgr.Instance.MainEntity.Attribute.Level) >= AppConst.ChaseLev)
                {
                    distance = Util.GetEntityDistance(CacheModel, GameMgr.Instance.MainEntity.CacheModel);
                    if (distance <= AppConst.ChaseDis)
                    {
                        LockCache = GameMgr.Instance.MainEntity.CacheModel;
                    }
                }
            }
        }

        foreach (KeyValuePair<int, NetEntity> kv in TSCData.Instance.EntityDic)
        {
            if (kv.Value.Id == Id)
            {
                continue;
            }
            if (kv.Value.IsAlive)
            {
                if (kv.Value.State != StateType.STATE_PROTECT)
                {
                    if (Util.CanKillBody(Occupation, kv.Value.Occupation))
                    {
                        if (Util.GetEntityLevelGap(Attribute.Level, kv.Value.Attribute.Level) >= AppConst.ChaseLev)
                        {
                            float dis = Util.GetEntityDistance(CacheModel, kv.Value.CacheModel);
                            if (dis < AppConst.ChaseDis)
                            {
                                if (distance > dis)
                                {
                                    distance = dis;
                                    LockCache = kv.Value.CacheModel;
                                }
                            }
                        }
                    }
                }
            }
        }

        if (null != LockCache)
        {
            Debug.LogError("身邊最近的玩家" + LockCache.name);
            TransferAIState(LockCache);
        }
    }

    void TransferAIState(Transform target)
    {
        if (null != target)
        {
            chaseTime = 0;
            Vector3 dir = target.position - CacheModel.position;
            UpdateDir(dir);

            if ((lastSkillTime > AppConst.SkillInterval) || (lastAcceTime > AppConst.AcctInterval))
            {
                if (lastSkillTime > AppConst.SkillInterval)
                {
                    lastSkillTime = 0;
                    lastKillTime = 0;
                    NpcControl.SetTransition(Transition.Skill, this);
                }
                else if (lastAcceTime > AppConst.AcctInterval)
                {
                    lastAcceTime = 0;
                    lastKillTime = 0;
                    NpcControl.SetTransition(Transition.Acct, this);
                }
            }
        }
    }

    public bool IsOutOfRange()
    {
        if (null == LockCache)
        {
            return true;
        }
        return (Util.GetEntityDistance(CacheModel, LockCache)) > AppConst.ChaseDis;
    }

    public bool IsOutOfTime()
    {
        return chaseTime > AppConst.ChaseTime;
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!IsAlive)
        {
            return;
        }

        Transform temp = hit.transform;

        if (temp.CompareTag(AppConst.TAG_OBSTACLE))
        {
            Vector3 dir = CacheModel.position - temp.position;
            UpdateDir(dir);
            NpcControl.SetTransition(Transition.FreeWalk, this);
        }
        else if (temp.CompareTag(AppConst.TAG_NETENTITY))
        {
            NetEntity entity = temp.GetComponent<NetEntity>();
            Vector3 dir = CacheModel.position - temp.position;
            UpdateDir(dir);
            if (Util.CanKillBody(entity.Occupation, Occupation))
            {
                NpcControl.SetTransition(Transition.Dead, this);
            }
            else
            {
                NpcControl.SetTransition(Transition.FreeWalk, this);
            }
        }
        else if (temp.CompareTag(AppConst.TAG_PLAYER))
        {
            Vector3 dir = CacheModel.position - temp.position;
            UpdateDir(dir);
            if (Util.CanKillBody(GameMgr.Instance.MainEntity.Occupation, Occupation))
            {
                NpcControl.SetTransition(Transition.Dead, this);
            }
            else
            {
                NpcControl.SetTransition(Transition.FreeWalk, this);
            }
        }
    }
}
