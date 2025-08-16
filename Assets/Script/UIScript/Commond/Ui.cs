using System;
using System.Collections;
using System.Collections.Generic;
using Goods;
using UnityEngine;
using HeroEditor.Common;
using HeroEditor.Common.Enums;

enum GoodsType
{
    Equip = 1,
    Good = 2,
}

enum GoodType
{
}

public class Ui : MonoBehaviour
{
    private static Ui _instance;
    private List<GoodTypeInfo> GoodsConfig;
    private List<GoodInfo> equipGoodList;
    private List<GoodInfo> goodGoodList;
    private Dictionary<int, GoodInfo> goodCacheList;
    private Dictionary<int, SpriteGroupEntry> equipCacheList;
    public static Ui Instance
    {
        get
        {
            if ( _instance == null)
            {
                _instance = FindObjectOfType<Ui>();
            }          
            return _instance;
        }
    }
    private GameObject flutteViewRef;
    void Start()
    {
        flutteViewRef = Resources.Load<GameObject>("Ref/LayerRef/UIRef/FlutterWindowsLayer");
        GoodsConfig = Resources.Load<GoodsConfig>("Configs/Data/GoodsConfig").goodTypeInfoList;
        equipCacheList = new Dictionary<int, SpriteGroupEntry>();
        equipGoodList = GoodsConfig.Find((obj) => obj.goodType == (int) GoodsType.Equip)?.goodInfoList;
        goodGoodList = GoodsConfig.Find((obj) => obj.goodType == (int) GoodsType.Good)?.goodInfoList;
        goodCacheList = new Dictionary<int, GoodInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowFlutterView(string text)
    {
        UIManager.Instance.AddUINode(flutteViewRef, new Vector2(0, 0), new object[] {text});
    }

    public GoodInfo GetGoodInfo(int type, int id)
    {
        GoodInfo goodInfo;
        if (goodCacheList.TryGetValue(id, out goodInfo))
        {
            return goodInfo;
        }
        else
        {
            //创建
            List<GoodInfo> goodInfoList;
            if (type == (int) GoodsType.Equip)
            {
                goodInfoList = equipGoodList;
            }
            else
            {
                goodInfoList = goodGoodList;
            }

            goodInfo = goodInfoList.Find((obj) => obj.good == id);
            if (goodInfo == null)
            {
                Debug.Log("类型" + type + "的道具" + id + "不存在");
            }

            goodCacheList[id] = goodInfo;
            return goodInfo;
        }
    }

    public string GetMonsterName(int type)
    {
        switch ((MonsterType) type)
        {
            case MonsterType.Wolf:
                return "狼";
            default:
                return "普通小兵";
        }
    }

    public string GetGoodClassifier(int type, int id)
    {
        GoodInfo goodInfo = GetGoodInfo(type, id);
        if (type == (int) GoodsType.Equip)
        {
            switch ((EquipmentPart) goodInfo.type)
            {
                case EquipmentPart.MeleeWeapon2H:
                case EquipmentPart.MeleeWeaponPaired:
                case EquipmentPart.Armor:
                    return "套";
                case EquipmentPart.Helmet:
                case EquipmentPart.Belt:
                case EquipmentPart.Shield:
                case EquipmentPart.Glasses:
                case EquipmentPart.Back:
                case EquipmentPart.Mask:
                    return "个";
                case EquipmentPart.Vest:
                case EquipmentPart.Gloves:
                case EquipmentPart.Pauldrons:
                case EquipmentPart.Earrings:
                    return "副";
                case EquipmentPart.Boots:
                    return "双";
                case EquipmentPart.MeleeWeapon1H:
                case EquipmentPart.Bow:
                case EquipmentPart.Firearm1H:
                case EquipmentPart.Firearm2H:
                    return "把";
                case EquipmentPart.Cape:
                    return "件";
                default:
                    return "个";
            }
        }
        else
        {
            switch ((GoodType) goodInfo.type)
            {
            }
        }

        return "";
    }

    //根据任务类型和id获取名字
    public string GetTaskTargetName(int taskType, int id)
    {
        if (taskType == (int) TaskType.Kill)
        {
            return Ui.Instance.GetMonsterName(id);
        }
        else
        {
            return GoodsConfig.Find((obj) => obj.goodType == (int) GoodsType.Good).goodInfoList
                .Find((obj) => obj.good == id)?.name;
        }
    }

    public void RemoveAllChildren(Transform parent)
    {
        // 从最后一个子节点开始删除，避免索引变化导致漏删
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            // 销毁子节点（会从父节点中移除）
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}
