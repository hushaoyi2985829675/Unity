using System.Collections;
using System.Collections.Generic;
using EquipNs;
using HeroEditor.Common.Enums;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : PanelBase
{
    [SerializeField] private GameObject goodNode;
    [SerializeField] private GameObject equipNode;
    [SerializeField] private GameObject weaponNode;
    [SerializeField] private GameObject armorNode;
    [SerializeField] private Image weaponIcon;
    [SerializeField] private Image armorIcon;
    [SerializeField] private Image goodIcon;
    [SerializeField] private Text nameText;
    [SerializeField] private Text lvText;
    [SerializeField] private Text descText;
    [SerializeField] private Transform attrParent;
    [SerializeField] private GameObject attrPrefab;
    private int type;
    private int id;

    public override void onEnter(params object[] data)
    {
    }

    public override void onShow(params object[] data)
    {
        type = (int) data[0];
        id = (int) data[1];
        equipNode.gameObject.SetActive(false);
        goodNode.gameObject.SetActive(false);
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (type == (int) GoodsType.Equip)
        {
            equipNode.gameObject.SetActive(true);
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

            lvText.text = $"Lv.{equipInfo.lv.ToString()}";
            //刷新属性
            RefreshAttr(equipInfo);
        }
        else
        {
            goodNode.gameObject.SetActive(true);
            goodIcon.sprite = Ui.Instance.GetGoodIcon(type, id);
        }

        nameText.text = Ui.Instance.GetGoodName(type, id);
        descText.text = Ui.Instance.GetGoodDes(type, id);
    }

    public void RefreshAttr(EquipInfo equipInfo)
    {
        string attr = equipInfo.attr;
        List<AttrClass> attrList = Ui.Instance.FormatAttrStr(attr);
        foreach (AttrClass attrClass in attrList)
        {
            Text attrText = Instantiate(attrPrefab, attrParent).GetComponent<Text>();
            string value = Ui.Instance.GetAttrText(attrClass);
            attrText.text = value;
        }
    }

    public override void onExit()
    {
        Ui.Instance.RemoveAllChildren(attrParent);
    }
}