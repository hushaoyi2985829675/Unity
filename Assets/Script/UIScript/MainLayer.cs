using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainLayer : PanelBase
{
    public override void onExit()
    {
        gameObject.transform.SetParent(null);
        Destroy(gameObject);
    }
    // Update is called once per frame
    public override void onEnter(object[] data)
    {
        
    }
}
