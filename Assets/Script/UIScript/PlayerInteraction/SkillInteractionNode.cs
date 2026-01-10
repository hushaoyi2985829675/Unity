using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CommonScripts;
using SkillNs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = System.Object;

public class SkillInteractionNode : PanelBase
{
    [SerializeField]
    private Image skillImg;

    [SerializeField]
    private Image lockImg;

    [SerializeField]
    private EventTrigger skillTrigger;

    private int skillId;
    public void Awake()
    {
        skillTrigger = GetComponent<EventTrigger>();
        Tool.BindTrigger(skillTrigger, EventTriggerType.PointerDown, (baseEventData) =>
        {
            OnClick();
        });
    }

    public override void onEnter(params object[] data)
    {
        
    }

    public override void onShow(params object[] data)
    {
        int id = (int) data[0];
        RefreshUI(id);
    }

    public void RefreshUI(int id)
    {
        skillId = id;
        lockImg.SetActive(id == 0);
        skillImg.SetActive(id != 0);
        if (id == 0)
        {
            return;
        }

        Sprite icon = Ui.Instance.GetGoodIcon((int) GoodsType.Skill, id);
        skillImg.sprite = icon;
    }

    private void OnClick()
    {
        if (skillId == 0)
        {
            return;
        }

        EventManager.Instance.PostEvent(GameEventType.PlayerSkillEvent, new Object[] {skillId});
    }

    public override void onExit()
    {
    }
}