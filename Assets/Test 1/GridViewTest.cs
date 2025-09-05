using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridViewTest : MonoBehaviour
{
    public GridView gridView;
    public Button button;

    void Start()
    {
        gridView.AddRefreshEvent(Create);
        gridView.SetItemAndRefresh(40);
        button.onClick.AddListener(() => { gridView.SetItemAndRefresh(5); });
    }

    void Create(int index, GameObject item)
    {
        CardNode cardNode = item.GetComponent<CardNode>();
        cardNode.SetCardData(GoodsType.Equip, 1, index + 1);
    }

    // Update is called once per frame
    void Update()
    {
    }
}