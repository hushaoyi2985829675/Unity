using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlacksmithLayer : PanelBase
{
    public Transform parentNode;

    [Header("页面")] [SerializeField]
    public GameObject foundryNode;

    [SerializeField] private GameObject BreakDownNode;
    [SerializeField] private GameObject synthesizeNode;
    public ToggleTableView toggleTableView;
    private GameObject layer;

    public override void onEnter(params object[] data)
    {
        toggleTableView.InitTabView(TabChange);
    }

    public override void onShow(object[] data)
    {
        toggleTableView.SetSelTag(0);
    }
    public void TabChange(int i)
    {
        if (layer != null)
        {
            CloseUILayer(layer);
            layer = null;
        }

        //铸造
        if (i == 0)
        {
            layer = foundryNode;
        }
        else if (i == 1)
        {
            layer = BreakDownNode;
        }
        else
        {
            layer = synthesizeNode;
        }

        AddLayerNode(layer);
    }

    void AddLayerNode(GameObject layer)
    {
        AddUILayer(layer, parentNode);
    }

    public override void onExit()
    {
    }
}