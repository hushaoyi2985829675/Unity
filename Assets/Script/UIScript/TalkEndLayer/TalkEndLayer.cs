using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkEndLayer : PanelBase
{
    public GameObject parent;
    public GameObject item;
    string[] buttonList = { "πÿ±’“≥√Ê" };
    void Start()
    {
        
    }

    void createButton()
    {
        foreach (var str in buttonList)
        {
            var btn = Instantiate(item);
            btn.SetActive(true);
            btn.transform.SetParent(parent.transform);
            item.transform.Find("Text").gameObject.GetComponent<Text>().text = str;
        }
    }
    void Update()
    {
        
    }

    public override void onEnter(params object[] data)
    {
        createButton();
    }

    public override void onExit()
    {
        
    }
}
