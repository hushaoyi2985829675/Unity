using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NpcTalkTask;
using UnityEngine.Events;

public class OptionScript : MonoBehaviour
{
    public Text text;
    public Button btn;
    private BaseOperator operatorInfo;
    private Action<int> callback;

    private void Start()
    {
        btn.onClick.AddListener(BtnClick);
    }

    public void InitData(BaseOperator operatorInfo, Action<int> callback)
    {
        this.operatorInfo = operatorInfo;
        this.callback = callback;
        refreshUI();
    }

    private void refreshUI()
    {
        text.text = operatorInfo.title;
    }

    void BtnClick()
    {
        callback(operatorInfo.id);
    }
}
