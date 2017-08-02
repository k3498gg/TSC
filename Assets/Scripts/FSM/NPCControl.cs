using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NPCControl : MonoBehaviour
{
    public GameObject player;
    public Transform[] path;
    private FSMSystem fsm;

    //该方法用来改变有限状态机的状体，有限状态机基于当前的状态和通过的过渡状态。如果当前的状态没有用来通过的过度状态，则会抛出错误
    public void SetTransition(Transition t)
    {
        fsm.SwitchTransition(t);
    }

    public void Start()
    {
        MakeFSM();
    }

    public void Update()
    {
        fsm.CurrentState.OnUpdate(player, gameObject);
        fsm.CurrentState.OnExcute(player, gameObject);
    }


    //NPC有两个状态分别是在路径中巡逻和追逐玩家
    //如果他在第一个状态并且SawPlayer 过度状态被出发了，它就转变到ChasePlayer状态
    //如果他在ChasePlayer状态并且LostPlayer状态被触发了，它就转变到FollowPath状态

    private void MakeFSM()//建造状态机
    {
        FollowPathState follow = new FollowPathState(path);
        follow.AddTransition(Transition.SawPlayer, StateID.ChasingPlayer);

        ChasePlayerState chase = new ChasePlayerState();
        chase.AddTransition(Transition.LostPlayer, StateID.FollowingPath);

        fsm = new FSMSystem();
        fsm.AddState(follow);//添加状态到状态机，第一个添加的状态将作为初始状态
        fsm.AddState(chase);
    }
}

