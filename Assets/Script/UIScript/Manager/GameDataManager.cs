using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using EquipNs;
using GoodsNs;
using HeroEditor.Common;
using HeroEditor.Common.Enums;
using NpcTalkTaskNs;
using PlayerLvAttrNs;
using ResourceNs;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
    private string path = "GameData/PlayerData/";
    private SpriteCollection SpriteCollection;

    [Header("游戏存储数据")]
    private BagData BagData;

    private PlayerLocalValueData playerLocalValueData;
    private PlayerValueData playerValueData;
    private PlayerResData playerResData;
    private ShopBuyData shopBuyData;
    private PlayerEquipData playerEquipData;
    private PlayerTaskData playerTaskData;


    public void Awake()
    {
        SpriteCollection = Resources.Load<SpriteCollection>("Configs/Data/SpriteCollection");
        playerLocalValueData = Resources.Load<PlayerLocalValueData>(path + "LocalData/PlayerLocalValueData");
        playerValueData = Resources.Load<PlayerValueData>(path + "PersistenceData/PlayerValueData");
        playerResData = Resources.Load<PlayerResData>(path + "PersistenceData/playerResData");
        shopBuyData = Resources.Load<ShopBuyData>(path + "PersistenceData/shopBuyData");
        playerEquipData = Resources.Load<PlayerEquipData>(path + "PersistenceData/PlayerEquipData");
        playerTaskData = Resources.Load<PlayerTaskData>(path + "PersistenceData/PlayerTaskData");
        BagData = Resources.Load<BagData>(path + "PersistenceData/BagData");
        //LoadPlayerData("playerResData", playerResData);
        //PlayerLocalValueData.Create();
        // PlayerEquipData.Create();
        playerLocalValueData.InitPlayerValue();
    }

    public void Start()
    {
    }

    void SavePlayerData<T>(string fileName, T data)
    {
        string path = Path.Combine(Application.persistentDataPath, "Data", fileName + ".txt");
        var json = JsonUtility.ToJson(data);
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(path, json);
    }

    void LoadPlayerData<T>(string FileName, T data) where T : ScriptableBase
    {
        var path = Path.Combine(Application.persistentDataPath, "Data", FileName + ".txt");
        if (!File.Exists(path))
        {
            data.Create();
            File.Create(path).Close();
        }
        else
        {
            data.Clear();
            var json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, data);
        }
    }

    #region 背包方法

    //增加装备
    public void AddEquip(int id)
    {
    }

    //增加道具
    public void AddGood(GoodsType goodsType, int id, int num)
    {
        if (goodsType == GoodsType.Equip)
        {
            BagData.AddEquip(id, num);
        }
        else if (goodsType == GoodsType.Good)
        {
            BagData.AddGood(id, num);
        }
    }

    //获取装备数据
    public List<EquipmentInfo> GetBagData(EquipmentPart part)
    {
        return BagData.GetEquipListData(part);
    }

    //获取道具数据
    public List<GoodData> GetGoodData(GoodsCategory type)
    {
        return BagData.GetGoodListData(type);
    }

    #endregion

    //移除装备
    public void RemoveEquipData(int id, int num)
    {
        BagData.RemoveEquip(id, num);
    }

    #region 资源方法

    //增加资源
    public void AddRes(ResClass res)
    {
        playerResData.AddResNum(res.resourceId, (int) res.num);
        EventManager.Instance.PostEvent(GameEventType.ResEvent, new object[] {res.resourceId});
    }

    //减少资源
    public void DecreaseRes(int id, int num)
    {
        playerResData.DecreaseResNum(id, num);
        EventManager.Instance.PostEvent(GameEventType.ResEvent, new object[] {id});
    }

    //获取资源数量
    public int GetResNum(int id)
    {
        return playerResData.GetResNum(id);
    }

    //获取商店购买数据
    public List<ShopBuyInfo> GetShopBuyList()
    {
        return shopBuyData.shopBuyList;
    }

    //增加购买数据
    public void AddBuyInfoData(int type, int id, int costId, int costNum)
    {
        shopBuyData.AddBuyInfo(type, id);
        DecreaseRes(costId, costNum);
        //增加实体道具
        AddGood((GoodsType) type, id, 1);
    }

    #endregion

    #region 主角装备方法

    //获取主角部位的当前装备
    public EquipData GetPlayerEquipData(EquipmentPart part)
    {
        return playerEquipData.GetPlayerEquipData(part);
    }

    //更改主角部位的当前装备
    public void SetPlayerEquipData(int id)
    {
        int part = Ui.Instance.GetEquipInfo(id).part;
        RemoveEquipAttr((EquipmentPart) part);
        playerEquipData.SetPlayerEquipData(id);
        AddEquipAttr((EquipmentPart) part);
    }

    //卸下主角部位当前装备
    public void RemovePlayerEquipData(EquipmentPart part)
    {
        RemoveEquipAttr(part);
        playerEquipData.RemovePlayerEquipData(part);
    }

    #endregion

    #region 主角属性方法

    public float GetPlayerAttrValue(AttrType attrType)
    {
        return playerLocalValueData.GetPlayerAttrValue(attrType);
    }

    public void RemoveEquipAttr(EquipmentPart part)
    {
        EquipData equipData = GetPlayerEquipData(part);
        if (equipData.id == -1)
        {
            return;
        }

        EquipInfo equipInfo = Ui.Instance.GetEquipInfo(equipData.id);
        List<AttrClass> attrList = Ui.Instance.FormatAttrStr(equipInfo.attr);
        foreach (AttrClass attrInfo in attrList)
        {
            playerLocalValueData.SetPlayerAttrValue(attrInfo.attrType, -attrInfo.value);
        }
    }

    //玩家等级
    public int GetPlayerLv()
    {
        return playerValueData.GetPlayerLv();
    }

    public int GetPlayerHp()
    {
        return playerValueData.GetPlayerHp();
    }

    public void SetPlayerHp(int hp)
    {
        playerValueData.SetPlayerHp(hp);
        playerLocalValueData.SetPlayerAttrValue(AttrType.CurHealth, hp - playerLocalValueData.CurHp);
    }

    public int GetPlayerExp()
    {
        return playerValueData.GetPlayerExp();
    }

    public void SetPlayerExp(int exp)
    {
        playerValueData.SetPlayerExp(exp);
    }

    public List<int> GetPlayerSkillList()
    {
        return playerValueData.skillIdList;
    }

    public PlayerLocalValueData GetPlayerLocalValueInfo()
    {
        return playerLocalValueData;
    }

    public void AddEquipAttr(EquipmentPart part)
    {
        EquipData equipData = GetPlayerEquipData(part);
        EquipInfo equipInfo = Ui.Instance.GetEquipInfo(equipData.id);
        List<AttrClass> attrList = Ui.Instance.FormatAttrStr(equipInfo.attr);
        foreach (AttrClass attrInfo in attrList)
        {
            playerLocalValueData.SetPlayerAttrValue(attrInfo.attrType, attrInfo.value);
        }
    }

    #endregion

    #region 任务方法

    public List<TaskInfo> GetPlayerTaskData()
    {
        return playerTaskData.TaskList;
    }

    public void RemoveTask(int taskId)
    {
        playerTaskData.RemoveTask(taskId);
    }

    public void AddTask(int npcId, PlayerTaskLvInfo taskInfo)
    {
        playerTaskData.AddTask(npcId, taskInfo);
    }

    public void MarkTaskCompletion(int taskId)
    {
        playerTaskData.MarkTaskCompletion(taskId);
    }

    #endregion
}