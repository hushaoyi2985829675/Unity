using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerEquipData", menuName = "GameData/PlayerEquipData")]
public class PlayerEquipData : ScriptableBase
{
    public List<EquipInfo> EquipInfo = new List<EquipInfo>()
    {
        new EquipInfo("Armor"),
        new EquipInfo("Weapon")
    };

    public void SaveEquip(EquipInfo data)
    {
        var equipInfo = EquipInfo.Find(info => info.name == data.name);
        equipInfo.SpriteGroupEntry = data.SpriteGroupEntry;
    }

    public override void Clear()
    {
        EquipInfo = new List<EquipInfo>()
        {
            new EquipInfo("Armor"),
            new EquipInfo("Weapon")
        };
    }
}

[System.Serializable]
public class EquipInfo
{
    public string name;
    public SpriteGroupEntry SpriteGroupEntry;
    public EquipmentPart Part;

    public EquipInfo(string name)
    {
        this.name = name;
    }
}