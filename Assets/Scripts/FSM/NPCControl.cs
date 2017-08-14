using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NPCControl : MonoBehaviour
{
    private NetEntity m_netEntity;
    private FSMSystem fsm;

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
    public void SetTransition(Transition t,NetEntity entity)
    {
        if(null != fsm)
        {
            fsm.SwitchTransition(t, entity);
        }
    }

    public void Start()
    {
        MakeFSM();
        SetTransition(Transition.FreeWalk, NEntity);
    }

    public void Update()
    {
        if (null == fsm)
        {
            return;
        }
        if(null== NEntity)
        {
            return;
        }

        if(!NEntity.IsAlive)
        {
            return;
        }
        fsm.CurrentState.OnUpdate(NEntity);
        fsm.CurrentState.OnExcute(NEntity);
    }


    //NPC有两个状态分别是在路径中巡逻和追逐玩家
    //如果他在第一个状态并且SawPlayer 过度状态被出发了，它就转变到ChasePlayer状态
    //如果他在ChasePlayer状态并且LostPlayer状态被触发了，它就转变到FollowPath状态

    private void MakeFSM()//建造状态机
    {
        PlayerIdleState idle = new PlayerIdleState();
        idle.AddTransition(Transition.FreeWalk, StateID.Walk);
        idle.AddTransition(Transition.Dead, StateID.Dead);

        PlayerWalkState walk = new PlayerWalkState();
        walk.AddTransition(Transition.FreeWalk, StateID.Walk);
        walk.AddTransition(Transition.Acct, StateID.Acct);
        walk.AddTransition(Transition.Skill, StateID.Skill);
        walk.AddTransition(Transition.Dead, StateID.Dead);

        PlayerDeadState dead = new PlayerDeadState();
        dead.AddTransition(Transition.Dead, StateID.Dead);
        dead.AddTransition(Transition.Idle, StateID.Idle);

        //LostPlayer lostPlayer = new LostPlayer();
        //lostPlayer.AddTransition(Transition.SawPlayer, StateID.ChasingPlayer);
        //lostPlayer.AddTransition(Transition.Dead, StateID.Dead);

        //ChasePlayerState chase = new ChasePlayerState();
        //chase.AddTransition(Transition.LostPlayer, StateID.LostPlayer);
        //chase.AddTransition(Transition.Dead, StateID.Dead);

        PlayerAcceState acce = new PlayerAcceState();
        acce.AddTransition(Transition.Acct, StateID.Acct);
        acce.AddTransition(Transition.Skill, StateID.Skill);
        acce.AddTransition(Transition.FreeWalk, StateID.Walk);
        acce.AddTransition(Transition.Dead, StateID.Dead);

        PlayerSkillState skill = new PlayerSkillState();
        skill.AddTransition(Transition.Skill, StateID.Skill);
        skill.AddTransition(Transition.Acct, StateID.Acct);
        skill.AddTransition(Transition.FreeWalk, StateID.Walk);
        skill.AddTransition(Transition.Dead, StateID.Dead);

        fsm = new FSMSystem();
        fsm.AddState(walk);
        fsm.AddState(acce);
        fsm.AddState(skill);
        fsm.AddState(dead);
    }
}

