using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BagData", menuName = "GameData/BagData")]
public class BagData : ScriptableBase
{
    public List<EquipmentInfo> Weapon;
    public List<EquipmentInfo> Armor;
    public List<EquipmentInfo> Helmet;
    public List<EquipmentInfo> Equipments;

    public void AddEquip(SpriteGroupEntry data, int id)
    {
        List<EquipmentInfo> tempList = new List<EquipmentInfo>();
        Equip.EquipInfo equipInfo = Ui.Instance.GetEquipInfo(id);
        switch ((EquipmentPart) equipInfo.part)
        {
            case EquipmentPart.MeleeWeapon1H:
            case EquipmentPart.MeleeWeapon2H:
                tempList = Weapon;
                break;
            case EquipmentPart.Armor:
                tempList = Armor;
                break;
            case EquipmentPart.Helmet:
                tempList = Helmet;
                break;
        }

        var info = tempList.Find((info) => info.SpriteGroupEntry.Id == data.Id);
        if (info != null)
        {
            info.num += 1;
        }
        else
        {
            tempList.Add(new EquipmentInfo(data, id));
        }

        var equipmentInfo = Equipments.Find((info) => info.SpriteGroupEntry.Id == data.Id);
        if (equipmentInfo != null)
        {
            equipmentInfo.num += 1;
        }
        else
        {
            Equipments.Add(new EquipmentInfo(data, id));
        }
    }

    public override void Clear()
    {
        Weapon = new List<EquipmentInfo>();
        Equipments = new List<EquipmentInfo>();
    }
}

[System.Serializable]
public class EquipmentInfo
{
    public SpriteGroupEntry SpriteGroupEntry;
    public int num;
    public int id;

    public EquipmentInfo()
    {
    }

    public EquipmentInfo(SpriteGroupEntry data, int id)
    {
        SpriteGroupEntry = data;
        num = 1;
        this.id = id;
    }
}