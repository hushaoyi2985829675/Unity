using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using Goods;
using UnityEngine;

[CreateAssetMenu(fileName = "BagData", menuName = "GameData/BagData")]
public class BagData : ScriptableBase
{
    public List<EquipmentInfo> WeaponList;
    public List<EquipmentInfo> ArmorList;
    public List<EquipmentInfo> HelmetList;
    public List<EquipmentInfo> EquipmentList;
    public List<GoodData> GoodList;

    public void AddEquip(SpriteGroupEntry data, int id)
    {
        List<EquipmentInfo> tempList = new List<EquipmentInfo>();
        Equip.EquipInfo equipInfo = Ui.Instance.GetEquipInfo(id);
        switch ((EquipmentPart) equipInfo.part)
        {
            case EquipmentPart.MeleeWeapon1H:
            case EquipmentPart.MeleeWeapon2H:
                tempList = WeaponList;
                break;
            case EquipmentPart.Armor:
                tempList = ArmorList;
                break;
            case EquipmentPart.Helmet:
                tempList = HelmetList;
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

        var equipmentInfo = EquipmentList.Find((info) => info.SpriteGroupEntry.Id == data.Id);
        if (equipmentInfo != null)
        {
            equipmentInfo.num += 1;
        }
        else
        {
            EquipmentList.Add(new EquipmentInfo(data, id));
        }
    }

    //增加道具
    public void AddGood(int id, int num)
    {
        GoodData goodInfo = GoodList.Find((obj) => obj.id == id);
        if (goodInfo != null)
        {
            goodInfo.num += 1;
            return;
        }

        GoodList.Add(new GoodData(id, num));
    }

    //移除道具
    public void RemoveGood(int id, int num)
    {
        GoodData goodInfo = GoodList.Find((obj) => obj.id == id);
        if (goodInfo.num - num <= 0)
        {
            GoodList.Remove(goodInfo);
            return;
        }

        goodInfo.num -= num;
    }

    public override void Clear()
    {
        WeaponList = new List<EquipmentInfo>();
        EquipmentList = new List<EquipmentInfo>();
        ArmorList = new List<EquipmentInfo>();
        GoodList = new List<GoodData>();
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

[System.Serializable]
public class GoodData
{
    public int num;
    public int id;

    public GoodData(int id, int num)
    {
        this.id = id;
        this.num = num;
    }
}