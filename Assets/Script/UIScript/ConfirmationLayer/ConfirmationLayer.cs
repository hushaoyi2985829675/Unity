using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationLayer : PanelBase
{
    [SerializeField] private Button cancelBtn;
    [SerializeField] private Button confirmBtn;
    [SerializeField] private Text title;

    public override void onEnter(params object[] data)
    {
        cancelBtn.onClick.AddListener(() => { UIManager.Instance.CloseLayer(gameObject); });
    }

    public override void onShow(params object[] data)
    {
        confirmBtn.onClick.RemoveAllListeners();
        confirmBtn.onClick.AddListener(() =>
        {
            if (data[1] is Action action)
            {
                action();
            }

            UIManager.Instance.CloseLayer(gameObject);
        });
        title.text = string.Format("确定要进行{0}吗", data[0] as string);
    }

    public override void onExit()
    {
    }
}