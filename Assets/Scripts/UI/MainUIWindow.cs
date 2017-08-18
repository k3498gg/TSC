using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TouchControlsKit;
using DG.Tweening;

public class MainUIWindow : UIBaseWindow
{
    private TCKJoystick m_Joystick;
    private Transform[] m_skills;
    private Image[] m_btnImage;
    private Image[] m_timerImage;
    private CanvasRenderer[] m_Renders;
    private UISkill[] m_skillInfos;
    private bool isInit = false;
    private EffectType m_EffectType = EffectType.NONE;
    private UIDead m_UIDead;

    public EffectType EffType
    {
        get
        {
            return m_EffectType;
        }

        set
        {
            m_EffectType = value;
        }
    }

    public override void InitWindowData()
    {
        base.InitWindowData();
        windowData.showMode = UIWindowShowMode.DoNothing;
        windowData.windowType = UIWindowType.Normal;
        this.windowID = WindowID.WindowID_MainUI;
        this.preWindowID = WindowID.WindowID_Invaild; //初始化的界面前置界面为null
        Init();
    }

    private void Init()
    {
        if (isInit)
        {
            return;
        }
        isInit = true;
        Transform cache = transform;
        m_Joystick = cache.Find("Joystick").GetComponent<TCKJoystick>();
        m_UIDead = cache.Find("Dead").GetComponent<UIDead>();

        m_skills = new Transform[2];
        m_btnImage = new Image[2];
        m_timerImage = new Image[2];
        m_Renders = new CanvasRenderer[2];
        m_skillInfos = new UISkill[2];

        for (int i = 0; i < m_skills.Length; i++)
        {
            m_skills[i] = cache.Find("Skill/Skill" + (i + 1).ToString());
            m_btnImage[i] = m_skills[i].GetComponent<Image>();
            m_timerImage[i] = cache.Find("Skill/Skill" + (i + 1).ToString() + "/Timer").GetComponent<Image>();
            m_Renders[i] = m_timerImage[i].GetComponent<CanvasRenderer>();
            m_skillInfos[i] = m_skills[i].GetComponent<UISkill>();

            if (i < GameMgr.Instance.MainEntity.Attribute.Skills.Length)
            {
                m_skillInfos[i].SkillId = GameMgr.Instance.MainEntity.Attribute.Skills[i];
            }
            if (i == 0)
            {
                m_Renders[i].cull = false;
                m_timerImage[i].fillAmount = GameMgr.Instance.MainEntity.Attribute.CurPhy / GameMgr.Instance.MainEntity.Attribute.MaxPhy;
            }
            else
            {
                m_Renders[i].cull = true;
            }
            UGUIEventListener.Get(m_skills[i].gameObject).onPress = OnPress;
        }
    }

    void OnPress(GameObject go, bool press)
    {
        if (!GameMgr.Instance.MainEntity.IsAlive)
        {
            return;
        }

        if (press)
        {
            TriggerSkill(go);
        }
        else
        {
            CancelSkill(go);
        }
    }

    private void OnEnable()
    {
        //EventCenter.Instance.Register<Event_StopSkill>(StopSkill);
        //EventCenter.Instance.Register<Event_StopAcct>(StopAcct);
        //EventCenter.Instance.Register<Event_OpenAcct>(OpenAcct);
        EventCenter.Instance.Register<Event_RoleDead>(RoleDead);
        BindStickEvt();
    }

    private void OnDisable()
    {
        //EventCenter.Instance.Unregister<Event_StopSkill>(StopSkill);
        //EventCenter.Instance.Unregister<Event_StopAcct>(StopAcct);
        //EventCenter.Instance.Unregister<Event_OpenAcct>(OpenAcct);
        EventCenter.Instance.Unregister<Event_RoleDead>(RoleDead);
        UnBindStickEvt();
    }

    void RoleDead(object o, Event_RoleDead evt)
    {
        if (null != m_UIDead)
        {
            m_UIDead.ShowDeadUI();
        }
    }

    //void StopSkill(object o, Event_StopSkill evt)
    //{
    //    EffType = EffectType.NONE;
    //}

    //void StopAcct(object o, Event_StopAcct evt)
    //{
    //    Debug.LogError("停止使用加速技能");
    //}

    //void OpenAcct(object o, Event_OpenAcct evt)
    //{
    //    Debug.LogError("可以使用加速技能");
    //}

    void BindStickEvt()
    {
        if (null != m_Joystick)
        {
            m_Joystick.BindAxes(DownEvent, EActionEvent.Press);
            m_Joystick.BindAxes(UpEvent, EActionEvent.Up);
        }
    }

    void UnBindStickEvt()
    {
        if (null != m_Joystick)
        {
            m_Joystick.UnBindAxes(DownEvent, EActionEvent.Press);
            m_Joystick.UnBindAxes(UpEvent, EActionEvent.Up);
        }
    }

    private void DownEvent(float x, float y)
    {
        if (x == 0 && y == 0)
        {
            return;
        }

        if (null == GameMgr.Instance)
        {
            return;
        }

        if (null == GameMgr.Instance.MainEntity)
        {
            return;
        }

        if (null == GameMgr.Instance.CameraController)
        {
            return;
        }

        if (!GameMgr.Instance.ARPGAnimatController.IsCanMove)
        {
            return;
        }

        if (!GameMgr.Instance.MainEntity.IsAlive)
        {
            return;
        }

        if (GameMgr.Instance.MainEntity.RoleEntityControl.Fsm.CurrentStateID == RoleStateID.Skill || GameMgr.Instance.MainEntity.RoleEntityControl.Fsm.CurrentStateID == RoleStateID.CrashPlayer)
        {
            return;
        }

        float angle = Mathf.Rad2Deg * (Mathf.Atan2(x, y));
        GameMgr.Instance.MainEntity.UpdateRotation(angle);
        if (EffType == EffectType.ACCELERATE)
        {
            GameMgr.Instance.MainEntity.EndCurrentStateToOtherState(RoleStateID.Acct);
        }
        else
        {
            GameMgr.Instance.MainEntity.EndCurrentStateToOtherState(RoleStateID.Walk);
        }
    }

    private void UpEvent(float x, float y)
    {
        GameMgr.Instance.MainEntity.EndCurrentStateToOtherState(RoleStateID.Idle);
    }

    private void Awake()
    {
        InitWindowData();
    }

    private void Update()
    {
        if (null == GameMgr.Instance.MainEntity)
        {
            return;
        }

        if (GameMgr.Instance.MainEntity.IsRecoverEnergy)
        {
            if (GameMgr.Instance.MainEntity.Attribute.CurPhy < GameMgr.Instance.MainEntity.Attribute.MaxPhy)
            {
                GameMgr.Instance.MainEntity.Attribute.CurPhy += AppConst.PhyRecoverSpeed * Time.deltaTime;
            }
            else
            {
                GameMgr.Instance.MainEntity.IsRecoverEnergy = false;
            }
            float val = GameMgr.Instance.MainEntity.Attribute.CurPhy / GameMgr.Instance.MainEntity.Attribute.MaxPhy;
            m_timerImage[0].fillAmount = val;
        }
        else
        {
            if (EffType == EffectType.ACCELERATE)
            {
                float val = GameMgr.Instance.MainEntity.Attribute.CurPhy / GameMgr.Instance.MainEntity.Attribute.MaxPhy;
                m_timerImage[0].fillAmount = val;
            }
        }
    }

    void CancelSkill(GameObject go)
    {
        EffType = EffectType.NONE;
        int idx = System.Array.IndexOf(m_skills, go.transform);
        if (idx != -1)
        {
            switch (GameMgr.Instance.MainEntity.RoleEntityControl.Fsm.CurrentStateID)
            {
                case RoleStateID.Acct:
                    //GameMgr.Instance.MainEntity.RoleEntityControl.SetTransition(RoleTransition.FreeWalk, GameMgr.Instance.MainEntity);
                    GameMgr.Instance.MainEntity.EndCurrentStateToOtherState(RoleStateID.Idle);
                    break;
                case RoleStateID.Skill:

                    break;
            }
        }
    }

    void TriggerSkill(GameObject go)
    {
        if (GameMgr.Instance.MainEntity.IsUsingAcctOrSkill())
        {
            return;
        }

        int idx = System.Array.IndexOf(m_skills, go.transform);
        if (idx != -1)
        {
            SkillEvent(idx);
        }
    }

    void SkillEvent(int idx)
    {
        EffType = (EffectType)(idx + 1);
        switch (EffType)
        {
            case EffectType.ACCELERATE:
                //GameMgr.Instance.MainEntity.EndCurrentStateToOtherState(RoleStateID.Acct);
                break;
            case EffectType.WALKINSTANT:
                GameMgr.Instance.MainEntity.EndCurrentStateToOtherState(RoleStateID.Skill);
                break;
        }

        SkillInfo skillInfo = InfoMgr<SkillInfo>.Instance.GetInfo(m_skillInfos[idx].SkillId);
        EffectInfo effectInfo = InfoMgr<EffectInfo>.Instance.GetInfo(skillInfo.effectID);
        if (effectInfo.cd > 0)
        {
            m_btnImage[idx].raycastTarget = false;
            m_timerImage[idx].fillAmount = 1;
            m_Renders[idx].cull = false;
            m_timerImage[idx].DOFillAmount(0, (float)effectInfo.cd / AppConst.factor).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    m_btnImage[idx].raycastTarget = true;
                    m_Renders[idx].cull = true;
                });
        }
        else
        {
            Debug.LogError("技能没有CD。。。,有能量消耗");
        }
    }
}
