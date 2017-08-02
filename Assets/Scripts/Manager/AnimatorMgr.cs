using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorMgr : Singleton<AnimatorMgr>
{
    //private AnimatorStateInfo m_currentStateInfo;
    ////private AnimatorStateInfo m_nextStateInfo;
    ////private bool isInTransaction = false;

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
    //    if (null == GameMgr.Instance.AnimController.animator)
    //    {
    //        return;
    //    }

    //    CurrentStateInfo = GameMgr.Instance.AnimController.animator.GetCurrentAnimatorStateInfo(layer);
    //    //m_nextStateInfo = GameMgr.Instance.AnimController.animator.GetNextAnimatorStateInfo(layer);
    //    //isInTransaction = GameMgr.Instance.AnimController.animator.IsInTransition(layer);


    //    //if (GameMgr.Instance.AnimController.Attack > 0)
    //    //{
    //    //    if (CurrentStateInfo.fullPathHash == atk1_NameHash && CurrentStateInfo.normalizedTime > 0.95f)
    //    //    {
    //    //        Debug.LogError(CurrentStateInfo.fullPathHash + "  " + idle_NameHash + " " + atk1_NameHash);
    //    //        GameMgr.Instance.AnimController.Attack = 0;
    //    //    }
    //    //}

    //    if (GameMgr.Instance.AnimController.Skill > 0)
    //    {
    //        if (CurrentStateInfo.fullPathHash == skill1_NameHash && CurrentStateInfo.normalizedTime > 0.95f)
    //        {
    //            GameMgr.Instance.AnimController.Skill = 0;
    //        }
    //    }
    //}

}
