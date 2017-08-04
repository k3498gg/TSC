using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class First : MonoBehaviour
{
    private bool isInit = false;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        if (isInit)
        {
            return;
        }
        isInit = true;
        SetTargetFramRate();
        Transform cache = transform;
        Transform m_Btn = cache.Find("Button");

        UGUIEventListener.Get(m_Btn.gameObject).onClick = EnterGame;
    }


    void EnterGame(GameObject go)
    {
        SceneManager.LoadScene(1);
    }

    void SetTargetFramRate()
    {
#if UNITY_EDITOR
        Application.targetFrameRate = 60;
#elif UNITY_ANDROID || UNITY_IPHONE
           Application.targetFrameRate = 30;
#endif
    }
}
