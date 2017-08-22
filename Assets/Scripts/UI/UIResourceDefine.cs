using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum WindowID
{
    WindowID_Invaild = 0,
    WindowID_FirstUI,
    WindowID_MainUI,

    //WindowID_Rank,          // 排行榜界面
    //WindowID_Rank_Detail,   // 排行榜详情界面
    //WindowID_Rank_OwnDetail,
    //WindowID_Level,
    //WindowID_LevelDetail,
    //WindowID_Matching,
    //WindowID_MatchResult,
    //WindowID_Skill,
    //WindowID_TopBar,
    //WindowID_MessageBox,
}

public enum UIWindowType
{
    Normal,    // 可退出界面(普通窗口类型等)
    Fixed,     // 固定窗口(UITopBar等)
    PopUp,     // 模式窗口(固定弹框，屏蔽弹框一下界面大的操作)
}

public enum UIWindowShowMode
{
    DoNothing,
    HideOther,     // 关闭闭其他界面
    NeedBack,      // 点击返回按钮关闭当前,不关闭其他界面(需要调整好层级关系)
    NoNeedBack,    // 关闭TopBar,关闭其他界面,不加入backSequence队列
}

public enum UIWindowColliderMode
{
    None,      // 显示该界面不包含碰撞背景
    Normal,    // 碰撞透明背景
    WithBg,    // 碰撞非透明背景
}


