using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Assets.HeroEditor.Common.CommonScripts;
using Equip;
using Goods;
using UnityEngine;
using HeroEditor.Common;
using HeroEditor.Common.Enums;
using GoodInfo = Goods.GoodInfo;

enum GoodType
{
}

public class ResClass
{
    public int resourcdId;
    public int num;
}

public class Ui : MonoBehaviour
{
    private static Ui _instance;
    private GoodsConfig GoodsConfig;
    private EquipConfig EquipConfig;
    private IconCollection IconCollection;
    private Dictionary<int, Equip.EquipInfo> equipGoodList;

    private Dictionary<int, GoodInfo> goodGoodList;

    //装备图标缓存
    private Dictionary<string, Sprite> equipIconList;
    private Dictionary<int, Sprite> goodIconList; 
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
        GoodsConfig = Resources.Load<GoodsConfig>("Configs/Data/GoodsConfig");
        EquipConfig = Resources.Load<EquipConfig>("Configs/Data/EquipConfig");
        IconCollection = Resources.Load<IconCollection>("Configs/Data/IconCollection");
        equipGoodList = EquipConfig.equipInfoList.ToDictionary(key => key.equip, value => value);
        goodGoodList = GoodsConfig.goodInfoList.ToDictionary(key => key.good, value => value);
        goodIconList = new Dictionary<int, Sprite>();
        equipIconList = new Dictionary<string, Sprite>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowFlutterView(string text)
    {
        UIManager.Instance.AddPopLayer(flutteViewRef, new Vector2(0, 0), new object[] {text});
    }

    //获取装备配置信息
    public Equip.EquipInfo GetEquipInfo(int id)
    {
        return equipGoodList[id];
    }

    //获取道具配置信息
    public GoodInfo GetGoodInfo(int id)
    {
        return goodGoodList[id];
    }

    //获取道具名字
    public string GetGoodName(int type, int id)
    {
        switch ((GoodsType) type)
        {
            case GoodsType.Equip:
                Equip.EquipInfo equipInfo = GetEquipInfo(id);
                return equipInfo.name;
                break;
            case GoodsType.Good:
                GoodInfo goodInfo = GetGoodInfo(id);
                return goodInfo.name;
                break;
            default:
                return "";
        }
    }

    //获取道具简介
    public string GetGoodDes(int type, int id)
    {
        switch ((GoodsType) type)
        {
            case GoodsType.Equip:
                Equip.EquipInfo equipInfo = GetEquipInfo(id);
                return equipInfo.desc;
                break;
            case GoodsType.Good:
                GoodInfo goodInfo = GetGoodInfo(id);
                return goodInfo.desc;
                break;
            default:
                return "";
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

    // public string GetGoodClassifier(int type, int id)
    // {
    //     GoodInfo goodInfo = GetGoodInfo(type, id);
    //     if (type == (int) GoodsType.Equip)
    //     {
    //         switch ((EquipmentPart) goodInfo.type)
    //         {
    //             case EquipmentPart.MeleeWeapon2H:
    //             case EquipmentPart.MeleeWeaponPaired:
    //             case EquipmentPart.Armor:
    //                 return "套";
    //             case EquipmentPart.Helmet:
    //             case EquipmentPart.Belt:
    //             case EquipmentPart.Shield:
    //             case EquipmentPart.Glasses:
    //             case EquipmentPart.Back:
    //             case EquipmentPart.Mask:
    //                 return "个";
    //             case EquipmentPart.Vest:
    //             case EquipmentPart.Gloves:
    //             case EquipmentPart.Pauldrons:
    //             case EquipmentPart.Earrings:
    //                 return "副";
    //             case EquipmentPart.Boots:
    //                 return "双";
    //             case EquipmentPart.MeleeWeapon1H:
    //             case EquipmentPart.Bow:
    //             case EquipmentPart.Firearm1H:
    //             case EquipmentPart.Firearm2H:
    //                 return "把";
    //             case EquipmentPart.Cape:
    //                 return "件";
    //             default:
    //                 return "个";
    //         }
    //     }
    //     else
    //     {
    //         switch ((GoodType) goodInfo.type)
    //         {
    //         }
    //     }
    //
    //     return "";
    // }
    
    //根据任务类型和id获取名字
    public string GetTaskTargetName(int taskType, int id)
    {
        if (taskType == (int) TaskType.Kill)
        {
            return Ui.Instance.GetMonsterName(id);
        }
        else
        {
            return goodGoodList[id].name;
        }
    }

    //删除节点下所有子节点
    public void RemoveAllChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }

    //获取道具Icon
    public Sprite GetEquipIcon(GoodsType type, int id)
    {
        Sprite sprite;
        switch (type)
        {
            case GoodsType.Equip:
                string eId = equipGoodList[id].id;
                if (equipIconList.ContainsKey(eId))
                {
                    return equipIconList[eId];
                }
                else
                {
                    sprite = IconCollection.FindIcon(eId);
                    equipIconList[eId] = sprite;
                    return sprite;
                }

                break;
            case GoodsType.Good:
                if (goodIconList.ContainsKey(id))
                {
                    return goodIconList[id];
                }

                sprite = LoadSprite(goodGoodList[id].icon);
                goodIconList[id] = sprite;
                return sprite;
                break;
            default:
                return null;
        }
    }

    //加載图片
    public Sprite LoadSprite(string name)
    {
        return Resources.Load<Sprite>("img/UI" + name);
    }

    //格式化字符串 1*3||1*3
    public List<ResClass> FormatStr(string str)
    {
        List<ResClass> resList = new List<ResClass>();
        string[] strs = str.Split("||");
        for (int i = 0; i < strs.Length; i++)
        {
            string[] s = strs[i].Split('*');
            ResClass res = new ResClass();
            res.resourcdId = int.Parse(s[0]);
            res.num = int.Parse(s[1]);
            resList.Add(res);
        }

        return resList;
    }

    //根据名字查找子节点
    public GameObject GetChild(Transform parent, string name)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).name == name)
            {
                return parent.GetChild(i).gameObject;
            }

            GameObject obj = GetChild(parent.GetChild(i), name);
            if (obj != null)
            {
                return obj;
            }
        }

        return null;
    }

}
