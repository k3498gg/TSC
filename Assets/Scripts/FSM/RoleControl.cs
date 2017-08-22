using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RoleControl : MonoBehaviour
{
    private RoleFSMSystem fsm;
    private Entity m_Entity;
    private bool init = false;

    public RoleFSMSystem Fsm
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

    public Entity MEntity
    {
        get
        {
            if(null == m_Entity)
            {
                m_Entity = GameMgr.Instance.MainEntity;
            }
            return m_Entity;
        }
    }


    //该方法用来改变有限状态机的状体，有限状态机基于当前的状态和通过的过渡状态。如果当前的状态没有用来通过的过度状态，则会抛出错误
    public void SetTransition(RoleTransition t,Entity entity)
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
        SetTransition(RoleTransition.Idle, MEntity);
    }

    public void Update()
    {
        if (null == Fsm)
        {
            return;
        }
        if(null== MEntity)
        {
            return;
        }

        if(!GameMgr.Instance.IsEnterGame)
        {
            return;
        }

        if(!MEntity.IsAlive)
        {
            return;
        }
        Fsm.CurrentState.OnUpdate(MEntity);
        Fsm.CurrentState.OnExcute(MEntity);
    }


    //NPC有两个状态分别是在路径中巡逻和追逐玩家
    //如果他在第一个状态并且SawPlayer 过度状态被出发了，它就转变到ChasePlayer状态
    //如果他在ChasePlayer状态并且LostPlayer状态被触发了，它就转变到FollowPath状态

    private void MakeFSM()//建造状态机
    {
        RoleIdleState idle = new RoleIdleState();
        idle.AddTransition(RoleTransition.Idle, RoleStateID.Idle);
        idle.AddTransition(RoleTransition.FreeWalk, RoleStateID.Walk);
        idle.AddTransition(RoleTransition.Dead, RoleStateID.Dead);
        idle.AddTransition(RoleTransition.Acct, RoleStateID.Acct);
        idle.AddTransition(RoleTransition.Skill, RoleStateID.Skill);
        idle.AddTransition(RoleTransition.CrashPlayer, RoleStateID.CrashPlayer);

        RoleWalkState walk = new RoleWalkState();
        walk.AddTransition(RoleTransition.FreeWalk, RoleStateID.Walk);
        walk.AddTransition(RoleTransition.Acct, RoleStateID.Acct);
        walk.AddTransition(RoleTransition.Skill, RoleStateID.Skill);
        walk.AddTransition(RoleTransition.Dead, RoleStateID.Dead);
        walk.AddTransition(RoleTransition.Idle, RoleStateID.Idle);
        walk.AddTransition(RoleTransition.CrashPlayer, RoleStateID.CrashPlayer);

        RoleDeadState dead = new RoleDeadState();
        dead.AddTransition(RoleTransition.Dead, RoleStateID.Dead);
        dead.AddTransition(RoleTransition.Idle, RoleStateID.Idle);

        RoleAcctState acce = new RoleAcctState();
        acce.AddTransition(RoleTransition.Acct, RoleStateID.Acct);
        acce.AddTransition(RoleTransition.Skill, RoleStateID.Skill);
        acce.AddTransition(RoleTransition.FreeWalk, RoleStateID.Walk);
        acce.AddTransition(RoleTransition.Dead, RoleStateID.Dead);
        acce.AddTransition(RoleTransition.Idle, RoleStateID.Idle);
        acce.AddTransition(RoleTransition.CrashPlayer, RoleStateID.CrashPlayer);

        RoleSkillState skill = new RoleSkillState();
        skill.AddTransition(RoleTransition.Skill, RoleStateID.Skill);
        skill.AddTransition(RoleTransition.Acct, RoleStateID.Acct);
        skill.AddTransition(RoleTransition.FreeWalk, RoleStateID.Walk);
        skill.AddTransition(RoleTransition.Dead, RoleStateID.Dead);
        skill.AddTransition(RoleTransition.Idle, RoleStateID.Idle);
        skill.AddTransition(RoleTransition.CrashPlayer, RoleStateID.CrashPlayer);
        skill.AddTransition(RoleTransition.Switch, RoleStateID.Switch);

        RoleSwitchState sw = new RoleSwitchState();
        sw.AddTransition(RoleTransition.Skill, RoleStateID.Skill);
        sw.AddTransition(RoleTransition.Acct, RoleStateID.Acct);
        sw.AddTransition(RoleTransition.FreeWalk, RoleStateID.Walk);
        sw.AddTransition(RoleTransition.Idle, RoleStateID.Idle);
        sw.AddTransition(RoleTransition.Dead, RoleStateID.Dead);
        sw.AddTransition(RoleTransition.CrashPlayer, RoleStateID.CrashPlayer);

        RoleCrashState crash = new RoleCrashState();
        crash.AddTransition(RoleTransition.Skill, RoleStateID.Skill);
        crash.AddTransition(RoleTransition.Acct, RoleStateID.Acct);
        crash.AddTransition(RoleTransition.FreeWalk, RoleStateID.Walk);
        crash.AddTransition(RoleTransition.Dead, RoleStateID.Dead);
        crash.AddTransition(RoleTransition.Idle, RoleStateID.Idle);

        Fsm = new RoleFSMSystem();
        Fsm.AddState(idle);
        Fsm.AddState(walk);
        Fsm.AddState(acce);
        Fsm.AddState(skill);
        Fsm.AddState(dead);
        Fsm.AddState(sw);
        Fsm.AddState(crash);
    }
}

