using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Equip;
using Foundry;
using HeroEditor.Common;
using HeroEditor.Common.Enums;
using UnityEngine;
using UnityEngine.UI;

enum EquipClassif
{
    Weapon = 0,
    Armor = 1,
    Helmet = 2
}

public class foundryNode : PanelBase
{
    public GridView gridView;
    public EquipTabView tabView;
    public GameObject materialCardNode;
    public Transform materialNode;
    public Transform UINode;
    public Text nameText;
    public Text desText;
    public CardNode cardNode;
    public Button button;
    private List<FoundryInfo> FoundConfigList;
    private Dictionary<int, Equip.EquipInfo> equipInfoList;
    private List<FoundryInfo> FoundryList;
    private Dictionary<int, List<FoundryInfo>> FoundryClassifyList;
    
    private CardNode oldCardNode;
    private int selId;

    public override void onEnter(params object[] data)
    {
        FoundConfigList = Resources.Load<FoundryConfig>("Configs/Data/FoundryConfig").foundryInfoList;
        equipInfoList = Resources.Load<EquipConfig>("Configs/Data/EquipConfig").equipInfoList
            .ToDictionary(key => key.equip, value => value);
        FoundryClassifyList = new Dictionary<int, List<FoundryInfo>>();
        AddListener();
        InitData();
        tabView.InitTabView(ChangeTag);
        refreshUI();
    }

    public override void onShow(object[] data)
    {
        selId = 0;
        refreshUI();
        tabView.SetSelTag(EquipmentPart.MeleeWeapon1H);
    }
    private void AddListener()
    {
        gridView.AddRefreshEvent(CreateItem);
        button.onClick.AddListener(FoundClick);
    }

    private void InitData()
    {
        //装备分类
        foreach (var foundryInfo in FoundConfigList)
        {
            Equip.EquipInfo equipInfo = equipInfoList[foundryInfo.foundry];
            if (!FoundryClassifyList.ContainsKey(equipInfo.part))
            {
                FoundryClassifyList[equipInfo.part] = new List<FoundryInfo>();
            }

            FoundryClassifyList[equipInfo.part].Add(foundryInfo);
        }
    }

    private void refreshUI()
    {
        if (selId == 0)
        {
            UINode.gameObject.SetActive(false);
        }
    }

    private void ChangeTag(int tag)
    {
        FoundryList = FoundryClassifyList.ContainsKey(tag) ? FoundryClassifyList[tag] : null;
        if (FoundryList == null)
        {
            gridView.SetItemAndRefresh(0);
        }
        else
        {
            gridView.SetItemAndRefresh(FoundryList.Count);
        }
    }
    
    private void CreateItem(int index, GameObject item)
    {
        FoundryInfo foundryInfo = FoundryList[index];
        // Equip.EquipInfo equip = equipInfoList.Find((obj) => foundryInfo.foundry == obj.equip);
        CardNode cardNode = item.GetComponent<CardNode>();
        cardNode.SetCardData(GoodsType.Equip, foundryInfo.foundry);
        cardNode.SetSelState(selId == foundryInfo.foundry);
        if (selId == foundryInfo.foundry)
        {
            oldCardNode = cardNode;
        }
        cardNode.SetClick(() =>
        {
            if (selId == foundryInfo.foundry)
            {
                return;
            }

            selId = foundryInfo.foundry;
            cardNode.SetSelState(true);
            if (oldCardNode != null)
            {
                oldCardNode.SetSelState(false);
            }

            oldCardNode = cardNode;
            RefreshFoundry();
        });
    }

    private void RefreshFoundry()
    {
        UINode.gameObject.SetActive(true);
        //刷新名字
        string name = Ui.Instance.GetGoodName((int) GoodsType.Equip, selId);
        nameText.text = name;
        string desc = Ui.Instance.GetGoodDes((int) GoodsType.Equip, selId);
        desText.text = desc;
        cardNode.SetCardData(GoodsType.Equip, selId, 1);
        //刷新材料
        CreateMaterial();
    }

    public void CreateMaterial()
    {
        Ui.Instance.RemoveAllChildren(materialNode);
        Equip.EquipInfo equip = Ui.Instance.GetEquipInfo(selId);
        List<ResClass> resList = Ui.Instance.FormatStr(equip.synthesisRoute);
        for (int i = 0; i < resList.Count; i++)
        {
            ResClass resClass = resList[i];
            GameObject temp = Instantiate(materialCardNode, materialNode);
            float x = -(resList.Count - 1) * 150 / 2 + i * 150;
            temp.transform.localPosition = new Vector3(x, 0, 0);
            CardNode cardNode = temp.transform.Find("CardNode").GetComponent<CardNode>();
            cardNode.SetSelState(false);
            cardNode.SetCardData(GoodsType.Ingredient, resClass.resourceId, resClass.num);
            if (i == resList.Count - 1)
            {
                GameObject horizontalLine = Ui.Instance.GetChild(temp.transform, "HorizontalLine");
                horizontalLine.SetActive(false);
            }
        }
    }

    public void FoundClick()
    {
        if (selId == 0)
        {
            return;
        }

        Equip.EquipInfo equipInfo = Ui.Instance.GetEquipInfo(selId);
        GameDataManager.Instance.AddEquip(selId);
        Ui.Instance.ShowFlutterView("获得" + equipInfo.name);
    }

    public override void onExit()
    {
    }
}