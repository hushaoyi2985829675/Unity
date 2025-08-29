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
using Ingredient;
using Resource;
using GoodInfo = Goods.GoodInfo;

enum GoodType
{
}

public class ResClass
{
    public int resourceId;
    public int num;
}

public class Ui : Singleton<Ui>
{
    private IconCollection IconCollection;

    //道具表
    private Dictionary<int, GoodInfo> GoodConfig;
    private Dictionary<int, Equip.EquipInfo> EquipConfig;
    private Dictionary<int, MaterialInfo> IngredientConfig;
    private Dictionary<int, ResourceInfo> ResourceConfig;
    
    private SpriteCollection SpriteCollection;

    //图标缓存
    private Dictionary<string, Sprite> equipIconList;
    private Dictionary<int, Sprite> goodIconList;
    private Dictionary<int, Sprite> ingredientIconList;
    private Dictionary<int, Sprite> resourceIconList;
    private GameObject flutteViewRef;

    private void Awake()
    {
        EquipConfig = Resources.Load<EquipConfig>("Configs/Data/EquipConfig").equipInfoList
            .ToDictionary(key => key.equip, value => value);
        GoodConfig = Resources.Load<GoodsConfig>("Configs/Data/GoodsConfig").goodInfoList
            .ToDictionary(key => key.good, value => value);
        IngredientConfig = Resources.Load<IngredientConfig>("Configs/Data/IngredientConfig").materialInfoList
            .ToDictionary(key => key.material, value => value);
        ResourceConfig = Resources.Load<ResourceConfig>("Configs/Data/ResourceConfig").resourceInfoList
            .ToDictionary(key => key.resource, value => value);
        flutteViewRef = Resources.Load<GameObject>("Ref/LayerRef/UIRef/Commond/FlutterWindowsLayer");
        IconCollection = Resources.Load<IconCollection>("Configs/Data/IconCollection");
        SpriteCollection = Resources.Load<SpriteCollection>("Configs/Data/SpriteCollection");
        goodIconList = new Dictionary<int, Sprite>();
        equipIconList = new Dictionary<string, Sprite>();
        ingredientIconList = new Dictionary<int, Sprite>();
        resourceIconList = new Dictionary<int, Sprite>();
    }

    void Start()
    {
      
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
        return EquipConfig[id];
    }

    //获取道具配置信息
    public GoodInfo GetGoodInfo(int id)
    {
        return GoodConfig[id];
    }

    //获取材料配置信息
    public MaterialInfo GetIngredientInfo(int id)
    {
        return IngredientConfig[id];
    }

    //获取资源配置信息
    public ResourceInfo GetResourceInfo(int id)
    {
        return ResourceConfig[id];
    }
    //获取道具名字
    public string GetGoodName(int type, int id)
    {
        switch ((GoodsType) type)
        {
            case GoodsType.Equip:
                Equip.EquipInfo equipInfo = GetEquipInfo(id);
                return equipInfo.name;
            case GoodsType.Good:
                GoodInfo goodInfo = GetGoodInfo(id);
                return goodInfo.name;
            case GoodsType.Ingredient:
                MaterialInfo materialInfo = GetIngredientInfo(id);
                return materialInfo.name;
            case GoodsType.Resource:
                ResourceInfo resourceInfo = GetResourceInfo(id);
                return resourceInfo.name;
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
            case GoodsType.Good:
                GoodInfo goodInfo = GetGoodInfo(id);
                return goodInfo.desc;
            // case GoodsType.Ingredient:
            //     MaterialInfo materialInfo = GetIngredientInfo(id);
            //     return materialInfo.desc;
            // case GoodsType.Resource:
            //     ResourceInfo resourceInfo = GetResourceInfo(id);
            //     return resourceInfo.desc;
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
            return GoodConfig[id].name;
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
    public Sprite GetGoodIcon(GoodsType type, int id)
    {
        Sprite sprite;
        switch (type)
        {
            case GoodsType.Equip:
                string eId = EquipConfig[id].id;
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
            
            case GoodsType.Good:
                if (goodIconList.ContainsKey(id))
                {
                    return goodIconList[id];
                }

                sprite = LoadSprite(GoodConfig[id].icon);
                goodIconList[id] = sprite;
                return sprite;
            case GoodsType.Ingredient:
                if (ingredientIconList.ContainsKey(id))
                {
                    return ingredientIconList[id];
                }

                sprite = LoadSprite(IngredientConfig[id].icon);
                ingredientIconList[id] = sprite;
                return sprite;
            case GoodsType.Resource:
                if (resourceIconList.ContainsKey(id))
                {
                    return resourceIconList[id];
                }

                sprite = LoadSprite(ResourceConfig[id].icon);
                resourceIconList[id] = sprite;
                return sprite;
            default:
                return null;
        }
    }

    //加載图片
    public Sprite LoadSprite(string name)
    {
        string path = "";
        string str = name.Split("_")[0];
        if (str.Equals("i"))
        {
            path = "Ingredient";
        }
        else if (str.Equals("g"))
        {
            path = "Good";
        }
        else if (str.Equals("r"))
        {
            path = "Resource";
        }
        else
        {
            Debug.Log("图片名字有误");
        }

        return Resources.Load<Sprite>("img/" + path + "/" + name);
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
            res.resourceId = int.Parse(s[0]);
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

    //获取装备词条
    public SpriteGroupEntry GetEquipEntry(EquipmentPart part, string id)
    {
        SpriteGroupEntry entry;
        switch (part)
        {
            case EquipmentPart.MeleeWeapon1H:
                entry = SpriteCollection.MeleeWeapon1H.Find(data => data.Id == id);
                break;
            case EquipmentPart.Armor:
                entry = SpriteCollection.Armor.Find(data => data.Id == id);
                break;
            case EquipmentPart.Shield:
                entry = SpriteCollection.Shield.Find(data => data.Id == id);
                break;
            default:
                entry = null;
                break;
        }

        return entry;
    }
}
