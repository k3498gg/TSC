using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropMgr : Singleton<ItemDropMgr>
{
    private float mTotalTime = 0;

    private List<DropItemInfo> dropItemInfos;

    public List<DropItemInfo> DropItemInfos
    {
        get
        {
            if (null == dropItemInfos)
            {
                dropItemInfos = new List<DropItemInfo>();
            }
            return dropItemInfos;
        }

        set
        {
            dropItemInfos = value;
        }
    }

    public void InitMapDrop()
    {
        //if (!GameMgr.Instance.DicMapInfo.ContainsKey(mapId))
        //{
        //    return;
        //}
        //MapInfo mapInfo = GameMgr.Instance.DicMapInfo[mapId];

        List<ItemMapInfo> itemMaps = EntityMgr.Instance.GetCurItemMapInfo(GameMgr.Instance.MapId);
        if (null != itemMaps)
        {
            List<ItemMapInfo> points = itemMaps.FindAll(delegate (ItemMapInfo map) { return map.IMapType == ItemMapType.POINT; });
            HashSet<int> set = new HashSet<int>();
            int cout = points.Count;
            while (set.Count < AppConst.ItemFreshCount)
            {
                int r = Random.Range(0, cout);
                if (!set.Contains(r))
                {
                    set.Add(r);
                }

                if (cout == set.Count)
                {
                    break;
                }
            }

            int poz = 0;
            foreach (int idx in set)
            {
                if (idx < points.Count)
                {
                    poz++;
                    InistantDropItem(points[idx]);
                }
            }


            for (int idx = 0; idx < (int)MapArea.AREA_MAX; idx++)
            {
                List<ItemMapInfo> items = itemMaps.FindAll(delegate (ItemMapInfo map) { return map.IMapType == ItemMapType.ITEM && map.Area == (MapArea)idx; });
                if (items.Count > 0)
                {
                    set.Clear();
                    cout = items.Count;
                    while (set.Count < AppConst.ItemAreaCount)
                    {
                        int r = Random.Range(0, cout);
                        if (!set.Contains(r))
                        {
                            set.Add(r);
                        }

                        if (cout == set.Count)
                        {
                            break;
                        }
                    }

                    foreach (int i in set)
                    {
                        if (i < items.Count)
                        {
                            InistantDropItem(items[i]);
                        }
                    }
                }
            }
        }
    }

    void InistantDropItem(ItemMapInfo mapInfo)
    {
        float offX = Random.Range(-mapInfo.Width, mapInfo.Width);
        float offY = Random.Range(-mapInfo.Height, mapInfo.Height);
        GameObject go = Spawner((int)ItemType.ITEM_ENERGY, new Vector3(mapInfo.PosX + offX, 0, mapInfo.PosY + offY), ResourceType.RESOURCE_ITEM, GameMgr.Instance.ItemRoot);
        DropItemInfo dropInfo = Util.AddComponent<DropItemInfo>(go);
        dropInfo.ItemId = mapInfo.Index;
        dropInfo.InfoId = (int)ItemType.ITEM_ENERGY;
        dropInfo.Area = mapInfo.Area;
        //dropInfo.DropAI = DropAI.LATER;
        EntityMgr.Instance.DropItemDic[dropInfo.ItemId] = dropInfo;
    }

    public GameObject Spawner(int id, Vector3 v, ResourceType t, Transform parent)
    {
        ItemInfo item = InfoMgr<ItemInfo>.Instance.GetInfo(id);
        return ResourcesMgr.Instance.Spawner(item.model, v, t, parent);
    }

    public void Despawner(ResourceType t, DropItemInfo dropItem)
    {
        if (null == dropItem)
        {
            return;
        }

        int itemId = dropItem.ItemId;
        MapArea area = dropItem.Area;

        if (EntityMgr.Instance.BackItemMapDic.ContainsKey(area))
        {
            EntityMgr.Instance.BackItemMapDic[area].Add(itemId);
        }
        else
        {
            List<int> itemIds = new List<int>();
            itemIds.Add(itemId);
            EntityMgr.Instance.BackItemMapDic[area] = itemIds;
        }

        if (EntityMgr.Instance.DropItemDic.ContainsKey(dropItem.ItemId))
        {
            EntityMgr.Instance.DropItemDic.Remove(dropItem.ItemId);
        }
        Despawner(t, dropItem.Cache);
    }

    public void Despawner(ResourceType t, Transform inst)
    {
        PoolMgr.Instance.Despawner(t, inst);
    }


    private void FreshMapDrop()
    {
        foreach (KeyValuePair<MapArea, List<int>> kv in EntityMgr.Instance.BackItemMapDic)
        {
            //kv.Key 
        }
    }

    void FreshMapDropByArea(MapArea area)
    {
        List<ItemMapInfo> list = EntityMgr.Instance.GetCurItemMapInfo(GameMgr.Instance.MapId);

    }

    public void Update(float delateTime)
    {
        mTotalTime += delateTime;
        if (mTotalTime <= 30)
        {
            return;
        }

        mTotalTime = 0;
        //重新刷新一次道具
        FreshMapDrop();
    }
}
