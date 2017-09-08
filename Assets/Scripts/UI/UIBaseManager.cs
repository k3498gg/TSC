using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public abstract class UIBaseManager : UnitySingleton<UIBaseManager>
{
    private static EnumComparer<WindowID> fastComparer = new EnumComparer<WindowID>();
    private Dictionary<WindowID, UIBaseWindow> allWindows;
    private Dictionary<WindowID, UIBaseWindow> shownWindows;
    private Stack<BackWindowSequenceData> backSequence;
    protected int managedWindowId = 0;

    //// 当前显示活跃界面
    //protected UIBaseWindow curShownNormalWindow = null;
    //// 上一活跃界面
    //protected UIBaseWindow lastShownNormalWindow = null;


    protected Dictionary<WindowID, UIBaseWindow> AllWindows
    {
        get
        {
            if (null == allWindows)
            {
                allWindows = new Dictionary<WindowID, UIBaseWindow>(fastComparer);
            }
            return allWindows;
        }
        set
        {
            allWindows = value;
        }
    }

    internal abstract Camera GetUiCamera();

    protected Dictionary<WindowID, UIBaseWindow> ShownWindows
    {
        get
        {
            if (null == shownWindows)
            {
                shownWindows = new Dictionary<WindowID, UIBaseWindow>(fastComparer);
            }
            return shownWindows;
        }
        set
        {
            shownWindows = value;
        }
    }

    protected Stack<BackWindowSequenceData> BackSequence
    {
        get
        {
            if (null == backSequence)
            {
                backSequence = new Stack<BackWindowSequenceData>();
            }
            return backSequence;
        }
        set
        {
            backSequence = value;
        }
    }

    public virtual UIBaseWindow GetGameWindow(WindowID id)
    {
        if (!IsWindowInControl(id))
            return null;
        if (AllWindows.ContainsKey(id))
            return AllWindows[id];
        else
            return null;
    }

    public virtual T GetGameWindowScript<T>(WindowID id) where T : UIBaseWindow
    {
        UIBaseWindow baseWindow = GetGameWindow(id);
        if (baseWindow != null)
            return (T)baseWindow;
        return (T)((object)null);
    }

    protected bool IsWindowInControl(WindowID id)
    {
        int targetId = 1 << ((int)id);
        return ((managedWindowId & targetId) == targetId);
    }

    protected void AddWindowInControl(WindowID id)
    {
        int targetId = 1 << ((int)id);
        managedWindowId |= targetId;
    }

    public virtual void ShowWindow(WindowID id)
    {

    }

    protected virtual void RealShowWindow(UIBaseWindow baseWindow, WindowID id)
    {
        baseWindow.ShowWindow();
        shownWindows[id] = baseWindow;
        //if (baseWindow.windowData.windowType == UIWindowType.Normal)
        //{
        //    // 改变当前显示Normal窗口
        //    lastShownNormalWindow = curShownNormalWindow;
        //    curShownNormalWindow = baseWindow;
        //}
    }

    public void ClearBackSequence()
    {
        if (backSequence != null)
            backSequence.Clear();
    }

    protected virtual UIBaseWindow ReadyToShowBaseWindow(WindowID id)
    {
        return null;
    }

    public virtual void ClearAllWindow()
    {
        if (allWindows != null)
        {
            foreach (KeyValuePair<WindowID, UIBaseWindow> window in allWindows)
            {
                UIBaseWindow baseWindow = window.Value;
                baseWindow.DestroyWindow();
            }
            allWindows.Clear();
            shownWindows.Clear();
            backSequence.Clear();
        }
    }

    public virtual void HideWindow(WindowID id, Action onCompleted = null)
    {
        WindowDirectlyHide(id, onCompleted);
    }

    protected virtual void WindowDirectlyHide(WindowID id, Action onComplete)
    {
        if (!IsWindowInControl(id))
        {
            Debug.Log("UIRankManager has no control power of " + id.ToString());
            return;
        }
        if (!shownWindows.ContainsKey(id))
            return;

    
        if (onComplete != null)
            onComplete();

        shownWindows[id].HideWindow(null);
        shownWindows.Remove(id);
    }

    //protected bool RealReturnWindow()
    //{
    //    if (backSequence.Count == 0)
    //    {
    //        // 如果当前BackSequenceData 不存在返回数据
    //        // 检测当前Window的preWindowId是否指向上一级合法菜单
    //        if (curShownNormalWindow == null)
    //            return false;

    //        WindowID preWindowId = curShownNormalWindow.GetPreWindowID;
    //        if (preWindowId != WindowID.WindowID_Invaild)
    //        {
    //            HideWindow(curShownNormalWindow.GetID, delegate
    //            {
    //                ShowWindow(preWindowId);
    //            });
    //        }
    //        else
    //            Debug.LogWarning("currentShownWindow " + curShownNormalWindow.GetID + " preWindowId is " + WindowID.WindowID_Invaild);
    //        return false;
    //    }
    //    BackWindowSequenceData backData = backSequence.Peek();
    //    if (backData != null)
    //    {
    //        WindowID hideId = backData.hideTargetWindow.GetID;
    //        if (backData.hideTargetWindow != null && shownWindows.ContainsKey(hideId))
    //            HideWindow(hideId, delegate
    //            {
    //                if (backData.backShowTargets != null)
    //                {
    //                    for (int i = 0; i < backData.backShowTargets.Count; i++)
    //                    {
    //                        WindowID backId = backData.backShowTargets[i];
    //                        ShowWindowForBack(backId);
    //                        if (i == backData.backShowTargets.Count - 1)
    //                        {
    //                            Debug.Log("change currentShownNormalWindow : " + backId);
    //                            {
    //                                // 改变当前活跃Normal窗口
    //                                this.lastShownNormalWindow = this.curShownNormalWindow;
    //                                this.curShownNormalWindow = GetGameWindow(backId);
    //                            }
    //                        }
    //                    }
    //                }

    //                // 隐藏当前界面
    //                backSequence.Pop();
    //            });
    //        else
    //            return false;
    //    }
    //    return true;
    //}

    //protected void ShowWindowForBack(WindowID id)
    //{
    //    // 检测控制权限
    //    if (!this.IsWindowInControl(id))
    //    {
    //        Debug.Log("UIManager has no control power of " + id.ToString());
    //        return;
    //    }
    //    if (shownWindows.ContainsKey(id))
    //        return;

    //    UIBaseWindow baseWindow = GetGameWindow(id);
    //    baseWindow.ShowWindow();
    //    shownWindows[baseWindow.GetID] = baseWindow;

    //}


    protected abstract void InitWindowControl();
}