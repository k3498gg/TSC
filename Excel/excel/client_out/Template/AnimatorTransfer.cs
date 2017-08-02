using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


[Serializable]
public class AnimatorTransfer: IInfo
{
    public int id; // 编号
    public TransferValue type; // 类型
    public TransferParam param; // 参数
    public int value; // 值

    public  void Load(BinaryReader reader)
    {
        id = reader.ReadInt32();
 
        type = (TransferValue)reader.ReadInt32();
 
        param = (TransferParam)reader.ReadInt32();
 
        value = reader.ReadInt32();
 
    }

    public  int GetKey()
    {
        return id;
    }

}
