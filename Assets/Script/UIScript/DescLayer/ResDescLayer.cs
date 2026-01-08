using System.Collections;
using System.Collections.Generic;
using EquipNs;
using GoodWaysNs;
using HeroEditor.Common.Enums;
using ResourceNs;
using UnityEngine;
using UnityEngine.UI;

public class ResDescLayer : PanelBase
{
    [SerializeField]
    private Image goodIcon;

    [SerializeField]
    private Text nameText;

    [SerializeField]
    private Text descText;

    [SerializeField]
    private Transform wayNodeTrans;

    [SerializeField]
    private Text goodNumText;

    private int id;

    public override void onEnter(params object[] data)
    {
    }

    public override void onShow(params object[] data)
    {
        id = (int) data[0];

        RefreshUI();
    }

    private void RefreshUI()
    {
        goodIcon.sprite = Ui.Instance.GetGoodIcon((int) GoodsType.Resource, id);
        nameText.text = Ui.Instance.GetGoodName((int) GoodsType.Resource, id);
        descText.text = Ui.Instance.GetGoodDes((int) GoodsType.Resource, id);
        goodNumText.text = string.Format("拥有: {0}", GameDataManager.Instance.GetResNum(id));
        //路径
        GameObject wayNodeRef = Ui.Instance.GetLayerRef("DescLayer/WayNode");
        ResourceInfo resourceInfo = Ui.Instance.GetResourceInfo(id);
        string[] wayIds = resourceInfo.ways.Split(",");
        for (int i = 0; i < wayIds.Length; i++)
        {
            int wayId = int.Parse(wayIds[i]);
            AddUINode(wayNodeRef, wayNodeTrans, new object[] {wayId});
        }
    }


    public override void onExit()
    {
    }
}