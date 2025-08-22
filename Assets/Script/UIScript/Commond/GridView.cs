using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class GridView : MonoBehaviour
{
    int HorizontalNum;
    int VerticalNum;
    int viewCount;
    private int cellNum;
    Vector2 ItemSize;
    float SpaceX;
    float SpaceY;
    public RectTransform ScrollView;
    public RectTransform Content;
    public GameObject Item;
    Dictionary<int, GameObject> Items = new Dictionary<int, GameObject>();
    int oldMinIndex;
    int oldMaxIndex;
    Stack<GameObject> Stack = new Stack<GameObject>();
    Action<int, GameObject> RefreshItemEvent;

    public void SetItemNumAndSpace(int num, int horizontalNum, float spaceX, float spaceY)
    {
        cellNum = num;
        HorizontalNum = Math.Min(horizontalNum, num);
        VerticalNum = (int) Mathf.Ceil((num * 1.0f) / HorizontalNum);
        ItemSize = Item.GetComponent<RectTransform>().sizeDelta;
        SpaceX = spaceX;
        SpaceY = spaceY;
        ScrollView.GetComponent<ScrollRect>().onValueChanged.AddListener(onUpdate);
        Content.anchoredPosition = new Vector2(0, Content.anchoredPosition.x);
        Content.sizeDelta = new Vector2(Content.sizeDelta.x, VerticalNum * (SpaceY + ItemSize.y));
        viewCount = (int) Mathf.Ceil(ScrollView.rect.height / (ItemSize.y + SpaceY) * 1f) * HorizontalNum;
        oldMinIndex = 0;
        oldMaxIndex = 0;
        RefreshAllItem();
    }

    private void onUpdate(Vector2 pos)
    {
        RefreshItem();
    }

    public void RefreshItem(bool isRefreshAll = false)
    {
        int minIdx = Math.Max((int) (Content.anchoredPosition.y / (ItemSize.y + SpaceY)) * HorizontalNum, 0);
        minIdx = Math.Min(minIdx, cellNum - HorizontalNum);
        int maxIdx = Math.Min(minIdx + viewCount, cellNum - 1);
        Debug.Log(maxIdx);
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
            if (!isRefreshAll && Items.ContainsKey(i))
            {
                continue;
            }

            GameObject item = null;
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
            float startPos = ((HorizontalNum - 1) * (ItemSize.x + SpaceX)) / 2;
            x = -startPos + (ItemSize.x + SpaceX) * (i % HorizontalNum);
            y = -(ItemSize.y + SpaceY) * (int) (i / HorizontalNum);
            item.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, 0);
            Items[i] = item;
            RefreshItemEvent?.Invoke(i, item);
        }
    }

    public void RefreshAllItem()
    {
        Ui.Instance.RemoveAllChildren(Content);
        Items.Clear();
        Stack.Clear();
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
}