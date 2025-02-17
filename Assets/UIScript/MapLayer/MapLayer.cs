using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapLayer : PanelBase
{
    public HorizontalView scrollView;
    public MapConfig config;
    public override void onEnter(params object[] data)
    {
        scrollView.AddRefreshEvent(RefreshItem);
        scrollView.SetNum(config.data.Count);
    }

    public void RefreshItem(int i,GameObject item)
    { 
        var data = config.data.Find(data => data.Id == i + 1);
        if (data != null)
        { 
            item.transform.Find("Button").GetComponent<Image>().sprite = data.Sprite;
        }
    }
    public override void onExit()
    {
        
    }
}
