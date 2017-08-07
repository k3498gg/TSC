using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropMgr : Singleton<ItemDropMgr>
{
    private float mTotalTime = 0;
    private int min = (int)ItemType.ITEM_MARK;
    private int max = (int)ItemType.ITEM_ENERGY;

    //private List<DropItemInfo> dropItemInfos;

    //public List<DropItemInfo> DropItemInfos
    //{
    //    get
    //    {
    //        if (null == dropItemInfos)
    //        {
    //            dropItemInfos = new List<DropItemInfo>();
    //        }
    //        return dropItemInfos;
    //    }

    //    set
    //    {
    //        dropItemInfos = value;
    //    }
    //}

    public void InitMapDrop()
    {
        List<ItemMapInfo> itemMaps = TSCData.Instance.GetCurItemMapInfo(GameMgr.Instance.MapId);
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

            //int min = (int)ItemType.ITEM_MARK;
            //int max = (int)ItemType.ITEM_ENERGY;
            foreach (int idx in set)
            {
                if (idx < points.Count)
                {
                    int r = Random.Range(min,max);
                    InistantDropItem(points[idx],(ItemType)r);
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
                            InistantDropItem(items[i],ItemType.ITEM_ENERGY);
                        }
                    }
                }
            }
        }
    }

    void InistantDropItem(ItemMapInfo mapInfo, ItemType type)
    {
        Debug.LogError("产生道具类型:" + type);
        float offX = Random.Range(-mapInfo.Width, mapInfo.Width);
        float offY = Random.Range(-mapInfo.Height, mapInfo.Height);
        GameObject go = Spawner((int)type, new Vector3(mapInfo.PosX + offX, 0, mapInfo.PosY + offY), ResourceType.RESOURCE_ITEM, GameMgr.Instance.ItemRoot);
        DropItemInfo dropInfo = Util.AddComponent<DropItemInfo>(go);
        dropInfo.ItemId = mapInfo.Index;
        dropInfo.InfoId = (int)type;
        dropInfo.Area = mapInfo.Area;
        dropInfo.IsLock = false;
        TSCData.Instance.DropItemDic[dropInfo.ItemId] = dropInfo;
    }

    GameObject Spawner(int id, Vector3 v, ResourceType t, Transform parent)
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

        ItemDespawnerInfo despawnItem = new ItemDespawnerInfo(dropItem.ItemId, dropItem.InfoId);
        MapArea area = dropItem.Area;

        if (TSCData.Instance.BackItemMapDic.ContainsKey(area))
        {
            TSCData.Instance.BackItemMapDic[area].Add(despawnItem);
        }
        else
        {
            List<ItemDespawnerInfo> itemIds = new List<ItemDespawnerInfo>();
            itemIds.Add(despawnItem);
            TSCData.Instance.BackItemMapDic[area] = itemIds;
        }

        if (TSCData.Instance.DropItemDic.ContainsKey(dropItem.ItemId))
        {
            TSCData.Instance.DropItemDic.Remove(dropItem.ItemId);
        }
        Despawner(t, dropItem.Cache);
    }

    void Despawner(ResourceType t, Transform inst)
    {
        PoolMgr.Instance.Despawner(t, inst);
    }

    public void Clear()
    {
        TSCData.Instance.BackItemMapDic.Clear();
        TSCData.Instance.DropItemDic.Clear();
        PoolMgr.Instance.Despawner(ResourceType.RESOURCE_ITEM);
    }


    private void FreshMapDrop()
    {
        foreach (KeyValuePair<MapArea, List<ItemDespawnerInfo>> kv in TSCData.Instance.BackItemMapDic)
        {
            FreshMapDropByArea(kv.Key);
        }
        TSCData.Instance.BackItemMapDic.Clear();
    }

    void FreshMapDropByArea(MapArea area)
    {
        List<ItemMapInfo> list = TSCData.Instance.GetCurItemMapInfo(GameMgr.Instance.MapId);
        if (null == list)
        {
            return;
        }

        if (TSCData.Instance.BackItemMapDic.ContainsKey(area))
        {
            if (!TSCData.Instance.TotalAreaItemMapDic.ContainsKey(area))
            {
                List<ItemMapInfo> temp = list.FindAll(delegate (ItemMapInfo mapInfo) { return mapInfo.Area == area; });
                TSCData.Instance.TotalAreaItemMapDic[area] = temp;
            }

            List<ItemMapInfo> areaItems = TSCData.Instance.TotalAreaItemMapDic[area];
            List<ItemDespawnerInfo> spans = TSCData.Instance.BackItemMapDic[area];
            int count = areaItems.Count;
            if(count == 0)
            {
                return;
            }
            int nedCount = spans.Count;
            HashSet<int> set = new HashSet<int>();
            int idx = 0;
            while (set.Count < nedCount)
            {
                int index = Random.Range(0, count);
 
                if (TSCData.Instance.DropItemDic.ContainsKey(areaItems[index].Index))
                {
                    continue;
                }

                if (!set.Contains(index))
                {
                    set.Add(index);
                }

                idx++;
                if (idx > AppConst.ItemFreshCount)
                {
                    break;
                }
            }

            int spawcount = 0;
            foreach(int pos in set)
            {
                if(pos < areaItems.Count)
                {
                    if(spans[spawcount].InfoId != (int)ItemType.ITEM_ENERGY)
                    {
                        int r = Random.Range(min, max); //状态道具刷新（随机一个）
                        InistantDropItem(areaItems[pos], (ItemType)r);
                    }
                    else
                    {
                        //积分道具刷新
                        InistantDropItem(areaItems[pos], (ItemType)spans[spawcount].InfoId);
                    }
                    spawcount++;
                }
            }
            set.Clear();
        }
    }

    public void Update(float delateTime)
    {
        mTotalTime += delateTime;
        if (mTotalTime <= 10)
        {
            return;
        }

        mTotalTime = 0;
        //重新刷新一次道具
        FreshMapDrop();
    }
}
