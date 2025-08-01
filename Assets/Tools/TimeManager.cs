using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class TimeItem
{
    public int idx;
    public Action<float> action;
    public float time;
    public float curTime;
}
public class TimeManager : SingletonBehaviour<TimeManager>
{
    private List<TimeItem> items = new List<TimeItem>();
    private int idx;
    private 
    void Start()
    {
        idx = 0;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (TimeItem item in items )
        {
            if (item.curTime >= item.time)
            {
                item.action(Time.deltaTime);
                item.curTime = 0;
            }
            else
            {
                item.curTime = item.curTime + Time.deltaTime;
            }
        }
    }

    public int AddAction(Action<float> action,float time)
    {
        idx++;
        TimeItem item = new TimeItem();
        item.idx = idx;
        item.action = action;
        item.time = time;
        items.Add(item);
        return idx;
    }
}
