using Assets.HeroEditor.Common.CommonScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HeroEditor.Common.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EquipSlotNode : MonoBehaviour
{
    public List<EquipSlot> itemList;
    private Dictionary<EquipmentPart, EquipSlot> equipDict;


    public void Awake()
    {
        equipDict = new Dictionary<EquipmentPart, EquipSlot>();
    }

    public void RefreshAllEquip()
    {
        foreach (EquipSlot equipSlot in itemList)
        {
            equipSlot.RefreshEquip();
            equipDict[equipSlot.part] = equipSlot;
        }
    }

    public void RefreshEquip(EquipmentPart part)
    {
        equipDict[part].RefreshEquip();
    }
}