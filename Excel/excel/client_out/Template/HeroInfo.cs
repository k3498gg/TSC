using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


// 资源类型UI,角色,特效,动画,道具
public enum ResourceType
{
    NONE = 0,  //
    RESOURCE_UI = 1,  //UI资源
    RESOURCE_ENTITY = 2,  //角色模型
    RESOURCE_PARTICLE = 3,  //粒子特效
    RESOURCE_ANIMATOR = 4,  //动作文件
    RESOURCE_ITEM = 5,  //道具模型
    RESOURCE_OBSTACLE = 6,  //障碍物模型
    RESOURCE_NET = 7,  //网络角色
};

[Serializable]
public class HeroInfo: IInfo
{
    public int id; // 编号
    public int lanID; // 描述语言ID
    public string model; // 模型
    public string icon; // 图标
    public int occupationId; // 职业ID

    public  void Load(BinaryReader reader)
    {
        id = reader.ReadInt32();
 
        lanID = reader.ReadInt32();
 
        int modelLen = reader.ReadInt32();
        model = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(modelLen));
 
        int iconLen = reader.ReadInt32();
        icon = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(iconLen));
 
        occupationId = reader.ReadInt32();
 
    }

    public  int GetKey()
    {
        return id;
    }

}
