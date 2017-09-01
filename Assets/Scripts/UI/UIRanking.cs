using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRanking : MonoBehaviour
{
    private int count = 8;
    private bool init = false;
    private Transform m_cache;
    private UIRankInfo m_rankInfo;
    public List<UIRankInfo> rankList = new List<UIRankInfo>();
   

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
    }

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        if (init)
            return;
        init = true;
        rankList.Clear();
        m_rankInfo = Cache.Find("SelfRank").GetComponent<UIRankInfo>();
        for (int i = 0; i < count; i++)
        {
            rankList.Add(Util.AddComponent<UIRankInfo>(Cache.Find("Viewport/Grid/Ranking_" + (i + 1).ToString()).gameObject));
        }
    }


    public void SetRankInfo(List<EntityInfo> list)
    {
        int idx = -1;
        for (int i = 0; i < rankList.Count; i++)
        {
            if (i < list.Count)
            {
                if (list[i].Socre > GameMgr.Instance.MainEntity.Attribute.Score)
                {
                    rankList[i].SetRangeInfo(list[i]);
                }
                else
                {
                    if (idx == -1)
                    {
                        idx = i;
                        rankList[i].SetRangeInfo(GameMgr.Instance.MainEntity.Attribute.Score,TSCData.Instance.Role.Name, GameMgr.Instance.MainEntity.KillCount, null);
                        m_rankInfo.SetRangeInfo(idx, GameMgr.Instance.MainEntity.Attribute.Score, GameMgr.Instance.MainEntity.KillCount, TSCData.Instance.Role.Name);
                    }
                    else
                    {
                        rankList[i].SetRangeInfo(list[i - 1]);
                    }
                }

                Util.SetActive(rankList[i].Cache.gameObject, true);
            }
            else
            {
                Util.SetActive(rankList[i].Cache.gameObject, false);
            }
        }

        if(idx == -1)
        {
            for(int i = count; i< list.Count;i++ )
            {
                if (list[i].Socre > GameMgr.Instance.MainEntity.Attribute.Score)
                {
                    continue;
                }

                idx = i;
                break;
            }
            if(idx == -1)
            {
                idx = list.Count;
            }

            m_rankInfo.SetRangeInfo(idx, GameMgr.Instance.MainEntity.Attribute.Score, GameMgr.Instance.MainEntity.KillCount, TSCData.Instance.Role.Name);
        }
    }
}
