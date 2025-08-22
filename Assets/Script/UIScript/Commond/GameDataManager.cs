using System;
using System.Collections;
using System.Collections.Generic;
using HeroEditor.Common;
using HeroEditor.Common.Enums;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    private static GameDataManager _instance;

    public static GameDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameDataManager>();
            }

            return _instance;
        }
    }

    private SpriteCollection SpriteCollection;
    [Header("游戏存储数据")] public BagData BagData;
    public PlayerLvData PlayerLvData;
    public PlayerValueData PlayerValueData;

    public void Start()
    {
        SpriteCollection = Resources.Load<SpriteCollection>("Configs/Data/SpriteCollection");
    }

    //设置装备
    public void AddEquip(SpriteGroupEntry entry, EquipmentPart part)
    {
        BagData.AddEquip(entry, part);
    }
}