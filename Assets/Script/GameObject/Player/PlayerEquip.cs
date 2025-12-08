using System;
using HeroEditor.Common.Enums;
using HeroEditor.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.FantasyInventory.Scripts.Interface.Elements;
using EquipNs;

public class PlayerEquip : MonoBehaviour
{
    Character character;
    Transform edge;
    public bool isUpdateEquip;

    private void Awake()
    {
        EventManager.Instance.AddEvent(GameEventType.WearEquipEvent, new object[] {(Action<EquipmentPart>) RefreshEquip});
        character = GetComponent<Character>();
        isUpdateEquip = true;
    }

    private void Start()
    {
        edge = Ui.Instance.GetChild(transform, "Edge").transform;
        if (isUpdateEquip)
        {
            RefreshEquip(EquipmentPart.MeleeWeapon1H);
            RefreshEquip(EquipmentPart.Helmet);
            RefreshEquip(EquipmentPart.Armor);
        }
    }

    private void RefreshEquip(EquipmentPart part)
    {
        EquipData equipData = GameDataManager.Instance.GetPlayerEquipData(part);
        if (part == EquipmentPart.MeleeWeapon1H)
        {
            RefreshEdge(equipData);
        }
        
        if (equipData.id == -1)
        {
            character.Equip(null, part);
            return;
        }

        EquipInfo equipInfo = Ui.Instance.GetEquipInfo(equipData.id);
        SpriteGroupEntry spriteGroupEntry = Ui.Instance.GetEquipEntry(part, equipInfo.id);
        character.Equip(spriteGroupEntry, part);
    }

    private void RefreshEdge(EquipData equipData)
    {
        if (equipData.id == -1)
        {
            edge.localPosition = new Vector3(1, edge.localPosition.y, edge.localPosition.z);
            return;
        }

        EquipInfo equipInfo = Ui.Instance.GetEquipInfo(equipData.id);
        edge.localPosition = new Vector3(equipInfo.attacDis, edge.localPosition.y, edge.localPosition.z);
    }

    public void RemoveAllEquip()
    {
        character.Equip(null, EquipmentPart.MeleeWeapon1H);
        character.Equip(null, EquipmentPart.Helmet);
        EquipInfo equipInfo = Ui.Instance.GetEquipInfo(14);
        SpriteGroupEntry spriteGroupEntry = Ui.Instance.GetEquipEntry(EquipmentPart.Armor, equipInfo.id);
        character.Equip(spriteGroupEntry, EquipmentPart.Armor);
    }
}
