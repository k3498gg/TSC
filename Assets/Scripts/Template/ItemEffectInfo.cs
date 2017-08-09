using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


[Serializable]
public class ItemEffectInfo: IInfo
{
    public int id; // 编号
    public ItemType effType; // 效果类型
    public int param1; // 参数1
    public int param2; // 参数2

    public  void Load(BinaryReader reader)
    {
        id = reader.ReadInt32();
 
        effType = (ItemType)reader.ReadInt32();
 
        param1 = reader.ReadInt32();
 
        param2 = reader.ReadInt32();
 
    }

    public  int GetKey()
    {
        return id;
    }

}
