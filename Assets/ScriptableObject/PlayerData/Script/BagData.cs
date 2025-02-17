using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GoodType
{ 
    Good,
    Equip
}
[CreateAssetMenu(fileName = "New Data", menuName = "Character Data/BagData")]
public class BagData : ScriptableObject,ScriptableBase
{
    public List<EquipmentInfo> MeleeWeapon1H;
    public List<EquipmentInfo> Equipments;
    public void AddEquip(SpriteGroupEntry data, EquipmentPart part)
    {      
        if (part == EquipmentPart.MeleeWeapon1H)
        {
            var info = MeleeWeapon1H.Find((info) => info.SpriteGroupEntry.Id == data.Id);
            if (info != null)
            {
                info.num += 1;
            }
            else
            {
                MeleeWeapon1H.Add(new EquipmentInfo(data, part));
            }
        }

        var equipmentInfo = Equipments.Find((info) => info.SpriteGroupEntry.Id == data.Id);
        if (equipmentInfo != null)
        {
            equipmentInfo.num += 1;
        }
        else
        {
            Equipments.Add(new EquipmentInfo(data,part));
        }
    }

    public void Clear()
    {
        MeleeWeapon1H = new List<EquipmentInfo>();
        Equipments = new List<EquipmentInfo>();
    }
}
[System.Serializable]
public class EquipmentInfo
{
    public SpriteGroupEntry SpriteGroupEntry;
    public int num;
    public EquipmentPart Part;
    public EquipmentInfo() { }
    public EquipmentInfo(SpriteGroupEntry data,EquipmentPart part) 
    {
        SpriteGroupEntry = data;
        num = 1;
        Part = part;
    }
} 
