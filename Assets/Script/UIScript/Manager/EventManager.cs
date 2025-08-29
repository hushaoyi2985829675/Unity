using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

class EventBase
{
    private int id;
}

public class EventManager : Singleton<EventManager>
{
    private Dictionary<int, Action> resEvent;

    private void Awake()
    {
        resEvent = new Dictionary<int, Action>();
    }

    public void AddResEvent(int id, Action action)
    {
        resEvent[id] = action;
    }

    public void PostResEvent(int id)
    {
        resEvent[id]?.Invoke();
    }
}