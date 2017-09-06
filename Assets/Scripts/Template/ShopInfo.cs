using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


// 装备类型
public enum ShopItemType
{
    NONE = 0,  //
    ShopItem_ALL = 1,  //所有装备
    ShopItem_TAO = 2,  //套装
    ShopItem_TIGER = 3,  //老虎
    ShopItem_STICK = 4,  //棒子
    ShopItem_CHICK = 5,  //鸡
};

// 装备类型
public enum MoneyType
{
    NONE = 0,  //
    MoneyType_COIN = 1,  //金币
    MoneyType_DIAMAND = 2,  //钻石
};

[Serializable]
public class ShopInfo: IInfo
{
    public int id; // 编号
    public int nameId; // 名字
    public string icon; // ICON
    public int modelId; // 模型ID
    public int priceType; // 金币类型
    public int price; // 价格
    public int equipType; // 装备类型

    public  void Load(BinaryReader reader)
    {
        id = reader.ReadInt32();
 
        nameId = reader.ReadInt32();
 
        int iconLen = reader.ReadInt32();
        icon = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(iconLen));
 
        modelId = reader.ReadInt32();
 
        priceType = reader.ReadInt32();
 
        price = reader.ReadInt32();
 
        equipType = reader.ReadInt32();
 
    }

    public  int GetKey()
    {
        return id;
    }

}
