using System.Collections;
using System.Collections.Generic;
using GoodWaysNs;
using UnityEngine;
using UnityEngine.UI;

public class WayNode : PanelBase
{
    private int id;

    [SerializeField]
    private Text wayName;

    public override void onEnter(params object[] data)
    {
    }

    public override void onShow(params object[] data)
    {
        id = (int) data[0];
        GoodWay goodWayInfo = Ui.Instance.GetWayInfoById(id);
        wayName.text = goodWayInfo.way;
    }

    public override void onExit()
    {
    }
}