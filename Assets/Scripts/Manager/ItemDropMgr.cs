using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropMgr : Singleton<ItemDropMgr>
{
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

    public void InitMapDrop(int mapId)
    {
        if (!GameMgr.Instance.DicMapInfo.ContainsKey(mapId))
        {
            return;
        }
        MapInfo mapInfo = GameMgr.Instance.DicMapInfo[mapId];
        if (null != mapInfo)
        {
            List<ItemMapInfo> points = mapInfo.ItemMapInfo.FindAll(delegate (ItemMapInfo map) { return map.IMapType == ItemMapType.POINT; });
            List<ItemMapInfo> items = mapInfo.ItemMapInfo.FindAll(delegate (ItemMapInfo map) { return map.IMapType == ItemMapType.ITEM; });
            HashSet<int> set = new HashSet<int>();
            int cout = points.Count;
            while (set.Count < 5)
            {
                int r = Random.Range(0, cout);
                if (!set.Contains(r))
                {
                    set.Add(r);
                }
            }

            int poz = 0;
            foreach (int idx in set)
            {
                if (idx < points.Count)
                {
                    Debuger.LogError("随机五个点刷新五种道具位置:" + idx);
                    poz++;
                    float offX = Random.Range(-points[idx].Width, points[idx].Width);
                    float offY = Random.Range(-points[idx].Height, points[idx].Height);
                    GameObject go = Spawner(poz, new Vector3(points[idx].PosX + offX, 0, points[idx].PosY + offY), ResourceType.RESOURCE_ITEM, GameMgr.Instance.ItemRoot);
                    DropItemInfo dropInfo = Util.AddComponent<DropItemInfo>(go);
                    dropInfo.ItemId = idx;
                    dropInfo.InfoId = poz;
                    dropInfo.Area = points[idx].Area;
                    dropInfo.DropAI = DropAI.LATER;
                    EntityMgr.Instance.DropItemDic[idx] = dropInfo;
                }
            }


            for (int idx = 0; idx < items.Count; idx++)
            {
                float offX = Random.Range(-points[idx].Width, points[idx].Width);
                float offY = Random.Range(-points[idx].Height, points[idx].Height);
                GameObject go = Spawner((int)ItemType.ITEM_ENERGY, new Vector3(items[idx].PosX + offX, 0, items[idx].PosY + offY), ResourceType.RESOURCE_ITEM, GameMgr.Instance.ItemRoot);
                DropItemInfo dropInfo = Util.AddComponent<DropItemInfo>(go);
                dropInfo.ItemId = idx;
                dropInfo.InfoId = poz;
                dropInfo.Area = points[idx].Area;
                dropInfo.DropAI = DropAI.LATER;
                EntityMgr.Instance.DropItemDic[idx] = dropInfo;
            }
        }
    }

    public GameObject Spawner(int id, Vector3 v, ResourceType t, Transform parent)
    {
        ItemInfo item = InfoMgr<ItemInfo>.Instance.GetInfo(id);
        return ResourcesMgr.Instance.Spawner(item.model, v, t, parent);
    }
}
