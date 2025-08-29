using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlacksmithLayer : PanelBase
{
    public Transform toggleGroup;
    public Transform parentNode;
    public GameObject foundryNode;
    public GameObject sellNode;
    public Button closeButton;
    public ToggleTableView toggleTableView;
    private int selTag = 0;
    private PanelBase oldLayer;

    public override void onEnter(params object[] data)
    {
        toggleTableView.AddChangeEvent(TabChange);
        toggleTableView.InitData(0);
        //处理标签
        closeButton.onClick.AddListener(() => { UIManager.Instance.CloseLayer(gameObject.name); });
    }

    public void TabChange(int i)
    {
        GameObject layer;
        selTag = i;
        //铸造
        if (i == 0)
        {
            layer = sellNode;
        }
        else if (i == 1)
        {
            layer = foundryNode;
        }
        else
        {
            layer = foundryNode;
        }

        AddLayerNode(layer);
    }

    void AddLayerNode(GameObject layer)
    {
        if (oldLayer != null)
        {
            UIManager.Instance.CloseUINode(oldLayer.name);
        }

        oldLayer = UIManager.Instance.AddUINode(layer, parentNode);
    }

    public override void onExit()
    {
    }
}