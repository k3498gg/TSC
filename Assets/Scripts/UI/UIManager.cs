using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIManager : UIBaseManager
{
    public Transform UIParent;
    public Transform UINormalWindowRoot;
    public Transform UIPopUpWindowRoot;
    public Transform UIFixedWidowRoot;
    private bool init = false;
    private Camera uiCamera;
    private Canvas m_canvas;

    internal override Camera GetUiCamera()
    {
        if (null == uiCamera)
        {
            if (null != m_canvas)
            {
                uiCamera = m_canvas.worldCamera;
            }
        }
        return uiCamera;
    }

    //private void Start()
    //{
    //    ShowWindow(WindowID.WindowID_FirstUI);
    //}

    void Init()
    {
        if (init)
        {
            return;
        }
        init = true;
        if (null == UIParent)
        {
            UIParent = transform.parent;
            m_canvas = UIParent.GetComponent<Canvas>();
        }

        //通用弹框父节点
        if (null == UINormalWindowRoot)
        {
            UINormalWindowRoot = new GameObject("UINormalWindowRoot").transform;
            Util.AddChildToTarget(UIParent, UINormalWindowRoot);
        }

        //置顶弹出的父节点
        if (UIPopUpWindowRoot == null)
        {
            UIPopUpWindowRoot = new GameObject("UIPopUpWindowRoot").transform;
            Util.AddChildToTarget(UIParent, UIPopUpWindowRoot);
        }

        //固定位置的界面父节点 类似ToolBar
        if (UIFixedWidowRoot == null)
        {
            UIFixedWidowRoot = new GameObject("UIFixedWidowRoot").transform;
            Util.AddChildToTarget(UIParent, UIFixedWidowRoot);
        }

        InitWindowControl();
    }

    protected override void InitWindowControl()
    {
        this.managedWindowId = 0;
        AddWindowInControl(WindowID.WindowID_FirstUI);
        AddWindowInControl(WindowID.WindowID_MainUI);
        AddWindowInControl(WindowID.WindowID_Shopping);
        AddWindowInControl(WindowID.WindowID_Bag);
        AddWindowInControl(WindowID.WindowID_Setting);
        AddWindowInControl(WindowID.WindowID_Confirm);
    }

    public override void ShowWindow(WindowID id)
    {
        Init();
        UIBaseWindow baseWindow = ReadyToShowBaseWindow(id);

        if (baseWindow != null)
        {
            RealShowWindow(baseWindow, id);

            // 是否清空当前导航信息(回到主菜单)
            if (baseWindow.windowData.isStartWindow)
            {
                ClearBackSequence();
            }
        }
    }


    protected override UIBaseWindow ReadyToShowBaseWindow(WindowID id)
    {
        // 检测控制权限
        if (!this.IsWindowInControl(id))
        {
            Debug.Log("UIManager has no control power of " + id.ToString());
            return null;
        }
        if (ShownWindows.ContainsKey(id))
            return null;

        // 隐藏callWindowId指向窗口
        /*if(showData != null)
            HideWindow(showData.callWindowId, null);*/

        UIBaseWindow baseWindow = GetGameWindow(id);
        bool newAdded = false;
        if (!baseWindow)
        {
            newAdded = true;
            // 窗口不存在从内存进行加载
            if (AppConst.windowPrefabPath.ContainsKey(id))
            {
                GameObject prefab = ResourcesMgr.Instance.LoadResource<GameObject>(ResourceType.RESOURCE_UI, AppConst.windowPrefabPath[id]);
                if (prefab != null)
                {
                    GameObject uiObject = (GameObject)GameObject.Instantiate(prefab);
                    Util.SetActive(uiObject, true);
                    baseWindow = uiObject.GetComponent<UIBaseWindow>();
                    switch (baseWindow.windowData.windowType)
                    {
                        case UIWindowType.Normal:
                            Util.AddChildToTarget(UINormalWindowRoot, baseWindow.Cache);
                            break;
                        case UIWindowType.Fixed:
                            Util.AddChildToTarget(UIFixedWidowRoot, baseWindow.Cache);
                            break;
                        case UIWindowType.PopUp:
                            Util.AddChildToTarget(UIPopUpWindowRoot, baseWindow.Cache);
                            break;
                    }

                    AllWindows[id] = baseWindow;
                }
            }
        }

        if (baseWindow == null)
        {
            Debug.LogError("[window instance is null.]" + id.ToString());
            return null;
        }

        if (newAdded)
            baseWindow.ResetWindow();

        //// 导航系统数据更新
        //RefreshBackSequenceData(baseWindow);
        //// 调整层级depth
        //AdjustBaseWindowDepth(baseWindow);
        //// 添加背景Collider
        //AddColliderBgForWindow(baseWindow);
        return baseWindow;
    }

}

