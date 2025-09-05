using System.Collections;
using System.Collections.Generic;
using HeroEditor.Common.Enums;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : PanelBase
{
    [SerializeField] private Image EquipIcon;
    [SerializeField] private Image GoodIcon;
    [SerializeField] private Text nameText;
    [SerializeField] private Text descText;
    private int type;
    private int id;

    public override void onEnter(params object[] data)
    {
    }

    public override void onShow(params object[] data)
    {
        type = (int) data[0];
        id = (int) data[1];
        EquipIcon.gameObject.SetActive(false);
        GoodIcon.gameObject.SetActive(false);
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (type == (int) GoodsType.Equip)
        {
            EquipIcon.gameObject.SetActive(true);
            //刷新Icon
            Equip.EquipInfo equipInfo = Ui.Instance.GetEquipInfo(id);
            EquipIcon.sprite = Ui.Instance.GetEquipEntry((EquipmentPart) equipInfo.part, equipInfo.id)?.Sprite;
        }
        else
        {
            GoodIcon.gameObject.SetActive(true);
            GoodIcon.sprite = Ui.Instance.GetGoodIcon(type, id);
        }

        nameText.text = Ui.Instance.GetGoodName(type, id);
        descText.text = Ui.Instance.GetGoodDes(type, id);
    }

    public override void onExit()
    {
    }
}