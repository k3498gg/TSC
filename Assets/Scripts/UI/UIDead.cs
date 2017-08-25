using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIDead : MonoBehaviour
{
    private Transform cache;
    private Transform btnBack;
    private Transform btnLive;
    private Text timer;
    private bool init = false;
    private string TimerDataId = string.Empty;

    void Init()
    {
        if (init)
        {
            return;
        }
        init = true;
        cache = transform;
        btnBack = cache.Find("BtnBack");
        btnLive = cache.Find("BtnLive");
        timer = cache.Find("Timer/Timer").GetComponent<Text>();

        UGUIEventListener.Get(btnBack.gameObject).onClick = BackToMain;
        UGUIEventListener.Get(btnLive.gameObject).onClick = HeroRelive;
    }

    void Handler(Timer.TimerData data)
    {
        if (null != timer)
        {
            timer.text = (data.invokeTimes - 1).ToString();
            if (data.invokeTimes == 1)
            {
                Debug.LogError("倒計時時間到,復活!");
                HeroRelive(null);
            }
        }
    }

    public void HideDeadUI()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    public void ShowDeadUI()
    {
        Invoke("DelayToShowDeadUI", 1.0f);
    }

    void DelayToShowDeadUI()
    {
        Init();
        timer.text = AppConst.RebornTime.ToString();
        Debug.LogError("Die HideUI!!!");
        if (!gameObject.activeSelf)
        {
           gameObject.SetActive(true);
        }
        cache.localScale = Vector3.one;
        Timer.TimerData data = Timer.Instance.AddTimer(1, AppConst.RebornTime, true, Handler);
        TimerDataId = data.ID;
    }

    void RemoveReliveHandler()
    {
        if (!string.IsNullOrEmpty(TimerDataId))
        {
            Timer.Instance.RemoveTimer(TimerDataId);
            TimerDataId = string.Empty;
        }
    }

    void BackToMain(GameObject go)
    {
        GameMgr.Instance.IsEnterGame = false;
        Timer.Instance.Clear();
        TSCData.Instance.Clear();
        PoolMgr.Instance.DespawnerAll();
        GameMgr.Instance.MainEntity.Clear();
        HideDeadUI();
        UIManager.Instance.ShowWindow(WindowID.WindowID_FirstUI);
        UIManager.Instance.HideWindow(WindowID.WindowID_MainUI);
        SceneManager.LoadScene(1);
        //cache.localScale = Vector3.zero;
    }

    void HeroRelive(GameObject go)
    {
        RemoveReliveHandler();
        GameMgr.Instance.MainEntity.Relive();
    }
}
