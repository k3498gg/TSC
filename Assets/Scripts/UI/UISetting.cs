using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : UIBaseWindow
{
    private GameObject m_close;
    private Slider m_slider;
    private bool init = false;

    private void Awake()
    {
        InitWindowData();
    }

    public override void InitWindowData()
    {
        base.InitWindowData();
        windowData.showMode = UIWindowShowMode.DoNothing;
        windowData.windowType = UIWindowType.PopUp;
        this.windowID = WindowID.WindowID_Setting;
        this.preWindowID = WindowID.WindowID_Invaild; //初始化的界面前置界面为null
        Init();
    }

    private void Init()
    {
        if(init)
        {
            return;
        }
        init = true;
        m_close = Cache.Find("Close").gameObject;
        m_slider = Cache.Find("Slider").GetComponent<Slider>();

        m_slider.onValueChanged.AddListener(OnValueChanged);
        UGUIEventListener.Get(m_close).onClick = Close;
    }

    void Close(GameObject go)
    {
        UIManager.Instance.HideWindow(WindowID.WindowID_Setting);
    }

    void OnValueChanged(float val)
    {
        Debuger.LogError("val:" + val);
    }
}
