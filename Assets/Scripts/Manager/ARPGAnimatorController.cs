using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARPGAnimatorController : BaseAnimatorController
{
    private bool isCanMove = true;
    public bool IsCanMove
    {
        get
        {
            return !(Attack > 0 || Die) && isCanMove;
        }
        set
        {
            isCanMove = value;
        }
    }

    //private AnimatorStateInfo m_currentStateInfo;
    //private AnimatorStateInfo m_nextStateInfo;
    //private bool isInTransaction = false;

    //private static int layer = 0;
    //public static int idle_NameHash = Animator.StringToHash("Base Layer.AnimaState_IDLE");
    //public static int atk1_NameHash = Animator.StringToHash("Base Layer.AnimaState_ATK1");
    //public static int skill1_NameHash = Animator.StringToHash("Base Layer.AnimaState_SKILL1");

    //public AnimatorStateInfo CurrentStateInfo
    //{
    //    get
    //    {
    //        return m_currentStateInfo;
    //    }

    //    set
    //    {
    //        m_currentStateInfo = value;
    //    }
    //}

    //public void Update()
    //{
    //    if (null == animator)
    //    {
    //        return;
    //    }


    //    if (Skill > 0)
    //    {
    //        CurrentStateInfo = animator.GetCurrentAnimatorStateInfo(layer);

    //        if (CurrentStateInfo.fullPathHash == skill1_NameHash && CurrentStateInfo.normalizedTime > 0.95f)
    //        {
    //            Skill = 0;
    //        }
    //    }
    //}
}
