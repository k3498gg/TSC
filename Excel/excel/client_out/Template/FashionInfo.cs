using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


[Serializable]
public class FashionInfo: IInfo
{
    public int id; // 编号
    public int tigerId; // 老虎
    public int stickId; // 棒子
    public int chickId; // 鸡

    public  void Load(BinaryReader reader)
    {
        id = reader.ReadInt32();
 
        tigerId = reader.ReadInt32();
 
        stickId = reader.ReadInt32();
 
        chickId = reader.ReadInt32();
 
    }

    public  int GetKey()
    {
        return id;
    }

}
