using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using Goods;
using UnityEngine;

[System.Serializable]
public class EquipmentInfo
{
    public SpriteGroupEntry SpriteGroupEntry;
    public int id;
    public int num;

    public EquipmentInfo()
    {
    }

    public EquipmentInfo(SpriteGroupEntry data, int id, int num)
    {
        SpriteGroupEntry = data;
        this.num = num;
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

[CreateAssetMenu(fileName = "BagData", menuName = "GameData/BagData")]
public class BagData : ScriptableBase
{
    public List<EquipmentInfo> MeleeWeapon1HList = new List<EquipmentInfo>();
    public List<EquipmentInfo> ArmorList = new List<EquipmentInfo>();
    public List<EquipmentInfo> HelmetList = new List<EquipmentInfo>();
    public List<EquipmentInfo> EquipmentList = new List<EquipmentInfo>();
    public List<GoodData> GoodList = new List<GoodData>();

    //增加装备
    public void AddEquip(SpriteGroupEntry data, int id, int num = 1)
    {
        List<EquipmentInfo> tempList = new List<EquipmentInfo>();
        Equip.EquipInfo equipInfo = Ui.Instance.GetEquipInfo(id);
        switch ((EquipmentPart) equipInfo.part)
        {
            case EquipmentPart.MeleeWeapon1H:
            case EquipmentPart.MeleeWeapon2H:
                tempList = MeleeWeapon1HList;
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
            info.num += num;
        }
        else
        {
            tempList.Add(new EquipmentInfo(data, id, num));
        }
        
        var equipmentInfo = EquipmentList.Find((info) => info.SpriteGroupEntry.Id == data.Id);
        if (equipmentInfo != null)
        {
            equipmentInfo.num += num;
        }
        else
        {
            EquipmentList.Add(new EquipmentInfo(data, id, num));
        }
    }

    //移除装备
    public void RemoveEquip(int id, int num = 1)
    {
        Equip.EquipInfo equipInfo = Ui.Instance.GetEquipInfo(id);
        List<EquipmentInfo> equipList = GetEquipListData((EquipmentPart) equipInfo.part);
        EquipmentInfo equipData = equipList.Find((obj) => obj.id == id);
        if (equipData is null)
        {
            Debug.Log(string.Format("背包装备没有id为{0}", id));
            return;
        }

        equipData.num -= num;
        if (equipData.num <= 0)
        {
            equipList.Remove(equipData);
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

    //获取装备列表数据
    public List<EquipmentInfo> GetEquipListData(EquipmentPart part)
    {
        List<EquipmentInfo> equipList;
        switch (part)
        {
            case EquipmentPart.MeleeWeapon1H:
                equipList = MeleeWeapon1HList;
                break;
            case EquipmentPart.Armor:
                equipList = ArmorList;
                break;
            case EquipmentPart.Helmet:
                equipList = HelmetList;
                break;
            default:
                equipList = MeleeWeapon1HList;
                Debug.Log("这个部位未实现");
                break;
        }

        return equipList;
    }
    
    public override void Clear()
    {
        MeleeWeapon1HList = new List<EquipmentInfo>();
        EquipmentList = new List<EquipmentInfo>();
        ArmorList = new List<EquipmentInfo>();
        GoodList = new List<GoodData>();
    }
}

