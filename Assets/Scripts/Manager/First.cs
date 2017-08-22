using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class First : UIBaseWindow
{
    private bool isInit = false;
    private InputField m_InputField;

    void Awake()
    {
        InitWindowData();
    }

    public override void InitWindowData()
    {
        base.InitWindowData();
        windowData.showMode = UIWindowShowMode.DoNothing;
        windowData.windowType = UIWindowType.Normal;
        this.windowID = WindowID.WindowID_FirstUI;
        this.preWindowID = WindowID.WindowID_Invaild; //初始化的界面前置界面为null
        Init();
    }

    void Init()
    {
        if (isInit)
        {
            return;
        }
        isInit = true;

        Transform cache = transform;
        Transform m_Btn = cache.Find("Button");
        m_InputField = cache.Find("InputField").GetComponent<InputField>();
        m_InputField.onValueChanged.AddListener(OnValueChanged);
        UGUIEventListener.Get(m_Btn.gameObject).onClick = EnterGame;
    }


    void EnterGame(GameObject go)
    {
        if (null != m_InputField)
        {
            string name = m_InputField.text.Trim();
            if (!string.IsNullOrEmpty(name))
            {
                SceneManager.LoadScene(2);
            }else
            {
                m_InputField.placeholder.color = Color.red;
            }
        }
    }

    void OnValueChanged(string value)
    {
        if(string.IsNullOrEmpty(value))
        {
            m_InputField.placeholder.color = Color.red;
        }else
        {
            m_InputField.text = m_InputField.text.Trim();
        }
    }


}
