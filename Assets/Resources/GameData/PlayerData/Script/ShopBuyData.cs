using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopBuyInfo
{
    public int buyNum;
    public int type;
    public int id;

    public ShopBuyInfo()
    {
    }

    public ShopBuyInfo(int type, int id, int buyNum)
    {
        this.type = type;
        this.id = id;
        this.buyNum = buyNum;
    }
}

[CreateAssetMenu(fileName = "ShopBuyData", menuName = "GameData/ShopBuyData")]
public class ShopBuyData : ScriptableBase
{
    public List<ShopBuyInfo> shopBuyList;

    public void AddBuyInfo(int type, int id)
    {
        ShopBuyInfo shopBuyInfo = shopBuyList.Find((obj) => obj.type == type && obj.id == id);
        if (shopBuyInfo == null)
        {
            shopBuyList.Add(new ShopBuyInfo(type, id, 1));
        }
        else
        {
            shopBuyInfo.buyNum += 1;
        }
    }

    public override void Create()
    {
        shopBuyList = new List<ShopBuyInfo>();
    }

    public override void Clear()
    {
        shopBuyList = new List<ShopBuyInfo>();
    }
}