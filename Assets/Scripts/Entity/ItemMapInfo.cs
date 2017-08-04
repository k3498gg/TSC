using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemMapType
{
    NONE = 0,
    POINT = 1, //特殊道具
    ITEM = 2   //积分道具
}

[Serializable]
public class ItemMapInfo
{
    private int index;
    private ItemMapType iMapType;
    private MapArea area;
    private float width;
    private float height;
    private float posX;
    private float posY;


    public float Width
    {
        get
        {
            return width;
        }

        set
        {
            width = value;
        }
    }

    public float Height
    {
        get
        {
            return height;
        }

        set
        {
            height = value;
        }
    }

    public ItemMapType IMapType
    {
        get
        {
            return iMapType;
        }

        set
        {
            iMapType = value;
        }
    }

    public float PosX
    {
        get
        {
            return posX;
        }

        set
        {
            posX = value;
        }
    }

    public float PosY
    {
        get
        {
            return posY;
        }

        set
        {
            posY = value;
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

    public int Index
    {
        get
        {
            return index;
        }

        set
        {
            index = value;
        }
    }

    public ItemMapInfo() { }

    public ItemMapInfo(ItemMapType mType, MapArea a, float x, float y, float w, float h)
    {
        this.IMapType = mType;
        this.Area = a;
        this.posX = x;
        this.PosY = y;
        this.Width = w;
        this.Height = h;
    }

    public override string ToString()
    {
        return this.Index + " " + this.IMapType + "  " + this.Area + " " + this.posX + "  " + this.PosY + "  " + this.Width + "  " + this.Height;
    }
}

[Serializable]
public class MapInfo
{
    private int id; //地图ID
    private List<ItemMapInfo> itemMapInfo; //地图对应的道具布局表

    public int Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public List<ItemMapInfo> ItemMapInfo
    {
        get
        {
            return itemMapInfo;
        }

        set
        {
            itemMapInfo = value;
        }
    }
}


public class ItemDespawnerInfo
{
    private int itemId;
    private int infoId;

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

    public ItemDespawnerInfo()
    { }


    public ItemDespawnerInfo(int _itemId,int _infoId)
    {
        this.ItemId = _itemId;
        this.InfoId = _infoId;
    }
}



