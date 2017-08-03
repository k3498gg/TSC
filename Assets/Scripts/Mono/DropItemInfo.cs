using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DropAI
{
    NONE = 0,  //消失不再刷新
    INSTANCE = 1,//消失立即刷新
    LATER =2, //消失等待一定时间刷新
}


public class DropItemInfo : MonoBehaviour
{
    private int itemId;
    private int infoId;
    private MapArea area;
    private DropAI dropAI;

    public int ItemId
    {
        get
        {
            return itemId;
        }

        set
        {
            itemId = value;
        }
    }

    public int InfoId
    {
        get
        {
            return infoId;
        }

        set
        {
            infoId = value;
        }
    }

    public MapArea Area
    {
        get
        {
            return area;
        }

        set
        {
            area = value;
        }
    }

    public DropAI DropAI
    {
        get
        {
            return dropAI;
        }

        set
        {
            dropAI = value;
        }
    }
}
