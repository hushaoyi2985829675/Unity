using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRewardLayer : PanelBase
{
    public Transform parent;
    public CardNode cardNode;
    private List<ResClass> rewardList;

    public override void onEnter(params object[] data)
    {
        
    }

    public override void onShow(object[] data)
    {
        rewardList = (List<ResClass>) data[0];
        RefreshUI();
    }
    void RefreshUI()
    {
        foreach (ResClass reward in rewardList)
        {
            CardNode node = Instantiate(cardNode, parent).GetComponent<CardNode>();
            node.transform.localPosition = new Vector3(0, 0, 0);
            node.SetCardData(reward.goodsType, reward.resourceId, (int) reward.num);
            node.ShowName();
        }
    }

    public override void onExit()
    {
        Ui.Instance.RemoveAllChildren(parent);
    }
}