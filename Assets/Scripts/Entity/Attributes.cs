using System.Collections;
using System.Collections.Generic;

public class Attributes
{
    private int level;
    private float speed;     //移动速度
    private float baseSpeed;  //基础速度
    private float hp;
    private float maxHp;

    private int score;

    public int Level
    {
        get
        {
            return level - AppConst.randomValue;
        }

        set
        {
            level = value + AppConst.randomValue;
        }
    }

    public float Speed
    {
        get
        {
            return speed - AppConst.randomValue;
        }

        set
        {
            speed = value + AppConst.randomValue;
        }
    }

    public int Score
    {
        get
        {
            return score - AppConst.randomValue;
        }

        set
        {
            score = value + AppConst.randomValue;
        }
    }

    public float BaseSpeed
    {
        get
        {
            return baseSpeed - AppConst.randomValue;
        }

        set
        {
            baseSpeed = value + AppConst.randomValue;
        }
    }

    public float Hp
    {
        get
        {
            return hp;
        }

        set
        {
            hp = value;
        }
    }

    public float MaxHp
    {
        get
        {
            return maxHp;
        }

        set
        {
            maxHp = value;
        }
    }
}
