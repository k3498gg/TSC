using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHudManager : UnitySingleton<UIHudManager>
{
    private Transform m_cache;

    public Transform Cache
    {
        get
        {
            if (null == m_cache)
            {
                m_cache = transform;
            }
            return m_cache;
        }

        set
        {
            m_cache = value;
        }
    }

    public GameObject SpawnerHUD()
    {
        return  ResourcesMgr.Instance.Spawner(AppConst.HUD_Prefab, ResourceType.RESOURCE_UI, Cache);
    }

    //public UIHUDName SpawnerHUD(string entityName,Transform follow)
    //{
    //    GameObject go = ResourcesMgr.Instance.Spawner(AppConst.HUD_Prefab, ResourceType.RESOURCE_UI, Cache);
    //    UIHUDName hudName = Util.AddComponent<UIHUDName>(go);
    //    if (null != hudName)
    //    {
    //        hudName.Init();
    //        hudName.SetTarget(follow);
    //        hudName.SetName(entityName);
    //    }
    //    return hudName;
    //}

    public void Despawner(Transform inst)
    {
        if(null != inst)
        {
            ResourcesMgr.Instance.Despawner(ResourceType.RESOURCE_UI, inst);
        }
    }

}
