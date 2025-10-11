using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Equip;
using Goods;
using HeroEditor.Common;
using HeroEditor.Common.Enums;
using NpcTalkTask;
using PlayerLvAttr;
using Resource;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
    private string path = "GameData/PlayerData/";
    private SpriteCollection SpriteCollection;
    [Header("游戏存储数据")] private BagData BagData;
    private PlayerValueData PlayerValueData;
    private PlayerResData PlayerResData;
    private ShopBuyData ShopBuyData;
    private PlayerEquipData PlayerEquipData;
    private PlayerTaskData PlayerTaskData;
    

    public void Awake()
    {
        SpriteCollection = Resources.Load<SpriteCollection>("Configs/Data/SpriteCollection");
        PlayerValueData = Resources.Load<PlayerValueData>(path + "PlayerValueData");
        PlayerResData = Resources.Load<PlayerResData>(path + "PlayerResData");
        ShopBuyData = Resources.Load<ShopBuyData>(path + "ShopBuyData");
        PlayerEquipData = Resources.Load<PlayerEquipData>(path + "PlayerEquipData");
        PlayerTaskData = Resources.Load<PlayerTaskData>(path + "PlayerTaskData");
        BagData = Resources.Load<BagData>(path + "BagData");
        //LoadPlayerData("PlayerResData", PlayerResData);
        PlayerValueData.Create();
        PlayerEquipData.Create();
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

        data.Clear();
        var json = File.ReadAllText(path);
        JsonUtility.FromJsonOverwrite(json, data);
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
        PlayerResData.AddResNum(res.resourceId, (int) res.num);
        EventManager.Instance.PostResEvent(res.resourceId);
    }

    //减少资源
    public void DecreaseRes(int id, int num)
    {
        PlayerResData.DecreaseResNum(id, num);
        EventManager.Instance.PostResEvent(id);
    }

    //获取资源数量
    public int GetResNum(int id)
    {
        return PlayerResData.GetResNum(id);
    }

    //获取商店购买数据
    public List<ShopBuyInfo> GetShopBuyList()
    {
        return ShopBuyData.shopBuyList;
    }

    //增加购买数据
    public void AddBuyInfoData(int type, int id, int costId, int costNum)
    {
        ShopBuyData.AddBuyInfo(type, id);
        DecreaseRes(costId, costNum);
        //增加实体道具
        AddGood((GoodsType) type, id, 1);
    }

    #endregion

    #region 主角装备方法

    //获取主角部位的当前装备
    public EquipData GetPlayerEquipData(EquipmentPart part)
    {
        return PlayerEquipData.GetPlayerEquipData(part);
    }

    //更改主角部位的当前装备
    public void SetPlayerEquipData(int id)
    {
        int part = Ui.Instance.GetEquipInfo(id).part;
        RemoveEquipAttr((EquipmentPart) part);
        PlayerEquipData.SetPlayerEquipData(id);
        AddEquipAttr((EquipmentPart) part);
    }

    //卸下主角部位当前装备
    public void RemovePlayerEquipData(EquipmentPart part)
    {
        RemoveEquipAttr(part);
        PlayerEquipData.RemovePlayerEquipData(part);
    }

    #endregion

    #region 主角属性方法

    public float GetPlayerAttrValue(AttrType attrType)
    {
        return PlayerValueData.GetPlayerAttrValue(attrType);
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
            PlayerValueData.SetPlayerAttrValue(attrInfo.attrType, -attrInfo.value);
        }
    }

    public int GetPlayerLv()
    {
        return PlayerValueData.GetPlayerLv();
    }

    public PlayerInfo GetPlayerValueInfo()
    {
        return PlayerValueData.GetPlayerValueInfo();
    }

    public void AddEquipAttr(EquipmentPart part)
    {
        EquipData equipData = GetPlayerEquipData(part);
        EquipInfo equipInfo = Ui.Instance.GetEquipInfo(equipData.id);
        List<AttrClass> attrList = Ui.Instance.FormatAttrStr(equipInfo.attr);
        foreach (AttrClass attrInfo in attrList)
        {
            PlayerValueData.SetPlayerAttrValue(attrInfo.attrType, attrInfo.value);
        }
    }

    #endregion

    #region 任务方法

    public List<TaskInfo> GetPlayerTaskData()
    {
        return PlayerTaskData.TaskList;
    }

    public void RemoveTask(int taskId)
    {
        PlayerTaskData.RemoveTask(taskId);
    }

    public void AddTask(int npcId, PlayerTaskLvInfo taskInfo)
    {
        PlayerTaskData.AddTask(npcId, taskInfo);
    }

    public void MarkTaskCompletion(int taskId)
    {
        PlayerTaskData.MarkTaskCompletion(taskId);
    }

    #endregion
}