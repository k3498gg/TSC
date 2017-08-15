using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    private float m_roddirTime = 8;
    private float m_judgeTime = 0;
    private float m_judgeInter = 5;

    private void RandomDir(NetEntity entity)
    {
        if (null == entity)
        {
            return;
        }

        m_totalTime += Time.deltaTime;
        if (m_totalTime < m_roddirTime)
        {
            return;
        }

        m_totalTime = 0;
        m_roddirTime = Random.Range(5, 10);

        if (TSCData.Instance.DropItemDic.Count > 0)
        {
            int idx = Random.Range(0, TSCData.Instance.DropItemDic.Count);
            Transform t = GameMgr.Instance.ItemRoot.GetChild(idx);
            if (null != t)
            {
                Vector3 dir = t.position - entity.CacheModel.position;
                entity.CacheModel.forward = dir;
            }
        }
        else
        {
            entity.CacheModel.forward = entity.CacheModel.forward * -1;
        }
    }

    private void SimpleMove(NetEntity entity)
    {
        if (null != entity)
        {
            entity.SimpleMove(1);
        }
    }

    private void JudgeEntity(NetEntity entity)
    {
        if (null == entity)
        {
            return;
        }


        if (m_judgeTime < m_judgeInter)
        {
            m_judgeTime += Time.deltaTime;
        }
        else
        {
            m_judgeTime = 0;
            //判断主角是否能KILL AI
            bool kill = Util.CanKillBody(GameMgr.Instance.MainEntity.Occupation, entity.Occupation);
            if (kill)
            {
                Debug.LogError(Util.GetEntityDistance(GameMgr.Instance.MainEntity.CacheModel, entity.CacheModel));
                if (Util.GetEntityDistance(GameMgr.Instance.MainEntity.CacheModel, entity.CacheModel) < 5)
                {
                    //能被主角杀死，逃离
                    Vector3 dir = GameMgr.Instance.MainEntity.CacheModel.position - entity.CacheModel.position;
                    entity.CacheModel.forward = dir;
                }
            }
            else
            {
                foreach (KeyValuePair<int, NetEntity> kv in TSCData.Instance.EntityDic)
                {
                    bool k = Util.CanKillBody(kv.Value.Occupation, entity.Occupation);
                    if (k)
                    {
                        if (Util.GetEntityDistance(kv.Value.CacheModel, entity.CacheModel) < 5)
                        {
                            //能被主角杀死，逃离
                            Vector3 dir = GameMgr.Instance.MainEntity.CacheModel.position - entity.CacheModel.position;
                            entity.CacheModel.forward = dir;
                            break;
                        }
                    }
                }
            }
        }
    }

    public override void OnUpdate(NetEntity entity)
    {
        if (null != entity)
        {
            SimpleMove(entity);
            RandomDir(entity);
            //JudgeEntity(entity);
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

    public override void OnEnter(NetEntity entity)
    {
        if (null != entity)
        {
            entity.Accelerate();
            entity.ArpgAnimatContorller.Walk = true;
        }
    }

    public override void OnExit(NetEntity entity)
    {
        if (null != entity)
        {
            entity.StopAccelerate();
            entity.ArpgAnimatContorller.Walk = false;
        }
    }

    public override void OnUpdate(NetEntity entity)
    {
        SimpleMove(entity);
    }

    public override void OnExcute(NetEntity entity)
    {

    }

}


//构造状态五：技能
public class PlayerSkillState : FSMState
{
    public PlayerSkillState()
    {
        stateID = StateID.Skill;
    }

    public override void OnEnter(NetEntity entity)
    {
        if (null != entity)
        {
            entity.Walkinstant();
        }
    }

    public override void OnExit(NetEntity entity)
    {
        if (null != entity)
        {
            entity.StopSkill(CollisionType.NONE);
            entity.ArpgAnimatContorller.Walk = false;
        }
    }

    public override void OnUpdate(NetEntity entity)
    {
        entity.SimpleMove(AppConst.AISkillSpeed);
    }

    public override void OnExcute(NetEntity entity)
    {

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
        if (null != entity)
        {
            entity.Relive();
        }
    }
}


public class PlayerIdleState : FSMState
{
    public PlayerIdleState()
    {
        stateID = StateID.Idle;
    }

    public override void OnEnter(NetEntity entity)
    {

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


