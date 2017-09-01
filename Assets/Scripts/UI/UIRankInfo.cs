using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRankInfo : MonoBehaviour
{
    private Image m_icon;
    private Text m_name;
    private Text m_range;
    private Text m_killCount;
    private Text m_score;

    private bool init = false;
    private Transform m_cache;

    public Transform Cache
    {
        get
        {
            if (null == m_cache)
            {
                m_cache = transform;
            }
            return m_cache;
        }

        set
        {
            m_cache = value;
        }
    }

    void Init()
    {
        if (init)
            return;
        init = true;
        m_icon = Cache.Find("Image").GetComponent<Image>();
        m_name = Cache.Find("Text").GetComponent<Text>();
        m_range = Cache.Find("Range").GetComponent<Text>();
        m_killCount = Cache.Find("kill").GetComponent<Text>();
        m_score = Cache.Find("score").GetComponent<Text>();
    }

    public void SetRangeInfo(EntityInfo info)
    {
        Init();
        NameInfo _info = InfoMgr<NameInfo>.Instance.GetInfo(info.NameIdx);
        string name = string.Empty;
        if (null != _info)
        {
            name = _info.name;
        }
        SetRangeInfo(info.Socre, name,info.Killcount, null);
    }

    public void SetRangeInfo(int rank, string name,int killCount, Sprite icon)
    {
        Init();
        if (null != icon)
        {
            m_icon.sprite = icon;
        }

        m_name.text = name.ToString();
        m_score.text = rank.ToString();
        m_killCount.text = killCount.ToString();
    }


    public void SetRangeInfo(int range,int rank,int killCount,string name)
    {
        Init();
        m_range.text = (range+1).ToString();
        m_name.text = name.ToString();
        m_score.text = rank.ToString();
        m_killCount.text = killCount.ToString();
    }
}
