using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum DropAI
{
    NONE = 0,  //消失不再刷新
    INSTANCE = 1,//消失立即刷新
    LATER = 2, //消失等待一定时间刷新
}


public class DropItemInfo : IEntity
{
    private int itemId;
    private int infoId;
    private MapArea area;
    private Transform cache;
    private Transform child;
    private bool isLock = false; //用于判断当前是否被其他玩家占用

    public int ItemId
    {
        get
        {
            return itemId;
        }

        set
        {
            itemId = value;
        }
    }

    public int InfoId
    {
        get
        {
            return infoId;
        }

        set
        {
            infoId = value;
        }
    }

    public MapArea Area
    {
        get
        {
            return area;
        }

        set
        {
            area = value;
        }
    }

    public Transform Cache
    {
        get
        {
            if (null == cache)
            {
                cache = transform;
            }
            return cache;
        }

        set
        {
            cache = value;
        }
    }

    public bool IsLock
    {
        get
        {
            return isLock;
        }

        set
        {
            isLock = value;
        }
    }

    public Transform Child
    {
        get
        {
            if (null == child)
            {
                child = Cache.GetChild(0);
            }
            return child;
        }

        set
        {
            child = value;
        }
    }

    public void FlyToEntity(Entity entity)
    {
        if (null == entity)
        {
            return;
        }
        if (IsLock)
        {
            return;
        }
        IsLock = true;

        Vector3 v = entity.CacheModel.position;

        Cache.DOJump(v, 3, 1, 0.35f).SetEase(Ease.Linear).OnComplete(() =>
        {
            ItemDropMgr.Instance.Despawner(ResourceType.RESOURCE_ITEM, this);
            ItemInfo item = InfoMgr<ItemInfo>.Instance.GetInfo(infoId);
            if(null == item)
            {
                return;
            }
            ItemEffectInfo effect = InfoMgr<ItemEffectInfo>.Instance.GetInfo(item.effectId);
            if(null == effect)
            {
                return;
            }
            ItemType type = effect.effType;
            switch (type)
            {
                case ItemType.ITEM_MARK: //问号变身

                    break;
                case ItemType.ITEM_MAGNET: //吸铁石

                    break;
                case ItemType.ITEM_TRANSFERGATE: //传送门

                    break;
                case ItemType.ITEM_SPEED://速度变化

                    break;
                case ItemType.ITEM_PROTECT: //保护罩

                    break;
                case ItemType.ITEM_ENERGY:
                    EnergyUpdate(entity, effect);
                    break;
            }
        });
    }

    void MarkUpdate()
    {
        Debug.LogError("变身效果");
    }

    //吸鐵石
    void MagnetUpdate()
    {
        Debug.LogError("吸铁石");
    }

    void TransferUpdate()
    {
        Debug.LogError("传送门");
    }

    void SpeedUpdate()
    {
        Debug.LogError("速度变化");
    }

    void ProtectUpdate(Entity entity, ItemEffectInfo effect)
    {
        Debug.LogError("保护时间");
    }

    void EnergyUpdate(Entity entity, ItemEffectInfo effect)
    {
        entity.Attribute.Score += effect.score;
        entity.Attribute.CurPhy += effect.phys;
        if (entity.Attribute.CurPhy > entity.Attribute.MaxPhy)
        {
            entity.Attribute.CurPhy = entity.Attribute.MaxPhy;
        }
    }
}
