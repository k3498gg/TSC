using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


// 道具各种效果
public enum ItemType
{
    NONE = 0,  //
    ITEM_MARK = 1,  //问号
    ITEM_MAGNET = 2,  //吸铁石
    ITEM_TRANSFERGATE = 3,  //传送门
    ITEM_SPEED = 4,  //变速
    ITEM_PROTECT = 5,  //保护罩
    ITEM_ENERGY = 6,  //积分
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
    public int effectId; // 道具效果

    public  void Load(BinaryReader reader)
    {
        id = reader.ReadInt32();
 
        name = reader.ReadInt32();
 
        describe = reader.ReadInt32();
 
        int modelLen = reader.ReadInt32();
        model = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(modelLen));
 
        scale = reader.ReadInt32();
 
        offset = reader.ReadInt32();
 
        effectId = reader.ReadInt32();
 
    }

    public  int GetKey()
    {
        return id;
    }

}
