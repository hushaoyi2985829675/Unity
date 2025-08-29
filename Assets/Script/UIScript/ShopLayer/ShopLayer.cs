using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopLayer : PanelBase
{
    public Transform parent;
    public Button closeBtn;
    public ToggleTableView tableVie;
    public ShopNode shopNode;

    public override void onEnter(params object[] data)
    {
        tableVie.AddChangeEvent(TabChange);
        tableVie.InitData(1);
    }

    private void TabChange(int idx)
    {
        PanelBase panel = shopNode;
        if (idx == 0)
        {
            panel = shopNode;
        }

        OpenLayer(panel);
    }

    private void OpenLayer(PanelBase panel)
    {
        UIManager.Instance.AddUINode(panel.gameObject, parent);
    }

    public override void onExit()
    {
    }
}