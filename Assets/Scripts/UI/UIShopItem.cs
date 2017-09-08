using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour
{
    private Text m_price;
    private Image m_image;
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
        m_price = m_cache.Find("Price").GetComponent<Text>();
        m_image = m_cache.Find("Skin").GetComponent<Image>();
        m_button = m_cache.GetComponent<Button>();
        m_button.onClick.AddListener(OpenItemDetail);
        //UGUIEventListener.Get(Cache).onClick = OpenItemDetail;
    }

    public void SetShopItemInfo(EquipInfo shopInfo)
    {
        if (null == shopInfo)
            return;
        Init();
        Id = shopInfo.id;
        m_price.text = LanguageMgr.Instance.GetText(shopInfo.nameId) + ":" + shopInfo.price.ToString();
        m_image.sprite = null;
        SetLock(TSCData.Instance.ContainSkin(Id));
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
            Debug.LogError("已经购买的了皮肤ID：" + Id.ToString());
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
            TSCData.Instance.AddSkin(Id);
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
            else
            {
                if (info.fashionId != 0 || info.fashionId != -1)
                {
                    FashionInfo fashionInfo = InfoMgr<FashionInfo>.Instance.GetInfo(info.fashionId);
                    if (TSCData.Instance.ContainSkin(fashionInfo.tigerId)
                        && TSCData.Instance.ContainSkin(fashionInfo.stickId)
                        && TSCData.Instance.ContainSkin(fashionInfo.chickId))
                    {
                        TSCData.Instance.AddSkin(info.fashionId);
                    }
                }
            }


            UIShopInfo shop = UIManager.Instance.GetGameWindowScript<UIShopInfo>(WindowID.WindowID_Shopping);
            if (null != shop)
            {
                shop.BuySkinLock();
            }
            TSCData.Instance.SaveHeroSkin();
        }
    }
}
