using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


[Serializable]
public class ObstacleInfo: IInfo
{
    public int id; // 编号
    public string model; // 模型

    public  void Load(BinaryReader reader)
    {
        id = reader.ReadInt32();
 
        int modelLen = reader.ReadInt32();
        model = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(modelLen));
 
    }

    public  int GetKey()
    {
        return id;
    }

}
