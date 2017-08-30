using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSCGame : MonoBehaviour
{
    void Update()
    {
        if(null == GameMgr.Instance)
        {
            return;
        }

        if(GameMgr.Instance.IsEnterGame)
        {
            ProfilerSample.BeginSample("TSCGame");
            Timer.Instance.Update(Time.deltaTime,Time.unscaledDeltaTime);
            TSCData.Instance.Update(Time.deltaTime);
            ItemDropMgr.Instance.Update(Time.deltaTime);
            ProfilerSample.BeginSample("TSCGame");
        }
    }
}
