using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMgr : Singleton<EntityMgr>
{
    private Dictionary<int, IEntity> m_EntityDic; //实体字典存储中心

    private Dictionary<int, DropItemInfo> m_DropItemDic;

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

}
