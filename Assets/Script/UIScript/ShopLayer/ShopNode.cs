using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNode : PanelBase
{
    public GridView gridView;

    public override void onEnter(params object[] data)
    {
        gridView.AddRefreshEvent(CreateItem);
        gridView.SetItemAndRefresh(40);
    }

    public void CreateItem(int index, GameObject item)
    {
    }

    public override void onExit()
    {
    }
}