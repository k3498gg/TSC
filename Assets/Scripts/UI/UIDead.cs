using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIDead : MonoBehaviour
{
    private Transform btnBack;
    private Transform btnLive;
    private Text timer;
    private bool init = false;

    void Init()
    {
        if(init)
        {
            return;
        }
        init = true;
        btnBack = transform.Find("BtnBack");
        btnLive = transform.Find("BtnLive");
        timer = transform.Find("Timer/Timer").GetComponent<Text>();

        UGUIEventListener.Get(btnBack.gameObject).onClick = BackToMain;
        UGUIEventListener.Get(btnLive.gameObject).onClick = HeroRelive;
    }

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        Timer.Instance.AddTimer(1, 3, true, Handler);
    }

    void Handler(Timer.TimerData data)
    {
        if(null != timer)
        {
            timer.text = data.invokeTimes.ToString();
        }
    }

    void BackToMain(GameObject go)
    {
        GameMgr.Instance.IsEnterGame = false;
        TSCData.Instance.Clear();
        PoolMgr.Instance.DespawnerAll();
        SceneManager.LoadScene(AppConst.First);
    }


    void HeroRelive(GameObject go)
    {

    }
}
