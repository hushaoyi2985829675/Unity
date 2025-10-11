using System;
using System.Collections;
using System.Collections.Generic;
using HeroEditor.Common.Enums;
using UnityEngine;

public class BagEquipNode : PanelBase
{
    [SerializeField] private EquipTabView equipTabView;

    [SerializeField] private GridView gridView;

    //当前选择类型的装备信息
    private List<EquipmentInfo> selEquipList;
    private int selTag;
    private int selIdx;
    private CardNode oldCardNode;
    private Action<GoodsType, int, GameObject, EquipmentPart> callback;

    public override void onEnter(params object[] data)
    {
        callback = data[0] as Action<GoodsType, int, GameObject, EquipmentPart>;
        gridView.AddRefreshEvent(CreateGridViewItem);
        equipTabView.InitTabView(ChangeTag);
    }

    public override void onShow(params object[] data)
    {
        selTag = (int) EquipmentPart.MeleeWeapon1H;
        oldCardNode = null;
        selIdx = -1;
        RefreshUI();
    }

    private void RefreshUI()
    {
        equipTabView.SetSelTag(selTag);
    }

    private void ChangeTag(int tag)
    {
        selTag = tag;
        selIdx = -1;
        selEquipList = GameDataManager.Instance.GetBagData((EquipmentPart) selTag);
        gridView.SetItemAndRefresh(20);
    }

    private void CreateGridViewItem(int index, GameObject item)
    {
        CardNode cardNode = item.GetComponent<CardNode>();
        if (index < selEquipList.Count)
        {
            EquipmentInfo equipmentInfo = selEquipList[index];
            cardNode.SetCardData(GoodsType.Equip, equipmentInfo.id, equipmentInfo.num);
            cardNode.SetClick(() =>
            {
                if (selIdx != index)
                {
                    if (oldCardNode != null)
                    {
                        oldCardNode.SetSelState(false);
                    }

                    cardNode.SetSelState(true);
                    oldCardNode = cardNode;
                }

                selIdx = index;
                callback(GoodsType.Equip, equipmentInfo.id, item, (EquipmentPart) selTag);
            });
            cardNode.SetSelState(selIdx == index);
        }

        cardNode.SetIsNull(index >= selEquipList.Count);
    }

    public override void onExit()
    {
        gridView.Clear();
    }
}