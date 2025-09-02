using System;
using System.Collections;
using System.Collections.Generic;
using Shop;
using UnityEngine;

public class ShopNode : PanelBase
{
    public GridView gridView;
    private List<ShopInfo> ShopConfig;
    public List<ShopBuyInfo> ShopBuyData;
    public override void onEnter(params object[] data)
    {
        ShopBuyData = GameDataManager.Instance.GetShopBuyList();
        ShopConfig = (List<ShopInfo>) data[0];
        gridView.AddRefreshEvent(CreateItem);
        gridView.SetItemAndRefresh(ShopConfig.Count);
    }

    public void CreateItem(int index, GameObject item)
    {
        ShopInfo info = ShopConfig[index];
        int? curBuyNum = ShopBuyData.Find((obj) => obj.type == info.goodType && obj.id == info.id)?.buyNum;
        if (curBuyNum == null)
        {
            curBuyNum = 0;
        }

        ShopItem shopItem = item.GetComponent<ShopItem>();
        shopItem.InitData(info, curBuyNum.Value, () =>
        {
            Debug.Log(info.id);
        });
    }

    public void BuyClick(ShopInfo shopInfo, ShopItem shopItem, ShopInfo info)
    {
        Debug.Log(shopInfo.id);
        // ResClass resClass = Ui.Instance.FormatStr(shopInfo.price)[0];
        // int resNum = GameDataManager.Instance.GetResNum(resClass.resourceId);
        // if (resNum < resClass.num)
        // {
        //     Ui.Instance.ShowFlutterView(Ui.Instance.GetGoodName((int) GoodsType.Resource, resClass.resourceId) + "不足!");
        //     return;
        // }
        //
        // GameDataManager.Instance.AddBuyInfoData(shopInfo.goodType, shopInfo.id, resClass.resourceId, resClass.num);
        // int curBuyNum = ShopBuyData.Find((obj) => obj.type == info.goodType && obj.id == info.id).buyNum;
        // shopItem.RefreshItem(curBuyNum);
        // List<ResClass> resList = new List<ResClass>()
        // {
        //     new ResClass(shopInfo.id, shopInfo.num)
        // };
        // Ui.Instance.ShowReward(resList, (GoodsType) shopInfo.goodType);
    }

    public override void onExit()
    {
    }
}