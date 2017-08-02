using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindowData
{
    public bool isStartWindow = false; //标记是否为启动的窗口界面，如果回到启动窗口界面，清楚缓存链
    public UIWindowType windowType = UIWindowType.Normal;
    public UIWindowShowMode showMode = UIWindowShowMode.DoNothing;
    public UIWindowColliderMode colliderMode = UIWindowColliderMode.None;
}

public class BackWindowSequenceData
{
    public UIBaseWindow hideTargetWindow;
    public List<WindowID> backShowTargets; //记录windowdata子界面
}

//public class ShowWindowData
//{
//    // Reset窗口
//    public bool forceResetWindow = false;
//    // Clear导航信息
//    public bool forceClearBackSeqData = false;
//    // Object 数据
//    public object data;
//}

public delegate bool BoolDelegate();


