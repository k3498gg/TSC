using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMgr : Singleton<EntityMgr>
{
    private static EnumComparer<MapArea> fastCompare = new EnumComparer<MapArea>();

    private Dictionary<int, IEntity> m_EntityDic; //实体字典存储中心

    private Dictionary<int, MapInfo> m_dicMapInfo; //地圖道具佈局

    private Dictionary<int, DropItemInfo> m_DropItemDic;

    private Dictionary<MapArea, List<int>> backItemMapDic; //回收的道具 即將刷新

    public Dictionary<int, IEntity> EntityDic
    {
        get
        {
            if (null == m_EntityDic)
            {
                m_EntityDic = new Dictionary<int, IEntity>();
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

    public Dictionary<MapArea, List<int>> BackItemMapDic
    {
        get
        {
            if(null == backItemMapDic)
            {
                backItemMapDic = new Dictionary<MapArea, List<int>>(fastCompare);
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

    public List<ItemMapInfo> GetCurItemMapInfo(int mapId)
    {
        if (DicMapInfo.ContainsKey(mapId))
        {
            return DicMapInfo[mapId].ItemMapInfo;
        }
        return null;
    }


}
