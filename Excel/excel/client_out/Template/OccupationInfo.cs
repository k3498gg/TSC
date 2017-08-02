using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


[Serializable]
public class OccupationInfo: IInfo
{
    public int id; // 编号
    public int lanID; // 描述语言ID
    public string icon; // 职业图标
    public int[] skillId; // 技能ID

    public  void Load(BinaryReader reader)
    {
        id = reader.ReadInt32();
 
        lanID = reader.ReadInt32();
 
        int iconLen = reader.ReadInt32();
        icon = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(iconLen));
 
        skillId = new int[2];
        for(int i = 0; i < 2; ++i)
        {
            skillId[i] = reader.ReadInt32();
        }
 
    }

    public  int GetKey()
    {
        return id;
    }

}
