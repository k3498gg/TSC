using System.Collections;
using System.Collections.Generic;

public class Attributes
{
    private uint level;
    private float speed;     //移动速度
    private float baseSpeed;  //基础速度

    private uint score;

    public uint Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }

    public uint Score
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
        }
    }

    public float BaseSpeed
    {
        get
        {
            return baseSpeed;
        }

        set
        {
            baseSpeed = value;
        }
    }
}
