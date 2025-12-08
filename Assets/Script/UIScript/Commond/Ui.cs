using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.CommonScripts;
using EquipNs;
using GoodsNs;
using UnityEngine;
using UnityEngine.UI;
using HeroEditor.Common;
using HeroEditor.Common.Enums;
using IngredientNs;
using MapNs;
using MonsterNs;
using NpcTalkTaskNs;
using PlayerLvAttrNs;
using ResourceNs;
using SkillNs;
using TalkNs;
using TaskNs;
using GoodInfo = GoodsNs.GoodInfo;
using Random = UnityEngine.Random;


[Serializable]
public class ResClass
{
    public GoodsType goodsType;
    public int resourceId;
    public int num;

    public ResClass()
    {
    }

    public ResClass(int resourceId, int num)
    {
        this.resourceId = resourceId;
        this.num = num;
        goodsType = GoodsType.Resource;
    }

    public ResClass(GoodsType goodsType, int resourceId, int num)
    {
        this.resourceId = resourceId;
        this.num = num;
        this.goodsType = goodsType;
    }
}


public class AttrClass
{
    public AttrType attrType;
    public float value;

    public AttrClass()
    {
    }

    public AttrClass(AttrType attrType, float num)
    {
        this.attrType = attrType;
        this.value = num;
    }
}

public class Ui : Singleton<Ui>
{
    //插件的表
    private IconCollection IconCollection;
    private SpriteCollection SpriteCollection;

    //表
    private Dictionary<int, GoodInfo> GoodConfig;
    private Dictionary<int, EquipInfo> EquipConfig;
    private Dictionary<int, MaterialInfo> IngredientConfig;
    private Dictionary<int, ResourceInfo> ResourceConfig;
    private Dictionary<string, Sprite> equipIconDict;
    private Dictionary<int, Sprite> goodIconDict;
    private Dictionary<int, Sprite> ingredientIconDict;
    private Dictionary<int, Sprite> resourceIconDict;
    private Dictionary<string, SpriteGroupEntry> SpriteGroupEntryDict;
    private Dictionary<int, PlayerLvInfo> PlayerLvAttrDict;
    private Dictionary<int, TalkInfo> talkInfoDict;
    private Dictionary<int, MapInfo> mapInfoDict;
    private Dictionary<int, TaskNs.TaskInfo> taskInfoDict;
    private Dictionary<int, MapLayerInfo> mapLayerInfoDict;
    private Dictionary<int, MonsterInfo> monsterConfig;
    private Dictionary<int, SkillInfo> skillInfoDict;

    private Dictionary<SceneType, string> sceneTypeDict;
    //打开的页面
    private GameObject flutteViewRef;
    private GameObject showRewardRef;
    private GameObject ConfirmationRef;

    // 存储数值与中文描述的映射
    private Dictionary<AttrType, string> attrChineseMapDict;
    private MaskLayer maskLayer;
    private void Awake()
    {
        flutteViewRef = Resources.Load<GameObject>("Ref/LayerRef/UIRef/Common/FlutterWindowsLayer");
        showRewardRef = Resources.Load<GameObject>("Ref/LayerRef/UIRef/ShowRewardLayer/ShowRewardLayer");
        ConfirmationRef = Resources.Load<GameObject>("Ref/LayerRef/UIRef/ConfirmationLayer/ConfirmationLayer");
        EquipConfig = Resources.Load<EquipConfig>("Configs/Data/EquipConfig").equipInfoList
            .ToDictionary(key => key.equip, value => value);
        GoodConfig = Resources.Load<GoodsConfig>("Configs/Data/GoodsConfig").goodInfoList
            .ToDictionary(key => key.good, value => value);
        IngredientConfig = Resources.Load<IngredientConfig>("Configs/Data/IngredientConfig").materialInfoList
            .ToDictionary(key => key.material, value => value);
        ResourceConfig = Resources.Load<ResourceConfig>("Configs/Data/ResourceConfig").resourceInfoList
            .ToDictionary(key => key.resource, value => value);
        PlayerLvAttrDict = Resources.Load<PlayerLvAttrConfig>("Configs/Data/PlayerLvAttrConfig").playerLvInfoList
            .ToDictionary(key => key.playerLv, value => value);
        talkInfoDict = Resources.Load<TalkConfig>("Configs/Data/TalkConfig").talkInfoList
            .ToDictionary(key => key.talk, value => value);
        IconCollection = Resources.Load<IconCollection>("Configs/Data/IconCollection");
        SpriteCollection = Resources.Load<SpriteCollection>("Configs/Data/SpriteCollection");
        mapInfoDict = Resources.Load<MapConfig>("Configs/Data/MapConfig").mapInfoList
            .ToDictionary(key => key.map, value => value);
        taskInfoDict = Resources.Load<TaskConfig>("Configs/Data/TaskConfig").taskInfoList
            .ToDictionary(key => key.task, value => value);
        mapLayerInfoDict = Resources.Load<MapLayerData>("Configs/StaticConfig/Config/MapLayerData").mapInfoList
            .ToDictionary(key => key.mapId, value => value);
        monsterConfig = Resources.Load<MonsterConfig>("Configs/Data/MonsterConfig").monsterInfoList.ToDictionary(key => key.monster, value => value);
        skillInfoDict = Resources.Load<SkillConfig>("Configs/Data/SkillConfig").skillInfoList.ToDictionary(key => key.id, value => value);
        SpriteGroupEntryDict = new Dictionary<string, SpriteGroupEntry>();
        goodIconDict = new Dictionary<int, Sprite>();
        equipIconDict = new Dictionary<string, Sprite>();
        ingredientIconDict = new Dictionary<int, Sprite>();
        resourceIconDict = new Dictionary<int, Sprite>();
        attrChineseMapDict = new Dictionary<AttrType, string>
        {
            {AttrType.Attack, "攻击力"},
            {AttrType.MaxHealth, "生命值"},
            {AttrType.MoveSpeed, "移速"},
            {AttrType.AttackSpeed, "攻速"},
            {AttrType.Armor, "护甲"},
            {AttrType.CritRate, "暴击率"},
            {AttrType.CritDamage, "暴击伤害"},
            {AttrType.DodgeRate, "闪避率"}
        };
        sceneTypeDict = new Dictionary<SceneType, string>()
        {
            {SceneType.FightScene, "FightScene"},
            {SceneType.MainScene, "MainScene"}
        };
    }

    void Start()
    {
        EventManager.Instance.AddEvent(GameEventType.ShowFlutterEvent, new object[] {(Action<string>) ShowFlutterView});
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //飘窗
    public void ShowFlutterView(string text)
    {
        UIManager.Instance.AddPopLayer(flutteViewRef, new Vector2(0, 0), new object[] {text});
    }

    //提示
    public void ShowConfirmationLayer(string text, Action action)
    {
        UIManager.Instance.OpenLayer(ConfirmationRef, new object[] {text, action});
    }
    //获取装备配置信息
    public EquipInfo GetEquipInfo(int id)
    {
        if (EquipConfig.ContainsKey(id))
        {
            return EquipConfig[id];
        }

        return null;
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
                EquipInfo equipInfo = GetEquipInfo(id);
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
                EquipInfo equipInfo = GetEquipInfo(id);
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


    public string GetTalkText(int id)
    {
        return talkInfoDict[id].text;
    }

    public string GetTaskDes(int id)
    {
        return taskInfoDict[id].des;
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
    public Sprite GetGoodIcon(int type, int id)
    {
        Sprite sprite;
        switch ((GoodsType) type)
        {
            case GoodsType.Equip:
                string eId = EquipConfig[id].id;
                if (equipIconDict.ContainsKey(eId))
                {
                    return equipIconDict[eId];
                }
                else
                {
                    sprite = IconCollection.FindIcon(eId);
                    equipIconDict[eId] = sprite;
                    return sprite;
                }
            
            case GoodsType.Good:
                if (goodIconDict.ContainsKey(id))
                {
                    return goodIconDict[id];
                }

                sprite = LoadIcon(GoodConfig[id].icon);
                goodIconDict[id] = sprite;
                return sprite;
            case GoodsType.Ingredient:
                if (ingredientIconDict.ContainsKey(id))
                {
                    return ingredientIconDict[id];
                }

                sprite = LoadIcon(IngredientConfig[id].icon);
                ingredientIconDict[id] = sprite;
                return sprite;
            case GoodsType.Resource:
                if (resourceIconDict.ContainsKey(id))
                {
                    return resourceIconDict[id];
                }

                sprite = LoadIcon(ResourceConfig[id].icon);
                resourceIconDict[id] = sprite;
                return sprite;
            default:
                return null;
        }
    }

    //加載图标
    private Sprite LoadIcon(string name)
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
            Debug.Log("图片名字有误 " + name);
        }

        path = "Img/DynamicImg/Icon/" + path + "/" + name;

        return Resources.Load<Sprite>(path);
    }

    //加载图片
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
        else if (str.Equals("m"))
        {
            path = "Map";
        }
        else
        {
            Debug.Log("图片名字有误 " + name);
        }

        path = "Img/DynamicImg/Sprite/" + path + "/" + name;

        return Resources.Load<Sprite>(path);
    }
    //格式化字符串 1*3||1*3
    public List<ResClass> FormatResStr(string str)
    {
        List<ResClass> resList = new List<ResClass>();
        string[] strs = str.Split("||");
        for (int i = 0; i < strs.Length; i++)
        {
            string[] s = strs[i].Split('*');
            ResClass res = new ResClass();
            res.goodsType = (GoodsType) int.Parse(s[0]);
            res.resourceId = int.Parse(s[1]);
            res.num = int.Parse(s[2]);
            resList.Add(res);
        }

        return resList;
    }

    public List<ResClass> FormatResStr(string str, GoodsType goodsType)
    {
        List<ResClass> resList = new List<ResClass>();
        string[] strs = str.Split("||");
        for (int i = 0; i < strs.Length; i++)
        {
            string[] s = strs[i].Split('*');
            ResClass res = new ResClass();
            res.goodsType = goodsType;
            res.resourceId = int.Parse(s[0]);
            res.num = int.Parse(s[1]);
            resList.Add(res);
        }

        return resList;
    }

    //格式化字符串 1*3||1*3
    public List<AttrClass> FormatAttrStr(string str)
    {
        List<AttrClass> attrList = new List<AttrClass>();
        string[] strs = str.Split("||");
        for (int i = 0; i < strs.Length; i++)
        {
            string[] s = strs[i].Split('*');
            AttrClass attrInfo = new AttrClass();
            attrInfo.attrType = (AttrType) int.Parse(s[0]);
            attrInfo.value = float.Parse(s[1]);
            attrList.Add(attrInfo);
        }
        
        return attrList;
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

    //根据名字查找子节点
    public GameObject FindChildByTag(Transform parent, string layer)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).gameObject.layer == LayerMask.NameToLayer(layer))
            {
                return parent.GetChild(i).gameObject;
            }

            GameObject obj = FindChildByTag(parent.GetChild(i), layer);
            if (obj != null)
            {
                return obj;
            }
        }

        return null;
    }

    //根据名字查找子节点
    public T FindChildByTag<T>(Transform parent, string layer) where T : Component
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).gameObject.layer == LayerMask.NameToLayer(layer))
            {
                return parent.GetChild(i).gameObject.GetComponent<T>();
            }

            T obj = FindChildByTag<T>(parent.GetChild(i), layer);
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
        if (SpriteGroupEntryDict.ContainsKey(id))
        {
            return SpriteGroupEntryDict[id];
        }

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
            case EquipmentPart.Helmet:
                entry = SpriteCollection.Helmet.Find(data => data.Id == id);
                break;
            case EquipmentPart.MeleeWeapon2H:
                entry = SpriteCollection.MeleeWeapon2H.Find(data => data.Id == id);
                break;
            default:
                entry = null;
                break;
        }

        if (entry == null)
        {
            Debug.Log("部位未实现");
        }

        return entry;
    }

    //打开恭喜获得页面
    public void ShowReward(List<ResClass> resList)
    {
        UIManager.Instance.OpenLayer(showRewardRef, new object[] {resList});
    }

    //展示奖励
    public void ShowReward(ResClass resClass)
    {
        List<ResClass> resList = new List<ResClass>();
        resList.Add(resClass);
        UIManager.Instance.OpenLayer(showRewardRef, new object[] {resList});
    }
    //设置置灰
    public void SetGray(Image image, bool isGray)
    {
        // Gray gray = button.GetComponent<Gray>();
        // gray.SetGray(isGray);
        if (isGray)
        {
            image.color = new Color(101 / 255f, 101 / 255f, 101 / 255f);
        }
        else
        {
            image.color = new Color(255, 255, 255);
        }
    }

    //获取属性名字 + 值
    public String GetAttrText(AttrClass attrClass)
    {
        string name = attrChineseMapDict[attrClass.attrType];
        string value = "";
        if (attrClass.value > 0)
        {
            value = $"  +  <color=#{ColorUtility.ToHtmlStringRGB(MyColor.Green)}>{attrClass.value}</color>";
        }
        else
        {
            value = $"  -  <color=#{ColorUtility.ToHtmlStringRGB(MyColor.Red)}>{MathF.Abs(attrClass.value)}</color>";
        }

        return name + value;
    }

    //获取地图信息
    public MapInfo GetMapInfo(int id)
    {
        return mapInfoDict[id];
    }

    //获取地图表
    public Dictionary<int, MapInfo> GetMapInfoList()
    {
        return mapInfoDict;
    }

    //获取地图对应的预制体信息
    public MapLayerInfo GetMapLayerInfo(int mapId)
    {
        return mapLayerInfoDict[mapId];
    }


    //获取属性名字
    public string GetAttrName(AttrType attrType)
    {
        return attrChineseMapDict[attrType];
    }

    //获取主角当前等级基础属性
    public PlayerLvInfo GetPlayerLvAttr()
    {
        int lv = GameDataManager.Instance.GetPlayerLv();

        return PlayerLvAttrDict[lv];
    }
    
    //获取玩家基础数值
    public float GetPlayerBaseAttrValue(AttrType attrType)
    {
        int lv = GameDataManager.Instance.GetPlayerLv();
        PlayerLvInfo PlayerInfo = PlayerLvAttrDict[lv];
        switch (attrType)
        {
            case AttrType.Attack:
                return PlayerInfo.attack;
            case AttrType.MaxHealth:
                return PlayerInfo.health;
            case AttrType.MoveSpeed:
                return PlayerInfo.moveSpeed;
            case AttrType.Armor:
                return PlayerInfo.armor;
            case AttrType.CritRate:
                return PlayerInfo.critRate;
            case AttrType.CritDamage:
                return PlayerInfo.critDamage;
            case AttrType.AttackSpeed:
                return PlayerInfo.attackSpeed;
            case AttrType.DodgeRate:
                return PlayerInfo.dodgeRate;
            default:
                return -1;
        }
    }

    //获取加载的怪物数值
    public MonsterInfo GetMonsterValue(int id)
    {
        if (monsterConfig.ContainsKey(id))
        {
            return monsterConfig[id];
        }

        Debug.Log("id为" + id + "的怪物数值找不到");
        return null;
    }

    //是否暴击
    public bool IsCriticalAttack(float critical)
    {
        return Random.value <= critical;
    }

    //技能
    public Dictionary<int, SkillInfo> GetSkillConfig()
    {
        return skillInfoDict;
    }

    public SkillInfo GetSkillInfoById(int id)
    {
        return skillInfoDict[id];
    }
}
