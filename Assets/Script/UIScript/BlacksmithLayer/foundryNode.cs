using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EquipNs;
using FoundryNs;
using HeroEditor.Common;
using HeroEditor.Common.Enums;
using UnityEngine;
using UnityEngine.UI;

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
    private Dictionary<int, EquipInfo> equipInfoList;
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
        tabView.SetSelTag((int) EquipmentPart.MeleeWeapon1H);
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
            EquipInfo equipInfo = equipInfoList[foundryInfo.foundry];
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
        CloseNodeAllUINode(materialNode);
        EquipInfo equip = Ui.Instance.GetEquipInfo(selId);
        List<ResClass> resList = Ui.Instance.FormatResStr(equip.synthesisRoute, GoodsType.Ingredient);
        for (int i = 0; i < resList.Count; i++)
        {
            ResClass resClass = resList[i];
            GameObject temp = AddUINode(materialCardNode, materialNode,
                new object[] {resClass.resourceId, resClass.num, i < resList.Count - 1}).gameObject;
            float x = -(resList.Count - 1) * 150 / 2 + i * 150;
            temp.transform.localPosition = new Vector3(x, 0, 0);
        }
    }

    public void FoundClick()
    {
        if (selId == 0)
        {
            return;
        }
        GameDataManager.Instance.AddEquip(selId);
        Ui.Instance.ShowReward(new ResClass(GoodsType.Equip, selId, 1));
    }

    public override void onExit()
    {
    }
}