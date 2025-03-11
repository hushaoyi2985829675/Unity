using HeroEditor.Common.Enums;
using HeroEditor.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.HeroEditor.Common.CharacterScripts;

public partial class Player
{
    [Header("玩家当前装备数据")]
    public PlayerEquipData PlayerEquipData;
    public void CheckoutEquip(SpriteGroupEntry item, EquipmentPart part)
    {
        GetComponent<Character>().Equip(item, part);
        EquipInfo data;
        switch (part)
        {   
            case EquipmentPart.Armor:
                data = PlayerEquipData.EquipInfo.Find(data => data.name == "Armor");
                data.SpriteGroupEntry = item;
                data.Part = part;
                data.name = "Armor";
                break;
            case EquipmentPart.MeleeWeapon1H:
                data = PlayerEquipData.EquipInfo.Find(data => data.name == "Weapon");
                data.SpriteGroupEntry = item;
                data.Part = part;
                data.name = "Weapon";
                break;
            case EquipmentPart.MeleeWeapon2H:
                data = PlayerEquipData.EquipInfo.Find(data => data.name == "Weapon");
                data.SpriteGroupEntry = item;
                data.Part = part;
                data.name = "Weapon";
                break;
            default: data = null;
                break;
        }
        PlayerEquipData.SaveEquip(data);
    }
    public void UpdateEquip()
    {
        foreach (var info in PlayerEquipData.EquipInfo)
        {
            if (info.SpriteGroupEntry != null && info.SpriteGroupEntry.Id != null && info.SpriteGroupEntry.Id != "")
            {
                GetComponent<Character>().Equip(info.SpriteGroupEntry, info.Part);
            }
        }
    }
}
