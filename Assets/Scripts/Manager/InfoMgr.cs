using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public interface IInfo
{
    void Load(BinaryReader br);
    int GetKey();
}


//public class InfoMgr<T> where T : IInfo , new()
public class InfoMgr<T> : Singleton<InfoMgr<T>> where T : IInfo, new()
{
    private Dictionary<int, T> _dict;

    public Dictionary<int, T> Dict
    {
        get
        {
            if (null == _dict)
            {
                _dict = new Dictionary<int, T>();
            }
            return _dict;
        }

        set
        {
            _dict = value;
        }
    }

    public void Init(string file)
    {
        Clear();
        FileStream fs = null;
        BinaryReader br = null;
        try
        {
            fs = File.OpenRead(file);
            br = new BinaryReader(fs, System.Text.UTF8Encoding.UTF8);
            if (null != br)
            {
                int count = br.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    T t = new T();
                    t.Load(br);
                    int k = t.GetKey();
                    if (Dict.ContainsKey(k))
                    {
                        Debug.Log("key <color=red> " + k.ToString() + " </color> duplicate!");
                        return;
                    }
                    Dict[k] = t;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Exception:" + e.Message);
        }
        finally
        {
            if (null != fs)
            {
                fs.Close();
            }

            if (null != br)
            {
                br.Close();
            }
        }
    }

    public T GetInfo(int key)
    {
        if (Dict.ContainsKey(key))
            return Dict[key];

        return default(T);
    }

    void Clear()
    {
        if (null != _dict)
        {
            _dict.Clear();
        }
    }
}
