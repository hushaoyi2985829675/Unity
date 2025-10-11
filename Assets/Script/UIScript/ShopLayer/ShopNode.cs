using System;
using System.Collections;
using System.Collections.Generic;
using Equip;
using HeroEditor.Common.Enums;
using Shop;
using UnityEngine;
using UnityEngine.UI;

public class ShopNode : PanelBase
{
    public GridView gridView;
    public EquipTabView tabView;
    private Dictionary<int, List<ShopInfo>> ShopConfigDic;
    private List<ShopBuyInfo> ShopBuyData;
    private List<ShopInfo> shopList;
    public override void onEnter(params object[] data)
    {
        ShopConfigDic = new Dictionary<int, List<ShopInfo>>();
        ShopBuyData = GameDataManager.Instance.GetShopBuyList();
        gridView.AddRefreshEvent(CreateItem);
        InitData((List<ShopInfo>) data[0]);
        tabView.InitTabView(ChangeTag);
    }

    public override void onShow(object[] data)
    {
        tabView.SetSelTag((int) EquipmentPart.MeleeWeapon1H);
    }

    public void InitData(List<ShopInfo> ShopConfig)
    {
        foreach (ShopInfo shopInfo in ShopConfig)
        {
            EquipInfo equipInfo = Ui.Instance.GetEquipInfo(shopInfo.id);
            if (ShopConfigDic.ContainsKey(equipInfo.part))
            {
                ShopConfigDic[equipInfo.part].Add(shopInfo);
            }
            else
            {
                ShopConfigDic.Add(equipInfo.part, new List<ShopInfo>() {shopInfo});
            }
        }
    }

    private void ChangeTag(int part)
    {
        shopList = ShopConfigDic[part];
        gridView.SetItemAndRefresh(shopList.Count);
    }

    private void CreateItem(int index, GameObject item)
    {
        ShopInfo info = shopList[index];
        int? curBuyNum = ShopBuyData.Find((obj) => obj.type == info.goodType && obj.id == info.id)?.buyNum;
        if (curBuyNum == null)
        {
            curBuyNum = 0;
        }

        ShopItem shopItem = item.GetComponent<ShopItem>();
        shopItem.InitData(info, curBuyNum.Value, () =>
        {
            BuyClick(info, shopItem);
        });
    }

    private void BuyClick(ShopInfo shopInfo, ShopItem shopItem)
    {
        ResClass resClass = Ui.Instance.FormatResStr(shopInfo.price)[0];
        int resNum = GameDataManager.Instance.GetResNum(resClass.resourceId);
        if (resNum < resClass.num)
        {
            Ui.Instance.ShowFlutterView(Ui.Instance.GetGoodName((int) GoodsType.Resource, resClass.resourceId) + "不足!");
            return;
        }

        //二次弹窗
        Ui.Instance.ShowConfirmationLayer("购买", () =>
        {
            GameDataManager.Instance.AddBuyInfoData(shopInfo.goodType, shopInfo.id, resClass.resourceId,
                (int) resClass.num);
            int curBuyNum = ShopBuyData.Find((obj) => obj.type == shopInfo.goodType && obj.id == shopInfo.id).buyNum;
            shopItem.RefreshItem(curBuyNum);
            List<ResClass> resList = new List<ResClass>()
            {
                new ResClass((GoodsType) shopInfo.goodType, shopInfo.id, shopInfo.num)
            };
            Ui.Instance.ShowReward(resList);
        });
    }
    
    public override void onExit()
    {
    }
}