using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IEntity : MonoBehaviour
{
    private UIHUDName m_hudName;
    private int killCount;
    private int beKillCount;


    public UIHUDName HudName
    {
        get
        {
            return m_hudName;
        }

        set
        {
            m_hudName = value;
        }
    }

    public int KillCount
    {
        get
        {
            return killCount;
        }

        set
        {
            killCount = value;
        }
    }

    public int BeKillCount
    {
        get
        {
            return beKillCount;
        }

        set
        {
            beKillCount = value;
        }
    }

    public void KillBody()
    {
        KillCount++;
    }

    public void BeKilled()
    {
        BeKillCount++;
    }
}
