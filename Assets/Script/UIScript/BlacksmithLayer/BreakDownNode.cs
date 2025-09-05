using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Equip;
using Foundry;
using HeroEditor.Common;
using HeroEditor.Common.Enums;
using Ingredient;
using UnityEngine;
using UnityEngine.UI;


public class BreakDownNode : PanelBase
{
    public GridView gridView;
    public GridView materialGridView;
    public EquipTabView tabView;
    public Transform UINode;
    public Text nameText;
    public Text desText;
    public Button button;
    private Dictionary<int, List<ResClass>> ingredientList;
    private Dictionary<int, List<EquipmentInfo>> EquipClassifyList;
    private List<EquipmentInfo> equipList;
    private CardNode oldCardNode;
    private int selId;

    public override void onEnter(params object[] data)
    {
        AddListener();
        InitData();
        tabView.InitTabView(ChangeTag);
        refreshUI();
    }

    public override void onShow(object[] data)
    {
        refreshUI();
        tabView.SetSelTag(EquipmentPart.MeleeWeapon1H);
    }

    private void AddListener()
    {
        gridView.AddRefreshEvent(CreateItem);
        materialGridView.AddRefreshEvent(CreateIngredientItem);
        button.onClick.AddListener(FoundClick);
    }

    private void InitData()
    {
        ingredientList = new Dictionary<int, List<ResClass>>();
        EquipClassifyList = new Dictionary<int, List<EquipmentInfo>>();
    }

    private void refreshUI()
    {
        selId = 0;
        if (selId == 0)
        {
            UINode.gameObject.SetActive(false);
        }
    }

    private void ChangeTag(int tag)
    {
        equipList = GameDataManager.Instance.GetBagData((EquipmentPart) tag);
        gridView.SetItemAndRefresh(equipList.Count);
    }

    private void CreateItem(int index, GameObject item)
    {
        EquipmentInfo equipInfo = equipList[index];
        CardNode cardNode = item.GetComponent<CardNode>();
        cardNode.SetCardData(GoodsType.Equip, equipInfo.id);
        cardNode.SetSelState(selId == equipInfo.id);
        if (selId == equipInfo.id)
        {
            oldCardNode = cardNode;
        }

        cardNode.SetClick(() =>
        {
            if (selId == equipInfo.id)
            {
                return;
            }

            selId = equipInfo.id;
            cardNode.SetSelState(true);
            if (oldCardNode != null)
            {
                oldCardNode.SetSelState(false);
            }

            oldCardNode = cardNode;
            RefreshText();
            refreshIngredient();
        });
    }

    private void CreateIngredientItem(int index, GameObject item)
    {
        CardNode cardNode = item.GetComponent<CardNode>();
        cardNode.SetSelState(true);
        List<ResClass> ingredient = ingredientList[selId];
        cardNode.SetCardData(GoodsType.Ingredient, ingredient[index].resourceId, ingredient[index].num);
    }

    private void RefreshText()
    {
        UINode.gameObject.SetActive(true);
        //刷新名字
        string name = Ui.Instance.GetGoodName((int) GoodsType.Equip, selId);
        nameText.text = name;
        string desc = Ui.Instance.GetGoodDes((int) GoodsType.Equip, selId);
        desText.text = desc;
    }

    private void refreshIngredient()
    {
        InitIngredient();
        materialGridView.SetItemAndRefresh(ingredientList[selId].Count);
    }

    private void InitIngredient()
    {
        if (!ingredientList.ContainsKey(selId))
        {
            ingredientList[selId] = new List<ResClass>();
            Equip.EquipInfo equipInfo = Ui.Instance.GetEquipInfo(selId);
            List<ResClass> resList = Ui.Instance.FormatStr(equipInfo.synthesisRoute);
            foreach (var res in resList)
            {
                ingredientList[selId].Add(res);
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