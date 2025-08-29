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

    private void Start()
    {
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
        }
    }

    public void InitData(int tag)
    {
        selTag = tag;
        action?.Invoke(selTag);
    }

    public void AddChangeEvent(Action<int> action)
    {
        this.action = action;
    }
}