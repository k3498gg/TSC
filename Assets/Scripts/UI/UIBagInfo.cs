using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBagInfo : UIBaseWindow
{
    private Toggle[] m_toggles;
    //private ScrollRect m_scrollRect;
    private ToggleGroup m_group;

    private bool init = false;
    private GameObject m_close;
    private List<UIBagItem> m_bagItems;
    private GameObject m_prefab;
    private Transform m_parent;
    private List<int> skins;

    public List<int> Skins
    {
        get
        {
            if (null == skins)
            {
                skins = new List<int>();
            }
            return skins;
        }
    }

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
        m_bagItems = new List<UIBagItem>();
        m_prefab = ResourcesMgr.Instance.LoadResource<GameObject>(ResourceType.RESOURCE_UI, AppConst.BagItem);
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

    private void OnEnable()
    {
        HashSet<int> sets = TSCData.Instance.GetSkin();
        Skins.Clear();
        Skins.AddRange(sets);
        Skins.Sort();
        m_toggles[0].isOn = true;
    }

    public void Refresh(int tigerId,int stickId,int chickId, int fashionId)
    {
        for (int i = 0; i < m_bagItems.Count; i++)
        {
            if (m_bagItems[i].Id == tigerId || m_bagItems[i].Id == stickId || m_bagItems[i].Id == chickId || m_bagItems[i].Id == fashionId)
            {
                m_bagItems[i].SetUseState(true);
            }
            else
            {
                m_bagItems[i].SetUseState(false);
            }
        }
    }

    void ShowBagItemByType(ShopItemType type)
    {
        if (null != m_prefab)
        {
            if (Skins.Count > m_bagItems.Count)
            {
                for (int i = m_bagItems.Count; i < Skins.Count; i++)
                {
                    GameObject uiObject = (GameObject)GameObject.Instantiate(m_prefab);
                    uiObject.transform.SetParent(m_parent);
                    uiObject.transform.localScale = Vector3.one;
                    UIBagItem item = Util.AddComponent<UIBagItem>(uiObject);
                    m_bagItems.Add(item);
                }
            }


            for (int i = 0; i < m_bagItems.Count; i++)
            {
                if (i < Skins.Count)
                {
                    int val = Skins[i];
                    EquipInfo info = InfoMgr<EquipInfo>.Instance.GetInfo(val);
                    if (null != info)
                    {
                        if (type == ShopItemType.ShopItem_ALL)
                        {
                            m_bagItems[i].SetBagItemInfo(info);
                            Util.SetActive(m_bagItems[i].Cache, true);
                        }
                        else
                        {
                            if (info.equipType + 1 == (int)type)
                            {
                                m_bagItems[i].SetBagItemInfo(info);
                                Util.SetActive(m_bagItems[i].Cache, true);
                            }
                            else
                            {
                                Util.SetActive(m_bagItems[i].Cache, false);
                            }
                        }
                    }
                    else
                    {
                        Util.SetActive(m_bagItems[i].Cache, false);
                    }
                }
                else
                {
                    Util.SetActive(m_bagItems[i].Cache, false);
                }
            }
        }
    }

    void Close(GameObject go)
    {
        m_group.SetAllTogglesOff();
        UIManager.Instance.HideWindow(WindowID.WindowID_Bag);
    }
}
