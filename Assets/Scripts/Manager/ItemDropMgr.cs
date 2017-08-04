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

            int min = (int)ItemType.ITEM_MARK;
            int max = (int)ItemType.ITEM_ENERGY;
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
        float offX = Random.Range(-mapInfo.Width, mapInfo.Width);
        float offY = Random.Range(-mapInfo.Height, mapInfo.Height);
        GameObject go = Spawner((int)type, new Vector3(mapInfo.PosX + offX, 0, mapInfo.PosY + offY), ResourceType.RESOURCE_ITEM, GameMgr.Instance.ItemRoot);
        DropItemInfo dropInfo = Util.AddComponent<DropItemInfo>(go);
        dropInfo.ItemId = mapInfo.Index;
        dropInfo.InfoId = (int)type;
        dropInfo.Area = mapInfo.Area;
        dropInfo.IsLock = false;
        //dropInfo.DropAI = DropAI.LATER;
        Debug.LogError(mapInfo.Index +"　"+type);
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

        //int itemId = dropItem.ItemId;
        ItemDespawnerInfo despawnItem = new ItemDespawnerInfo(dropItem.ItemId, dropItem.InfoId);
        MapArea area = dropItem.Area;

        if (EntityMgr.Instance.BackItemMapDic.ContainsKey(area))
        {
            EntityMgr.Instance.BackItemMapDic[area].Add(despawnItem);
        }
        else
        {
            List<ItemDespawnerInfo> itemIds = new List<ItemDespawnerInfo>();
            itemIds.Add(despawnItem);
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
        foreach (KeyValuePair<MapArea, List<ItemDespawnerInfo>> kv in EntityMgr.Instance.BackItemMapDic)
        {
            FreshMapDropByArea(kv.Key);
        }
        EntityMgr.Instance.BackItemMapDic.Clear();
    }

    void FreshMapDropByArea(MapArea area)
    {
        List<ItemMapInfo> list = EntityMgr.Instance.GetCurItemMapInfo(GameMgr.Instance.MapId);
        if (null == list)
        {
            return;
        }

        if (EntityMgr.Instance.BackItemMapDic.ContainsKey(area))
        {
            if (!EntityMgr.Instance.TotalAreaItemMapDic.ContainsKey(area))
            {
                List<ItemMapInfo> temp = list.FindAll(delegate (ItemMapInfo mapInfo) { return mapInfo.Area == area; });
                EntityMgr.Instance.TotalAreaItemMapDic[area] = temp;
            }

            List<ItemMapInfo> areaItems = EntityMgr.Instance.TotalAreaItemMapDic[area];
            List<ItemDespawnerInfo> spans = EntityMgr.Instance.BackItemMapDic[area];
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
 
                if (EntityMgr.Instance.DropItemDic.ContainsKey(areaItems[index].Index))
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
                    InistantDropItem(areaItems[pos], (ItemType) spans[spawcount].InfoId);
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
