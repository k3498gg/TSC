using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMgr : Singleton<ObstacleMgr>
{
    public void InitMapObstacle()
    {
        List<Obstacle> obstaces = TSCData.Instance.GetCurObstacleInfo(GameMgr.Instance.MapId);
        if (null != obstaces)
        {
            for (int i = 0; i < obstaces.Count; i++)
            {
                InistantObstacle(obstaces[i]);
            }
        }
    }

    void InistantObstacle(Obstacle obstacle)
    {
        float PosX = obstacle.PosX;
        float PosY = obstacle.PosY;
        GameObject go = Spawner((int)obstacle.ObsType, new Vector3(obstacle.PosX, obstacle.Poshh, obstacle.PosY), ResourceType.RESOURCE_OBSTACLE, GameMgr.Instance.ObstacleRoot);
        Transform temp = go.transform;
        temp.localScale = new Vector3(obstacle.Width, obstacle.Hh, obstacle.Height);
        temp.localRotation = Quaternion.Euler(0, obstacle.Rotate, 0);

        ObstacleEntity entity = Util.AddComponent<ObstacleEntity>(go);
        entity.Index = obstacle.Id;
        entity.Width = obstacle.Width;
        entity.Height = obstacle.Height;
        entity.Obs_type = obstacle.ObsType;
        TSCData.Instance.ObstacleDic[entity.Index] = entity;
    }

    GameObject Spawner(int id, Vector3 v, ResourceType t, Transform parent)
    {
        ObstacleInfo info = InfoMgr<ObstacleInfo>.Instance.GetInfo(id);
        return ResourcesMgr.Instance.Spawner(info.model, v, t, parent);
    }

    public void Despawner(ResourceType t, ObstacleEntity obs)
    {
        if (null == obs)
        {
            return;
        }
        TSCData.Instance.ObstacleDic.Remove(obs.Index);
        PoolMgr.Instance.Despawner(t, obs.Cache);
    }

    public void Clear()
    {
        TSCData.Instance.ObstacleDic.Clear();
        PoolMgr.Instance.Despawner(ResourceType.RESOURCE_OBSTACLE);
    }

}
