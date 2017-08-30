using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


[Serializable]
public class NameInfo: IInfo
{
    public int id; // 编号
    public string name; // 名字

    public  void Load(BinaryReader reader)
    {
        id = reader.ReadInt32();
 
        int nameLen = reader.ReadInt32();
        name = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(nameLen));
 
    }

    public  int GetKey()
    {
        return id;
    }

}
