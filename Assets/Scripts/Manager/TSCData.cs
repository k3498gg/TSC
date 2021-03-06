﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSCData : Singleton<TSCData>
{
    private static EnumComparer<MapArea> fastCompare = new EnumComparer<MapArea>();

    private Dictionary<int, NetEntity> m_EntityDic; //实体字典存储中心

    private Dictionary<int, EntityInfo> m_EntityInfoDic;

    private Dictionary<int, MapInfo> m_dicMapInfo; //地圖道具佈局

    private Dictionary<int, DropItemInfo> m_DropItemDic; //掉落道具

    private Dictionary<MapArea, List<ItemDespawnerInfo>> backItemMapDic; //回收的道具 即將刷新

    private Dictionary<MapArea, List<ItemMapInfo>> totalAreaItemMapDic;

    private Dictionary<int, ObstacleEntity> obstacleDic;

    private Dictionary<int, DropItemInfo> m_RareEnergyDic;

    private Dictionary<int, SugarEntity> m_SugarEntityDic;

    private HashSet<int> m_skinSet = new HashSet<int>();

    private RoleData role;

    public Dictionary<int, NetEntity> EntityDic
    {
        get
        {
            if (null == m_EntityDic)
            {
                m_EntityDic = new Dictionary<int, NetEntity>();
            }
            return m_EntityDic;
        }

        set
        {
            m_EntityDic = value;
        }
    }

    public Dictionary<int, DropItemInfo> DropItemDic
    {
        get
        {
            if (null == m_DropItemDic)
            {
                m_DropItemDic = new Dictionary<int, DropItemInfo>();
            }
            return m_DropItemDic;
        }

        set
        {
            m_DropItemDic = value;
        }
    }

    public Dictionary<MapArea, List<ItemDespawnerInfo>> BackItemMapDic
    {
        get
        {
            if (null == backItemMapDic)
            {
                backItemMapDic = new Dictionary<MapArea, List<ItemDespawnerInfo>>(fastCompare);
            }
            return backItemMapDic;
        }

        set
        {
            backItemMapDic = value;
        }
    }

    public Dictionary<int, MapInfo> DicMapInfo
    {
        get
        {
            if (null == m_dicMapInfo)
            {
                m_dicMapInfo = new Dictionary<int, MapInfo>();
            }
            return m_dicMapInfo;
        }

        set
        {
            m_dicMapInfo = value;
        }
    }

    public Dictionary<MapArea, List<ItemMapInfo>> TotalAreaItemMapDic
    {
        get
        {
            if (null == totalAreaItemMapDic)
            {
                totalAreaItemMapDic = new Dictionary<MapArea, List<ItemMapInfo>>(fastCompare);
            }
            return totalAreaItemMapDic;
        }

        set
        {
            totalAreaItemMapDic = value;
        }
    }

    public Dictionary<int, ObstacleEntity> ObstacleDic
    {
        get
        {
            if (null == obstacleDic)
            {
                obstacleDic = new Dictionary<int, ObstacleEntity>();
            }
            return obstacleDic;
        }

        set
        {
            obstacleDic = value;
        }
    }

    public Dictionary<int, DropItemInfo> RareEnergyDic
    {
        get
        {
            if (null == m_RareEnergyDic)
            {
                m_RareEnergyDic = new Dictionary<int, DropItemInfo>();
            }
            return m_RareEnergyDic;
        }
        set
        {
            m_RareEnergyDic = value;
        }
    }

    public Dictionary<int, SugarEntity> SugarEntityDic
    {
        get
        {
            if (null == m_SugarEntityDic)
            {
                m_SugarEntityDic = new Dictionary<int, SugarEntity>();
            }
            return m_SugarEntityDic;
        }

        set
        {
            m_SugarEntityDic = value;
        }
    }

    public RoleData Role
    {
        get
        {
            if (null == role)
            {
                role = new RoleData();
            }
            return role;
        }

        set
        {
            role = value;
        }
    }

    public Dictionary<int, EntityInfo> EntityInfoDic
    {
        get
        {
            if (null == m_EntityInfoDic)
            {
                m_EntityInfoDic = new Dictionary<int, EntityInfo>();
            }
            return m_EntityInfoDic;
        }

        set
        {
            m_EntityInfoDic = value;
        }
    }

    public MapInfo GetCurrentMapInfo(int mapId)
    {
        if (DicMapInfo.ContainsKey(mapId))
        {
            return DicMapInfo[mapId];
        }
        return null;
    }


    public List<ItemMapInfo> GetCurItemMapInfo(int mapId)
    {
        if (DicMapInfo.ContainsKey(mapId))
        {
            return DicMapInfo[mapId].ItemMapInfo;
        }
        return null;
    }

    public List<Obstacle> GetCurObstacleInfo(int mapId)
    {
        if (DicMapInfo.ContainsKey(mapId))
        {
            return DicMapInfo[mapId].Obstacles;
        }
        return null;
    }

    public void ClearSkinData()
    {
        m_skinSet.Clear();
    }

    public void AddSkin(int skinId)
    {
        if (!m_skinSet.Contains(skinId))
        {
            m_skinSet.Add(skinId);
        }
    }

    public bool ContainSkin(int skinId)
    {
        return m_skinSet.Contains(skinId);
    }

    public HashSet<int> GetSkin()
    {
        return m_skinSet;
    }


    public void Clear()
    {
        foreach (KeyValuePair<int, NetEntity> kv in EntityDic)
        {
            kv.Value.Clear();
        }
        EntityDic.Clear();
        EntityInfoDic.Clear();
        ObstacleDic.Clear();
        DropItemDic.Clear();
        BackItemMapDic.Clear();
        TotalAreaItemMapDic.Clear();
        RareEnergyDic.Clear();
        SugarEntityDic.Clear();
    }

    float fresh_time = 0;
    internal void Update(float deltaTime)
    {
        if (fresh_time < AppConst.FreshInterval)
        {
            fresh_time += Time.deltaTime;
            return;
        }
        fresh_time = 0;
        foreach (KeyValuePair<int, NetEntity> kv in EntityDic)
        {
            if (kv.Value.IsAlive)
            {
                continue;
            }

            if (Time.realtimeSinceStartup - kv.Value.LastDeadTime > AppConst.FreshInterval)
            {
                kv.Value.Relive();
                //ResourcesMgr.Instance.Spawner(ResourceType.RESOURCE_NET, kv.Value.CacheModel, GameMgr.Instance.EntityRoot, GameMgr.Instance.RandomLocation());
            }
        }
    }

    public void SaveHeroName()
    {
        Util.SaveHeroName(Role.Name);
    }

    void ReadHeroName()
    {
        Role.Name = Util.ReadHeroName();
    }

    void ReadHeroCoin()
    {
        Role.Money = Util.GetHeroData(AppConst.KeyCoin);
    }

    void ReadHeroMaxScore()
    {
        Role.MaxScore = Util.GetHeroData(AppConst.KeyScore);
    }

    void ReadHeroMaxKillCount()
    {
        Role.MaxKillCount = Util.GetHeroData(AppConst.KeyKilled);
    }

    public void ReadHeroData()
    {
        ReadHeroMaxKillCount();
        ReadHeroMaxScore();
        ReadHeroCoin();
        ReadHeroName();
        ReadHeroSkin();
    }

    public void SaveHeroSkin()
    {
        Util.SaveHeroSkin(m_skinSet);
    }

    void ReadHeroSkin()
    {
        m_skinSet.Clear();
        m_skinSet = Util.ReadHeroSkin();
    }

}


public class RoleData
{
    private string name;
    private int keyTigerID;
    private int keyStickID;
    private int keyChickID;
    private int money;
    private int maxScore;
    private int maxKillCount;


    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public int KeyTigerID
    {
        get
        {
            return keyTigerID;
        }

        set
        {
            if (keyTigerID != value)
            {
                keyTigerID = value;
            }
        }
    }

    public int KeyStickID
    {
        get
        {
            return keyStickID;
        }

        set
        {
            if (keyStickID != value)
            {
                keyStickID = value;
            }
        }
    }

    public int KeyChickID
    {
        get
        {
            return keyChickID;
        }

        set
        {
            if (keyChickID != value)
            {
                keyChickID = value;
            }
        }
    }

    public int Money
    {
        get
        {
            return money - AppConst.randomValue;
        }

        set
        {
            money = value + AppConst.randomValue;
        }
    }

    public int MaxScore
    {
        get
        {
            return maxScore;
        }

        set
        {
            maxScore = value;
        }
    }

    public int MaxKillCount
    {
        get
        {
            return maxKillCount;
        }

        set
        {
            maxKillCount = value;
        }
    }
}