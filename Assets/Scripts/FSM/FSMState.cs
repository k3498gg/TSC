using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 为过渡加入枚举标签
/// 不要修改第一个标签，NullTransition会在FSMSytem类中使用
/// </summary>
public enum Transition
{
    NullTransition = 0,  //用这个过度来代表你的系统中不存在的状态
    SawPlayer,//这里配合NPCControl添加两个NPC的过渡(搜寻目标，丢失目标)
    LostPlayer,
    FreeWalk,
    Acct,
    Skill,
    Dead,
    Idle
}

/// <summary>
/// 为状态加入枚举标签
/// 不要修改第一个标签，NullStateID会在FSMSytem中使用 
/// </summary>
public enum StateID
{
    NullStateID = 0,//使用这个ID来代表你系统中不存在的状态ID    
    ChasingPlayer,//这里配合NPCControl添加两个状态
    LostPlayer,
    Walk,
    Acct,
    Skill,
    Dead,
    Idle
}

/// <summary>
/// 这个类代表状态在有限状态机系统中
/// 每个状态都有一个由一对搭档（过渡-状态）组成的字典来表示当前状态下如果一个过渡被触发状态机会进入那个状态
/// Reason方法被用来决定那个过渡会被触发
/// Act方法来表现NPC出在当前状态的行为
/// </summary>
public abstract class FSMState
{
    private static EnumComparer<Transition> fastComparer = new EnumComparer<Transition>();
    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>(fastComparer);
    protected StateID stateID;
    public StateID ID { get { return stateID; } }

    public void AddTransition(Transition trans, StateID id)
    {
        //验证每个参数是否合法
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSMState ERROR: NullStateID is not allowed for a real ID");
            return;
        }

        //要知道这是一个确定的有限状态机（每个状态后对应一种状态，而不能产生分支）
        //检查当前的过渡是否已经在字典中了
        if (map.ContainsKey(trans))
        {
            Debug.LogError("FSMState ERROR: State " + stateID.ToString() + " already has transition " + trans.ToString() +
                           "Impossible to assign to another state");
            return;
        }

        map.Add(trans, id);
    }

    /// <summary>
    /// 这个方法用来在状态字典中删除transition-state对儿
    /// 如果过渡并不存在于状态字典中，那么将会打印出一个错误
    /// </summary>
    public void DeleteTransition(Transition trans)
    {
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed");
            return;
        }

        //再删除之前确认该键值对是否存在于状态字典中（键值对集合）
        if (map.ContainsKey(trans))
        {
            map.Remove(trans);
            return;
        }
        Debug.LogError("FSMState ERROR: Transition " + trans.ToString() + " passed to " + stateID.ToString() +
                       " was not on the state's transition list");
    }

    /// <summary>
    /// 该方法在该状态接收到一个过渡时返回状态机需要成为的新状态
    /// </summary>
    public StateID GetOutputState(Transition trans)
    {
        if (map.ContainsKey(trans))
        {
            return map[trans];
        }
        return StateID.NullStateID;
    }

    /// <summary>
    /// 这个方法用来设立进入状态前的条件
    /// 在状态机分配它到当前状态之前他会被自动调用
    /// </summary>
    public virtual void OnEnter(NetEntity entity) { }

    /// <summary>
    /// 这个方法用来让一切都是必要的，例如在有限状态机变化的另一个时重置变量。
    /// 在状态机切换到新的状态之前它会被自动调用。
    /// </summary>
    public virtual void OnExit(NetEntity entity) { }

    /// <summary>
    /// 表现-->该方法用来控制NPC在游戏世界中的行为
    /// NPC的任何动作，移动或者交流都需要防止在这儿
    /// NPC是被该类约束下对象的一个引用
    /// </summary>
    public abstract void OnExcute(NetEntity entity);

    /// <summary>
    /// 动机(状态更新)-->这个方法用来决定当前状态是否需要过渡到列表中的其他状态
    /// NPC是被该类约束下对象的一个引用
    /// </summary>
    public abstract void OnUpdate(NetEntity entity);

    ////根据需求定义自己的状态机行为方法
    //public abstract void OnExcute();

    ////状态更新函数
    //public abstract void OnUpdate();

}


/// <summary>
/// 该类便是有限状态机类
/// 它持有者NPC的状态集合并且有添加，删除状态的方法，以及改变当前正在执行的状态
/// </summary>
public class FSMSystem
{
    private List<FSMState> states;
    //通过预装一个过渡的唯一方式来盖面状态机的状态
    //不要直接改变当前的状态
    private StateID currentStateID;
    private FSMState currentState;

    public StateID CurrentStateID
    {
        private set { currentStateID = value; }
        get { return currentStateID; }
    }
    public FSMState CurrentState
    {
        private set { currentState = value; }
        get { return currentState; }
    }

    public FSMSystem()
    {
        if(null == states)
        {
            states = new List<FSMState>();
        }else
        {
            states.Clear();
        }
    }
    /// <summary>
    /// 这个方法为有限状态机置入新的状态
    /// 或者在该状态已经存在于列表中时打印错误信息
    /// 第一个添加的状态也是最初的状态!
    /// </summary>
    public void AddState(FSMState s)
    {
        //在添加前检测空引用
        if (s == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
        }

        //被装在的第一个状态也是初始状态
        //这个状态便是状态机开始时的状态
        if (states.Count == 0)
        {
            states.Add(s);
            CurrentState = s;
            CurrentStateID = s.ID;
            return;
        }
        //如果该状态未被添加过，则加入集合
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].ID == s.ID)
            {
                Debug.LogError("FSM ERROR: Impossible to add state " + s.ID.ToString() +
                               " because state has already been added");
                return;
            }
        }
        states.Add(s);
    }

    /// <summary>
    /// 该方法删除一个已存在以状态几个中的状态
    /// 在它不存在时打印错误信息
    /// </summary>
    public void DeleteState(StateID id)
    {
        //在删除前检查其是否为空状态
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }

        //遍历集合如果存在该状态则删除它
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].ID == id)
            {
                states.Remove(states[i]);
                return;
            }
        }

        Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() +
                       ". It was not on the list of states");
    }

    /// <summary>
    /// 状态改变
    /// </summary>
    public void SwitchTransition(Transition trans,NetEntity entity)
    {
        //在改变当前状态前检测NullTransition
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSM ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        //在改变当前状态前检测当前状态是否可作为过渡的参数
        StateID id = CurrentState.GetOutputState(trans);
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSM ERROR: State " + CurrentStateID.ToString() + " does not have a target state " +
                           " for transition " + trans.ToString());
            return;
        }

     
        //更新当前的状态个和状态编号
        CurrentStateID = id;
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].ID == CurrentStateID)
            {
                //离开旧状态执行方法
                CurrentState.OnExit(entity);
                CurrentState = states[i];
                //进入新的状态
                CurrentState.OnEnter(entity);
                break;
            }
        }
    }
}