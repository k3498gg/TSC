using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


[Serializable]
public class AnimatorTransInfo: IInfo
{
    public int id; // 编号
    public string idle; // idle
    public string walk; // walk
    public string free; // free
    public string atk1; // atk1
    public string atk2; // atk2
    public string atk3; // atk3
    public string skill1; // skill1
    public string skill2; // skill2
    public string skill3; // skill3
    public string die; // die
    public string live; // live

    public  void Load(BinaryReader reader)
    {
        id = reader.ReadInt32();
 
        int idleLen = reader.ReadInt32();
        idle = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(idleLen));
 
        int walkLen = reader.ReadInt32();
        walk = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(walkLen));
 
        int freeLen = reader.ReadInt32();
        free = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(freeLen));
 
        int atk1Len = reader.ReadInt32();
        atk1 = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(atk1Len));
 
        int atk2Len = reader.ReadInt32();
        atk2 = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(atk2Len));
 
        int atk3Len = reader.ReadInt32();
        atk3 = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(atk3Len));
 
        int skill1Len = reader.ReadInt32();
        skill1 = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(skill1Len));
 
        int skill2Len = reader.ReadInt32();
        skill2 = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(skill2Len));
 
        int skill3Len = reader.ReadInt32();
        skill3 = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(skill3Len));
 
        int dieLen = reader.ReadInt32();
        die = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(dieLen));
 
        int liveLen = reader.ReadInt32();
        live = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(liveLen));
 
    }

    public  int GetKey()
    {
        return id;
    }

}
