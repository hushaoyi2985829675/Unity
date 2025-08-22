using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlacksmithLayer : PanelBase
{
    public Transform toggleGroup;
    public Transform parentNode;
    public GameObject foundryNode;
    public Button closeButton;

    public override void onEnter(params object[] data)
    {
        //处理标签
        InitTab();
        TabChange(true, 0);
        closeButton.onClick.AddListener(() => { UIManager.Instance.CloseLayer(gameObject.name); });
    }

    public void InitTab()
    {
        for (int i = 0; i < toggleGroup.childCount; i++)
        {
            Toggle toggle = toggleGroup.GetChild(i).GetComponent<Toggle>();
            int idx = i;
            toggle.isOn = i == 0;
            toggle.onValueChanged.AddListener((isOn) => { TabChange(isOn, idx); });
        }
    }

    public void TabChange(bool isOn, int i)
    {
        //铸造
        if (i == 0)
        {
            FoundryClick(isOn);
        }
        else if (i == 1)
        {
        }
        else
        {
        }
    }

    void FoundryClick(bool isOn)
    {
        if (isOn)
        {
            UIManager.Instance.AddUINode(foundryNode, parentNode);
        }
        else
        {
            UIManager.Instance.CloseUINode(foundryNode.name);
        }
    }

    public override void onExit()
    {
    }
}