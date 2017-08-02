using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


[Serializable]
public class LanTxtInfo: IInfo
{
    public int id; // 编号
    public string text; // 文本

    public  void Load(BinaryReader reader)
    {
        id = reader.ReadInt32();
 
        int textLen = reader.ReadInt32();
        text = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(textLen));
 
    }

    public  int GetKey()
    {
        return id;
    }

}
