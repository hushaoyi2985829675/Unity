using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class GridView : MonoBehaviour
{
    int HorizontalNum;
    int VerticalNum;
    int cellNum;
    Vector2 ItemSize;
    float SpaceX;
    float SpaceY;
    public RectTransform ScrollView;
    public RectTransform Content;
    public GameObject Item;
    Dictionary<int ,GameObject> Items = new Dictionary<int, GameObject>();
    int oldMinIndex;
    int oldMaxIndex;
    Stack<GameObject> Stack = new Stack<GameObject>();
    Action<int,GameObject> RefreshItemEvent;

    public void SetItemNumAndSpace(int horizontalNum, int verticalNum, float spaceX,float spaceY)
    {
        HorizontalNum = horizontalNum;
        VerticalNum = verticalNum;
        ItemSize = Item.GetComponent<RectTransform>().sizeDelta;
        SpaceX = spaceX;
        SpaceY = spaceY;
        Content.sizeDelta = new Vector2(Content.sizeDelta.x, ItemSize.y * VerticalNum + (VerticalNum - 1) * SpaceY);
    }
    public void LoadGridViewData()
    {
        oldMinIndex = 0;
        oldMaxIndex = 0;
        RefreshItem();
    }
    
    public void RefreshItem()
    {
        int minIdx = Math.Max((int)(Content.anchoredPosition.y / (ItemSize.y + SpaceY)) * HorizontalNum,0);
        int maxIdx = Math.Min((int)Mathf.Ceil(((Content.anchoredPosition.y + ScrollView.sizeDelta.y) / (ItemSize.y + SpaceY))) * HorizontalNum - 1,VerticalNum * HorizontalNum - 1);
        for (int i = oldMinIndex; i < minIdx; i++)
        {
            Stack.Push(Items[i]);
            Items.Remove(i);
        }
        for (int i = maxIdx + 1; i <= oldMaxIndex; i++)
        {
            Stack.Push(Items[i]);
            Items.Remove(i);
        }
        oldMinIndex = minIdx;
        oldMaxIndex = maxIdx;
        for (int i = minIdx; i <= maxIdx; i++)
        {
            if (Items.ContainsKey(i))
            {
                continue;
            }
            GameObject item = null ;
            if (Stack.Count > 0)
            {                
                item = Stack.Pop();
            }
            else
            {
               item = Instantiate(Item, Content);
               item.SetActive(true);
            }
            float x = 0;
            float y = 0;
            float startPos = ((HorizontalNum - 1) / 2f) * (ItemSize.x + SpaceX);
            x = startPos + (ItemSize.x + SpaceX) * (i % HorizontalNum);
            y = -(ItemSize.y + SpaceY) * (int)(i / HorizontalNum);
            item.GetComponent<RectTransform>().localPosition = new Vector2(x, y);     
            Items[i] = item;
            RefreshItemEvent.Invoke(i,item);
        }
    }
    public void AddRefreshEvent(Action<int, GameObject> action)
    {
        if (RefreshItemEvent != null)
        {
            return;
        }
        RefreshItemEvent = action;
    }
    public void RefreshAllItem()
    {
        foreach (var item in Items)
        {
            RefreshItemEvent.Invoke(item.Key, item.Value);
        }
    }
}
