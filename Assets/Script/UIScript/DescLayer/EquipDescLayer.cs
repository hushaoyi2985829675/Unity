using System.Collections;
using System.Collections.Generic;
using EquipNs;
using HeroEditor.Common.Enums;
using UnityEngine;
using UnityEngine.UI;

public class EquipDescLayer : PanelBase
{
    [SerializeField]
    private GameObject weaponNode;

    [SerializeField]
    private GameObject armorNode;

    [SerializeField]
    private Image weaponIcon;

    [SerializeField]
    private Image armorIcon;

    [SerializeField]
    private Text nameText;

    [SerializeField]
    private Text lvText;

    [SerializeField]
    private Text descText;

    [SerializeField]
    private Transform attrParent;

    [SerializeField]
    private Transform wayParent;

    private GameObject attrPrefab;
    private GameObject wayNodeRef;

    private int type;
    private int id;

    public override void onEnter(params object[] data)
    {
        type = (int) GoodsType.Equip;
        attrPrefab = Ui.Instance.GetLayerRef("DescLayer/AttrText");
        wayNodeRef = Ui.Instance.GetLayerRef("DescLayer/WayNode");
    }

    public override void onShow(params object[] data)
    {
        id = (int) data[0];
        RefreshUI();
    }

    private void RefreshUI()
    {
        armorNode.gameObject.SetActive(false);
        weaponNode.gameObject.SetActive(false);
        //刷新Icon
        EquipInfo equipInfo = Ui.Instance.GetEquipInfo(id);
        if (equipInfo.part == (int) EquipmentPart.Armor)
        {
            armorNode.gameObject.SetActive(true);
            armorIcon.sprite = Ui.Instance.GetGoodIcon(type, id);
        }
        else
        {
            weaponNode.gameObject.SetActive(true);
            weaponIcon.sprite = Ui.Instance.GetEquipEntry((EquipmentPart) equipInfo.part, equipInfo.id)?.Sprite;
        }

        nameText.text = Ui.Instance.GetGoodName(type, id);
        lvText.text = $"Lv.{equipInfo.lv.ToString()}";
        descText.text = Ui.Instance.GetGoodDes(type, id);
        //刷新属性
        RefreshAttr(equipInfo);
        //刷新获取途径
        RefreshWay(equipInfo);
    }

    private void RefreshWay(EquipInfo equipInfo)
    {
        string[] wayIds = equipInfo.ways.Split(",");
        for (int i = 0; i < wayIds.Length; i++)
        {
            int wayId = int.Parse(wayIds[i]);
            AddUINode(wayNodeRef, wayParent, new object[] {wayId});
        }
    }

    private void RefreshAttr(EquipInfo equipInfo)
    {
        string attr = equipInfo.attr;
        List<AttrClass> attrList = Ui.Instance.FormatAttrStr(attr);
        foreach (AttrClass attrClass in attrList)
        {
            Text attrText = Instantiate(attrPrefab, attrParent).GetComponent<Text>();
            string value = Ui.Instance.GetAttrText(attrClass, true);
            attrText.text = value;
        }
    }

    public override void onExit()
    {
        Ui.Instance.RemoveAllChildren(attrParent);
    }
}