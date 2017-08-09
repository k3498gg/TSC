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

    public override void OnEnter()
    {
        Debug.Log("FollowingPath BeforeEntering--------");
    }

    public override void OnExit()
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
    //private NetEntity m_NetEntity;

    public PlayerWalkState()
    {
        stateID = StateID.Walk;
    }

    public override void OnEnter()
    {
        //Debug.Log("PlayerWalkState BeforeEntering--------");
    }

    public override void OnExit()
    {
        //Debug.Log("PlayerWalkState BeforeLeaving---------");
        //if (null != m_NetEntity)
        //{
        //    m_NetEntity.ArpgAnimatContorller.Walk = false;
        //}
    }

    public override void OnUpdate(NetEntity entity)
    {
        if(null != entity)
        {
            entity.CharacterController.SimpleMove(entity.CacheModel.forward * Time.deltaTime * entity.Attribute.Speed);
        }
        //Debug.LogError("PlayerWalkState ... OnUpdate");
    }

    public override void OnExcute(NetEntity entity)
    {
        if (null != entity)
        {
            entity.ArpgAnimatContorller.Walk = true;
        }
    }


}


//构造状态三： 追逐玩家
public class ChasePlayerState : FSMState
{
    public ChasePlayerState()
    {
        stateID = StateID.ChasingPlayer;
    }

    public override void OnEnter()
    {
        Debug.Log("ChasingPlayer BeforeEntering--------");
    }

    public override void OnExit()
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

    public override void OnEnter()
    {
        Debug.Log("PlayerAcceState BeforeEntering--------");
    }

    public override void OnExit()
    {
        Debug.Log("PlayerAcceState BeforeLeaving---------");
    }

    public override void OnUpdate(NetEntity entity)
    {
        Debug.Log("PlayerAcceState OnUpdate---------");

    }

    public override void OnExcute(NetEntity entity)
    {
        Debug.Log("PlayerAcceState OnExcute---------");
    }

}


//构造状态五：技能
public class PlayerSkillState : FSMState
{
    public PlayerSkillState()
    {
        stateID = StateID.Skill;
    }

    public override void OnEnter()
    {
        Debug.Log("PlayerSkillState BeforeEntering--------");
    }

    public override void OnExit()
    {
        Debug.Log("PlayerSkillState BeforeLeaving---------");
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

    public override void OnEnter()
    {
        Debug.Log("PlayerDeadState BeforeEntering--------");
    }

    public override void OnExit()
    {
        Debug.Log("PlayerDeadState BeforeLeaving---------");
    }

    public override void OnUpdate(NetEntity entity)
    {
        Debug.Log("PlayerDeadState OnUpdate---------");

    }

    public override void OnExcute(NetEntity entity)
    {
        Debug.Log("PlayerDeadState OnExcute---------");
    }
}


