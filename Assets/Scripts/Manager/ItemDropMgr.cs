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
                    float offX = Random.Range(-points[idx].Width, points[idx].Width);
                    float offY = Random.Range(-points[idx].Height, points[idx].Height);
                    GameObject go = Spawner(poz, new Vector3(points[idx].PosX + offX, 0, points[idx].PosY + offY), ResourceType.RESOURCE_ITEM, GameMgr.Instance.ItemRoot);
                    DropItemInfo dropInfo = Util.AddComponent<DropItemInfo>(go);
                    dropInfo.ItemId = EntityMgr.Instance.DropItemDic.Count;
                    dropInfo.InfoId = poz;
                    dropInfo.Area = points[idx].Area;
                    dropInfo.DropAI = DropAI.LATER;
                    EntityMgr.Instance.DropItemDic[dropInfo.ItemId] = dropInfo;
                }
            }


            for (int idx = 0; idx < (int)MapArea.AREA_MAX; idx++)
            {
                List<ItemMapInfo> items = mapInfo.ItemMapInfo.FindAll(delegate (ItemMapInfo map) { return map.IMapType == ItemMapType.ITEM && map.Area == (MapArea)idx; });
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
                            float offX = Random.Range(-items[i].Width, items[i].Width);
                            float offY = Random.Range(-items[i].Height, items[i].Height);
                            GameObject go = Spawner((int)ItemType.ITEM_ENERGY, new Vector3(items[i].PosX + offX, 0, items[i].PosY + offY), ResourceType.RESOURCE_ITEM, GameMgr.Instance.ItemRoot);
                            DropItemInfo dropInfo = Util.AddComponent<DropItemInfo>(go);
                            dropInfo.ItemId = EntityMgr.Instance.DropItemDic.Count; 
                            dropInfo.InfoId = (int)ItemType.ITEM_ENERGY;
                            dropInfo.Area = items[i].Area;
                            dropInfo.DropAI = DropAI.LATER;
                            EntityMgr.Instance.DropItemDic[dropInfo.ItemId] = dropInfo;
                        }
                    }
                }
            }
        }
    }

    public GameObject Spawner(int id, Vector3 v, ResourceType t, Transform parent)
    {
        ItemInfo item = InfoMgr<ItemInfo>.Instance.GetInfo(id);
        return ResourcesMgr.Instance.Spawner(item.model, v, t, parent);
    }

    public void Update(float delateTime)
    {
        mTotalTime += delateTime;
        if(mTotalTime <= 30)
        {
            return;
        }

        mTotalTime = 0;

        //重新刷新一次道具

    }
}
