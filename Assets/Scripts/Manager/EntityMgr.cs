using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMgr : Singleton<EntityMgr>
{
    private Dictionary<int, IEntity> m_EntityDic; //实体字典存储中心

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

}
