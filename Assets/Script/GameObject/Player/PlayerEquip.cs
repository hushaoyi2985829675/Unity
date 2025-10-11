using System;
using HeroEditor.Common.Enums;
using HeroEditor.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.FantasyInventory.Scripts.Interface.Elements;
using Equip;

public class PlayerEquip : MonoBehaviour
{
    Character character;

    private void Start()
    {
        character = GetComponent<Character>();
        RefreshEquip(EquipmentPart.MeleeWeapon1H);
        RefreshEquip(EquipmentPart.Helmet);
        RefreshEquip(EquipmentPart.Armor);
        EventManager.Instance.AddWearEquipAction(RefreshEquip);
    }

    private void RefreshEquip(EquipmentPart part)
    {
        EquipData equipData = GameDataManager.Instance.GetPlayerEquipData(part);
        if (equipData.id == -1)
        {
            character.Equip(null, part);
            return;
        }

        EquipInfo equipInfo = Ui.Instance.GetEquipInfo(equipData.id);
        SpriteGroupEntry spriteGroupEntry = Ui.Instance.GetEquipEntry(part, equipInfo.id);
        character.Equip(spriteGroupEntry, part);
    }
}
