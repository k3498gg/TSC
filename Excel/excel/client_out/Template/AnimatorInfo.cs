using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


// 动作状态
public enum AnimatState
{
    AnimaState_IDLE = 1,  //IDLE
    AnimaState_WALK = 2,  //WALK
    AnimaState_FREE = 3,  //FREE(表演)
    AnimaState_ATK1 = 4,  //ATK1
    AnimaState_ATK2 = 5,  //ATK2
    AnimaState_ATK3 = 6,  //ATK3
    AnimaState_SKILL1 = 7,  //SKILL1
    AnimaState_SKILL2 = 8,  //SKILL2
    AnimaState_SKILL3 = 9,  //SKILL3
    AnimaState_DIE = 10,  //DIE
    AnimaState_LIVE = 11,  //LIVE(复活)
    AnimaState_MAX = 12,  //最大值
};

// 触发条件
public enum TransferValue
{
    TransferValue_INT = 1,  //int
    TransferValue_BOOL = 2,  //bool
    TransferValue_EXIT = 3,  //exit
    TransferValue_TRIGGER = 4,  //trigger
};

// 切换参数
public enum TransferParam
{
    WALK = 1,  //walk
    FREE = 2,  //free
    ATK = 3,  //atk
    SKILL = 4,  //skill
    DIE = 5,  //die
    EXIT = 6,  //exit
};

[Serializable]
public class AnimatorInfo: IInfo
{
    public int id; // 编号
    public string name; // 名字
    public int idle; // idle
    public int walk; // walk
    public int free; // free
    public int atk1; // atk1
    public int atk2; // atk2
    public int atk3; // atk3
    public int skill1; // skill1
    public int skill2; // skill2
    public int skill3; // skill3
    public int die; // die
    public int live; // live

    public  void Load(BinaryReader reader)
    {
        id = reader.ReadInt32();
 
        int nameLen = reader.ReadInt32();
        name = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(nameLen));
 
        idle = reader.ReadInt32();
 
        walk = reader.ReadInt32();
 
        free = reader.ReadInt32();
 
        atk1 = reader.ReadInt32();
 
        atk2 = reader.ReadInt32();
 
        atk3 = reader.ReadInt32();
 
        skill1 = reader.ReadInt32();
 
        skill2 = reader.ReadInt32();
 
        skill3 = reader.ReadInt32();
 
        die = reader.ReadInt32();
 
        live = reader.ReadInt32();
 
    }

    public  int GetKey()
    {
        return id;
    }

}
