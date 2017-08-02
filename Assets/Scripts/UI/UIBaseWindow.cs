using UnityEngine;
using System.Collections;
using System;

public class UIBaseWindow : MonoBehaviour
{
    // 当前界面ID
    protected WindowID windowID = WindowID.WindowID_Invaild;
    // 指向上一级界面ID(BackSequence无内容，返回上一级)
    protected WindowID preWindowID = WindowID.WindowID_Invaild;
    public WindowData windowData = null;

    public WindowID GetID
    {
        get
        {
            if (this.windowID == WindowID.WindowID_Invaild)
                Debug.LogError("window id is " + WindowID.WindowID_Invaild);
            return windowID;
        }
        private set { windowID = value; }
    }

    public WindowID GetPreWindowID
    {
        get { return preWindowID; }
        private set { preWindowID = value; }
    }

    public virtual void ResetWindow()
    {
    }

    public virtual void DestroyWindow()
    {
        GameObject.Destroy(this.gameObject);
    }

    /// <summary>
    /// 初始化窗口数据
    /// </summary>
    public virtual void InitWindowData()
    {
        if (windowData == null)
            windowData = new WindowData();
    }

    public virtual void ShowWindow(Action action = null)
    {
        Util.SetActive(this.gameObject, true);
        if(null != action)
        {
            action();
        }
    }

    public virtual void HideWindow(Action action = null)
    {
        Util.SetActive(this.gameObject, false);
        if (null != action)
        {
            action();
        }
    }
}
