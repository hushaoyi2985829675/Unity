using System;
using System.Collections;
using System.Collections.Generic;
using HeroEditor.Common.Enums;
using Unity.VisualScripting;
using UnityEngine;

class EventBase
{
    private int id;
}

public class EventManager : Singleton<EventManager>
{
    //资源更新事件
    private Dictionary<int, Action> resEvent;

    //玩家装备更新事件
    private Action<EquipmentPart> wearEquipAction;

    //弹窗世间
    private Action<string> showFlutterAction;
    private void Awake()
    {
        resEvent = new Dictionary<int, Action>();
    }

    //添加资源更新事件
    public void AddResEvent(int id, Action action)
    {
        resEvent[id] = action;
    }

    //调用资源移除事件
    public void PostResEvent(int id)
    {
        resEvent[id]?.Invoke();
    }

    //添加弹窗事件
    public void AddShowFlutterAction(Action<string> wearAction)
    {
        showFlutterAction += wearAction;
    }

    //调用装备更新事件
    public void PostshowFlutterAction(string text)
    {
        showFlutterAction.Invoke(text);
    }

    //添加装备更新事件
    public void AddWearEquipAction(Action<EquipmentPart> wearAction)
    {
        wearEquipAction += wearAction;
    }

    //调用装备更新事件
    public void PostWearEquipAction(EquipmentPart part)
    {
        wearEquipAction.Invoke(part);
    }
}