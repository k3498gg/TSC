using System.Collections;
using System.Collections.Generic;

public class Attributes
{
    //private uint hp;
    //private uint mp;
    //private uint hpMax;
    //private uint mpMax;
    //private uint phyatk;
    //private uint magatk;
    //private uint phydef;
    //private uint magdef;
    //public uint Hp
    //{
    //    get
    //    {
    //        return hp;
    //    }

    //    set
    //    {
    //        hp = value;
    //    }
    //}

    //public uint Mp
    //{
    //    get
    //    {
    //        return mp;
    //    }

    //    set
    //    {
    //        mp = value;
    //    }
    //}

    //public uint HpMax
    //{
    //    get
    //    {
    //        return hpMax;
    //    }

    //    set
    //    {
    //        hpMax = value;
    //    }
    //}

    //public uint MpMax
    //{
    //    get
    //    {
    //        return mpMax;
    //    }

    //    set
    //    {
    //        mpMax = value;
    //    }
    //}

    //public uint Phyatk
    //{
    //    get
    //    {
    //        return phyatk;
    //    }

    //    set
    //    {
    //        phyatk = value;
    //    }
    //}

    //public uint Magatk
    //{
    //    get
    //    {
    //        return magatk;
    //    }

    //    set
    //    {
    //        magatk = value;
    //    }
    //}

    //public uint Phydef
    //{
    //    get
    //    {
    //        return phydef;
    //    }

    //    set
    //    {
    //        phydef = value;
    //    }
    //}

    //public uint Magdef
    //{
    //    get
    //    {
    //        return magdef;
    //    }

    //    set
    //    {
    //        magdef = value;
    //    }
    //}

    private uint level;
    private float speed;     //移动速度
    private float baseSpeed;

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
