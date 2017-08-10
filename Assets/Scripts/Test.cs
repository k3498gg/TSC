using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


public class Test : MonoBehaviour
{
    public Animator animator;
    // Use this for initialization
    void Start()
    {
        //LoadData<AnimatorInfo>(@Application.streamingAssetsPath + "/" + AppConst.TextDir + "/");
        //CreateTimer();
        //AnimatorTrigger();
    }

    void AnimatorTrigger()
    {
        animator.SetTrigger("New Trigger");
    }

    void LoadData<T>(string path) where T : IInfo, new()
    {
        string file = path + typeof(T).ToString().ToLower() + ".bin";
        if (!File.Exists(file))
        {
            Debug.LogError("file not exits:" + file);
            return;
        }
        InfoMgr<T>.Instance.Init(file);
    }

    void Update()
    {
        //animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //Debug.LogError(animStateInfo.normalizedTime);
        //ProfilerSample.BeginSample("TimerUpdate");
        //Timer.Instance.Update(Time.deltaTime);
        //ProfilerSample.EndSample();
    }

    void CreateTimer()
    {
        Timer.Instance.AddTimer(3, 3, true, Handler);
    }

    void Handler(Timer.TimerData data)
    {
        Debug.LogError(data.ID + " " + data.invokeTimes);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.LogError(collision.transform.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("Trigger Enter " + other.transform.name);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.LogError("Trigger Exit " + other.transform.name);
    }

}
