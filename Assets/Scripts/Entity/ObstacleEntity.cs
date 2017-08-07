using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleEntity : IEntity
{
    private int index;
    private Transform cache;

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
}
