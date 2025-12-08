using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using EquipNs;
using GoodsNs;
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
    public List<EquipmentInfo> MeleeWeapon1HList;
    public List<EquipmentInfo> ArmorList;
    public List<EquipmentInfo> HelmetList;
    public List<EquipmentInfo> EquipmentList;

    public List<GoodData> PotionList;
    public List<GoodData> BowList;
    public List<GoodData> FoodList;
    public List<GoodData> GoodList;

    //增加装备
    public void AddEquip(int id, int num = 1)
    {
        var equipInfo = Ui.Instance.GetEquipInfo(id);
        SpriteGroupEntry data = Ui.Instance.GetEquipEntry((EquipmentPart) equipInfo.part, equipInfo.id);
        List<EquipmentInfo> tempList = new List<EquipmentInfo>();
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

        var info = tempList.Find((info) => info.id == id);
        if (info != null)
        {
            info.num += num;
        }
        else
        {
            tempList.Add(new EquipmentInfo(data, id, num));
        }

        var equipmentInfo = EquipmentList.Find((info) => info.id == id);
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
        EquipInfo equipInfo = Ui.Instance.GetEquipInfo(id);
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
        GoodInfo goodInfo = Ui.Instance.GetGoodInfo(id);
        List<GoodData> goodList = GetGoodListData((GoodsCategory) goodInfo.type);
        GoodData goodData = goodList.Find((obj) => obj.id == id);
        if (goodData != null)
        {
            goodData.num += 1;
            GoodList.Find((obj) => obj.id == id).num += 1;
            return;
        }

        goodList.Add(new GoodData(id, num));
        GoodList.Add(new GoodData(id, num));
    }

    //移除道具
    public void RemoveGood(int id, int num, int type)
    {
        GoodData goodInfo = GoodList.Find((obj) => obj.id == id);
        if (goodInfo.num - num <= 0)
        {
            GoodList.Remove(goodInfo);
            return;
        }

        goodInfo.num -= num;
    }

    //获取道具列表数据
    public List<GoodData> GetGoodListData(GoodsCategory type)
    {
        List<GoodData> goodList;
        switch (type)
        {
            case GoodsCategory.Potion:
                goodList = PotionList;
                break;
            case GoodsCategory.Bow:
                goodList = BowList;
                break;
            case GoodsCategory.Food:
                goodList = FoodList;
                break;
            default:
                goodList = PotionList;
                Debug.Log("这个部位未实现");
                break;
        }

        return goodList;
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

    public override void Create()
    {
        MeleeWeapon1HList = new List<EquipmentInfo>();
        EquipmentList = new List<EquipmentInfo>();
        ArmorList = new List<EquipmentInfo>();
        GoodList = new List<GoodData>();
        PotionList = new List<GoodData>();
        BowList = new List<GoodData>();
        FoodList = new List<GoodData>();
    }

    public override void Clear()
    {
        Create();
    }
}

