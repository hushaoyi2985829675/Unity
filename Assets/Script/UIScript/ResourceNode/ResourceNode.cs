using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ResourceNs;
using UnityEngine;

public class ResourceNode : PanelBase
{
    Dictionary<int, ResourceInfo> ResourceConfig;
    public GameObject resNode;
    public Transform parent;

    public override void onEnter(params object[] data)
    {
        ResourceConfig = Resources.Load<ResourceConfig>("Configs/Data/ResourceConfig").resourceInfoList
            .ToDictionary(key => key.resource, value => value);
        transform.SetSiblingIndex(100);
        refreshUI();
    }

    public override void onShow(object[] data)
    {
    }
    void refreshUI()
    {
        foreach (var resource in ResourceConfig)
        {
            createRes(resource.Value.resource);
        }
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