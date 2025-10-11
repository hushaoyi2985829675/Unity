using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTableView : MonoBehaviour
{
    public int selTag = 0;
    private ToggleGroup toggleGroup;
    private Action<int> action;
    private Dictionary<int, Toggle> itemDic = new Dictionary<int, Toggle>();

    public void InitTabView(Action<int> callback)
    {
        action = callback;
        for (int i = 0; i < transform.childCount; i++)
        {
            Toggle toggle = transform.GetChild(i).GetComponent<Toggle>();
            int idx = i;
            toggle.isOn = i == selTag;
            int n = i;
            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (selTag == n || !isOn)
                {
                    return;
                }

                selTag = n;
                action?.Invoke(idx);
            });
            itemDic.Add(i, toggle);
        }
    }

    public void SetSelTag(int tag)
    {
        selTag = tag;
        itemDic[tag].isOn = true;
        action?.Invoke(selTag);
    }
}