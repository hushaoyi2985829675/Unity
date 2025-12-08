using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using EquipNs;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerEquipData", menuName = "GameData/PlayerEquipData")]
public class PlayerEquipData : ScriptableBase
{
    [SerializeField] private EquipData Weapon;
    [SerializeField] private EquipData Armor;
    [SerializeField] private EquipData Helmet;

    public void SetPlayerEquipData(int id)
    {
        EquipInfo equipInfo = Ui.Instance.GetEquipInfo(id);
        EquipData equipData = GetPlayerEquipData((EquipmentPart) equipInfo.part);
        equipData.id = id;
        equipData.name = equipInfo.name;
    }

    public EquipData GetPlayerEquipData(EquipmentPart part)
    {
        EquipData equipData = new EquipData("空", -1);
        switch (part)
        {
            
            case EquipmentPart.Armor:
                equipData = Armor;
                break;
            case EquipmentPart.Helmet:
                equipData = Helmet;
                break;
            case EquipmentPart.MeleeWeapon1H:
                equipData = Weapon;
                break;
        }

        return equipData;
    }

    public void RemovePlayerEquipData(EquipmentPart part)
    {
        EquipData equipData = GetPlayerEquipData(part);
        equipData.id = -1;
        equipData.name = "";
    }

    public override void Create()
    {
        Weapon = new EquipData("武器", -1);
        Armor = new EquipData("铠甲", 14);
        Helmet = new EquipData("头盔", -1);
    }

    public override void Clear()
    {
    }
}

[System.Serializable]
public class EquipData
{
    public string name;
    public int id;

    public EquipData(string name, int id)
    {
        this.name = name;
        this.id = id;
    }
}