using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


// 道具各种效果
public enum ItemType
{
    NONE = 0,  //
    ITEM_ACCT = 1,  //加速
    ITEM_ENERGY = 2,  //能量
    ITEM_SLOW = 3,  //减速
    ITEM_INSTANT = 4,  //瞬移
};

[Serializable]
public class ItemInfo: IInfo
{
    public int id; // 编号
    public int name; // 名字ID
    public int describe; // 描述ID
    public string model; // 道具模型
    public int scale; // 缩放比例
    public int offset; // 名字偏移

    public  void Load(BinaryReader reader)
    {
        id = reader.ReadInt32();
 
        name = reader.ReadInt32();
 
        describe = reader.ReadInt32();
 
        int modelLen = reader.ReadInt32();
        model = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(modelLen));
 
        scale = reader.ReadInt32();
 
        offset = reader.ReadInt32();
 
    }

    public  int GetKey()
    {
        return id;
    }

}
