using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnTest : MonoBehaviour
{
    public GameObject LayerPrefab;
    public TableView tabView;
    void Start()
    {
        tabView.AddRefreshEvent((idx,item) =>
        {
            
        });
        tabView.SetNum(30);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
