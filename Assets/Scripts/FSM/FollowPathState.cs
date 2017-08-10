using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//构造状态一： 按路径寻路
public class LostPlayer : FSMState
{
    //构造函数装填自己
    public LostPlayer()
    {
        stateID = StateID.LostPlayer;//别忘设置自己的StateID
    }

    public override void OnEnter(NetEntity entity)
    {
        Debug.Log("FollowingPath BeforeEntering--------");
    }

    public override void OnExit(NetEntity entity)
    {
        Debug.Log("FollowingPath BeforeLeaving---------");
    }

    //重写动机方法
    public override void OnUpdate(NetEntity entity)
    {
        //RaycastHit hit;
        //if (Physics.Raycast(npc.transform.position, npc.transform.forward, out hit, 15F))
        //{
        //    if (hit.transform.gameObject.tag == "Player")
        //        npc.GetComponent<NPCControl>().SetTransition(Transition.SawPlayer);
        //}
    }

    //重写表现方法
    public override void OnExcute(NetEntity entity)
    {
        //Vector3 vel = npc.GetComponent<Rigidbody>().velocity;
        //Vector3 moveDir = waypoints[currentWayPoint].position - npc.transform.position;

        //if (moveDir.magnitude < 1)
        //{
        //    currentWayPoint++;
        //    if (currentWayPoint >= waypoints.Length)
        //    {
        //        currentWayPoint = 0;
        //    }
        //}
        //else
        //{
        //    vel = moveDir.normalized * 10;

        //    npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
        //                                              Quaternion.LookRotation(moveDir),
        //                                              5 * Time.deltaTime);
        //    npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);
        //}

        //npc.GetComponent<Rigidbody>().velocity = vel;
    }



}

//构造状态二： 玩家自由行走
public class PlayerWalkState : FSMState
{
    public PlayerWalkState()
    {
        stateID = StateID.Walk;
    }

    public override void OnEnter(NetEntity entity)
    {
        Debug.Log("PlayerWalkState BeforeEntering--------");
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
        if(null == entity)
        {
            return;
        }

        m_totalTime += Time.deltaTime;
        if (m_totalTime < m_roddirTime)
        {
            return;
        }
        //else
        //{
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
        //}
    }

    private void SimpleMove(NetEntity entity)
    {
        if(null != entity)
        {
            //entity.CacheModel.Translate(entity.CacheModel.forward * Time.deltaTime * entity.Attribute.Speed);
            entity.CharaController.SimpleMove(entity.CacheModel.forward * Time.deltaTime * entity.Attribute.Speed);
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
            if(kill)
            {
                Debug.LogError(Util.GetEntityDistance(GameMgr.Instance.MainEntity.CacheModel, entity.CacheModel));
                if(Util.GetEntityDistance(GameMgr.Instance.MainEntity.CacheModel,entity.CacheModel) <　5)
                {
                    //能被主角杀死，逃离
                    Vector3 dir = GameMgr.Instance.MainEntity.CacheModel.position - entity.CacheModel.position;
                    entity.CacheModel.forward = dir;
                }
            }else
            {
                foreach(KeyValuePair<int,NetEntity> kv in TSCData.Instance.EntityDic)
                {
                    bool k = Util.CanKillBody(kv.Value.Occupation, entity.Occupation);
                    if(k)
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
        if(null != entity)
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


//构造状态三： 追逐玩家
public class ChasePlayerState : FSMState
{
    public ChasePlayerState()
    {
        stateID = StateID.ChasingPlayer;
    }

    public override void OnEnter(NetEntity entity)
    {
        Debug.Log("ChasingPlayer BeforeEntering--------");
    }

    public override void OnExit(NetEntity entity)
    {
        Debug.Log("ChasingPlayer BeforeLeaving---------");
    }

    public override void OnUpdate(NetEntity entity)
    {
        //if (Vector3.Distance(npc.transform.position, player.transform.position) >= 3)
        //    npc.GetComponent<NPCControl>().SetTransition(Transition.LostPlayer);
    }

    public override void OnExcute(NetEntity entity)
    {
        //Vector3 vel = npc.GetComponent<Rigidbody>().velocity;
        //Vector3 moveDir = player.transform.position - npc.transform.position;

        //npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
        //                                          Quaternion.LookRotation(moveDir),
        //                                          5 * Time.deltaTime);
        //npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);

        //vel = moveDir.normalized * 10;
        //npc.GetComponent<Rigidbody>().velocity = vel;
        //npc.transform.position = player.transform.position - new Vector3(0, 0, 0.5f);
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
            //entity.CacheModel.Translate(entity.CacheModel.forward * Time.deltaTime * entity.Attribute.Speed);
            entity.CharaController.SimpleMove(entity.CacheModel.forward * Time.deltaTime * entity.Attribute.Speed);
        }
    }

    public override void OnEnter(NetEntity entity)
    {
        if(null != entity)
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
        
        if(null != entity)
        {
            entity.Walkinstant();
        }
    }

    public override void OnExit(NetEntity entity)
    {
      
    }

    public override void OnUpdate(NetEntity entity)
    {
        Debug.Log("PlayerSkillState OnUpdate---------");

    }

    public override void OnExcute(NetEntity entity)
    {
        Debug.Log("PlayerSkillState OnExcute---------");
    }

}


//构造状态六：死亡状态
public class PlayerDeadState : FSMState
{
    public PlayerDeadState()
    {
        stateID = StateID.Dead;
    }

    public override void OnEnter(NetEntity entity)
    {
        if(null != entity)
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


//构造状态七：复活状态
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


