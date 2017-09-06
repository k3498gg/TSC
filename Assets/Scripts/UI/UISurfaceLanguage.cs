using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISurfaceLanguage : MonoBehaviour
{
    public int key;

    public int Key
    {
        get
        {
            return key;
        }

        set
        {
            if (key != value)
            {
                key = value;
            }
        }
    }

    void Start()
    {
        OnLocalize();
    }

    public void OnLocalize()
    {
        value = LanguageMgr.Instance.GetSurText(Key);
    }

    private string value
    {
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                Text txt = GetComponent<Text>();
                if (null != txt)
                {
                    txt.text = value;
                }
            }
        }
    }

    private string SurfaceText(int k)
    {
        string val = string.Empty;
        LanSurInfo lan = InfoMgr<LanSurInfo>.Instance.GetInfo(k);
        if (null != lan)
        {
            val = lan.text;
        }
        else
        {
            val = k.ToString();
        }
        return val;
    }

}
