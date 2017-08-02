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
        ProfilerSample.BeginSample("TSCGame");
    }

}
