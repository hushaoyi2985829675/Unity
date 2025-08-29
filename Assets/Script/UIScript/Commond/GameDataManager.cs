using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HeroEditor.Common;
using HeroEditor.Common.Enums;
using Resource;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
    private string path = "GameData/PlayerData/";
    private SpriteCollection SpriteCollection;
    [Header("游戏存储数据")] public BagData BagData;
    public PlayerLvData PlayerLvData;
    public PlayerValueData PlayerValueData;
    public PlayerResData PlayerResData;

    public void Awake()
    {
        SpriteCollection = Resources.Load<SpriteCollection>("Configs/Data/SpriteCollection");
        PlayerLvData = Resources.Load<PlayerLvData>(path + "PlayerLvData");
        PlayerValueData = Resources.Load<PlayerValueData>(path + "PlayerValueData");
        PlayerResData = Resources.Load<PlayerResData>(path + "PlayerResData");
        LoadPlayerData("PlayerResData", PlayerResData);
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
            File.Create(path).Close();
        }

        data.Clear();
        var json = File.ReadAllText(path);
        JsonUtility.FromJsonOverwrite(json, data);
    }

    //设置装备
    public void AddEquip(int id)
    {
        Equip.EquipInfo equipInfo = Ui.Instance.GetEquipInfo(id);
        SpriteGroupEntry entry = Ui.Instance.GetEquipEntry((EquipmentPart) equipInfo.part, equipInfo.id);
        BagData.AddEquip(entry, id);
    }

    //增加资源
    public void AddRes(int id, int num)
    {
        PlayerResData.AddResNum(id, num);
        EventManager.Instance.PostResEvent(id);
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
}