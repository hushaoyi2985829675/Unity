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
    PlayerMoveEvent,
    PlayerAttackEvent,
    PlayerJumpEvent,
    PlayerSkillEvent,
    PlayerSkillUpdateEvent,
    StairsEvent,
}

class EventBase
{
    private int id;
}

public class EventManager : Singleton<EventManager>
{
    private int id;

    private Stack<int> idStack;
    //资源更新事件
    private Dictionary<int, Dictionary<int, Action>> resAction;

    //玩家装备更新事件
    private Dictionary<int, Action<EquipmentPart>> wearEquipAction;

    //血量经验等级事件
    private Action playerStatusAction;

    //弹窗事件
    private Action<string> showFlutterAction;

    //移动事件
    private Action<int> PlayerMovekEventAction;

    //攻击事件
    private Action PlayerAttackEventAction;

    //跳跃事件
    private Action PlayerJumpEventAction;

    //技能事件
    private Action<int> playerSkillAction;

    //技能更新事件
    private Action<int> playerSkillUpdateAction;

    //楼梯模式事件
    private Action<bool> stairsAction;
    private void Awake()
    {
        id = 1;
        idStack = new Stack<int>();
        resAction = new Dictionary<int, Dictionary<int, Action>>();
        wearEquipAction = new Dictionary<int, Action<EquipmentPart>>();
    }

    //添加事件
    public int AddEvent(GameEventType eventType, object[] data)
    {
        int eventId = GetEventId();
        switch (eventType)
        {
            case GameEventType.ResEvent:
                int id = (int) data[0];
                if (!resAction.ContainsKey(id))
                {
                    resAction[id] = new Dictionary<int, Action>();
                }

                if (resAction[id].ContainsKey(eventId))
                {
                    resAction[id][eventId] += (Action) data[1];
                }
                else
                {
                    resAction[id].Add(eventId, (Action) data[1]);
                }

                break;
            case GameEventType.WearEquipEvent:
                wearEquipAction[eventId] = (Action<EquipmentPart>) data[0];
                break;
            case GameEventType.ShowFlutterEvent:
                break;
            case GameEventType.PlayerUIStateEvent:
                playerStatusAction = (Action) data[0];
                break;
            case GameEventType.PlayerMoveEvent:
                PlayerMovekEventAction = (Action<int>) data[0];
                break;
            case GameEventType.PlayerAttackEvent:
                PlayerAttackEventAction = (Action) data[0];
                break;
            case GameEventType.PlayerJumpEvent:
                PlayerJumpEventAction = (Action) data[0];
                break;
            case GameEventType.PlayerSkillEvent:
                playerSkillAction = (Action<int>) data[0];
                break;
            case GameEventType.PlayerSkillUpdateEvent:
                playerSkillUpdateAction = (Action<int>) data[0];
                break;
            case GameEventType.StairsEvent:
                stairsAction = (Action<bool>) data[0];
                break;
        }

        return eventId;
    }

    private int GetEventId()
    {
        if (idStack.Count > 0)
        {
            return idStack.Pop();
        }

        id++;
        return id - 1;
    }

    //移除资源更新事件
    public void RemoveEvent(GameEventType eventType, int eventId, object[] data = null)
    {
        switch (eventType)
        {
            case GameEventType.ResEvent:
                int id = (int) data[0];
                if (resAction.ContainsKey(id))
                {
                    if (resAction[id].ContainsKey(eventId))
                    {
                        resAction[id].Remove(eventId);
                    }
                }

                break;
            case GameEventType.WearEquipEvent:
                wearEquipAction.Remove(eventId);
                break;
            case GameEventType.ShowFlutterEvent:
                break;
            case GameEventType.PlayerUIStateEvent:
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
                if (resAction.ContainsKey(id))
                {
                    foreach (var resPair in resAction[id])
                    {
                        resPair.Value.Invoke();
                    }
                }
                break;
            case GameEventType.WearEquipEvent:
                foreach (var wearEquipPair in wearEquipAction)
                {
                    wearEquipPair.Value.Invoke((EquipmentPart) data[0]);
                }
                break;
            case GameEventType.ShowFlutterEvent:
                showFlutterAction?.Invoke((string) data[0]);
                break;
            case GameEventType.PlayerUIStateEvent:
                playerStatusAction?.Invoke();
                break;
            case GameEventType.PlayerMoveEvent:
                PlayerMovekEventAction?.Invoke((int) data[0]);
                break;
            case GameEventType.PlayerAttackEvent:
                PlayerAttackEventAction?.Invoke();
                break;
            case GameEventType.PlayerJumpEvent:
                PlayerJumpEventAction?.Invoke();
                break;
            case GameEventType.PlayerSkillEvent:
                playerSkillAction.Invoke((int) data[0]);
                break;
            case GameEventType.PlayerSkillUpdateEvent:
                playerSkillUpdateAction.Invoke((int) data[0]);
                break;
            case GameEventType.StairsEvent:
                stairsAction.Invoke((bool) data[0]);
                break;
        }
    }
    
}