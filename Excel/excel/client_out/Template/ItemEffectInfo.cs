using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


[Serializable]
public class ItemEffectInfo: IInfo
{
    public int id; // 编号
    public ItemType effType; // 效果类型
    public int score; // 积分
    public int phys; // 体力

    public  void Load(BinaryReader reader)
    {
        id = reader.ReadInt32();
 
        effType = (ItemType)reader.ReadInt32();
 
        score = reader.ReadInt32();
 
        phys = reader.ReadInt32();
 
    }

    public  int GetKey()
    {
        return id;
    }

}
