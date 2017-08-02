using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


[Serializable]
public class SkillInfo: IInfo
{
    public int id; // 编号
    public int lanID; // 描述语言ID
    public int effectID; // 效果表ID
    public int particleID; // 特效ID

    public  void Load(BinaryReader reader)
    {
        id = reader.ReadInt32();
 
        lanID = reader.ReadInt32();
 
        effectID = reader.ReadInt32();
 
        particleID = reader.ReadInt32();
 
    }

    public  int GetKey()
    {
        return id;
    }

}
