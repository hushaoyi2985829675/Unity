using System;
using System.Collections;
using System.Collections.Generic;
using HeroEditor.Common.Enums;
using Unity.VisualScripting;
using UnityEngine;

public enum GameEventType
{
    ResEvent,
    WearEquipEvent,
    ShowFlutterEvent,
    PlayerUIStateEvent,
}

class EventBase
{
    private int id;
}

public class EventManager : Singleton<EventManager>
{
    //资源更新事件
    private Dictionary<int, Action> resAction;

    //玩家装备更新事件
    private Action<EquipmentPart> wearEquipAction;

    //血量经验等级事件
    private Action playerStatusAction;

    //弹窗事件
    private Action<string> showFlutterAction;
    private void Awake()
    {
        resAction = new Dictionary<int, Action>();
    }

    //添加资源更新事件
    public void AddEvent(GameEventType eventType, object[] data)
    {
        switch (eventType)
        {
            case GameEventType.ResEvent:
                int id = (int) data[0];
                if (resAction.ContainsKey(id))
                {
                    resAction[id] += (Action) data[1];
                }
                else
                {
                    resAction.Add(id, (Action) data[1]);
                }

                break;
            case GameEventType.WearEquipEvent:
                wearEquipAction += (Action<EquipmentPart>) data[0];
                break;
            case GameEventType.ShowFlutterEvent:
                break;
            case GameEventType.PlayerUIStateEvent:
                playerStatusAction += (Action) data[0];
                break;
        }
    }

    //调用事件
    public void PostEvent(GameEventType eventType, object[] data = null)
    {
        switch (eventType)
        {
            case GameEventType.ResEvent:
                int id = (int) data[0];
                resAction[id]?.Invoke();
                break;
            case GameEventType.WearEquipEvent:
                wearEquipAction?.Invoke((EquipmentPart) data[0]);
                break;
            case GameEventType.ShowFlutterEvent:
                showFlutterAction?.Invoke((string) data[0]);
                break;
            case GameEventType.PlayerUIStateEvent:
                playerStatusAction?.Invoke();
                break;
        }
    }
}