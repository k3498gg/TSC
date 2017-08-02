using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemMapType
{
    NONE = 0,
    POINT = 1,
    ITEM = 2
}

[Serializable]
public class ItemMapInfo
{
    private ItemMapType iMapType;
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

    public ItemMapInfo() { }

    public ItemMapInfo(ItemMapType mType, float x, float y, float w, float h)
    {
        this.IMapType = mType;
        this.posX = x;
        this.PosY = y;
        this.Width = w;
        this.Height = h;
    }

    public override string ToString()
    {
        return this.IMapType + " " + this.posX + "  " + this.PosY + "  " + this.Width + "  " + this.Height;
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

