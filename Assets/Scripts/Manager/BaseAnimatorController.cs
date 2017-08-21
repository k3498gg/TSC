using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnimatorController : MonoBehaviour
{
    #region 状态机参数
    public static readonly int freeKey = Animator.StringToHash("FREE");
    public static readonly int walkKey = Animator.StringToHash("WALK");
    public static readonly int attackKey = Animator.StringToHash("ATK");
    public static readonly int skillKey = Animator.StringToHash("SKILL");
    public static readonly int dieKey = Animator.StringToHash("DIE");



    #endregion
    private bool free;
    private bool walk;
    private bool die;
    private int attack;
    private int skill;
    public Animator animator;
    //public GameObject m_baseTemp;

    public void  Init(Transform cache)
    {
        if (null != cache)
        {
            if (!cache.gameObject.activeSelf)
            {
                cache.gameObject.SetActive(true);
            }
            animator = cache.GetComponent<Animator>();
        }
        Reset();
    }

    public void Reset()
    {
        Walk = false;
        Die = false;
        Attack = 0;
        Skill = 0;
    }

    public bool Free
    {
        get
        {
            return free;
        }

        set
        {
            if(free != value)
            {
                free = value;
                if(null != animator)
                {
                    animator.SetBool(freeKey,value);
                }
            }
        }
    }

    public bool Walk
    {
        get
        {
            return walk;
        }

        set
        {
            if(walk != value)
            {
                walk = value;
                if(null != animator)
                {
                    animator.SetBool(walkKey, value);
                }
            }
        }
    }

    public bool Die
    {
        get
        {
            return die;
        }

        set
        {
            if(die != value)
            {
                die = value;
                animator.SetBool(dieKey, value);
            }
        }
    }

    public int Attack
    {
        get
        {
            return attack;
        }

        set
        {
            if (attack != value)
            {
                attack = value;
                animator.SetInteger(attackKey, value);
            }
        }
    }

    public int Skill
    {
        get
        {
            return skill;
        }

        set
        {
            if (skill != value)
            {
                skill = value;
                animator.SetInteger(skillKey, value);
            }
        }
    }
}
