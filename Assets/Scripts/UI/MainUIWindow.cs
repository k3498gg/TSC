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
            //UGUIEventListener.Get(m_skills[i].gameObject).onClick = TriggerSkill;
            UGUIEventListener.Get(m_skills[i].gameObject).onPress = OnPress;
        }
    }

    void OnPress(GameObject go, bool press)
    {
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
        EventCenter.Instance.Register<Event_StopSkill>(StopSkill);
        EventCenter.Instance.Register<Event_StopAcct>(StopAcct);
        EventCenter.Instance.Register<Event_OpenAcct>(OpenAcct);
        BindStickEvt();
    }

    private void OnDisable()
    {
        EventCenter.Instance.Unregister<Event_StopSkill>(StopSkill);
        EventCenter.Instance.Unregister<Event_StopAcct>(StopAcct);
        EventCenter.Instance.Unregister<Event_OpenAcct>(OpenAcct);
        UnBindStickEvt();
    }

    void StopSkill(object o,Event_StopSkill evt)
    {
        EffType = EffectType.NONE;
    }

    void StopAcct(object o,Event_StopAcct evt)
    {
        Debug.LogError("停止使用加速技能");
    }

    void OpenAcct(object o,Event_OpenAcct evt)
    {
        Debug.LogError("可以使用加速技能");
    }

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
        if (EffType == EffectType.WALKINSTANT)
        {
            return;
        }

        float angle = Mathf.Rad2Deg * (Mathf.Atan2(x, y));
        GameMgr.Instance.MainEntity.CacheModel.rotation = Quaternion.Euler(0, angle + GameMgr.Instance.CameraController.EulerY, 0);
        if (EffType != EffectType.ACCELERATE)
        {
            GameMgr.Instance.ARPGAnimatController.Walk = true;
        }
        GameMgr.Instance.CharacController.SimpleMove(GameMgr.Instance.MainEntity.CacheModel.forward * Time.deltaTime * GameMgr.Instance.MainEntity.Attribute.Speed);
    }

    private void UpEvent(float x, float y)
    {
        GameMgr.Instance.ARPGAnimatController.Walk = false;
    }

    private void Awake()
    {
        InitWindowData();
    }

    private void FixedUpdate()
    {
        if (null == GameMgr.Instance.MainEntity)
        {
            return;
        }

        switch (EffType)
        {
            case EffectType.ACCELERATE:
                AccelerateSkill();
                break;
            case EffectType.WALKINSTANT:
                WalkInstant();
                break;
        }

        if (GameMgr.Instance.MainEntity.IsRecoverEnergy)
        {
            if (GameMgr.Instance.MainEntity.Attribute.CurPhy < GameMgr.Instance.MainEntity.Attribute.MaxPhy)
            {
                GameMgr.Instance.MainEntity.Attribute.CurPhy += GameMgr.Instance.MainEntity.Attribute.CostPhySpeed * Time.deltaTime;
            }
            else
            {
                GameMgr.Instance.MainEntity.IsRecoverEnergy = false;
            }
            float val = GameMgr.Instance.MainEntity.Attribute.CurPhy / GameMgr.Instance.MainEntity.Attribute.MaxPhy;
            m_timerImage[0].fillAmount = val;
        }
    }

    //加速技能
    void AccelerateSkill()
    {
        if (GameMgr.Instance.MainEntity.Attribute.CurPhy <= 0)
        {
            CancelSkill(m_skills[0].gameObject);
            return;
        }
        GameMgr.Instance.MainEntity.Attribute.CurPhy = GameMgr.Instance.MainEntity.Attribute.CurPhy - GameMgr.Instance.MainEntity.Attribute.CostPhySpeed * Time.deltaTime;
        float val = GameMgr.Instance.MainEntity.Attribute.CurPhy / GameMgr.Instance.MainEntity.Attribute.MaxPhy;
        m_timerImage[0].fillAmount = val;
    }

    void WalkInstant()
    {
        GameMgr.Instance.CharacController.SimpleMove(GameMgr.Instance.MainEntity.CacheModel.forward * Time.deltaTime * GameMgr.Instance.MainEntity.Attribute.Speed);
    }

    void CancelAccelerate()
    {
        EffType = EffectType.NONE;
        GameMgr.Instance.MainEntity.StopAccelerate();
    }

    void CancelSkill(GameObject go)
    {
        if (EffType == EffectType.NONE)
        {
            return;
        }
        int idx = System.Array.IndexOf(m_skills, go.transform);
        if (idx != -1)
        {
            switch (EffType)
            {
                case EffectType.ACCELERATE:
                    CancelAccelerate();
                    break;
                case EffectType.WALKINSTANT:

                    break;
            }
        }
    }

    void TriggerSkill(GameObject go)
    {
        if (EffType != EffectType.NONE)
        {
            return;
        }

        int idx = System.Array.IndexOf(m_skills, go.transform);
        if (idx != -1)
        {
            Debuger.LogError(go.name + ": 您选择技能" + m_skillInfos[idx].SkillId);
            SkillEffect(idx);
        }
    }

    void SkillEffect(int idx)
    {
        SkillInfo skillInfo = InfoMgr<SkillInfo>.Instance.GetInfo(m_skillInfos[idx].SkillId);
        EffectInfo effectInfo = InfoMgr<EffectInfo>.Instance.GetInfo(skillInfo.effectID);
        EffType = (EffectType)effectInfo.id;
        if (EffType == EffectType.ACCELERATE)//加速
        {
            GameMgr.Instance.MainEntity.Accelerate(skillInfo,effectInfo);
        }
        else if (EffType == EffectType.WALKINSTANT) //冲锋
        {
            GameMgr.Instance.MainEntity.Walkinstant(skillInfo,effectInfo);
        }

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
