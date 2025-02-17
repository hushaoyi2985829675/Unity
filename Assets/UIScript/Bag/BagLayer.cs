using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.CommonScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class BagLayer : PanelBase
{
    [Header("人物")]
    public GameObject Player;
    [Header("背包数据")]
    public BagData BagData;
    Dictionary<int, EquipmentInfo> BagDataInfo;
    [Header("玩家当前装备数据")]
    public PlayerEquipData PlayerEquipData;
    [Header("Icon数据")]
    public IconCollection IconCollection;
    [Header("人物预制体")]
    public GameObject PlayerRef;
    GameObject BagPlayer;
    [Header("背包格子数量")]
    public int HorizontalNum;
    public int VerticalNum;
    [Header("每个格子间隔")]
    public Vector2 Space;
    [Header("视图节点")]
    public GridView GridView;
    public TabView TabView;
    [Header("装备栏")]
    public PlayerEquipSlot PlayerEquipSlot;
    private List<GameObject> TabViewList;
    string[] TabNameList;
    private int Tag;
    int SelectIdx;
    GameObject BeforeItem;

    public override void onEnter(object[] data)
    {
        BagDataInfo = BagData.Equipments.Select((vale, index) => new { Index = index, EquipmentInfo = vale }).ToDictionary(x => x.Index, x => x.EquipmentInfo);
        Player = GameObject.FindWithTag("Player");
        if (Player == null)
        {
            Debug.Log("背包在场景中没找到Player");
        }
        Tag = 0;
        SelectIdx = -1;
        BeforeItem = null;
        TabView.AddRefreshEvent(RefreshTabView);
        TabNameList = new string[]{ "装备", "道具", "道具" };
        TabView.SetNum(TabNameList.Length);
        onChangeTitle(true);
        PlayerEquipSlot.RefreshAllEquip();
        RefreshPlayer();
    }
    public void onRefreshItem(int index, GameObject item)
    {
        if (Tag == 0)
        {
            Sprite Sprite;
            item.transform.Find("Text").SetActive(false);
            if (BagDataInfo.ContainsKey(index))
            {
                var IconInfo = IconCollection.Icons.Find(data => data.Id == BagDataInfo[index].SpriteGroupEntry.Id);
                if (IconInfo != null)
                {
                    Sprite = IconInfo.Sprite;
                    if (BagDataInfo[index].num > 1)
                    {
                        item.transform.Find("Text").SetActive(true);
                        item.transform.Find("Text").GetComponent<Text>().text = BagDataInfo[index].num.ToString();
                    }   
                }
                else
                {
                    Sprite = null;
                    Debug.LogFormat("Id为{}的Icon没找到", BagDataInfo[index].SpriteGroupEntry.Id);
                }
            }
            else
            {
                Sprite = Resources.Load<Sprite>("UI/UI/public/Empty");
            }
            item.transform.Find("EquipSpr").GetComponent<Image>().sprite = Sprite;
            item.transform.Find("Checkmark").SetActive(SelectIdx == index);
            item.GetComponent<Button>().onClick.AddListener(() => {
                if (SelectIdx == index)
                {
                    return;
                }
                if (BeforeItem != null)
                {
                    BeforeItem.transform.Find("Checkmark").SetActive(false);
                }
                SelectIdx = index;
                item.transform.Find("Checkmark").SetActive(true);
                BeforeItem = item;
            });          
            
        }   
        
    }
    public void RefreshTabView(int index,GameObject item)
    {
        var toggle = item.GetComponent<Toggle>();
        toggle.isOn = Tag == index;
        toggle.onValueChanged.AddListener((isOn) => {
            if(Tag == index)
            {
                return;
            }
            Tag = index;
            onChangeTitle(isOn);
        });
        item.transform.Find("Background/Text").GetComponent<Text>().text = TabNameList[index];
    }
    public void onChangeTitle(bool isOn)
    {
        if (!isOn)
        {
            return;
        }
        if (Tag == 0)
        {
            GridView.AddRefreshEvent(onRefreshItem);
            GridView.SetItemNumAndSpace(HorizontalNum, VerticalNum, Space.x, Space.y);
            GridView.LoadGridViewData();
            GridView.RefreshAllItem();
        }
    }
    public void RefreshPlayer()
    {
        if (BagPlayer == null)
        {
            BagPlayer = Instantiate<GameObject>(PlayerRef);
            DontDestroyOnLoad(BagPlayer);
        }
        BagPlayer.SetActive(true);
        foreach (var info in PlayerEquipData.EquipInfo)
        {
            if (info.SpriteGroupEntry != null && info.SpriteGroupEntry.Id != null && info.SpriteGroupEntry.Id != "")
            {
                BagPlayer.transform.Find("Player").GetComponent<Character>().Equip(info.SpriteGroupEntry, info.Part);
            }
        }

    }
    public void EquipClick()
    {
        if(!BagDataInfo.ContainsKey(SelectIdx))
        { 
            return;
        }
        var info = BagDataInfo[SelectIdx];
        BagPlayer.transform.Find("Player").GetComponent<Character>().Equip(info.SpriteGroupEntry, info.Part);
        Player.GetComponent<Player>().CheckoutEquip(info.SpriteGroupEntry, info.Part);
    }
    public override void onExit()
    {
        BagPlayer.SetActive(false);
    }
}
