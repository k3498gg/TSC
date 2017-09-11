using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGameOverInfo : UIBaseWindow
{
    private GameObject m_shop;
    private GameObject m_back;
    private GameObject m_again;
    private Text m_score;
    private Text m_coin;
    private Text m_factor;
    private Text m_totalcoin;
    private bool init = false;


    void Awake()
    {
        InitWindowData();
    }

    public override void InitWindowData()
    {
        base.InitWindowData();
        windowData.showMode = UIWindowShowMode.DoNothing;
        windowData.windowType = UIWindowType.PopUp;
        this.windowID = WindowID.WindowID_Over;
        this.preWindowID = WindowID.WindowID_Invaild;
        Init();
    }

    void Init()
    {
        if (init)
        {
            return;
        }
        init = true;
        Transform cache = transform;
        m_score = cache.Find("Score/value").GetComponent<Text>();
        m_coin = cache.Find("Coin/value").GetComponent<Text>();
        m_factor = cache.Find("Factor/value").GetComponent<Text>();
        m_totalcoin = cache.Find("TotalCoin/value").GetComponent<Text>();

        m_shop = cache.Find("Shop").gameObject;
        m_back = cache.Find("Back").gameObject;
        m_again = cache.Find("Again").gameObject;

        UGUIEventListener.Get(m_shop).onClick = OpenShopUI;
        UGUIEventListener.Get(m_back).onClick = BackMainUI;
        UGUIEventListener.Get(m_again).onClick = AgainGame;
    }


    void SetRewardData()
    {
        Init();
        if (null != GameMgr.Instance.MainEntity)
        {
            m_score.text = (GameMgr.Instance.MainEntity.Attribute.Score).ToString();
            int gain = GameMgr.Instance.MainEntity.Attribute.Score / 100;
            m_coin.text = gain.ToString();
            int rand = Random.Range(1, 7);
            m_factor.text = rand.ToString();
            int total = rand * gain;
            m_totalcoin.text = total.ToString();

            if(total != 0)
            {
                TSCData.Instance.Role.Money += total;
                Util.SaveHeroCoin(AppConst.KeyCoin, total);
            }
        }
    }

    private void OnEnable()
    {
        SetRewardData();
    }


    void OpenShopUI(GameObject go)
    {
        UIManager.Instance.HideWindow(WindowID.WindowID_Over);
        UIManager.Instance.ShowWindow(WindowID.WindowID_Shopping);
        SceneManager.LoadScene(AppConst.MainSceneIndex);
    }

    void BackMainUI(GameObject go)
    {
        UIManager.Instance.HideWindow(WindowID.WindowID_Over);
        UIManager.Instance.ShowWindow(WindowID.WindowID_FirstUI);
        SceneManager.LoadScene(AppConst.MainSceneIndex);
    }

    void AgainGame(GameObject go)
    {
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            UIManager.Instance.HideWindow(WindowID.WindowID_Over);
            if (null == GameMgr.Instance)
            {
                GameObject prefab = ResourcesMgr.Instance.LoadResource<GameObject>(ResourceType.RESOURCE_MGR, "GameManager");
                if (null != prefab)
                {
                    GameObject.Instantiate(prefab);
                }
            }
            else
            {
                GameMgr.Instance.BeginGame();
            }
        }
    }
}
