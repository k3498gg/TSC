using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AppConst
{
    public static bool DebugMode = true;                       //调试模式
    public static string AppName = "Self";                       //应用程序名称
    public static string ExtName = ".unity3d";                   //素材扩展名
    public static string WWWDir = "StreamingAssets";           //素材目录 
    public static string ExtFbx = ".fbx";                        //fbx后缀名
    public static string AssetPath = "Assets";                                   
    public static string TextDir = "client";                     //文本文件目录
    public static string FileBin = "file.txt";                   //配置文件信息汇总
    public static char Separate = '|';
    public static string Version = "1.0";
    public static string VersionKey = "VersionKey";
    public static uint factor = 10000;   //配置表换算因子
    public static uint randomValue = 7515;
    public static string UIPrefabPath = "UIPrefab/";
    public static string EntityPrefabPath = "Model/";
    public static string ParticlePrefabPath = "Particle/";
    public static string AnimatorPrefabPath = "Animator/";
    public static string ItemPrefabPath = "Item/";
    public static int ItemAreaCount = 0; //区域普通能量点最大数量
    public static int ItemFreshCount = 0; //道具刷新上限
    public static float ItemSugarDistance = 0; //糖果的掉落半径


    public static Dictionary<WindowID, string> windowPrefabPath = new Dictionary<WindowID, string>()
        {
            { WindowID.WindowID_MainUI, "MainUI" },
        };

    public static string AppContentPath
    {
        get
        {
            return
#if UNITY_ANDROID
                "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE
                "file://"+Application.dataPath + "/Raw/";
#else
                "file:///" + Application.dataPath + "/" + WWWDir + "/";
#endif
        }
    }

    public static string AppStreamingPath
    {
        get
        {
            return Application.streamingAssetsPath;
        }
    }

    public static string AppPersistentPath
    {
        get
        {
            return Application.persistentDataPath;
        }
    }

    public static void InitConstData()
    {
        ConstInfo areaConstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_ENERGY_MAXCOUNT);
        ConstInfo sugarConstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_SUGAR_RADIO);
        ConstInfo freshConstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_ITEM_COUNT);
        ItemAreaCount = int.Parse(areaConstInfo.data) /(int) AppConst.factor;
        ItemFreshCount = int.Parse(freshConstInfo.data) / (int)AppConst.factor;
        ItemSugarDistance = float.Parse(sugarConstInfo.data) / AppConst.factor;
    }
} 