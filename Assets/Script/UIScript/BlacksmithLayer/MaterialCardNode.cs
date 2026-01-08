using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialCardNode : PanelBase
{
    [SerializeField] private CardNode cardNode;
    [SerializeField] private GameObject line;
    private int resourcId;
    private int num;
    private bool showLine;

    public override void onEnter(params object[] data)
    {
        cardNode.onEnter();
    }

    public override void onShow(params object[] data)
    {
        resourcId = (int) data[0];
        num = (int) data[1];
        showLine = (bool) data[2];
        RefreshUI();
    }

    private void RefreshUI()
    {
        cardNode.SetSelState(false);
        cardNode.SetCardData(GoodsType.Resource, resourcId);
        string str;
        int curNum = GameDataManager.Instance.GetResNum(resourcId);
        if (Ui.Instance.GetResNumIsEnough(GoodsType.Resource, resourcId, num, false))
        {
            str = $"<color={MyColor.GreenStr}>{curNum}</color>/{num}";
        }
        else
        {
            str = $"<color={MyColor.RedStr}>{curNum}</color>/{num}";
        }

        cardNode.SetNumText(str);
        line.SetActive(showLine);
    }

    public override void onExit()
    {
    }
}