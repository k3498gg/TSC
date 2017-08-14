using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


// 常量表枚举类型
public enum ConstType
{
    NONE = 0,  //
    CONST_SPEED = 1,  //移动基本速度
    CONST_MAX_PHY = 2,  //体力最大值
    CONST_COST_PHY_SPEED = 3,  //体力每秒消耗值
    CONST_ATK_RADIO = 4,  //碰撞范围半径
    CONST_REBORN = 5,  //复活等待时间
    CONST_ENERGY_MAXCOUNT = 6,  //普通能量点最大数量
    CONST_SUGAR_RADIO = 7,  //糖果掉落半径
    CONST_ITEM_COUNT = 8,  //道具刷新数量上线
    CONST_MAX_HP = 9,  //最大血量
    CONST_ATK_ANGLE = 10,  //攻击角度
    CONST_FLEE_DIS = 11,  //逃跑距离
    CONST_CHASE_DIS = 12,  //追逐距离
    CONST_ACC_INTERVAL = 13,  //AI加速时间间隔
    CONST_SKILL_INTERVAL = 14,  //AI技能时间间隔
    CONST_FLEE_INTERVAL = 15,  //AI逃跑周期
    CONST_ACC_MINTIME = 16,  //AI加速持续最小时间
    CONST_ACC_MAXTIME = 17,  //AI加速持续最大时间
    CONST_KILL_INTERVAL = 18,  //AI技能周期
    CONST_SKILL_MINTIME = 19,  //AI技能持续最小时间
    CONST_SKILL_MAXTIME = 20,  //AI技能持续最大时间
    CONST_CHASE_TIME = 21,  //AI追逐持续时间（正常走路情况追踪目标）
    CONST_ACCT_SPEED = 22,  //加速速度比例
    CONST_INSTANT_SPEED = 23,  //冲刺速度比例
};

// 碰撞停止类型
public enum CollisionType
{
    NONE = 0,  //
    Collision_NET = 1,  //玩家
    Collision_ITEM = 2,  //道具
    Collision_OBSTACLE = 3,  //障碍物
    Collision_NOTHING = 4,  //自动停止
};

[Serializable]
public class ConstInfo: IInfo
{
    public int id; // 编号
    public string data; // 数据

    public  void Load(BinaryReader reader)
    {
        id = reader.ReadInt32();
 
        int dataLen = reader.ReadInt32();
        data = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(dataLen));
 
    }

    public  int GetKey()
    {
        return id;
    }

}
