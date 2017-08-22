using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoad : MonoBehaviour
{

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += SceneLoaded;
        SetTargetFramRate();
    }

    void SetTargetFramRate()
    {
#if UNITY_EDITOR
        if (Application.targetFrameRate != 60)
        {
            Application.targetFrameRate = 60;
        }
#elif UNITY_ANDROID || UNITY_IPHONE
        if(Application.targetFrameRate != 30)
        {
            Application.targetFrameRate = 30;
        }
#endif
    }


    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex > 1)
        {
            if (null == GameMgr.Instance)
            {
                GameObject prefab = ResourcesMgr.Instance.LoadResource<GameObject>(ResourceType.RESOURCE_MGR, "GameManager");
                if (null != prefab)
                {
                    GameObject.Instantiate(prefab);
                }
            }else
            {
                GameMgr.Instance.BeginGame();
            }
        }
    }

    //private void OnLevelWasLoaded(int level)
    //{
    //    if (null == GameMgr.Instance)
    //    {
    //        GameObject prefab =  ResourcesMgr.Instance.LoadResource<GameObject>(ResourceType.RESOURCE_MGR, "GameManager");
    //        if(null != prefab)
    //        {
    //            GameObject go = (GameObject)GameObject.Instantiate(prefab);
    //        }
    //    }
    //}
}
