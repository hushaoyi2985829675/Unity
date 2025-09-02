using System;
using System.Collections;
using System.Collections.Generic;
using Shop;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Text nameText;
    public CardNode cardNode;
    public Text limitedBuy;
    public Button buyBtn;
    public Text buyText;
    public Image buyImage;
    private ShopInfo shopInfo;
    private int buyNum;
    private ResClass resClass;

    public void InitData(ShopInfo info, int buyNum, Action callback)
    {
        shopInfo = info;
        this.buyNum = buyNum;
        resClass = Ui.Instance.FormatStr(shopInfo.price)[0];
        buyBtn.onClick.AddListener(() => { callback(); });
        RefreshUI();
    }

    private void RefreshUI()
    {
        nameText.text = Ui.Instance.GetGoodName(shopInfo.goodType, shopInfo.id);
        cardNode.SetCardData((GoodsType) shopInfo.goodType, shopInfo.id, shopInfo.num);
        buyImage.sprite = Ui.Instance.GetGoodIcon(GoodsType.Resource, resClass.resourceId);
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
        Ui.Instance.SetGray(buyBtn, isGray);
    }

    public void RefreshItem(int buyNum)
    {
        this.buyNum = buyNum;
        BuyRefreshUI();
    }
}