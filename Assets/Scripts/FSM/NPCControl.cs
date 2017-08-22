using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NPCControl : MonoBehaviour
{
    private NetEntity m_netEntity;
    private FSMSystem fsm;
    private bool init = false;

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

    public FSMSystem Fsm
    {
        get
        {
            return fsm;
        }

        set
        {
            fsm = value;
        }
    }

    //该方法用来改变有限状态机的状体，有限状态机基于当前的状态和通过的过渡状态。如果当前的状态没有用来通过的过度状态，则会抛出错误
    public void SetTransition(Transition t,NetEntity entity)
    {
        if(null != Fsm)
        {
            Fsm.SwitchTransition(t, entity);
        }
    }

    public void Start()
    {
        if(init)
        {
            return;
        }
        init = true;
        MakeFSM();
        //SetTransition(Transition.Idle, NEntity);
        //NEntity.EndCurrentStateToOtherState(StateID.Idle);
    }

    public void Update()
    {
        if (null == Fsm)
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
        Fsm.CurrentState.OnUpdate(NEntity);
        Fsm.CurrentState.OnExcute(NEntity);
    }


    //NPC有两个状态分别是在路径中巡逻和追逐玩家
    //如果他在第一个状态并且SawPlayer 过度状态被出发了，它就转变到ChasePlayer状态
    //如果他在ChasePlayer状态并且LostPlayer状态被触发了，它就转变到FollowPath状态

    private void MakeFSM()//建造状态机
    {
        PlayerIdleState idle = new PlayerIdleState();
        idle.AddTransition(Transition.FreeWalk, StateID.Walk);
        idle.AddTransition(Transition.Dead, StateID.Dead);
        idle.AddTransition(Transition.Acct, StateID.Acct);
        idle.AddTransition(Transition.Skill, StateID.Skill);
        idle.AddTransition(Transition.CrashPlayer, StateID.CrashPlayer);

        PlayerWalkState walk = new PlayerWalkState();
        walk.AddTransition(Transition.FreeWalk, StateID.Walk);
        walk.AddTransition(Transition.Acct, StateID.Acct);
        walk.AddTransition(Transition.Skill, StateID.Skill);
        walk.AddTransition(Transition.Dead, StateID.Dead);
        walk.AddTransition(Transition.Idle, StateID.Idle);
        walk.AddTransition(Transition.CrashPlayer, StateID.CrashPlayer);

        PlayerDeadState dead = new PlayerDeadState();
        dead.AddTransition(Transition.Dead, StateID.Dead);
        dead.AddTransition(Transition.Idle, StateID.Idle);
        dead.AddTransition(Transition.FreeWalk, StateID.Walk);

        PlayerAcceState acce = new PlayerAcceState();
        acce.AddTransition(Transition.Acct, StateID.Acct);
        acce.AddTransition(Transition.Skill, StateID.Skill);
        acce.AddTransition(Transition.FreeWalk, StateID.Walk);
        acce.AddTransition(Transition.Dead, StateID.Dead);
        acce.AddTransition(Transition.Idle, StateID.Idle);
        acce.AddTransition(Transition.CrashPlayer, StateID.CrashPlayer);

        PlayerSkillState skill = new PlayerSkillState();
        skill.AddTransition(Transition.Skill, StateID.Skill);
        skill.AddTransition(Transition.Acct, StateID.Acct);
        skill.AddTransition(Transition.FreeWalk, StateID.Walk);
        skill.AddTransition(Transition.Dead, StateID.Dead);
        skill.AddTransition(Transition.Idle, StateID.Idle);
        skill.AddTransition(Transition.CrashPlayer, StateID.CrashPlayer);

        //PlayerSwitchState sw = new PlayerSwitchState();
        //sw.AddTransition(Transition.Skill, StateID.Skill);
        //sw.AddTransition(Transition.Acct, StateID.Acct);
        //sw.AddTransition(Transition.FreeWalk, StateID.Walk);
        //sw.AddTransition(Transition.Dead, StateID.Dead);
        //sw.AddTransition(Transition.Idle, StateID.Idle);
        //sw.AddTransition(Transition.CrashPlayer, StateID.CrashPlayer);

        PlayerCrashState crash = new PlayerCrashState();
        crash.AddTransition(Transition.Skill, StateID.Skill);
        crash.AddTransition(Transition.Acct, StateID.Acct);
        crash.AddTransition(Transition.FreeWalk, StateID.Walk);
        crash.AddTransition(Transition.Dead, StateID.Dead);
        crash.AddTransition(Transition.Idle, StateID.Idle);

        Fsm = new FSMSystem();
        Fsm.AddState(idle);
        Fsm.AddState(walk);
        Fsm.AddState(acce);
        Fsm.AddState(skill);
        Fsm.AddState(dead);
        //Fsm.AddState(sw);
        Fsm.AddState(crash);
    }
}

