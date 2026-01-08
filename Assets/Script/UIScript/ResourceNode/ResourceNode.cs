using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ResourceNs;
using UnityEngine;

public class ResourceNode : PanelBase
{
    public GameObject resNode;
    public Transform parent;

    public override void onEnter(params object[] data)
    {
        transform.SetSiblingIndex(100);
        refreshUI();
    }

    public override void onShow(object[] data)
    {
    }
    void refreshUI()
    {
        createRes((int) ResModel.Gold);
        createRes((int) ResModel.Diamond);
    }

    public void createRes(int id)
    {
        ResNode sprite = Instantiate(resNode, parent).GetComponent<ResNode>();
        sprite.InitData(id);
    }


    public override void onExit()
    {
    }
}