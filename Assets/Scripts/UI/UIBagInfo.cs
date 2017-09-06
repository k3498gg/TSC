using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBagInfo : UIBaseWindow
{
    private Toggle[] m_toggles;
    private ScrollRect m_scrollRect;
    private ToggleGroup m_group;

    private bool init = false;
    private GameObject m_close;
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
        this.windowID = WindowID.WindowID_Bag;
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
                ShowBagItemByType((ShopItemType)(i + 1));
                break;
            }
        }
    }

    void ShowBagItemByType(ShopItemType type)
    {
       
    }

    void Close(GameObject go)
    {
        m_group.SetAllTogglesOff();
        UIManager.Instance.HideWindow(WindowID.WindowID_Bag);
    }
}
