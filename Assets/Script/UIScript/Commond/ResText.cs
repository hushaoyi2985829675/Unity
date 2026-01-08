using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResText : PanelBase
{
    [SerializeField]
    Text text;

    [SerializeField]
    Image image;

    private ResClass costRes;
    private Action refreshCallback;

    private void Awake()
    {
    }

    public void RefreshUI(string cost)
    {
        costRes = Ui.Instance.FormatResStr(cost)[0];
        int curNUm = GameDataManager.Instance.GetResNum(costRes.resourceId);
        string color = curNUm >= costRes.num ? MyColor.GreenStr : MyColor.RedStr;
        text.text = string.Format("<color={0}>{1}</color>/{2}", color, curNUm, costRes.num);
        image.sprite = Ui.Instance.GetGoodIcon((int) costRes.goodsType, costRes.resourceId);
    }

    public override void onEnter(params object[] data)
    {
        string cost = (string) data[0];
        costRes = Ui.Instance.FormatResStr(cost)[0];
        AddEvent(GameEventType.ResEvent, new object[]
        {
            costRes.resourceId,
            (Action) (() =>
            {
                refreshCallback?.Invoke();
            })
        });
    }

    public override void onShow(params object[] data)
    {
        string cost = (string) data[0];
        if (data.Length > 1)
        {
            refreshCallback = (Action) data[1];
        }

        RefreshUI(cost);
    }

    public override void onExit()
    {
    }
}