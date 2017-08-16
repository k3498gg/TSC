using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Timer : Singleton<Timer>
{
    public class TimerData
    {
        public float time = 0;      //间隔时间执行
        public bool ingoreTimeScale = false;
        public int invokeTimes = 1;         // 0 = over , > 0 = times, < 0 = forever
        public TimerHandler handler = null;
        private string id = string.Empty;
        public bool isInitialized = false;
        private float intervalTime;
        public int data = 0;

        private void Initialize()
        {
            intervalTime = time;
            isInitialized = true;
        }

        public string ID
        {
            get
            {
                return id;
            }
        }

        public TimerData()
        {
            id = Guid.NewGuid().ToString();
        }

        public void Invoke(float delatTime, float ingoreTime)
        {
            if (!isInitialized)
            {
                Initialize();
            }
            if (invokeTimes != 0)
            {
                if (ingoreTimeScale)
                {
                    this.time -= ingoreTime;
                }
                else
                {
                    this.time -= delatTime;
                }

                if (this.time <= 0)
                {
                    if (null != handler)
                    {
                        handler(this);
                    }

                    --invokeTimes;

                    if (invokeTimes != 0)
                    {
                        this.time = intervalTime;
                    }
                    else
                    {
                        Timer.Instance.Delete(this);
                    }
                }
            }
        }
    }
    public delegate void TimerHandler(TimerData data);
    private float realtimeSinceStartup = 0;
    private List<TimerData> mList = new List<TimerData>();
    private List<TimerData> mUnused = new List<TimerData>();
    private int poolCount = 5; //缓存池TimerData的数量

    TimerData Create()
    {
        if (mUnused.Count > 0)
        {
            int idx = mUnused.Count - 1;
            TimerData data = mUnused[idx];
            mUnused.RemoveAt(idx);
            mList.Add(data);
            return data;
        }
        TimerData td = new TimerData();
        mList.Add(td);
        return td;
    }

    void Delete(TimerData data)
    {
        if (null == data)
        {
            Debuger.LogError("TimerData初始化失败");
            return;
        }
        data.isInitialized = false;
        data.data = 0;
        data.handler = null;
        mList.Remove(data);
        mUnused.Add(data);


        if (mUnused.Count > poolCount)
        {
            mUnused.RemoveAt(0);
            data = null;
        }
    }

    public TimerData AddTimer(float time, int invokeTimes, bool ingoreTimeScale, TimerHandler handler,int data)
    {
        TimerData timerData = AddTimer(time, invokeTimes, ingoreTimeScale, handler);
        timerData.data = data;
        return timerData;
    }

    public TimerData AddTimer(float time, int invokeTimes, bool ingoreTimeScale, TimerHandler handler)
    {
        TimerData data = Create();
        data.time = time;
        data.invokeTimes = invokeTimes;
        data.ingoreTimeScale = ingoreTimeScale;
        data.handler = handler;
        return data;
    }

    public void RemoveTimer(string id)
    {
        TimerData td = mList.Find(delegate (TimerData temp) { return temp.ID.Equals(id); });
        if (null != td)
        {
            RemoveTimer(td);
        }
    }

    //慎用此方法(当仅有一个此代理在调用的时候才能使用移除，否则移除错误的代理)
    public void RemoveTimer(TimerHandler handler)
    {
        TimerData td = mList.Find(delegate (TimerData temp) { return temp.handler == handler; });
        if (null != td)
        {
            RemoveTimer(td);
        }
    }

    public void RemoveTimer(TimerData data)
    {
        Delete(data);
    }

    public void Update(float deltaTime)
    {
        float ingoreTime = GetIngoreTime();
        for (int i = 0; i < mList.Count; i++)
        {
            mList[i].Invoke(deltaTime, ingoreTime);
        }
    }


    float GetIngoreTime()
    {
        float time = Time.realtimeSinceStartup - realtimeSinceStartup;
        realtimeSinceStartup = Time.realtimeSinceStartup;
        return time;
    }


    public void OnDestroy()
    {
        mList.Clear();
        mUnused.Clear();
        GC.Collect();
    }
}
