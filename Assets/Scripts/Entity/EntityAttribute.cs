using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttribute : Attributes
{
    private float atkdis;     //攻击范围,实际范围
    private float basedis;     //攻击范围基础值
    private int rebornTime; //复活时间
    private float curPhy;
    private int maxPhy; //最大体力
    private int costPhySpeed; //每秒消耗的体力
    private int[] skills;
    private int money;

    public float Atkdis
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
    public int RebornTime
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
    public int MaxPhy
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
    public int CostPhySpeed
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
    public int Money
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

    public float Basedis
    {
        get
        {
            return basedis - AppConst.randomValue;
        }

        set
        {
            basedis = value + AppConst.randomValue;
        }
    }
}
