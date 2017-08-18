using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleIdleState : RoleFSMState
{
    public RoleIdleState()
    {
        stateID = RoleStateID.Idle;
    }

    public override void OnEnter(Entity entity)
    {
        if (null != entity)
        {
            entity.ArpgAnimatContorller.Walk = false;
        }
    }

    public override void OnExit(Entity entity)
    {

    }

    public override void OnUpdate(Entity entity)
    {

    }

    public override void OnExcute(Entity entity)
    {

    }
}


public class RoleWalkState : RoleFSMState
{
    public RoleWalkState()
    {
        stateID = RoleStateID.Walk;
    }

    public override void OnEnter(Entity entity)
    {
        if (null != entity)
        {
            entity.ArpgAnimatContorller.Walk = true;
        }
    }

    public override void OnExit(Entity entity)
    {
        if (null != entity)
        {
            entity.ArpgAnimatContorller.Walk = false;
        }
    }

    public override void OnUpdate(Entity entity)
    {
        if (null != entity)
        {
            entity.SimpleMove();
        }
    }

    public override void OnExcute(Entity entity)
    {

    }
}

public class RoleAcctState : RoleFSMState
{

    public RoleAcctState()
    {
        stateID = RoleStateID.Acct;
    }

    public override void OnEnter(Entity entity)
    {
        if (null != entity)
        {
            entity.EnterAccelerate();
        }
    }

    public override void OnExit(Entity entity)
    {
        if (null != entity)
        {
            entity.StopAccelerate();
        }
    }

    public override void OnUpdate(Entity entity)
    {
        if (null != entity)
        {
            if (entity.Attribute.CurPhy > 0)
            {
                entity.Attribute.CurPhy = entity.Attribute.CurPhy - entity.Attribute.CostPhySpeed * Time.deltaTime;
            }
            else
            {
                entity.Attribute.CurPhy = 0;
                entity.RoleEntityControl.SetTransition(RoleTransition.FreeWalk, entity);
            }
        }
    }

    public override void OnExcute(Entity entity)
    {

    }
}


public class RoleSkillState : RoleFSMState
{
    public RoleSkillState()
    {
        stateID = RoleStateID.Skill;
    }

    public override void OnEnter(Entity entity)
    {
        if (null != entity)
        {
            entity.EnterWalkInstant();
        }
    }

    public override void OnExit(Entity entity)
    {
        if(null != entity)
        {
            entity.StopWalkInstant();
        }
    }

    public override void OnUpdate(Entity entity)
    {
        if(null != entity)
        {
            entity.SimpleMove();
        }
    }

    public override void OnExcute(Entity entity)
    {

    }
}


public class RoleSwitchState : RoleFSMState
{
    public RoleSwitchState()
    {
        stateID = RoleStateID.Switch;
    }

    private float enter_time = 0;
    private float m_total_time = 0.25f;
    public override void OnEnter(Entity entity)
    {
        if (null != entity)
        {
            entity.SwitchOtherOccp();
            enter_time = 0;
        }
    }

    public override void OnExit(Entity entity)
    {
        if(null != entity)
        {
            entity.ExitSwitchState();
        }
    }

    public override void OnUpdate(Entity entity)
    {
        if(null != entity)
        {
            enter_time += Time.deltaTime;
            if (enter_time < m_total_time)
            {
                return;
            }
            enter_time = 0;
            entity.EndCurrentStateToOtherState(RoleStateID.Idle);
        }
    }

    public override void OnExcute(Entity entity)
    {

    }
}


public class RoleCrashState : RoleFSMState
{
    public RoleCrashState()
    {
        stateID = RoleStateID.CrashPlayer;
    }

    private float enter_time = 0;
    public override void OnEnter(Entity entity)
    {
        if (null != entity)
        {
            enter_time = 0;
            entity.Attribute.Speed = entity.Attribute.BaseSpeed * AppConst.CrashSpeed;
        }
    }

    public override void OnExit(Entity entity)
    {
        if(null != entity)
        {
            entity.Attribute.Speed = entity.Attribute.BaseSpeed;
        }
    }

    public override void OnUpdate(Entity entity)
    {
        if (null != entity)
        {
            enter_time += Time.deltaTime;
            if (enter_time >= AppConst.CrashTime)
            {
                enter_time = 0;
                entity.EndCurrentStateToOtherState(RoleStateID.Idle);
            }
        }
    }

    public override void OnExcute(Entity entity)
    {

    }
}

public class RoleDeadState : RoleFSMState
{
    public RoleDeadState()
    {
        stateID = RoleStateID.Dead;
    }

    public override void OnEnter(Entity entity)
    {
        if (null != entity)
        {
            entity.Dead();
        }
    }

    public override void OnExit(Entity entity)
    {

    }

    public override void OnUpdate(Entity entity)
    {

    }

    public override void OnExcute(Entity entity)
    {

    }
}