using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _animator : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        Debug.LogError("OnStateEnter");
        Debug.LogError(animator.layerCount);
        Debug.LogError(animatorStateInfo.length +"  "+ layerIndex+ "  "+ animatorStateInfo.shortNameHash);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        Debug.LogError("OnStateExit");
        Debug.LogError(animator.layerCount);
        Debug.LogError(animatorStateInfo.length + "  " + layerIndex);
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        Debug.LogError("OnStateMove");
        Debug.LogError(animator.layerCount);
        Debug.LogError(animatorStateInfo.length + "  " + layerIndex);
        Debug.LogError(animatorStateInfo.normalizedTime);
    }

    //public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    //{
    //    Debug.LogError("OnStateUpdate");
    //    Debug.LogError(animator.layerCount);
    //    Debug.LogError(animatorStateInfo.length + "  " + layerIndex);
    //}

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        Debug.LogError("OnStateMachineEnter");
        Debug.LogError(animator.layerCount);
        Debug.LogError(stateMachinePathHash);
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        Debug.LogError("OnStateMachineExit");
        Debug.LogError(animator.layerCount);
        Debug.LogError(stateMachinePathHash);
    }
}
