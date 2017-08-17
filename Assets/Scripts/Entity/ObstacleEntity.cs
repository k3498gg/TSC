using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleEntity : IEntity
{
    private int index;
    private Transform cache;
    private float height;
    private float width;
    private ObstType obs_type;

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

    public Transform Cache
    {
        get
        {
            if(null == cache)
            {
                cache = transform;
            }
            return cache;
        }

        set
        {
            cache = value;
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

    public ObstType Obs_type
    {
        get
        {
            return obs_type;
        }

        set
        {
            obs_type = value;
        }
    }
}
