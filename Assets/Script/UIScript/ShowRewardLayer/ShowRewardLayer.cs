using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRewardLayer : PanelBase
{
    public Transform parent;
    public CardNode cardNode;
    private List<ResClass> rewardList;
    GoodsType goodsType;

    public override void onEnter(params object[] data)
    {
        rewardList = (List<ResClass>) data[0];
        goodsType = (GoodsType) data[1];
        RefreshUI();
    }

    void RefreshUI()
    {
        foreach (ResClass reward in rewardList)
        {
            Debug.Log(reward.resourceId);
            CardNode node = Instantiate(cardNode, parent).GetComponent<CardNode>();
            node.transform.localPosition = new Vector3(0, 0, 0);
            node.SetCardData(goodsType, reward.resourceId, reward.num);
            node.ShowName();
        }
    }

    public override void onExit()
    {
        Ui.Instance.RemoveAllChildren(parent);
    }
}