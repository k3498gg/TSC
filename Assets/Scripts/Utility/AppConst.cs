using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AppConst
{
    public static string First = "first";
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
    public static float hitRadio = 0.5f;
    public static int factor = 10000;   //配置表换算因子
    public static int randomValue = 7515;
    public static string UIPrefabPath = "UIPrefab/";
    public static string EntityPrefabPath = "Model/";
    public static string ParticlePrefabPath = "Particle/";
    public static string AnimatorPrefabPath = "Animator/";
    public static string ItemPrefabPath = "Item/";
    public static string ObstaclePrefabPath = "Obstacle/";
    public static string NetEntityPrefabPath = "Net/";
    public static string MgrPrefabPath = "Mgr/";
    public static int ItemAreaCount = 0; //区域普通能量点最大数量
    public static int ItemFreshCount = 0; //道具刷新上限
    public static float ItemSugarDistance = 0; //糖果的掉落半径
    public static string TAG_OBSTACLE = "Obstacle";
    public static string TAG_NETENTITY = "NetEntity";
    public static string TAG_PLAYER = "Player";
    public static string TAG_ITEM_MARK = "ItemMark";
    public static string TAG_ITEM_MAGNET = "ItemMagnet";
    public static string TAG_ITEM_TRANSFERGATE = "ItemTransfergate";
    public static string TAG_ITEM_SPEED = "ItemSpeed";
    public static string TAG_ITEM_PROTECT = "ItemProtect";
    public static string TAG_ITEM_ENERGY = "ItemEnergy";
    public static string NET_Entity = "NetEntity";
    public static int DropRareIndex = 0;




    public static Dictionary<WindowID, string> windowPrefabPath = new Dictionary<WindowID, string>()
        {
            { WindowID.WindowID_MainUI, "MainUI" },
            { WindowID.WindowID_FirstUI, "WelcomeUI" },
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

    public static float BaseSpeed;
    public static int RebornTime;
    public static float Basedis;
    public static int MaxPhy;
    public static int AtkAngle;
    public static int CostPhySpeed;
    public static int MaxHp;
    public static float FleeDis;
    public static float ChaseDis;
    public static float AcctInterval;
    public static float SkillInterval;
    public static float FleeInterval;
    public static float AcctMinTime;
    public static float AcctMaxTime;
    public static float KillInterval;

    public static float SkillMinTime;
    public static float SkillMaxTime;
    public static float ChaseTime;

    public static float AIAcceSpeed;
    public static float AISkillSpeed;
    public static int MaxCount;
    public static int MinCount;
    public static int ChaseLev;

    public static float AIDetectInterval;
    public static float AIRandomDirInterval;
    public static float AIRandomItemRadio;

    public static float PhyRecoverSpeed;
    public static float CrashSpeed;
    public static float CrashTime;
    public static float FreshInterval;

    public static float ReliveScoreParam1;
    public static float ReliveScoreParam2;
    public static float ReliveRandomParam1;
    public static float ReliveRandomParam2;

    public static float DropItemRangeParam1;
    public static float DropItemRangeParam2;


    public static void InitConstData()
    {
        ConstInfo areaConstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_ENERGY_MAXCOUNT);
        ConstInfo sugarConstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_SUGAR_RADIO);
        ConstInfo freshConstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_ITEM_COUNT);
        ItemAreaCount = int.Parse(areaConstInfo.data) / (int)AppConst.factor;
        ItemFreshCount = int.Parse(freshConstInfo.data) / (int)AppConst.factor;
        ItemSugarDistance = float.Parse(sugarConstInfo.data) / AppConst.factor;

        ConstInfo speedConstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_SPEED);
        BaseSpeed = float.Parse(speedConstInfo.data) / AppConst.factor ;
        ConstInfo rebornconstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_REBORN);
        RebornTime = int.Parse(rebornconstInfo.data) / AppConst.factor;
        ConstInfo atkDisconstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_ATK_RADIO);
        Basedis = float.Parse(atkDisconstInfo.data) / AppConst.factor;
        ConstInfo maxPhyconstInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_MAX_PHY);
        MaxPhy = int.Parse(maxPhyconstInfo.data) / AppConst.factor;
        ConstInfo perSpeedInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_COST_PHY_SPEED);
        CostPhySpeed = int.Parse(perSpeedInfo.data) / AppConst.factor;
        ConstInfo maxHpInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_MAX_HP);
        MaxHp = int.Parse(maxHpInfo.data) / AppConst.factor;
        ConstInfo atkAngleInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_ATK_ANGLE);
        AtkAngle = int.Parse(atkAngleInfo.data) / AppConst.factor;

        ConstInfo Flee = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_FLEE_DIS);
        FleeDis = float.Parse(Flee.data) / AppConst.factor;
        ConstInfo Chase = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_CHASE_DIS);
        ChaseDis = float.Parse(Chase.data) / AppConst.factor;

        ConstInfo AcctInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_ACC_INTERVAL);
        AcctInterval = float.Parse(AcctInfo.data) / AppConst.factor;
        ConstInfo SkillInter = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_SKILL_INTERVAL);
        SkillInterval = float.Parse(SkillInter.data) / AppConst.factor;

        ConstInfo FleeInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_FLEE_INTERVAL);
        FleeInterval = float.Parse(FleeInfo.data) / AppConst.factor;

        ConstInfo KillInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_KILL_INTERVAL);
        KillInterval = float.Parse(KillInfo.data) / AppConst.factor;

        ConstInfo AcctMinInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_ACC_MINTIME);
        AcctMinTime = float.Parse(AcctMinInfo.data) / AppConst.factor;
        ConstInfo AcctMaxInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_ACC_MAXTIME);
        AcctMaxTime = float.Parse(AcctMaxInfo.data) / AppConst.factor;

        ConstInfo SkillMinTimeInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_SKILL_MINTIME);
        SkillMinTime = float.Parse(SkillMinTimeInfo.data) / AppConst.factor;
        ConstInfo SkillMaxTimeInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_SKILL_MAXTIME);
        SkillMaxTime = float.Parse(SkillMaxTimeInfo.data) / AppConst.factor;
        ConstInfo ChaseTimeInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_CHASE_TIME);
        ChaseTime = float.Parse(ChaseTimeInfo.data) / AppConst.factor;

        ConstInfo AcceSpeedInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_ACCT_SPEED);
        AIAcceSpeed = float.Parse(AcceSpeedInfo.data) / AppConst.factor;
        ConstInfo AISkillSpeedInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_INSTANT_SPEED);
        AISkillSpeed = float.Parse(AISkillSpeedInfo.data) / AppConst.factor;

        ConstInfo MinCountInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_AI_MINCOUNT);
        MinCount = int.Parse(MinCountInfo.data) / AppConst.factor;
        ConstInfo MaxCountInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_AI_MAXCOUNT);
        MaxCount = int.Parse(MaxCountInfo.data) / AppConst.factor;
        ConstInfo ChaseLevInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_CHASE_LEV);
        ChaseLev = int.Parse(ChaseLevInfo.data) / AppConst.factor;

        ConstInfo AIDetectIntervalInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_AI_DETECT_RANGE_INTERVAL);
        AIDetectInterval = int.Parse(AIDetectIntervalInfo.data) / AppConst.factor;
        ConstInfo AIRandomDirIntervalInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_AI_RANDOM_DIR_INTERVAL);
        AIRandomDirInterval = int.Parse(AIRandomDirIntervalInfo.data) / AppConst.factor;

        ConstInfo AIRandomItemRadioInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_AI_RANDOM_ITEM_RADIO);
        AIRandomItemRadio = float.Parse(AIRandomItemRadioInfo.data) / AppConst.factor;

        ConstInfo PhyRecoverSpeednfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_PHY_RECOVER_SPEED);
        PhyRecoverSpeed = float.Parse(PhyRecoverSpeednfo.data) / AppConst.factor;
        ConstInfo CrashSpeedInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_CRASH_SPEED);
        CrashSpeed = float.Parse(CrashSpeedInfo.data) / AppConst.factor;
        ConstInfo CrashTimeInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_CRASH_TIME);
        CrashTime = float.Parse(CrashTimeInfo.data) / AppConst.factor;

        ConstInfo FreshIntervalInfo = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_FRESH_INTERVAL);
        FreshInterval = float.Parse(FreshIntervalInfo.data) / AppConst.factor;


        ConstInfo ReliveScoreParam1Info = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_RELIVE_PARAM1);
        ReliveScoreParam1 = float.Parse(ReliveScoreParam1Info.data) / AppConst.factor;
        ConstInfo ReliveScoreParam2Info = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_RELIVE_PARAM2);
        ReliveScoreParam2 = float.Parse(ReliveScoreParam2Info.data) / AppConst.factor;
        ConstInfo ReliveRandomParam1Info = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_RELIVE_RADOM_PARAM1);
        ReliveRandomParam1 = float.Parse(ReliveRandomParam1Info.data) / AppConst.factor;
        ConstInfo ReliveRandomParam2Info = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_RELIVE_RADOM_PARAM2);
        ReliveRandomParam2 = float.Parse(ReliveRandomParam2Info.data) / AppConst.factor;


        ConstInfo DropItemRangeParam1Info = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_ITEM_DROP_PARAM1);
        DropItemRangeParam1 = float.Parse(DropItemRangeParam1Info.data) / AppConst.factor;
        ConstInfo DropItemRangeParam2Info = InfoMgr<ConstInfo>.Instance.GetInfo((int)ConstType.CONST_ITEM_DROP_PARAM1);
        DropItemRangeParam2 = float.Parse(DropItemRangeParam2Info.data) / AppConst.factor;
    }

    public static void Clear()
    {
        DropRareIndex = 0;
    }
}