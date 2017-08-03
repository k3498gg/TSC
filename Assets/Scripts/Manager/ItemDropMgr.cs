using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropMgr : Singleton<Timer>
{
    private List<DropItemInfo> dropItemInfos;

    public List<DropItemInfo> DropItemInfos
    {
        get
        {
            if(null == dropItemInfos)
            {
                dropItemInfos = new List<DropItemInfo>();
            }
            return dropItemInfos;
        }

        set
        {
            dropItemInfos = value;
        }
    }

    public void Update()
    {

    }
}
