using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//构造状态一： 按路径寻路
public class FollowPathState : FSMState
{
    private int currentWayPoint;
    private Transform[] waypoints;

    //构造函数装填自己
    public FollowPathState(Transform[] wp)
    {
        waypoints = wp;
        currentWayPoint = 0;
        stateID = StateID.FollowingPath;//别忘设置自己的StateID
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
    public override void OnUpdate(GameObject player, GameObject npc)
    {
        RaycastHit hit;
        if (Physics.Raycast(npc.transform.position, npc.transform.forward, out hit, 15F))
        {
            if (hit.transform.gameObject.tag == "Player")
                npc.GetComponent<NPCControl>().SetTransition(Transition.SawPlayer);
        }
    }

    //重写表现方法
    public override void OnExcute(GameObject player, GameObject npc)
    {
        Vector3 vel = npc.GetComponent<Rigidbody>().velocity;
        Vector3 moveDir = waypoints[currentWayPoint].position - npc.transform.position;

        if (moveDir.magnitude < 1)
        {
            currentWayPoint++;
            if (currentWayPoint >= waypoints.Length)
            {
                currentWayPoint = 0;
            }
        }
        else
        {
            vel = moveDir.normalized * 10;

            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
                                                      Quaternion.LookRotation(moveDir),
                                                      5 * Time.deltaTime);
            npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);
        }

        npc.GetComponent<Rigidbody>().velocity = vel;
    }


    public override void OnExcute()
    {
    }

    public override void OnUpdate()
    {
    }
}

//构造状态二： 追逐玩家
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

    public override void OnUpdate(GameObject player, GameObject npc)
    {
        if (Vector3.Distance(npc.transform.position, player.transform.position) >= 3)
            npc.GetComponent<NPCControl>().SetTransition(Transition.LostPlayer);
    }

    public override void OnExcute(GameObject player, GameObject npc)
    {
        //Vector3 vel = npc.GetComponent<Rigidbody>().velocity;
        //Vector3 moveDir = player.transform.position - npc.transform.position;

        //npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
        //                                          Quaternion.LookRotation(moveDir),
        //                                          5 * Time.deltaTime);
        //npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);

        //vel = moveDir.normalized * 10;
        //npc.GetComponent<Rigidbody>().velocity = vel;
        npc.transform.position = player.transform.position - new Vector3(0, 0, 0.5f);
    }

    public override void OnExcute()
    {
    }

    public override void OnUpdate()
    {
    }
}

