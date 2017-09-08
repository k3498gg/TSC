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
    //HUD
    //private UIHUDName m_hudName;
    private string roleName;

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

    private float lastDeadTime;

    private int nameIdx = -1;

    private Vector3 m_hitDir;


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
                m_npcControl.Start();
            }
            return m_npcControl;
        }

        set
        {
            m_npcControl = value;
        }
    }

    public void CreateNetEntity(int idx)
    {
        Id = (idx + 1);
        int occp = idx % 3;
        OccpType oc = (OccpType)(occp + 1);
        int equipID = Util.GetNetEquipIdByOccp(oc);
        EquipInfo info = InfoMgr<EquipInfo>.Instance.GetInfo(equipID);
        if (null == info)
        {
            Debuger.LogError("裝備ID錯誤:" + equipID);
            return;
        }

        InitEntity(oc, info.modelId);
        RandomName();
        EndDeadState();
        ResetAttribute();
        CreateHUDName();
        CreateEntityInfo();
    }

    void CreateEntityInfo()
    {
        EntityInfo info = new EntityInfo();
        info.Id = Id;
        info.NameIdx = NameIdx;
        info.Socre = Attribute.Score;
        info.Killcount = KillCount;
        TSCData.Instance.EntityInfoDic[Id] = info;
    }

    void RandomName()
    {
        int idx = Random.Range(0, InfoMgr<NameInfo>.Instance.Dict.Count);
        NameInfo info = InfoMgr<NameInfo>.Instance.GetInfo(idx);
        NameIdx = idx;
        RoleName = info.name;
    }

    void CreateHUDName()
    {
        DespawnerHUDName();
        GameObject go = UIHudManager.Instance.SpawnerHUD();
        HudName = Util.AddComponent<UIHUDName>(go);
        HudName.Init();
        HudName.SetTarget(CacheModel);
        HudName.SetName(RoleName);
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
        CharaController.radius = AppConst.hitRadio;
    }

    void InitEntity(OccpType occp, int heroId)
    {
        Vector3 location = GameMgr.Instance.RandomLocation();
        CacheModel.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        CacheModel.position = location;
        ChangeOccp(occp, heroId);
        UpdateCharacControllerActive(true);
        HeroInfo heroInfo = InfoMgr<HeroInfo>.Instance.GetInfo(heroId);
        InitAttribute(heroInfo);
        InitSkill();
        Protect();
        init = true;
    }

    void UpdateCharacControllerActive(bool active)
    {
        CharaController.enabled = active;
    }


    void InitSkill()
    {
        skill1 = InfoMgr<SkillInfo>.Instance.GetInfo(1);
        effect1 = InfoMgr<EffectInfo>.Instance.GetInfo(skill1.effectID);
        skill2 = InfoMgr<SkillInfo>.Instance.GetInfo(2);
        effect2 = InfoMgr<EffectInfo>.Instance.GetInfo(skill2.effectID);
    }

    //GameObject SpawnerGO(int heroId, Vector3 v)
    //{
    //    HeroInfo heroInfo = InfoMgr<HeroInfo>.Instance.GetInfo(heroId);
    //    GameObject go = ResourcesMgr.Instance.Spawner(heroInfo.model, v, ResourceType.RESOURCE_ENTITY, transform);// ResourcesMgr.Instance.Instantiate(prefab);
    //    return go;
    //}

    void ChangeOccp(OccpType occp, int heroId)
    {
        HeroId = heroId;
        HeroInfo heroInfo = InfoMgr<HeroInfo>.Instance.GetInfo(heroId);
        GameObject go = ResourcesMgr.Instance.Spawner(heroInfo.model, ResourceType.RESOURCE_ENTITY, CacheModel);// ResourcesMgr.Instance.Instantiate(prefab);
        if (null == go)
        {
            Debug.LogError(heroInfo.model + " is null");
            return;
        }
        ResourcesMgr.Instance.Despawner(ResourceType.RESOURCE_ENTITY, RoleModel);
        RoleModel = null;
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
        //if(!go.activeSelf)
        //{
        //    go.SetActive(true);
        //}
        RoleModel = go.transform;
        RoleModel.parent = CacheModel;
        RoleModel.localScale = Vector3.one;
        RoleModel.localPosition = Vector3.zero;
        RoleModel.localRotation = Quaternion.identity;
        ArpgAnimatContorller = CacheModel.GetComponent<ARPGAnimatorController>();
        ArpgAnimatContorller.Init(RoleModel);
    }

    void InitAttribute(HeroInfo info)
    {
        Attribute.BaseSpeed = AppConst.BaseSpeed;
        Attribute.Speed = Attribute.BaseSpeed;
        //Attribute.RebornTime = AppConst.RebornTime;
        Attribute.Basedis = AppConst.Basedis;
        Attribute.Atkdis = Attribute.Basedis;
        Attribute.MaxPhy = AppConst.MaxPhy;
        Attribute.CurPhy = Attribute.MaxPhy;
        Attribute.CostPhySpeed = AppConst.CostPhySpeed;
        Attribute.MaxHp = AppConst.MaxHp;
        Attribute.Hp = Attribute.MaxHp;

        OccupationInfo occpInfo = InfoMgr<OccupationInfo>.Instance.GetInfo(info.occupationId);
        Attribute.Skills = occpInfo.skillId;
        //Attribute.Score = 0;
        //Attribute.Level = 0;
    }

    void DespawnerParticle(EffectType type)
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
    void SpawnerParticle(int id, EffectType type, Vector3 v, Transform parent)
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



    //等级变化
    int GetCurrentLevel()
    {
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

    public bool IsUsingSkill()
    {
        return NpcControl.Fsm.CurrentStateID == StateID.Skill;
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

    public void SwitchOccp()
    {
        if (IsAlive)
        {
            OccpType occp = Util.GetNextOccp(Occupation);
            int id = Util.GetNetEquipIdByOccp(occp);
            EquipInfo info = InfoMgr<EquipInfo>.Instance.GetInfo(id);
            if (null == info)
                return;
            ChangeOccp(occp, info.modelId);
            UpdateModelScale();
            EndCurrentStateToOtherState(StateID.Idle);
        }
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

    public void EnterIdle()
    {
        ArpgAnimatContorller.Reset();
    }

    public void ExitIdle()
    {
        //Debuger.Log("离开IDLE状态");
    }

    public void Dead()
    {
        init = false;
        UpdateCharacControllerActive(false);
        LastDeadTime = Time.realtimeSinceStartup;
        Attribute.Hp = 0;
        ArpgAnimatContorller.Die = true;
        Timer.Instance.AddTimer(2, 1, true, RemoveBody);
        DespawnerHUDName();
        CalculateScore();
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

    public void Relive()
    {
        Protect();
        CacheModel.gameObject.SetActive(true);
        InitEntity(Occupation, HeroId);
        UpdateModelScale();
        //CalculateScore();
        ArpgAnimatContorller.Reset();
        EndCurrentStateToOtherState(StateID.Idle);
        CreateHUDName();
    }

    void CalculateScore()
    {
        int score = (int)(Attribute.Score / AppConst.ReliveScoreParam1 - Attribute.Score * UnityEngine.Random.Range(AppConst.ReliveRandomParam1, AppConst.ReliveRandomParam2) / AppConst.ReliveScoreParam2);
        score = score > 0 ? score : 0;
        if (Attribute.Score != score)
        {
            Attribute.Score = score;
            //UpdateModelScale();
        }
        Attribute.Level = Attribute.Level < 0 ? 0 : Attribute.Level;
    }

    void UpdateModelScale()
    {
        if (null != RoleModel)
        {
            //int lev = GetCurrentLevel();
            //if (Attribute.Level != lev)
            //{
            Attribute.Level = GetCurrentLevel();
            LevelInfo level = InfoMgr<LevelInfo>.Instance.GetInfo(Attribute.Level);
            RoleModel.localScale = Vector3.one * (1.0f * level.scale / AppConst.factor);
            CharaController.radius = AppConst.hitRadio * level.hitscale / AppConst.factor;
            //}
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

        if (TSCData.Instance.EntityInfoDic.ContainsKey(Id))
        {
            TSCData.Instance.EntityInfoDic[Id].Socre = Attribute.Score;
            TSCData.Instance.EntityInfoDic[Id].Killcount = KillCount;
            TSCData.Instance.EntityInfoDic[Id].BeKillCount = BeKillCount;
        }
    }

    void RemoveBody(Timer.TimerData data)
    {
        if (!IsAlive)
        {
            CacheModel.gameObject.SetActive(false);
            ResourcesMgr.Instance.Despawner(ResourceType.RESOURCE_ENTITY, RoleModel);
            RoleModel = null;
        }
    }

    public void EndDeadState()
    {
        if (NpcControl.Fsm.CurrentStateID != StateID.Idle)
        {
            NpcControl.SetTransition(Transition.Idle, this);
        }
    }

    public void EndCurrentStateToOtherState(StateID id)
    {
        if (!IsAlive)
        {
            return;
        }
        if (NpcControl.Fsm.CurrentStateID != id)
        {
            switch (id)
            {
                case StateID.Idle:
                    NpcControl.SetTransition(Transition.Idle, this);
                    break;
                case StateID.Walk:
                    NpcControl.SetTransition(Transition.FreeWalk, this);
                    break;
                case StateID.Acct:
                    NpcControl.SetTransition(Transition.Acct, this);
                    break;
                case StateID.Skill:
                    NpcControl.SetTransition(Transition.Skill, this);
                    break;
                case StateID.Switch:
                    NpcControl.SetTransition(Transition.Switch, this);
                    break;
                case StateID.CrashPlayer:
                    NpcControl.SetTransition(Transition.CrashPlayer, this);
                    break;
                case StateID.Dead:
                    ItemDropMgr.Instance.DropRareItem(CacheModel.position, Attribute.Score, GameMgr.Instance.ItemRoot);
                    NpcControl.SetTransition(Transition.Dead, this);
                    break;
            }
        }
    }


    void ResetTimeValue()
    {
        lastFleeTime = 10;
        lastAcceTime = 10;
        lastSkillTime = 10;
        lastKillTime = 10;
        //chaseTime = 10;
    }

    private float lastFleeTime = 10;
    private float lastAcceTime = 10;
    private float lastSkillTime = 10;
    private float chaseTime = 0; //追擊持續時間
    private float lastKillTime = 10;

    private void OnDisable()
    {
        //Clear();
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
        lastFleeTime += Time.deltaTime;
        chaseTime += Time.deltaTime;
        mTotalTime += Time.deltaTime;
        if (mTotalTime <= 0.1f)
        {
            return;
        }

        mTotalTime = 0;

        DetectItem();
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

    public float LastDeadTime
    {
        get
        {
            return lastDeadTime;
        }

        set
        {
            lastDeadTime = value;
        }
    }

    public int NameIdx
    {
        get
        {
            return nameIdx;
        }

        set
        {
            nameIdx = value;
        }
    }

    //public UIHUDName HudName
    //{
    //    get
    //    {
    //        return m_hudName;
    //    }

    //    set
    //    {
    //        m_hudName = value;
    //    }
    //}

    public string RoleName
    {
        get
        {
            return roleName;
        }

        set
        {
            roleName = value;
        }
    }

    public Vector3 HitDir
    {
        get
        {
            return m_hitDir;
        }

        set
        {
            m_hitDir = value;
        }
    }

    float distance = 0;
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
        distance = 0;
        if (GameMgr.Instance.MainEntity.IsAlive)
        {
            if (!GameMgr.Instance.MainEntity.IsProtect())
            {
                if (Util.CanKillBody(Occupation, GameMgr.Instance.MainEntity.Occupation))
                {
                    if (GameMgr.Instance.MainEntity.Attribute.Level >= Attribute.Level - AppConst.ChaseLev)
                    {
                        distance = Util.GetEntityDistance(CacheModel, GameMgr.Instance.MainEntity.CacheModel);
                        if (distance <= AppConst.ChaseDis)
                        {
                            LockCache = GameMgr.Instance.MainEntity.CacheModel;
                        }
                    }
                }
                else if (Util.CanKillBody(GameMgr.Instance.MainEntity.Occupation, Occupation))
                {
                    if (lastFleeTime > AppConst.FleeInterval)
                    {
                        if (Util.GetEntityDistance(CacheModel, GameMgr.Instance.MainEntity.CacheModel) < AppConst.FleeDis)
                        {
                            lastFleeTime = 0;
                            CacheModel.forward = -CacheModel.forward;
                        }
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
                        if (kv.Value.Attribute.Level >= Attribute.Level - AppConst.ChaseLev)
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

            if (lastSkillTime > AppConst.SkillInterval)
            {
                lastSkillTime = 0;
                lastKillTime = 0;
                EndCurrentStateToOtherState(StateID.Skill);
            }
            else if (lastAcceTime > AppConst.AcctInterval)
            {
                lastAcceTime = 0;
                lastKillTime = 0;
                EndCurrentStateToOtherState(StateID.Acct);
            }
        }
    }

    public bool IsProtect()
    {
        return State == StateType.STATE_PROTECT;
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
            ObstacleEntity entity = temp.GetComponent<ObstacleEntity>();
            if (null != entity)
            {
                Vector3 dir = CacheModel.forward;
                switch (entity.Obs_type)
                {
                    case ObstType.ObsType_TA:
                        UpdateDir(Vector3.zero - CacheModel.position);
                        break;
                    case ObstType.ObsType_TB:
                    case ObstType.ObsType_TC:
                        UpdateDir(CacheModel.position - temp.position);
                        break;
                }
            }
            EndCurrentStateToOtherState(StateID.Walk);
        }
        else if (temp.CompareTag(AppConst.TAG_NETENTITY))
        {
            NetEntity entity = temp.GetComponent<NetEntity>();
            if (null != entity)
            {
                if (!entity.IsAlive)
                    return;

                Vector3 dir = CacheModel.position - temp.position;
                UpdateDir(dir);

                if (Util.CanKillBody(entity.Occupation, Occupation))
                {
                    entity.EndCurrentStateToOtherState(StateID.Walk);

                    if (IsProtect())
                    {
                        if (entity.IsUsingSkill())
                        {
                            HitDir = CacheModel.TransformVector(entity.CacheModel.position - CacheModel.position);
                            EndCurrentStateToOtherState(StateID.CrashPlayer);
                        }
                        else
                        {
                            EndCurrentStateToOtherState(StateID.Walk);
                        }
                    }
                    else
                    {
                        BeKilled();
                        entity.KillBody();
                        EndCurrentStateToOtherState(StateID.Dead);
                    }
                }
                else if (Util.CanKillBody(Occupation, entity.Occupation))
                {
                    EndCurrentStateToOtherState(StateID.Walk);

                    if (entity.IsProtect())
                    {
                        if (IsUsingSkill())
                        {
                            entity.HitDir = entity.CacheModel.TransformVector(CacheModel.position - entity.CacheModel.position);
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
                        HitDir = entity.CacheModel.TransformVector(entity.CacheModel.position - CacheModel.position);
                        EndCurrentStateToOtherState(StateID.CrashPlayer);
                    }
                    else
                    {
                        EndCurrentStateToOtherState(StateID.Walk);
                    }

                    if (IsUsingSkill())
                    {
                        entity.HitDir = entity.CacheModel.TransformVector(CacheModel.position - entity.CacheModel.position);
                        entity.EndCurrentStateToOtherState(StateID.CrashPlayer);
                    }
                    else
                    {
                        entity.EndCurrentStateToOtherState(StateID.Walk);
                    }
                }
            }
        }
        else if (temp.CompareTag(AppConst.TAG_PLAYER))
        {
            //Vector3 dir = CacheModel.position - temp.position;
            //UpdateDir(dir);
            if (!GameMgr.Instance.MainEntity.IsAlive)
            {
                return;
            }

            if (Util.CanKillBody(GameMgr.Instance.MainEntity.Occupation, Occupation))
            {
                GameMgr.Instance.MainEntity.EndCurrentStateToOtherState(RoleStateID.Idle);

                if (IsProtect())
                {
                    if (GameMgr.Instance.MainEntity.IsUsingSkill())
                    {
                        HitDir = CacheModel.position - GameMgr.Instance.MainEntity.CacheModel.position;
                        EndCurrentStateToOtherState(StateID.CrashPlayer);
                    }
                    else
                    {
                        EndCurrentStateToOtherState(StateID.Walk);
                    }
                }
                else
                {
                    BeKilled();
                    GameMgr.Instance.MainEntity.KillBody();
                    EndCurrentStateToOtherState(StateID.Dead);
                }
            }
            else if (Util.CanKillBody(Occupation, GameMgr.Instance.MainEntity.Occupation))
            {
                EndCurrentStateToOtherState(StateID.Walk);

                if (GameMgr.Instance.MainEntity.IsProtect())
                {
                    if (IsUsingSkill())
                    {
                        GameMgr.Instance.MainEntity.HitDir = GameMgr.Instance.MainEntity.CacheModel.position - CacheModel.position;
                        GameMgr.Instance.MainEntity.EndCurrentStateToOtherState(RoleStateID.CrashPlayer);
                    }
                    else
                    {
                        if (GameMgr.Instance.MainEntity.IsForceDrag)
                        {
                            GameMgr.Instance.MainEntity.EndCurrentStateToOtherState(RoleStateID.Walk);
                        }
                        else
                        {
                            GameMgr.Instance.MainEntity.EndCurrentStateToOtherState(RoleStateID.Idle);
                        }
                        //GameMgr.Instance.MainEntity.EndCurrentStateToOtherState(RoleStateID.Idle);
                    }
                }
                else
                {
                    GameMgr.Instance.MainEntity.BeKilled();
                    KillBody();
                    GameMgr.Instance.MainEntity.EndCurrentStateToOtherState(RoleStateID.Dead);
                }
            }
            else
            {
                if (GameMgr.Instance.MainEntity.IsUsingSkill())
                {
                    HitDir = (CacheModel.position - GameMgr.Instance.MainEntity.CacheModel.position);
                    EndCurrentStateToOtherState(StateID.CrashPlayer);
                }
                else
                {
                    if (IsUsingSkill())
                    {
                        GameMgr.Instance.MainEntity.HitDir = GameMgr.Instance.MainEntity.CacheModel.position - CacheModel.position;
                        GameMgr.Instance.MainEntity.EndCurrentStateToOtherState(RoleStateID.CrashPlayer);
                    }
                    else
                    {
                        if (GameMgr.Instance.MainEntity.IsForceDrag)
                        {
                            GameMgr.Instance.MainEntity.EndCurrentStateToOtherState(RoleStateID.Walk);
                        }
                        else
                        {
                            GameMgr.Instance.MainEntity.EndCurrentStateToOtherState(RoleStateID.Idle);
                        }
                        //GameMgr.Instance.MainEntity.EndCurrentStateToOtherState(RoleStateID.Idle);
                    }
                    //EndCurrentStateToOtherState(StateID.Walk);
                }
            }
        }
    }

    public void Clear()
    {
        DespawnerHUDName();
        RoleModel = null;
        ArpgAnimatContorller.animator = null;
        KillCount = 0;
        BeKillCount = 0;
    }
}
