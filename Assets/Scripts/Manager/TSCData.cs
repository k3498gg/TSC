using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSCData : Singleton<TSCData>
{
    private static EnumComparer<MapArea> fastCompare = new EnumComparer<MapArea>();

    private Dictionary<int, NetEntity> m_EntityDic; //实体字典存储中心

    private Dictionary<int, MapInfo> m_dicMapInfo; //地圖道具佈局

    private Dictionary<int, DropItemInfo> m_DropItemDic; //掉落道具

    private Dictionary<MapArea, List<ItemDespawnerInfo>> backItemMapDic; //回收的道具 即將刷新

    private Dictionary<MapArea, List<ItemMapInfo>> totalAreaItemMapDic;

    private Dictionary<int, ObstacleEntity> obstacleDic;

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
            if(null == m_DropItemDic)
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
            if(null == backItemMapDic)
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
            if(null == totalAreaItemMapDic)
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
            if(null == obstacleDic)
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


    public void Clear()
    {
        EntityDic.Clear();
        ObstacleDic.Clear();
        DropItemDic.Clear();
        BackItemMapDic.Clear();
        TotalAreaItemMapDic.Clear();
    }
}
