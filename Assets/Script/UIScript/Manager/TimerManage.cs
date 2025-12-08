using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class TimeInfo
{
    Action callback;
    private float callTime;
    private float curTime;
    public int id;
    private static Stack<int> idStack = new Stack<int>();
    private static int idInx;

    public TimeInfo(Action callback, float callTime)
    {
        this.callback = callback;
        this.callTime = callTime;
        curTime = 0;
        if (idStack.Count == 0)
        {
            idInx++;
            id = idInx;
        }
        else
        {
            id = idStack.Pop();
        }
    }

    public bool AddTime()
    {
        curTime += Time.deltaTime;
        if (curTime >= callTime)
        {
            callback();
            return true;
        }

        return false;
    }

    public void ScheduleCallback()
    {
        curTime += Time.deltaTime;
        if (curTime >= callTime)
        {
            callback();
            curTime = 0;
        }
    }
}

public class TimerManage : Singleton<TimerManage>
{
    private Dictionary<int, TimeInfo> delayDict = new Dictionary<int, TimeInfo>();
    private Dictionary<int, TimeInfo> scheduleDict = new Dictionary<int, TimeInfo>();

    public void AddDelayCallback(Action callback, float delay)
    {
        TimeInfo timeInfo = new TimeInfo(callback, delay);
        delayDict.Add(timeInfo.id, timeInfo);
    }

    public void Update()
    {
        foreach (KeyValuePair<int, TimeInfo> keyValuePair in delayDict)
        {
            TimeInfo timeInfo = keyValuePair.Value;
            if (timeInfo.AddTime())
            {
                delayDict.Remove(timeInfo.id);
            }
        }

        foreach (KeyValuePair<int, TimeInfo> keyValuePair in scheduleDict)
        {
            TimeInfo timeInfo = keyValuePair.Value;
            timeInfo.ScheduleCallback();
        }
    }

    public int AddScheduleCallback(Action callback, float delay, bool isCall = true)
    {
        if (isCall)
        {
            callback();
        }

        TimeInfo timeInfo = new TimeInfo(callback, delay);
        scheduleDict.Add(timeInfo.id, timeInfo);
        return timeInfo.id;
    }

    public void RemoveScheduleCallback(int id)
    {
        scheduleDict.Remove(id);
    }
}