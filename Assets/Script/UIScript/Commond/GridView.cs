using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class GridView : MonoBehaviour
{
    public int HorizontalNum = 4;
    private int horizontalNum;
    int verticalNum;
    int viewCount;
    private int cellNum;
    Vector2 ItemSize;
    public float SpaceX;
    public float SpaceY;
    public RectTransform ScrollView;
    public RectTransform Content;
    public GameObject Item;
    Dictionary<int, GameObject> Items = new Dictionary<int, GameObject>();
    int oldMinIndex;
    int oldMaxIndex;
    Stack<GameObject> Stack = new Stack<GameObject>();
    Action<int, GameObject> RefreshItemEvent;

    public void SetItemAndRefresh(int num)
    {
        cellNum = num;
        if (num <= 0)
        {
            Clear();
            return;
        }

        horizontalNum = HorizontalNum;
        horizontalNum = Math.Min(horizontalNum, num);
        verticalNum = (int) Mathf.Ceil((num * 1.0f) / horizontalNum);
        ItemSize = Item.GetComponent<RectTransform>().sizeDelta;
        ScrollView.GetComponent<ScrollRect>().onValueChanged.AddListener(onUpdate);
        Content.anchoredPosition = new Vector2(0, Content.anchoredPosition.x);
        Content.sizeDelta = new Vector2(Content.sizeDelta.x, verticalNum * (SpaceY + ItemSize.y));
        viewCount = (int) Mathf.Ceil(ScrollView.rect.height / (ItemSize.y + SpaceY) * 1f) * horizontalNum;
        oldMinIndex = 0;
        oldMaxIndex = 0;
        RefreshAllItem();
    }

    private void onUpdate(Vector2 pos)
    {
        if (cellNum > 0)
        {
            RefreshItem();
        }
    }

    public void RefreshItem(bool isRefreshAll = false)
    {
        int minIdx = Math.Max((int) (Content.anchoredPosition.y / (ItemSize.y + SpaceY)) * horizontalNum, 0);
        minIdx = Math.Min(minIdx, cellNum - horizontalNum);
        int maxIdx = (int) Math.Ceiling(Content.anchoredPosition.y / (ItemSize.y + SpaceY)) * horizontalNum;
        maxIdx = Math.Min(maxIdx + viewCount - 1, cellNum - 1);
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

            GameObject item = null;
            if (Stack.Count > 0)
            {
                item = Stack.Pop();
                Items[i] = item;
            }
            else
            {
                item = Instantiate(Item, Content);
                Items[i] = item;
            }

            float x = 0;
            float y = 0;
            float startPos = ((horizontalNum - 1) * (ItemSize.x + SpaceX)) / 2;
            x = -startPos + (ItemSize.x + SpaceX) * (i % horizontalNum);
            y = -(ItemSize.y + SpaceY) * (i / horizontalNum);
            item.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, 0);
            RefreshItemEvent?.Invoke(i, item);
        }
    }

    public void RefreshAllItem()
    {
        Clear();
        if (cellNum > 0)
        {
            RefreshItem(true);
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

    public void Clear()
    {
        Ui.Instance.RemoveAllChildren(Content);
        Items.Clear();
        Stack.Clear();
    }
}