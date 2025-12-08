using System;
using System.Collections;
using System.Collections.Generic;
using HeroEditor.Common.Enums;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : PanelBase
{
    [SerializeField] public EquipmentPart part;
    [SerializeField] private Image icon;
    [SerializeField] private Image defluatIcon;
    private EquipData equipInfo;
    private Action callback;

    public override void onEnter(params object[] data)
    {
        part = (EquipmentPart) data[0];
        callback = (Action) data[1];
        GetComponent<Button>().onClick.AddListener(RemoveEquipClick);
    }

    public override void onShow(params object[] data)
    {
        RefreshEquip();
    }

    public override void onExit()
    {
    }

    private void RemoveEquipClick()
    {
        GameDataManager.Instance.RemovePlayerEquipData(part);
        RefreshEquip();
        EventManager.Instance.PostEvent(GameEventType.WearEquipEvent, new object[] {part});
        callback();
    }

    public void RefreshEquip()
    {
        defluatIcon.gameObject.SetActive(false);
        icon.gameObject.SetActive(false);
        equipInfo = GameDataManager.Instance.GetPlayerEquipData(part);
        if (equipInfo.id != -1)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = Ui.Instance.GetGoodIcon((int) GoodsType.Equip, equipInfo.id);
        }
        else
        {
            defluatIcon.gameObject.SetActive(true);
        }
    }
}