using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class NPCControl : MonoBehaviour
{
    public GameObject player;
    private NetEntity m_netEntity;
    private FSMSystem fsm;
    private float m_Time;

    public NetEntity NEntity
    {
        get
        {
            if(null == m_netEntity)
            {
                m_netEntity = Util.AddComponent<NetEntity>(gameObject);
            }
            return m_netEntity;
        }

        set
        {
            m_netEntity = value;
        }
    }

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
        if (null == fsm)
        {
            return;
        }
        m_Time += Time.deltaTime;
        if (m_Time < 0.2f)
        {
            return;
        }

        m_Time = 0;
        fsm.CurrentState.OnUpdate(NEntity);
        fsm.CurrentState.OnExcute(NEntity);
    }


    //NPC有两个状态分别是在路径中巡逻和追逐玩家
    //如果他在第一个状态并且SawPlayer 过度状态被出发了，它就转变到ChasePlayer状态
    //如果他在ChasePlayer状态并且LostPlayer状态被触发了，它就转变到FollowPath状态

    private void MakeFSM()//建造状态机
    {
        PlayerWalkState walk = new PlayerWalkState();
        walk.AddTransition(Transition.FreeWalk, StateID.Walk);

        LostPlayer lostPlayer = new LostPlayer();
        lostPlayer.AddTransition(Transition.SawPlayer, StateID.ChasingPlayer);

        ChasePlayerState chase = new ChasePlayerState();
        chase.AddTransition(Transition.LostPlayer, StateID.LostPlayer);

        PlayerAcceState acce = new PlayerAcceState();
        acce.AddTransition(Transition.Acct, StateID.Acct);

        PlayerSkillState skill = new PlayerSkillState();
        skill.AddTransition(Transition.Skill, StateID.Skill);

        fsm = new FSMSystem();
        fsm.AddState(walk);
        fsm.AddState(lostPlayer);
        fsm.AddState(chase);
        fsm.AddState(acce);
        fsm.AddState(skill);
    }
}

