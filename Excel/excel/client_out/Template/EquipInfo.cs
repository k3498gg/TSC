using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


[Serializable]
public class EquipInfo: IInfo
{
    public int id; // 编号
    public int nameId; // 名字
    public string icon; // ICON
    public int modelId; // 模型ID
    public int isFree; // 是否免费
    public int priceType; // 金币类型
    public int price; // 价格
    public int equipType; // 装备类型
    public int fashionId; // 套装ID

    public  void Load(BinaryReader reader)
    {
        id = reader.ReadInt32();
 
        nameId = reader.ReadInt32();
 
        int iconLen = reader.ReadInt32();
        icon = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(iconLen));
 
        modelId = reader.ReadInt32();
 
        isFree = reader.ReadInt32();
 
        priceType = reader.ReadInt32();
 
        price = reader.ReadInt32();
 
        equipType = reader.ReadInt32();
 
        fashionId = reader.ReadInt32();
 
    }

    public  int GetKey()
    {
        return id;
    }

}
