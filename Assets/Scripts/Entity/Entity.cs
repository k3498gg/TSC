using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : IEntity
{
    //状态控制器
    private RoleControl m_RoleEntiytControl;
    private SkillInfo skillinfo1 = null;
    private EffectInfo effectinfo1 = null;
    private SkillInfo skillinfo2 = null;
    private EffectInfo effectinfo2 = null;
    //角色属性
    private EntityAttribute attribute;
    private Transform cacheAccelParticleTran; //加速特效
    private Transform cacheSkillParticleTran; //技能特效
    private Transform cacheParticleParent; //模型特效父节点
    //子节点，角色模型
    private Transform roleModel;
    //当前节点模型Entity
    private Transform cacheModel;

    private Vector3 hitDir ;

    private bool isRecoverEnergy = false;

    private ARPGAnimatorController m_arpgAnimatContorller;

    private int m_heroId;

    private bool init = false;

    public OccpType occupation = OccpType.NONE;

    private StateType m_state = StateType.NONE;

    private CharacterController m_characController;

    public string protectTimerID = string.Empty;


    public CharacterController CharacController
    {
        get
        {
            if (null == m_characController)
            {
                m_characController = CacheModel.GetComponent<CharacterController>();
            }
            return m_characController;
        }
        set
        {
            m_characController = value;
        }
    }


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

    public bool IsAlive
    {
        get
        {
            return Attribute.Hp > 0;
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

    public RoleControl RoleEntityControl
    {
        get
        {
            if (null == m_RoleEntiytControl)
            {
                m_RoleEntiytControl = Util.AddComponent<RoleControl>(gameObject);
                m_RoleEntiytControl.Start();
            }
            return m_RoleEntiytControl;
        }

        set
        {
            m_RoleEntiytControl = value;
        }
    }



    public Vector3 HitDir
    {
        get
        {
            return hitDir;
        }

        set
        {
            hitDir = value;
        }
    }



    //public string RoleName
    //{
    //    get
    //    {
    //        return roleName;
    //    }

    //    set
    //    {
    //        roleName = value;
    //    }
    //}

    //public bool IsWalking
    //{
    //    get
    //    {
    //        return isWalking;
    //    }

    //    set
    //    {
    //        isWalking = value;
    //    }
    //}

    public void InitEntity(OccpType occp, int heroId)
    {
        ChangeOccp(occp, heroId);
        UpdateCharacControllerActive(true);
        init = true;
    }

    //GameObject SpawnerGO(int heroId, Vector3 v)
    //{
    //    HeroInfo heroInfo = InfoMgr<HeroInfo>.Instance.GetInfo(heroId);
    //    GameObject go = ResourcesMgr.Instance.Spawner(heroInfo.model, v, ResourceType.RESOURCE_ENTITY, transform);// ResourcesMgr.Instance.Instantiate(prefab);
    //    return go;
    //}

    void ChangeOccp(OccpType occp, int heroId)
    {
        Occupation = occp;
        HeroId = heroId;
        HeroInfo heroInfo = InfoMgr<HeroInfo>.Instance.GetInfo(heroId);
        InitAttribute(heroInfo);
        InitSkill();
        ResourcesMgr.Instance.Despawner(ResourceType.RESOURCE_ENTITY, RoleModel);
        GameObject go = ResourcesMgr.Instance.Spawner(heroInfo.model, ResourceType.RESOURCE_ENTITY, CacheModel);// ResourcesMgr.Instance.Instantiate(prefab);
        if (null == go)
        {
            Debug.LogError("模型出錯!~~~");
            return;
        }
        InitCharactor(go);
    }


    //进入换职业状态
    public void SwitchOtherOccp()
    {
        OccpType occp = Util.GetNextOccp(occupation);
        int id = Util.GetHeroIdByOccp(occp);
        ChangeOccp(occp, id);
        UpdateModelScale();
    }


    public void ExitSwitchState()
    {

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
        ArpgAnimatContorller.Init(RoleModel);
        GameMgr.Instance.CameraController.SetTarget(CacheModel);

    }


    bool isInitAttr = false;
    void InitAttribute(HeroInfo info)
    {
        if (isInitAttr)
        {
            return;
        }
        isInitAttr = true;
        Attribute.BaseSpeed = AppConst.BaseSpeed;
        Attribute.Speed = Attribute.BaseSpeed;
        Attribute.Basedis = AppConst.Basedis;
        Attribute.Atkdis = Attribute.Basedis;
        Attribute.MaxPhy = AppConst.MaxPhy;
        Attribute.CurPhy = Attribute.MaxPhy;
        Attribute.CostPhySpeed = AppConst.CostPhySpeed;
        Attribute.MaxHp = AppConst.MaxHp;
        Attribute.Hp = Attribute.MaxHp;
        OccupationInfo occpInfo = InfoMgr<OccupationInfo>.Instance.GetInfo(info.occupationId);
        Attribute.Skills = occpInfo.skillId;
    }

    void InitSkill()
    {
        if (Attribute.Skills.Length > 1)
        {
            skillinfo1 = InfoMgr<SkillInfo>.Instance.GetInfo(Attribute.Skills[0]);
            effectinfo1 = InfoMgr<EffectInfo>.Instance.GetInfo(skillinfo1.effectID);
            skillinfo2 = InfoMgr<SkillInfo>.Instance.GetInfo(Attribute.Skills[1]);
            effectinfo2 = InfoMgr<EffectInfo>.Instance.GetInfo(skillinfo2.effectID);
        }
    }

    public void UpdateRotation(float angle)
    {
        CacheModel.rotation = Quaternion.Euler(0, angle + GameMgr.Instance.CameraController.EulerY, 0);
    }


    //public void BeginWalk()
    //{
    //    if (!IsWalking)
    //    {
    //        IsWalking = true;
    //        EndCurrentStateToOtherState(RoleStateID.Walk);
    //    }
    //}

    //public void StopWalk()
    //{
    //    if (IsWalking)
    //    {
    //        IsWalking = false;
    //        EndCurrentStateToOtherState(RoleStateID.Idle);
    //    }
    //}


    public void SimpleMove()
    {
        GameMgr.Instance.CharacController.SimpleMove(CacheModel.forward * Time.deltaTime * Attribute.Speed);
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
        Attribute.CurPhy = Attribute.CurPhy + effect.param2;
        if (Attribute.CurPhy > Attribute.MaxPhy)
        {
            Attribute.CurPhy = Attribute.MaxPhy;
        }
        UpdateModelScale();
    }

    internal void CreateEntity()
    {
        int occp = UnityEngine.Random.Range(1, (int)OccpType.Occp_MAZ);
        InitEntity((OccpType)occp, Util.GetHeroIdByOccp((OccpType)occp));
        ResetAttribute();
        Protect();
        EndCurrentStateToOtherState(RoleStateID.Idle);
        Vector3 location = GameMgr.Instance.RandomLocation();
        CacheModel.position = location;
        CreateHUDName();
    }

    void DespawnerHUDName()
    {
        if (null != HudName)
        {
            UIHudManager.Instance.Despawner(HudName.Cache);
            HudName.SetTarget(null);
            HudName = null;
        }
    }

    void CreateHUDName()
    {
        DespawnerHUDName();
        GameObject go = UIHudManager.Instance.SpawnerHUD();
        HudName = Util.AddComponent<UIHUDName>(go);
        HudName.Init();
        HudName.SetTarget(CacheModel);
        HudName.SetName(TSCData.Instance.Role.Name);
        HudName.Cache.localScale = Vector3.one;
    }

    void ResetAttribute()
    {
        Attribute.Score = 0;
        Attribute.Level = 0;
        Attribute.Money = 0;
        if (null != RoleModel)
        {
            RoleModel.localScale = Vector3.one;
        }
        CharacController.radius = AppConst.hitRadio;
    }

    void UpdateModelScale()
    {
        if (null != RoleModel)
        {
            Attribute.Level = GetCurrentLevel();
            LevelInfo level = InfoMgr<LevelInfo>.Instance.GetInfo(Attribute.Level);
            RoleModel.localScale = Vector3.one * (1.0f * level.scale / AppConst.factor);
            CharacController.radius = AppConst.hitRadio * level.hitscale / AppConst.factor;
        }
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
        Debuger.LogError("Level:" + lev);
        return lev;
    }

    public void UpdateState(StateType stateType, ItemInfo item, ItemEffectInfo effect)
    {
        switch (stateType)
        {
            case StateType.STATE_MARK:

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
        EventCenter.Instance.Publish<Event_StopAcct>(null, new Event_StopAcct());
        this.Attribute.Speed = Attribute.BaseSpeed * effect.param1 / AppConst.factor;
        Timer.Instance.AddTimer(item.timer, 1, true, BackSpeed);
    }

    void BackSpeed(Timer.TimerData data)
    {
        //发送事件 可以使用加速功能
        EventCenter.Instance.Publish<Event_OpenAcct>(null, new Event_OpenAcct());
        this.Attribute.Speed = Attribute.BaseSpeed;
    }


    void DetectItem()
    {
        foreach (KeyValuePair<int, DropItemInfo> kv in TSCData.Instance.DropItemDic)
        {
            if (kv.Value.IsLock)
            {
                continue;
            }
            if (Util.PtInCircleArea(kv.Value.Cache, CacheModel, Attribute.Atkdis + 0.5f))
            {
                kv.Value.FlyToEntity(this);
            }
        }

        foreach (KeyValuePair<int, DropItemInfo> kv in TSCData.Instance.RareEnergyDic)
        {
            if (kv.Value.IsLock)
            {
                continue;
            }
            if (Util.PtInCircleArea(kv.Value.Cache, CacheModel, Attribute.Atkdis + 0.5f))
            {
                kv.Value.FlyToEntity(this);
            }
        }
    }


    public void EndSkillStateToIdle()
    {
        if (RoleEntityControl.Fsm.CurrentStateID == RoleStateID.Skill)
        {
            RoleEntityControl.SetTransition(RoleTransition.Idle, this);
        }
    }

    public void EndCurrentStateToOtherState(RoleStateID id)
    {
        if (!IsAlive)
        {
            return;
        }

        if (RoleEntityControl.Fsm.CurrentStateID != id)
        {
            switch (id)
            {
                case RoleStateID.Idle:
                    RoleEntityControl.SetTransition(RoleTransition.Idle, this);
                    break;
                case RoleStateID.Walk:
                    RoleEntityControl.SetTransition(RoleTransition.FreeWalk, this);
                    break;
                case RoleStateID.Acct:
                    RoleEntityControl.SetTransition(RoleTransition.Acct, this);
                    break;
                case RoleStateID.Skill:
                    RoleEntityControl.SetTransition(RoleTransition.Skill, this);
                    break;
                case RoleStateID.Switch:
                    RoleEntityControl.SetTransition(RoleTransition.Switch, this);
                    break;
                case RoleStateID.CrashPlayer:
                    RoleEntityControl.SetTransition(RoleTransition.CrashPlayer, this);
                    break;
                case RoleStateID.Dead:
                    RoleEntityControl.SetTransition(RoleTransition.Dead, this);
                    ItemDropMgr.Instance.DropRareItem(CacheModel.position, Attribute.Score, GameMgr.Instance.ItemRoot);
                    break;
            }
        }
    }

    void TimerEndWalkInstant(Timer.TimerData data)
    {
        if (RoleEntityControl.Fsm.CurrentStateID != RoleStateID.Skill)
        {
            return;
        }
        EventCenter.Instance.Publish<Event_StopSkill>(null, new Event_StopSkill());
        EndCurrentStateToOtherState(RoleStateID.Switch);
    }

    public void EnterWalk()
    {
        ArpgAnimatContorller.Walk = true;
    }

    public void ExitWalk()
    {
        ArpgAnimatContorller.Walk = false;
    }

    public bool IsProtect()
    {
        return State == StateType.STATE_PROTECT;
    }

    void UpdateCharacControllerActive(bool active)
    {
        CharacController.enabled = active;
    }

    public void Dead()
    {
        init = false;
        Attribute.Hp = 0;
        UpdateCharacControllerActive(false);
        ArpgAnimatContorller.Die = true;
        DespawnerHUDName();
        //发送事件显示UI界面
        EventCenter.Instance.Publish<Event_RoleDead>(null, new Event_RoleDead(true));
    }

    public void Relive()
    {
        if (!IsAlive)
        {
            Protect();
            EventCenter.Instance.Publish<Event_RoleDead>(null, new Event_RoleDead(false));
            InitEntity(Occupation, HeroId);
            Attribute.Hp = Attribute.MaxHp;
            CalculateScore();
            EndCurrentStateToOtherState(RoleStateID.Idle);
            Vector3 location = GameMgr.Instance.RandomLocation();
            CacheModel.position = location;
            CreateHUDName();
        }
    }


    void CalculateScore()
    {
        int score = (int)(Attribute.Score / AppConst.ReliveScoreParam1 - Attribute.Score * UnityEngine.Random.Range(AppConst.ReliveRandomParam1, AppConst.ReliveRandomParam2) / AppConst.ReliveScoreParam2);
        score = score > 0 ? score : 0;
        if (Attribute.Score != score)
        {
            Attribute.Score = score;
            UpdateModelScale();
        }
        Attribute.Level = Attribute.Level < 0 ? 0 : Attribute.Level;
    }

    public void Protect()
    {
        State = StateType.STATE_PROTECT;
        Timer.TimerData data = Timer.Instance.AddTimer(3, 1, true, ProtectTimerOut);
        if (null != data)
        {
            protectTimerID = data.ID;
        }
    }

    void ProtectTimerOut(Timer.TimerData data)
    {
        if (State == StateType.STATE_PROTECT)
        {
            State = StateType.NONE;
        }
         protectTimerID = string.Empty;
    }

    public void StopAccelerate()
    {
        ArpgAnimatContorller.Walk = false;
        Attribute.Speed = Attribute.BaseSpeed;
        DespawnerParticle(EffectType.ACCELERATE);
    }

    public void EnterAccelerate()
    {
        ArpgAnimatContorller.Walk = true;
        if (null == skillinfo1)
        {
            skillinfo1 = InfoMgr<SkillInfo>.Instance.GetInfo(Attribute.Skills[0]);
            effectinfo1 = InfoMgr<EffectInfo>.Instance.GetInfo(skillinfo1.effectID);
        }
        else if (!InfoMgr<SkillInfo>.Instance.Dict.ContainsKey(skillinfo1.id))
        {
            skillinfo1 = InfoMgr<SkillInfo>.Instance.GetInfo(Attribute.Skills[0]);
            effectinfo1 = InfoMgr<EffectInfo>.Instance.GetInfo(skillinfo1.effectID);
        }
        float factor = effectinfo1.param * 1.0f / AppConst.factor;
        SpawnerParticle(skillinfo1.particleID, EffectType.ACCELERATE, CacheParticleParent.position, CacheParticleParent);
        Attribute.Speed = Attribute.BaseSpeed * factor;
    }

    public void StopWalkInstant()
    {
        ArpgAnimatContorller.Skill = 0;
        Attribute.Speed = Attribute.BaseSpeed;
        DespawnerParticle(EffectType.WALKINSTANT);
    }

    public void EnterWalkInstant()
    {
        ArpgAnimatContorller.Skill = 1;

        if (null == skillinfo2)
        {
            skillinfo2 = InfoMgr<SkillInfo>.Instance.GetInfo(Attribute.Skills[1]);
            effectinfo2 = InfoMgr<EffectInfo>.Instance.GetInfo(skillinfo2.effectID);
        }
        else if (!InfoMgr<SkillInfo>.Instance.Dict.ContainsKey(skillinfo2.id))
        {
            skillinfo2 = InfoMgr<SkillInfo>.Instance.GetInfo(Attribute.Skills[1]);
            effectinfo2 = InfoMgr<EffectInfo>.Instance.GetInfo(skillinfo2.effectID);
        }

        float factor = effectinfo2.param * 1.0f / AppConst.factor;
        SpawnerParticle(skillinfo2.particleID, EffectType.WALKINSTANT, CacheParticleParent.position, CacheParticleParent);
        Timer.Instance.AddTimer((float)effectinfo2.keeptime / AppConst.factor, 1, true, TimerEndWalkInstant);
        Attribute.Speed = Attribute.BaseSpeed * factor;
    }

    public bool IsUsingAcctOrSkill()
    {
        return (GameMgr.Instance.MainEntity.RoleEntityControl.Fsm.CurrentStateID == RoleStateID.Acct || GameMgr.Instance.MainEntity.RoleEntityControl.Fsm.CurrentStateID == RoleStateID.Skill);
    }

    public bool IsUsingSkill()
    {
        return (GameMgr.Instance.MainEntity.RoleEntityControl.Fsm.CurrentStateID == RoleStateID.Skill);
    }

    float mTotalTime = 0;
    float mInterval = 0.1f;
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

        mTotalTime += Time.deltaTime;
        if (mTotalTime <= mInterval)
        {
            return;
        }

        mTotalTime = 0;

        DetectItem();

     //   ItemDropMgr.Instance.DropRareItem(CacheModel.position, 1, 2, 1, GameMgr.Instance.ItemRoot);
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
            if (IsUsingSkill())
            {
                HitDir = CacheModel.forward * -1;
                EndCurrentStateToOtherState(RoleStateID.CrashPlayer);
            }
        }
        else if (temp.CompareTag(AppConst.TAG_NETENTITY))
        {
            NetEntity entity = temp.GetComponent<NetEntity>();
            if (null != entity)
            {
                if (!entity.IsAlive)
                {
                    return;
                }

                if (Util.CanKillBody(entity.Occupation, Occupation))
                {
                    entity.EndCurrentStateToOtherState(StateID.Walk);

                    if (IsProtect())
                    {
                        if (entity.IsUsingSkill())
                        {
                            HitDir =  CacheModel.position - entity.CacheModel.position ;
                            EndCurrentStateToOtherState(RoleStateID.CrashPlayer);
                        }
                        else
                        {
                            EndCurrentStateToOtherState(RoleStateID.Idle);
                        }
                    }
                    else
                    {
                        BeKilled();
                        entity.KillBody();
                        EndCurrentStateToOtherState(RoleStateID.Dead);
                    }
                }
                else if (Util.CanKillBody(Occupation, entity.Occupation))
                {
                    EndSkillStateToIdle();
                    if (entity.IsProtect())
                    {
                        if (IsUsingSkill())
                        {
                            entity.HitDir =  entity.CacheModel.position - CacheModel.position ;
                            entity.EndCurrentStateToOtherState(StateID.CrashPlayer);
                        }
                        else
                        {
                            entity.EndCurrentStateToOtherState(StateID.Walk);
                        }
                    }
                    else
                    {
                        entity.BeKilled();
                        KillBody();
                        entity.EndCurrentStateToOtherState(StateID.Dead);
                    }
                }
                else
                {
                    if (entity.IsUsingSkill())
                    {
                        HitDir = CacheModel.position - entity.CacheModel.position;
                        EndCurrentStateToOtherState(RoleStateID.CrashPlayer);
                    }
                    else
                    {
                        if (IsUsingSkill())
                        {
                            entity.HitDir = entity.CacheModel.position - CacheModel.position;
                            entity.EndCurrentStateToOtherState(StateID.CrashPlayer);
                        }
                        else
                        {
                            entity.EndCurrentStateToOtherState(StateID.Walk);
                        }

                        EndCurrentStateToOtherState(RoleStateID.Idle);
                    }
                }
            }
        }
    }


    public void Clear()
    {
        ArpgAnimatContorller.animator = null;
        RoleModel = null;
        isInitAttr = false;
        KillCount = 0;
        BeKillCount = 0;
    }

}
