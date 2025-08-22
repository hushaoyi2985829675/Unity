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
    public Transform tabView;
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
    private int selTag;

    public override void onEnter(params object[] data)
    {
        AddListener();
        InitData();
        refreshUI();
    }

    private void AddListener()
    {
        gridView.AddRefreshEvent(CreateItem);
        button.onClick.AddListener(FoundClick);
    }

    private void InitData()
    {
        selId = 0;
        selTag = -1;
        FoundConfigList = Resources.Load<FoundryConfig>("Configs/Data/FoundryConfig").foundryInfoList;
        equipInfoList = Resources.Load<EquipConfig>("Configs/Data/EquipConfig").equipInfoList
            .ToDictionary(key => key.equip, value => value);
        FoundryClassifyList = new Dictionary<int, List<FoundryInfo>>();
        //装备分类
        foreach (var foundryInfo in FoundConfigList)
        {
            Equip.EquipInfo equipInfo = equipInfoList[foundryInfo.foundry];
            int idx;
            switch ((EquipmentPart) equipInfo.part)
            {
                case EquipmentPart.MeleeWeapon1H:
                case EquipmentPart.MeleeWeapon2H:
                case EquipmentPart.MeleeWeaponPaired:
                    idx = (int) EquipClassif.Weapon;
                    break;
                case EquipmentPart.Armor:
                    idx = (int) EquipClassif.Armor;
                    break;
                default:
                    idx = (int) EquipClassif.Helmet;
                    break;
            }

            if (!FoundryClassifyList.ContainsKey(idx))
            {
                FoundryClassifyList[idx] = new List<FoundryInfo>();
            }

            FoundryClassifyList[idx].Add(foundryInfo);
        }
    }

    private void refreshUI()
    {
        InitTabView();
        if (selId == 0)
        {
            UINode.gameObject.SetActive(false);
        }
    }

    private void InitTabView()
    {
        for (int i = 0; i < tabView.childCount; i++)
        {
            Toggle toggle = tabView.GetChild(i).GetComponent<Toggle>();
            toggle.isOn = selTag == i;
            int n = i;
            toggle.onValueChanged.AddListener((isOn) =>
            {
                Debug.Log(n);
                ChangeTag(isOn, n);
            });
        }
    }

    private void ChangeTag(bool isOn, int tag)
    {
        if (selTag == tag || !isOn)
        {
            return;
        }

        Debug.Log(tag);
        selTag = tag;
        // if (!FoundryClassifyList.ContainsKey(tag))
        // {
        //     FoundryClassifyList[tag] = GetClassifyList(tag);
        //     Equip.EquipInfo equipInfo = equipInfoList[tag];
        // }

        FoundryList = FoundryClassifyList.ContainsKey(tag) ? FoundryClassifyList[tag] : null;
        if (FoundryList == null)
        {
            gridView.SetItemNumAndSpace(0, 4, 30, 30);
        }
        else
        {
            gridView.SetItemNumAndSpace(FoundryList.Count, 4, 30, 30);
        }
    }

    private void GetClassifyList(int tag)
    {
        // FoundryInfo foundryInfo = FoundConfigList[index];
        // Equip.EquipInfo equip = equipInfoList.Find((obj) => foundryInfo.foundry == obj.equip);
    }

    private void CreateItem(int index, GameObject item)
    {
        FoundryInfo foundryInfo = FoundryList[index];
        // Equip.EquipInfo equip = equipInfoList.Find((obj) => foundryInfo.foundry == obj.equip);
        CardNode cardNode = item.GetComponent<CardNode>();
        cardNode.SetCardData(GoodsType.Equip, foundryInfo.foundry);
        cardNode.SetSelState(selId == foundryInfo.foundry);
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
            ResClass res = resList[i];
            GameObject cardNode = Instantiate(materialCardNode, materialNode);
            float x = -(resList.Count - 1) * 150 / 2 + i * 150;
            cardNode.transform.localPosition = new Vector3(x, 0, 0);
            if (i == resList.Count - 1)
            {
                GameObject horizontalLine = Ui.Instance.GetChild(cardNode.transform, "HorizontalLine");
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
        SpriteGroupEntry entry = Ui.Instance.GetEquipEntry((EquipmentPart) equipInfo.part, equipInfo.id);
        GameDataManager.Instance.AddEquip(entry, (EquipmentPart) equipInfo.part);
        Ui.Instance.ShowFlutterView("获得" + equipInfo.name);
    }

    public override void onExit()
    {
    }
}