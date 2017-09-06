using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopInfo : UIBaseWindow
{
    private Toggle[] m_toggles;
    private ScrollRect m_scrollRect;
    private ToggleGroup m_group;

    private bool init = false;
    //private Transform m_cache;
    private GameObject m_close;
    private ShopItemType m_lastSelectType = ShopItemType.NONE;
    private List<UIShopItem> m_shopItems;
    private List<ShopInfo> m_shops;
    private GameObject m_prefab;
    private Transform m_parent;

    void Awake()
    {
        InitWindowData();
    }

    public override void InitWindowData()
    {
        base.InitWindowData();
        windowData.showMode = UIWindowShowMode.DoNothing;
        windowData.windowType = UIWindowType.Normal;
        this.windowID = WindowID.WindowID_Shopping;
        this.preWindowID = WindowID.WindowID_Invaild; //初始化的界面前置界面为null
        Init();
    }

    void Init()
    {
        if (init)
        {
            return;
        }
        init = true;
        //m_cache = transform;
        m_parent = Cache.Find("ScrollView/Viewport/Content");
        m_close = Cache.Find("Button").gameObject;
        m_group = Cache.Find("Grid").GetComponent<ToggleGroup>();
        m_toggles = new Toggle[5];
        m_toggles[0] = Cache.Find("Grid/Toggle_All").GetComponent<Toggle>();
        m_toggles[1] = Cache.Find("Grid/Toggle_Tao").GetComponent<Toggle>();
        m_toggles[2] = Cache.Find("Grid/Toggle_Tiger").GetComponent<Toggle>();
        m_toggles[3] = Cache.Find("Grid/Toggle_Stick").GetComponent<Toggle>();
        m_toggles[4] = Cache.Find("Grid/Toggle_Chick").GetComponent<Toggle>();

        for (int i = 0; i < m_toggles.Length; i++)
        {
            m_toggles[i].onValueChanged.AddListener(OnValueChanged);
        }

        UGUIEventListener.Get(m_close).onClick = Close;
        m_shopItems = new List<UIShopItem>();
        m_shops = new List<ShopInfo>(InfoMgr<ShopInfo>.Instance.Dict.Values);
        m_prefab = ResourcesMgr.Instance.LoadResource<GameObject>(ResourceType.RESOURCE_UI, AppConst.ShopItem);
    }

    void OnValueChanged(bool isOn)
    {
        for (int i = 0; i < m_toggles.Length; i++)
        {
            if (m_toggles[i].isOn)
            {
                ShowShopItemByType((ShopItemType)(i + 1));
                break;
            }
        }
    }

    private void OnEnable()
    {
        m_toggles[0].isOn = true;
    }

    void ShowItem(List<ShopInfo> shopInfos, ShopItemType type)
    {
        if (null != m_prefab)
        {
            if (m_shopItems.Count < shopInfos.Count)
            {
                for (int i = m_shopItems.Count; i < shopInfos.Count; i++)
                {
                    GameObject uiObject = (GameObject)GameObject.Instantiate(m_prefab);
                    uiObject.transform.SetParent(m_parent);
                    uiObject.transform.localScale = Vector3.one;
                    UIShopItem item = Util.AddComponent<UIShopItem>(uiObject);
                    m_shopItems.Add(item);
                }
            }

            for (int i = 0; i < m_shopItems.Count; i++)
            {
                if (i < shopInfos.Count)
                {
                    if (type == ShopItemType.ShopItem_ALL)
                    {
                        m_shopItems[i].SetShopItemInfo(shopInfos[i]);
                        Util.SetActive(m_shopItems[i].Cache, true);
                    }
                    else
                    {
                        if (shopInfos[i].equipType + 1 == (int)type)
                        {
                            m_shopItems[i].SetShopItemInfo(shopInfos[i]);
                            Util.SetActive(m_shopItems[i].Cache, true);
                        }
                        else
                        {
                            Util.SetActive(m_shopItems[i].Cache, false);
                        }
                    }
                }
                else
                {
                    Util.SetActive(m_shopItems[i].Cache, false);
                }
            }
        }
    }

    void ShowShopItemByType(ShopItemType type)
    {
        if (m_lastSelectType == type)
        {
            return;
        }
        m_lastSelectType = type;
        ShowItem(m_shops, type);
    }


    void Close(GameObject go)
    {
        m_group.SetAllTogglesOff();
        UIManager.Instance.HideWindow(WindowID.WindowID_Shopping);
    }

    public void BuySkinLock()
    {
        for(int i = 0; i< m_shopItems.Count;i++)
        {
            if(TSCData.Instance.ContainSkin(m_shopItems[i].Id))
            {
                m_shopItems[i].SetLock(true);
            }
        }
    }
}
