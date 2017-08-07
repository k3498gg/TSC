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
public class Obstacle
{
    private int id;
    private ObstType obsType = ObstType.ObsType_NONE;
    private float width;
    private float height;
    private float posX;
    private float posY;
    private float rotate;

    private float hh;
    private float poshh;

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

    public ObstType ObsType
    {
        get
        {
            return obsType;
        }

        set
        {
            obsType = value;
        }
    }

    public float Rotate
    {
        get
        {
            return rotate;
        }

        set
        {
            rotate = value;
        }
    }

    public float Hh
    {
        get
        {
            return hh;
        }

        set
        {
            hh = value;
        }
    }

    public float Poshh
    {
        get
        {
            return poshh;
        }

        set
        {
            poshh = value;
        }
    }

    public override string ToString()
    {
        return "id:" + Id + " ObsType:"+ ObsType + " pox:" + posX + "　poy: " + PosY + " height:" + height + " width:" + width;
    }
}


[Serializable]
public class MapInfo
{
    private int id; //地图ID
    private List<ItemMapInfo> itemMapInfo; //地图对应的道具布局表
    private List<Obstacle> obstacles;

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
            if(null == itemMapInfo)
            {
                itemMapInfo = new List<global::ItemMapInfo>();
            }
            return itemMapInfo;
        }

        set
        {
            itemMapInfo = value;
        }
    }

    public List<Obstacle> Obstacles
    {
        get
        {
            if(null == obstacles)
            {
                obstacles = new List<Obstacle>();
            }
            return obstacles;
        }

        set
        {
            obstacles = value;
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



