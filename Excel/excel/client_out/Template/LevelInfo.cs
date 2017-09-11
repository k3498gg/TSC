using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


[Serializable]
public class LevelInfo: IInfo
{
    public int id; // 编号
    public int hitscale; // 碰撞范围比例
    public int scale; // 缩放比例
    public int score; // 积分
    public string icon; // 状态
    public int offset; // 偏移

    public  void Load(BinaryReader reader)
    {
        id = reader.ReadInt32();
 
        hitscale = reader.ReadInt32();
 
        scale = reader.ReadInt32();
 
        score = reader.ReadInt32();
 
        int iconLen = reader.ReadInt32();
        icon = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(iconLen));
 
        offset = reader.ReadInt32();
 
    }

    public  int GetKey()
    {
        return id;
    }

}
