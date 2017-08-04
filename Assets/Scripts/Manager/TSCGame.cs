using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSCGame : MonoBehaviour
{
    void Update()
    {
        ProfilerSample.BeginSample("TSCGame");
        Timer.Instance.Update(Time.deltaTime);
        //AnimatorMgr.Instance.Update();
        ItemDropMgr.Instance.Update(Time.deltaTime);
        ProfilerSample.BeginSample("TSCGame");
    }


    private void OnDestroy()
    {
        Timer.Instance.OnDestroy();
    }
}
