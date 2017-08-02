using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


[Serializable]
public class ParticleInfo: IInfo
{
    public int id; // 编号
    public string data; // 模型
    public int delay; // 延迟播放时间

    public  void Load(BinaryReader reader)
    {
        id = reader.ReadInt32();
 
        int dataLen = reader.ReadInt32();
        data = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(dataLen));
 
        delay = reader.ReadInt32();
 
    }

    public  int GetKey()
    {
        return id;
    }

}
