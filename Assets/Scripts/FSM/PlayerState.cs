using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : FSMState
{
    public PlayerIdleState()
    {
        stateID = StateID.Idle;
    }

    private float enter_time = 0;
    private float m_total_time = 0.5f;
    public override void OnEnter(NetEntity entity)
    {
        if (null != entity)
        {
            entity.EnterIdle();
        }
    }

    public override void OnExit(NetEntity entity)
    {
        if (null != entity)
        {
            entity.ExitIdle();
        }
    }

    public override void OnUpdate(NetEntity entity)
    {
        if (null != entity)
        {
            enter_time += Time.deltaTime;
            if (enter_time < m_total_time)
            {
                return;
            }

            enter_time = 0;
            entity.EndCurrentStateToOtherState(StateID.Walk);
        }
    }

    public override void OnExcute(NetEntity entity)
    {

    }
}

public class PlayerWalkState : FSMState
{
    public PlayerWalkState()
    {
        stateID = StateID.Walk;
    }

    public override void OnEnter(NetEntity entity)
    {
        if (null != entity)
        {
            entity.ArpgAnimatContorller.Walk = true;
        }
    }

    public override void OnExit(NetEntity entity)
    {
        if (null != entity)
        {
            entity.ArpgAnimatContorller.Walk = false;
        }
    }

    private float m_totalTime = 0;
    private float m_detectTime = 0;

    private void RandomDir(NetEntity entity)
    {
        if (null == entity)
        {
            return;
        }

        m_totalTime += Time.deltaTime;
        m_detectTime += Time.deltaTime;

        if (m_detectTime < AppConst.AIDetectInterval)
        {
            return;
        }

        if (!entity.IsOutOfTime())
        {
            return;
        }

        if (!entity.IsOutOfRange())
        {
            return;
        }

        if (m_totalTime < AppConst.AIRandomDirInterval)
        {
            return;
        }

        m_totalTime = 0;

        if (TSCData.Instance.DropItemDic.Count > 0)
        {
            bool force = false;
            foreach (KeyValuePair<int, DropItemInfo> kv in TSCData.Instance.DropItemDic)
            {
                if (!kv.Value.IsLock)
                {
                    if (Util.GetEntityDistance(entity.CacheModel, kv.Value.Cache) < AppConst.AIRandomItemRadio)
                    {
                        entity.UpdateDir(kv.Value.Cache.position - entity.CacheModel.position);
                        force = true;
                        break;
                    }
                }
            }

            if (!force)
            {
                int idx = Random.Range(0, TSCData.Instance.DropItemDic.Count);
                Transform t = GameMgr.Instance.ItemRoot.GetChild(idx);
                if (null != t)
                {
                    entity.UpdateDir(t.position - entity.CacheModel.position);
                }
                else
                {
                    entity.UpdateDir(entity.CacheModel.forward * Random.Range(-1, 1));
                }
            }
        }
        else
        {
            entity.UpdateDir(entity.CacheModel.forward * Random.Range(-1, 1));
        }
    }

    private void SimpleMove(NetEntity entity)
    {
        if (null != entity)
        {
            entity.SimpleMove(1);
            entity.ChaseTarget();
        }
    }

    public override void OnUpdate(NetEntity entity)
    {
        if (null != entity)
        {
            if (entity.IsAlive)
            {
                SimpleMove(entity);
                RandomDir(entity);
            }
        }
    }

    public override void OnExcute(NetEntity entity)
    {

    }
}



//构造状态四：加速状态
public class PlayerAcceState : FSMState
{
    public PlayerAcceState()
    {
        stateID = StateID.Acct;
    }

    private void SimpleMove(NetEntity entity)
    {
        if (null != entity)
        {
            entity.SimpleMove(AppConst.AIAcceSpeed);
        }
    }

    private float m_enter_time = 0;
    private float m_end_time = 0;
    public override void OnEnter(NetEntity entity)
    {
        if (null != entity)
        {
            entity.Accelerate();
            m_enter_time = 0;
            m_end_time = Random.Range(AppConst.AcctMinTime, AppConst.AcctMaxTime);
        }
    }

    public override void OnExit(NetEntity entity)
    {
        if (null != entity)
        {
            entity.StopAccelerate();
            m_enter_time = 0;
        }
    }

    public override void OnUpdate(NetEntity entity)
    {
        SimpleMove(entity);
    }

    public override void OnExcute(NetEntity entity)
    {
        m_enter_time += Time.deltaTime;
        if (m_enter_time > m_end_time)
        {
            m_enter_time = 0;
            entity.EndCurrentStateToOtherState(StateID.Walk);
        }
    }
}


//构造状态五：技能
public class PlayerSkillState : FSMState
{
    public PlayerSkillState()
    {
        stateID = StateID.Skill;
    }

    private float m_enter_time = 0;

    public override void OnEnter(NetEntity entity)
    {
        if (null != entity)
        {
            m_enter_time = 0;
            entity.SkillEvent();
        }
    }

    public override void OnExit(NetEntity entity)
    {
        if (null != entity)
        {
            m_enter_time = 0;
            entity.StopSkill();
        }
    }

    public override void OnUpdate(NetEntity entity)
    {
        entity.SimpleMove(AppConst.AISkillSpeed);
    }

    public override void OnExcute(NetEntity entity)
    {
        if (null != entity)
        {
            m_enter_time += Time.deltaTime;
            if (m_enter_time > entity.GetSkillTime())
            {
                entity.SwitchOccp();
                //entity.EndCurrentStateToOtherState(StateID.Walk);
            }
        }
    }
}


public class PlayerDeadState : FSMState
{
    public PlayerDeadState()
    {
        stateID = StateID.Dead;
    }

    public override void OnEnter(NetEntity entity)
    {
        if (null != entity)
        {
            entity.Dead();
        }
    }

    public override void OnExit(NetEntity entity)
    {

    }

    public override void OnUpdate(NetEntity entity)
    {

    }

    public override void OnExcute(NetEntity entity)
    {

    }
}




//public class PlayerSwitchState : FSMState
//{
//    public PlayerSwitchState()
//    {
//        stateID = StateID.Switch;
//    }


//    public override void OnEnter(NetEntity entity)
//    {
//        if (null != entity)
//        {
//            entity.SwitchOccp();
//        }
//    }

//    public override void OnExit(NetEntity entity)
//    {

//    }

//    public override void OnUpdate(NetEntity entity)
//    {

//    }

//    public override void OnExcute(NetEntity entity)
//    {

//    }
//}


public class PlayerCrashState : FSMState
{
    public PlayerCrashState()
    {
        stateID = StateID.CrashPlayer;
    }

    private float enter_time = 0;
    public override void OnEnter(NetEntity entity)
    {
        if (null != entity)
        {
            enter_time = 0;
            entity.Attribute.Speed = entity.Attribute.BaseSpeed * AppConst.CrashSpeed;
        }
    }

    public override void OnExit(NetEntity entity)
    {
        if (null != entity)
        {
            enter_time = 0;
            entity.Attribute.Speed = entity.Attribute.BaseSpeed;
        }
    }

    void Crash(NetEntity entity)
    {
        if (null != entity)
        {
            entity.CharaController.SimpleMove(entity.HitDir.normalized * Time.deltaTime * entity.Attribute.Speed);
        }
    }

    public override void OnUpdate(NetEntity entity)
    {
        if (null != entity)
        {
            enter_time += Time.deltaTime;
            if (enter_time >= AppConst.CrashTime)
            {
                entity.EndCurrentStateToOtherState(StateID.Walk);
                return;
            }
            Crash(entity);
        }
    }

    public override void OnExcute(NetEntity entity)
    {

    }
}