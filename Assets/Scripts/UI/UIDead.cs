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
            }
        }
    }

    public void ShowDeadUI()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            Init();
        }
        cache.localScale = Vector3.one;
        Timer.Instance.AddTimer(1, 3, true, Handler);
    }

    void BackToMain(GameObject go)
    {
        GameMgr.Instance.IsEnterGame = false;
        TSCData.Instance.Clear();
        PoolMgr.Instance.DespawnerAll();
        SceneManager.LoadScene(AppConst.First);
        cache.localScale = Vector3.zero;
    }

    void HeroRelive(GameObject go)
    {
        cache.localScale = Vector3.zero;
    }
}
