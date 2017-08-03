using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


[Serializable]
public class ItemEffectInfo: IInfo
{
    public int id; // 编号
    public string data; // 参数

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
