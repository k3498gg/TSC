using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIOver : UIBaseWindow
{
    private GameObject m_YesBtn;
    private GameObject m_NoBtn;
    private bool init = false;
    private Action YesAction;
    private Action NoAction;
    private Text m_text;
    private Text m_title;

    private void Awake()
    {
        InitWindowData();
    }

    public override void InitWindowData()
    {
        base.InitWindowData();
        windowData.showMode = UIWindowShowMode.DoNothing;
        windowData.windowType = UIWindowType.PopUp;
        this.windowID = WindowID.WindowID_Over;
        this.preWindowID = WindowID.WindowID_Invaild; //初始化的界面前置界面为null
        Init();
    }


    void Init()
    {
        if(init)
        {
            return;
        }
        init = true;
        //m_cache = transform;
        m_YesBtn = Cache.Find("Yes").gameObject;
        m_NoBtn = Cache.Find("No").gameObject;
        m_text = Cache.Find("content").GetComponent<Text>();
        m_title = Cache.Find("title").GetComponent<Text>();

        UGUIEventListener.Get(m_YesBtn).onClick = YesEvent;
        UGUIEventListener.Get(m_NoBtn).onClick = NoEvent;
    }

    public void BindAction(Action _YesAction,Action _NoAction)
    {
        YesAction = _YesAction;
        NoAction = _NoAction;
    }

    public void SetTextContent(string title,string text)
    {
        m_title.text = title;
        m_text.text = text;
    }


    void Close()
    {
        UIManager.Instance.HideWindow(WindowID.WindowID_Confirm);
    }

    void YesEvent(GameObject go)
    {
        if(null != YesAction)
        {
            YesAction();
            YesAction = null;
        }
        Close();
    }
    
    void NoEvent(GameObject go)
    {
        if(null != NoAction)
        {
            NoAction();
            NoAction = null;
        }
        Close();
    }
}
