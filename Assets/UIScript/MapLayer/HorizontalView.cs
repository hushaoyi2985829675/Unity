using Assets.HeroEditor.FantasyInventory.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HorizontalView : MonoBehaviour
{
    [Header("Ä£°å")]
    public GameObject temp;
    [Header("¸¸Àà")]
    public GameObject content;
    [Header("×ó±ß¾à")]
    public float leftSpace;
    private int num;
    private GameObject scrollView;
    Dictionary<int, GameObject> itemList = new Dictionary<int, GameObject>();
    int minIndx = 0;
    int maxIndx = 0;
    Stack<GameObject> stack = new Stack<GameObject>();
    private int count;
    private float size;
    Action<int, GameObject> action;

    public void SetNum(int num)
    {
        scrollView = gameObject;
        scrollView.GetComponent<ScrollRect>().onValueChanged.AddListener(onUpdate);
        size = temp.GetComponent<RectTransform>().sizeDelta.x ;
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(size * num + leftSpace, content.GetComponent<RectTransform>().rect.height);
        this.num = num;
        RefreshData();
    }
    private void RefreshData()
    {
        count = (int)Mathf.Ceil(scrollView.GetComponent<RectTransform>().sizeDelta.x / size) + 1;
        for (int i = 0; i < count; i++)
        {
            var item = Instantiate(temp, content.transform);
            item.SetActive(true);
            item.transform.localPosition = new Vector3(size * i + leftSpace, 0, 0);
            itemList[i] = item;
            action.Invoke(i, item);
        }
        this.maxIndx = count - 1;
    }

    public void onUpdate(Vector2 v)
    {
        int minNum = (int)((-content.GetComponent<RectTransform>().anchoredPosition.x - leftSpace) / size);
        var maxNum = minNum + count - 1;
        if (minNum < 0 || maxNum > num - 1)
        {
            return;
        }

        for (int i = minIndx; i < minNum; i++)
        {
            itemList[i].SetActive(false);
            stack.Push(itemList[i]);
            itemList.Remove(i);
        }

        for (int i = maxNum + 1; i <= maxIndx; i++)
        {
            itemList[i].SetActive(false);
            stack.Push(itemList[i]);
            itemList.Remove(i);
        }
        minIndx = minNum;
        maxIndx = maxNum;
        for (int i = minNum; i <= maxNum; i++)
        {
            if (itemList.ContainsKey(i))
            {
                continue;
            }
            else
            {
                var item = stack.Pop();
                item.SetActive(true);
                item.transform.localPosition = new Vector3(size * i + leftSpace, 0, 0);
                itemList[i] = item;
                action.Invoke(i, item);
            }
        }
        UpdateScale();
    }
    public void UpdateScale()
    {
        foreach(var data in itemList)
        {
            GameObject item = data.Value;
            var worldPos = item.transform.parent.TransformPoint(item.transform.localPosition);
            var pos = item.transform.parent.parent.InverseTransformPoint(worldPos);
            
        }
    }
    public void AddRefreshEvent(Action<int,GameObject> action) 
    {
        this.action += action;
    }
}
