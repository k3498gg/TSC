﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageMgr : Singleton<LanguageMgr>
{
    public string GetText(int key,params string[] param)
    {
        LanTxtInfo txtInfo = InfoMgr<LanTxtInfo>.Instance.GetInfo(key);
        if (null != txtInfo)
        {
            if(param.Length >0)
            {
               return string.Format(txtInfo.text, param);
            }else
            {
                return txtInfo.text;
            }
        }
        return key.ToString();
    }

    public string GetSurText(int key,params string[] param)
    {
        LanSurInfo txtInfo = InfoMgr<LanSurInfo>.Instance.GetInfo(key);
        if (null != txtInfo)
        {
            if (param.Length > 0)
            {
                return string.Format(txtInfo.text, param);
            }
            return txtInfo.text;
        }
        return key.ToString();
    }
}
