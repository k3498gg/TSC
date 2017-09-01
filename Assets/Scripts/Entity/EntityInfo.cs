using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInfo
{
    private int id;
    private int nameIdx;
    private int socre;
    private int killcount;
    private int beKillCount;

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

    public int NameIdx
    {
        get
        {
            return nameIdx;
        }

        set
        {
            nameIdx = value;
        }
    }

    public int Socre
    {
        get
        {
            return socre;
        }

        set
        {
            socre = value;
        }
    }

    public int Killcount
    {
        get
        {
            return killcount;
        }

        set
        {
            killcount = value;
        }
    }

    public int BeKillCount
    {
        get
        {
            return beKillCount;
        }

        set
        {
            beKillCount = value;
        }
    }
}
