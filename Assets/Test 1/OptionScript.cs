using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NpcTalkTask;
using UnityEngine.Events;

public class OptionScript : PanelBase
{
    public Text text;
    public Button btn;
    private BaseOperator operatorInfo;
    private Action<int> callback;
    [SerializeField] private GameObject redDotRef;
    private RedDotNode redDot;
    private bool isCanTaskReceive;

    public override void onEnter(params object[] data)
    {
        operatorInfo = data[0] as BaseOperator;
        callback = data[1] as Action<int>;
        isCanTaskReceive = (bool) data[2];
        btn.onClick.AddListener(BtnClick);
    }

    public override void onShow(params object[] data)
    {
        refreshUI();
    }

    private void refreshUI()
    {
        text.text = operatorInfo.title;
        if ((OperatorType) operatorInfo.id == OperatorType.Task)
        {
            if (isCanTaskReceive)
            {
                redDot = AddUINode<RedDotNode>(redDotRef, transform);
            }
        }
    }

    void BtnClick()
    {
        callback(operatorInfo.id);
    }

    public override void onExit()
    {
    }
    
}
