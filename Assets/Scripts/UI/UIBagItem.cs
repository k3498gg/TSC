using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBagItem : MonoBehaviour
{
    private Text m_price;
    private Image m_image;
    private Transform m_flag; //用於標記當前穿的裝備
    private bool init = false;
    private Transform m_cache;
    private Button m_button;
    private int id;

    public GameObject Cache
    {
        get
        {
            if (null != m_cache)
            {
                return m_cache.gameObject;
            }
            return gameObject;
        }
    }

    public int Id
    {
        get
        {
            return id;
        }

        private set
        {
            id = value;
        }
    }

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        if (init)
            return;
        init = true;
        m_cache = transform;
        m_flag = m_cache.Find("Flag");
        m_price = m_cache.Find("Price").GetComponent<Text>();
        m_image = m_cache.Find("Skin").GetComponent<Image>();
        m_button = m_cache.GetComponent<Button>();
        m_button.onClick.AddListener(OpenItemDetail);
        //UGUIEventListener.Get(Cache).onClick = OpenItemDetail;
    }

    public void SetBagItemInfo(EquipInfo shopInfo)
    {
        if (null == shopInfo)
            return;
        Init();
        Id = shopInfo.id;
        m_price.text = LanguageMgr.Instance.GetText(shopInfo.nameId);
        m_image.sprite = null;
        SetUseState(id);
        //SetLock(TSCData.Instance.ContainSkin(Id));
    }

    void SetUseState(int id)
    {
        if (m_flag)
        {
            int fashionID = Util.GetFashionClothId(TSCData.Instance.Role.KeyTigerID, TSCData.Instance.Role.KeyStickID, TSCData.Instance.Role.KeyChickID);
            bool used = (id == TSCData.Instance.Role.KeyChickID || id == TSCData.Instance.Role.KeyStickID || id == TSCData.Instance.Role.KeyTigerID || id == fashionID);
            SetUseState(used);
        }
    }

    public void SetUseState(bool used)
    {
        if (m_flag)
        {
            Util.SetActive(m_flag.gameObject,used);
        }
    }

    public void SetLock(bool isLock)
    {
        if (m_image)
        {
            m_image.color = isLock ? Color.gray : Color.green;
        }
    }

    void OpenItemDetail()
    {
        if (TSCData.Instance.ContainSkin(Id))
        {
            EquipInfo info = InfoMgr<EquipInfo>.Instance.GetInfo(Id);
            if (null != info)
            {
                bool change = false;
                //int lastId = -1;
                //int lastFashionId = Util.GetFashionClothId(TSCData.Instance.Role.KeyTigerID, TSCData.Instance.Role.KeyStickID, TSCData.Instance.Role.KeyChickID);
                if (info.equipType + 1 == (int)ShopItemType.ShopItem_TIGER)
                {
                    if (TSCData.Instance.Role.KeyTigerID != Id)
                    {
                        //lastId = TSCData.Instance.Role.KeyTigerID;
                        TSCData.Instance.Role.KeyTigerID = Id;
                        change = true;
                    }
                }
                else if (info.equipType + 1 == (int)ShopItemType.ShopItem_STICK)
                {
                    if (TSCData.Instance.Role.KeyStickID != Id)
                    {
                        //lastId = TSCData.Instance.Role.KeyStickID;
                        TSCData.Instance.Role.KeyStickID = Id;
                        change = true;
                    }
                }
                else if (info.equipType + 1 == (int)ShopItemType.ShopItem_CHICK)
                {
                    if (TSCData.Instance.Role.KeyChickID != Id)
                    {
                        //lastId = TSCData.Instance.Role.KeyChickID;
                        TSCData.Instance.Role.KeyChickID = Id;
                        change = true;
                    }
                }
                else if (info.equipType + 1 == (int)ShopItemType.ShopItem_TAO)
                {
                    FashionInfo fasion = InfoMgr<FashionInfo>.Instance.GetInfo(info.id);
                    if (null != fasion)
                    {
                        if(TSCData.Instance.Role.KeyTigerID != fasion.tigerId)
                        {
                            TSCData.Instance.Role.KeyTigerID = fasion.tigerId;
                            if(!change)
                            {
                                change = true;
                            }
                        }
                    
                        if (TSCData.Instance.Role.KeyStickID != fasion.stickId)
                        {
                            TSCData.Instance.Role.KeyStickID = fasion.stickId;
                            if (!change)
                            {
                                change = true;
                            }
                        }

                        if (TSCData.Instance.Role.KeyChickID != fasion.chickId)
                        {
                            TSCData.Instance.Role.KeyChickID = fasion.chickId;
                            if (!change)
                            {
                                change = true;
                            }
                        }
                    }
                }

                if (change)
                {
                    UIBagInfo bagInfo = UIManager.Instance.GetGameWindowScript<UIBagInfo>(WindowID.WindowID_Bag);
                    int fashionId = Util.GetFashionClothId(TSCData.Instance.Role.KeyTigerID, TSCData.Instance.Role.KeyStickID, TSCData.Instance.Role.KeyChickID);
                    bagInfo.Refresh(TSCData.Instance.Role.KeyTigerID, TSCData.Instance.Role.KeyStickID, TSCData.Instance.Role.KeyChickID , fashionId);
                }
            }
        }
        else
        {
            EquipInfo info = InfoMgr<EquipInfo>.Instance.GetInfo(Id);
            UIManager.Instance.ShowWindow(WindowID.WindowID_Confirm);
            UIConfirm confirm = UIManager.Instance.GetGameWindowScript<UIConfirm>(WindowID.WindowID_Confirm);
            string context = LanguageMgr.Instance.GetText(19, info.price.ToString(), LanguageMgr.Instance.GetText(info.nameId));
            confirm.SetTextContent(string.Empty, context);
            confirm.BindAction(BuySkin, null);
        }


    }

    void BuySkin()
    {
        if (!TSCData.Instance.ContainSkin(Id))
        {
            SetLock(true);
            EquipInfo info = InfoMgr<EquipInfo>.Instance.GetInfo(Id);
            if (info.equipType + 1 == (int)ShopItemType.ShopItem_TAO)
            {
                FashionInfo fashionInfo = InfoMgr<FashionInfo>.Instance.GetInfo(Id);
                if (null != fashionInfo)
                {
                    TSCData.Instance.AddSkin(fashionInfo.tigerId);
                    TSCData.Instance.AddSkin(fashionInfo.stickId);
                    TSCData.Instance.AddSkin(fashionInfo.chickId);
                }
            }

            TSCData.Instance.AddSkin(Id);
            UIShopInfo shop = UIManager.Instance.GetGameWindowScript<UIShopInfo>(WindowID.WindowID_Shopping);
            if (null != shop)
            {
                shop.BuySkinLock();
            }
            TSCData.Instance.SaveHeroSkin();
        }
    }
}
