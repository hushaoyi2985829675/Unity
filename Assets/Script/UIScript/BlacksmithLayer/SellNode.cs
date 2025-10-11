using System.Collections;
using System.Collections.Generic;
using Equip;
using HeroEditor.Common.Enums;
using Shop;
using UnityEngine;
using UnityEngine.UI;

public class SellNode : PanelBase
{
    [SerializeField] private int sizeNum = 30;
    [SerializeField] private EquipTabView tabView;
    [SerializeField] private GridView gridView;
    [SerializeField] private Text nameText;
    [SerializeField] private Text descText;
    [SerializeField] private Text NumText;
    [SerializeField] private GameObject UINode;
    [SerializeField] private CardNode cardNode;
    [SerializeField] private Button addBtn;
    [SerializeField] private Button subBtn;
    [SerializeField] private Slider numSlider;
    [SerializeField] private Transform resNode;
    [SerializeField] private GameObject resCardNodeRef;
    [SerializeField] private Button sellBtn;
    [SerializeField] private GameObject numNode;
    private int selNum;
    private EquipmentInfo selInfo;
    private List<EquipmentInfo> equipList;
    private int selIdx;
    private CardNode oldCardNode;
    private List<ResClass> resList;
    public override void onEnter(params object[] data)
    {
        equipList = new List<EquipmentInfo>();
        selIdx = -1;
        gridView.SetItemAndRefresh(sizeNum);
        gridView.AddRefreshEvent(CreateItem);
        tabView.InitTabView(ChangeTag);
        addBtn.onClick.AddListener(AddBtnClick);
        subBtn.onClick.AddListener(SubBtnClick);
        numSlider.minValue = 1;
        numSlider.onValueChanged.AddListener((value) =>
        {
            selNum = (int) value;
            SetSelNumText();
        });
        sellBtn.onClick.AddListener(SellBtnClick);
    }

    public override void onShow(params object[] data)
    {
        UINode.SetActive(false);
        selIdx = -1;
        tabView.SetSelTag((int) EquipmentPart.MeleeWeapon1H);
    }

    private void ChangeTag(int tag)
    {
        selIdx = -1;
        oldCardNode = null;
        equipList = GameDataManager.Instance.GetBagData((EquipmentPart) tag);
        gridView.RefreshAllAction();
    }

    private void CreateItem(int index, GameObject item)
    {
        CardNode cardNode = item.GetComponent<CardNode>();
        if (equipList.Count > index)
        {
            EquipmentInfo equipmentInfo = equipList[index];
            cardNode.SetCardData(GoodsType.Equip, equipmentInfo.id, equipmentInfo.num);
            cardNode.SetSelState(selIdx == index);
            cardNode.SetClick(() =>
            {
                if (selIdx == index)
                {
                    return;
                }
                selIdx = index;
                cardNode.SetSelState(true);
                if (oldCardNode)
                {
                    oldCardNode.SetSelState(false);
                }

                oldCardNode = cardNode;
                RefreshDetails();
            });
        }

        cardNode.SetIsNull(equipList.Count <= index);
    }

    private void RefreshDetails()
    {
        selInfo = equipList[selIdx];
        numSlider.value = 1;
        selNum = 1;
        RefreshSlider();
        UINode.SetActive(true);
        //名字
        nameText.text = Ui.Instance.GetGoodName((int) GoodsType.Equip, selInfo.id);
        descText.text = Ui.Instance.GetGoodDes((int) GoodsType.Equip, selInfo.id);
        cardNode.SetCardData(GoodsType.Equip, selInfo.id);
        SetSelNumText();
        RefreshResNode();
    }

    private void RefreshSlider()
    {
        numNode.SetActive(selInfo.num > 1);
        if (selInfo.num > 1)
        {
            numSlider.maxValue = selInfo.num;
        }
    }

    private void SetSelNumText()
    {
        NumText.text = string.Format("数量:{0}", selNum);
    }

    private void AddBtnClick()
    {
        numSlider.value = Mathf.Min(numSlider.value + 1, selInfo.num);
    }

    private void SubBtnClick()
    {
        numSlider.value = Mathf.Max(numSlider.value - 1, 1);
    }

    private void RefreshResNode()
    {
        Ui.Instance.RemoveAllChildren(resNode);
        EquipInfo equipInfo = Ui.Instance.GetEquipInfo(selInfo.id);
        resList = Ui.Instance.FormatResStr(equipInfo.sellPrice);
        foreach (ResClass res in resList)
        {
            CardNode cardNode = Instantiate(resCardNodeRef, resNode).GetComponent<CardNode>();
            cardNode.SetCardData(res);
        }
    }

    private void SellBtnClick()
    {
        int oldNum = selInfo.num;
        GameDataManager.Instance.RemoveEquipData(selInfo.id, selNum);
        foreach (ResClass res in resList)
        {
            GameDataManager.Instance.AddRes(res);
        }

        if (oldNum <= selNum)
        {
            UINode.SetActive(false);
            selIdx = -1;
            gridView.RefreshAllAction();
        }
        else
        {
            numSlider.value = 1;
            selNum = 1;
            RefreshSlider();
            gridView.RefreshItem(selIdx);
        }

        Ui.Instance.ShowReward(resList);
    }

    public override void onExit()
    {
        
    }
}
