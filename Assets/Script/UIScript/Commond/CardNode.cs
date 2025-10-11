using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardNode : PanelBase
{
    [SerializeField] private Image icon;
    [SerializeField] private GameObject checkmark;
    [SerializeField] private Text numText;
    [SerializeField] private Text lvText;
    [SerializeField] private Text nameText;
    [SerializeField] private Button button;
    [SerializeField] private GameObject UINode;
    [SerializeField] private GameObject descLayer;
    private Action callback;
    private int id;
    private GoodsType type;


    public override void onEnter(params object[] data)
    {
        callback = DefaultClick;
        button.onClick.AddListener(() => { callback(); });
    }

    public override void onShow(params object[] data)
    {
    }

    public void DefaultClick()
    {
        UIManager.Instance.OpenLayer(descLayer, new object[] {type, id});
    }
    public void SetCardData(GoodsType goodsType, int id, int num = -1)
    {
        RefreshUI(goodsType, id, num);
    }

    public void SetCardData(ResClass resClass)
    {
        RefreshUI(resClass.goodsType, resClass.resourceId, (int) resClass.num);
    }

    private void RefreshUI(GoodsType goodsType, int id, int num = -1)
    {
        Sprite sprite = Ui.Instance.GetGoodIcon((int) goodsType, id);
        icon.sprite = sprite;
        if (num > 0)
        {
            numText.gameObject.SetActive(true);
            numText.text = num.ToString();
        }
        else
        {
            numText.gameObject.SetActive(false);
        }

        //等级
        if (goodsType == GoodsType.Equip)
        {
            lvText.text = $"Lv.{Ui.Instance.GetEquipInfo(id).lv.ToString()}";
        }
        else
        {
            lvText.gameObject.SetActive(false);
        }

        this.id = id;
        this.type = goodsType;
        Debug.Log(button.onClick.GetPersistentEventCount());
    }
    

    public void ShowName()
    {
        string name = Ui.Instance.GetGoodName((int) type, id);
        nameText.gameObject.SetActive(true);
        nameText.text = name;
    }

    public void SetClick(Action action)
    {
        callback = action;
    }

    public void SetSelState(bool isSel)
    {
        checkmark.SetActive(isSel);
    }

    public void SetIsNull(bool isNull)
    {
        UINode.SetActive(!isNull);
        if (isNull)
        {
            callback = () => { };
        }
    }


    public override void onExit()
    {
       
    }
}