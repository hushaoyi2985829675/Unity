using System;
using System.Collections;
using System.Collections.Generic;
using ShopNs;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : PanelBase
{
    [SerializeField]
    private Transform cardNodeTrans;

    [SerializeField]
    private Text nameText;

    [SerializeField]
    private GameObject cardNodeRef;

    [SerializeField]
    private Text limitedBuy;

    [SerializeField]
    private Button buyBtn;

    [SerializeField]
    private Text buyText;

    [SerializeField]
    private Image buyImage;
    private ShopInfo shopInfo;
    private int buyNum;
    private ResClass resClass;

    public override void onEnter(params object[] data)
    {
    }

    public override void onShow(params object[] data)
    {
    }

    public override void onExit()
    {
    }
    public void InitData(ShopInfo info, int buyNum, Action callback)
    {
        shopInfo = info;
        this.buyNum = buyNum;
        resClass = Ui.Instance.FormatResStr(shopInfo.price)[0];
        buyBtn.onClick.RemoveAllListeners();
        buyBtn.onClick.AddListener(() => { callback(); });
        RefreshUI();
    }

    private void RefreshUI()
    {
        CardNode cardNode = AddUINode<CardNode>(cardNodeRef, cardNodeTrans);
        cardNode.SetCardData((GoodsType) shopInfo.goodType, shopInfo.id, shopInfo.num);
        nameText.text = Ui.Instance.GetGoodName(shopInfo.goodType, shopInfo.id);
        buyImage.sprite = Ui.Instance.GetGoodIcon((int) GoodsType.Resource, resClass.resourceId);
        BuyRefreshUI();
    }

    private void BuyRefreshUI()
    {
        limitedBuy.text = string.Format("限购：{0}/{1}", buyNum, shopInfo.limitedBuy);
        int resNum = GameDataManager.Instance.GetResNum(resClass.resourceId);
        Color color = resNum >= resClass.num
            ? new Color(0x75 / 255f, 0xca / 255f, 0x6a / 255f)
            : new Color(0xff / 255f, 0x7e / 255f, 0x7e / 255f);
        buyText.color = color;
        buyText.text = string.Format("{0}/{1}", resNum, resClass.num);
        bool isGray = buyNum >= shopInfo.limitedBuy;
        Ui.Instance.SetGray(buyBtn.image, isGray);
    }

    public void RefreshItem(int buyNum)
    {
        this.buyNum = buyNum;
        BuyRefreshUI();
    }
}