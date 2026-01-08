using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CommonScripts;
using SkillNs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillInteractionNode : PanelBase
{
    [SerializeField]
    private Image skillImg;

    [SerializeField]
    private Image lockImg;

    private EventTrigger skillTrigger;

    public void Awake()
    {
        skillTrigger = GetComponent<EventTrigger>();
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
        lockImg.SetActive(id == 0);
        skillImg.SetActive(id != 0);
        if (id == 0)
        {
            return;
        }

        Sprite icon = Ui.Instance.GetGoodIcon((int) GoodsType.Skill, id);
        skillImg.sprite = icon;
    }

    public override void onExit()
    {
    }
}