using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


// 技能效果类型
public enum EffectType
{
    NONE = 0,  //
    ACCELERATE = 1,  //加速
    WALKINSTANT = 2,  //冲锋
};

[Serializable]
public class EffectInfo: IInfo
{
    public int id; // 编号
    public EffectType type; // 效果类型
    public int cd; // 效果CD时间
    public int iscanmove; // 是否可移动
    public int keeptime; // 持续时间
    public int param; // 效果参数

    public  void Load(BinaryReader reader)
    {
        id = reader.ReadInt32();
 
        type = (EffectType)reader.ReadInt32();
 
        cd = reader.ReadInt32();
 
        iscanmove = reader.ReadInt32();
 
        keeptime = reader.ReadInt32();
 
        param = reader.ReadInt32();
 
    }

    public  int GetKey()
    {
        return id;
    }

}
