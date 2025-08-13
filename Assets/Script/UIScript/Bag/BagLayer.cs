using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.CommonScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class BagLayer : PanelBase
{
    [Header("����")]
    public GameObject Player;
    [Header("��������")]
    public BagData BagData;
    Dictionary<int, EquipmentInfo> BagDataInfo;
    [Header("��ҵ�ǰװ������")]
    public PlayerEquipData PlayerEquipData;
    [Header("Icon����")]
    public IconCollection IconCollection;
    [Header("����Ԥ����")]
    public GameObject PlayerRef;
    GameObject BagPlayer;
    [Header("������������")]
    public int HorizontalNum;
    public int VerticalNum;
    [Header("ÿ�����Ӽ��")]
    public Vector2 Space;
    [Header("��ͼ�ڵ�")]
    public GridView GridView;
    public TabView TabView;
    [Header("װ����")]
    public PlayerEquipSlot PlayerEquipSlot;
    private List<GameObject> TabViewList;
    string[] TabNameList;
    private int Tag;
    int SelectIdx;
    GameObject BeforeItem;

    public override void onEnter(object[] data)
    {
        Button closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.CloseLayer(gameObject.name);
        });
        BagDataInfo = BagData.Equipments.Select((vale, index) => new { Index = index, EquipmentInfo = vale }).ToDictionary(x => x.Index, x => x.EquipmentInfo);
        Player = GameObject.FindWithTag("Player");
        if (Player == null)
        {
            Debug.Log("�����ڳ�����û�ҵ�Player");
        }
        Tag = 0;
        SelectIdx = -1;
        BeforeItem = null;
        TabView.AddRefreshEvent(RefreshTabView);
        TabNameList = new string[]{ "װ��", "����", "����" };
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
                    Debug.LogFormat("IdΪ{}��Iconû�ҵ�", BagDataInfo[index].SpriteGroupEntry.Id);
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
        //更新
        PlayerEquipSlot.RefreshAllEquip();
    }
    public override void onExit()
    {
        BagPlayer.SetActive(false);
    }
}
