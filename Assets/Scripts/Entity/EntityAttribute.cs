using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttribute : Attributes
{
    private uint atkdis;     //攻击范围
    private uint rebornTime; //复活时间
    private float curPhy;
    private uint maxPhy; //最大体力
    private uint costPhySpeed; //每秒消耗的体力
    private int[] skills;
    private uint money;

    public uint Atkdis
    {
        get
        {
            return atkdis - AppConst.randomValue;
        }

        set
        {
            atkdis = value + AppConst.randomValue;
        }
    }
    public uint RebornTime
    {
        get
        {
            return rebornTime - AppConst.randomValue;
        }

        set
        {
            rebornTime = value + AppConst.randomValue;
        }
    }
    public int[] Skills
    {
        get
        {
            return skills;
        }

        set
        {
            skills = value;
        }
    }
    public uint MaxPhy
    {
        get
        {
            return maxPhy - AppConst.randomValue;
        }

        set
        {
            maxPhy = value + AppConst.randomValue;
        }
    }
    public uint CostPhySpeed
    {
        get
        {
            return costPhySpeed - AppConst.randomValue;
        }

        set
        {
            costPhySpeed = value + AppConst.randomValue;
        }
    }
    public float CurPhy
    {
        get
        {
            return curPhy - AppConst.randomValue;
        }

        set
        {
            curPhy = value + AppConst.randomValue;
        }
    }
    public uint Money
    {
        get
        {
            return money - AppConst.randomValue;
        }

        set
        {
            money = value + AppConst.randomValue;
        }
    }
}
