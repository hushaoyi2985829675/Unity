using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskItem : PanelBase
{
    [SerializeField] private Text text;
    [SerializeField] private Toggle toggle;
    private Action<int> callback;
    private int id;

    public override void onEnter(params object[] data)
    {
        toggle.group = transform.parent.GetComponent<ToggleGroup>();
    }

    public override void onShow(params object[] data)
    {
    }
    public void InitData(int id, string title, Action<int> callback)
    {
        text.text = title;
        this.callback = callback;
        this.id = id;
        toggle.onValueChanged.AddListener(ToggleClick);
    }

    public void ToggleClick(bool isOn)
    {
        if (isOn)
        {
            callback(id);
        }
    }

    public override void onExit()
    {
    }
}