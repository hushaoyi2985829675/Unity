using Assets.HeroEditor.FantasyInventory.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HorizontalView : MonoBehaviour
{
    [Header("ģ��")]
    public GameObject temp;
    [Header("����")]
    public GameObject content;
    [Header("��߾�")]
    public float leftSpace;
    [Header("右边间隔")]
    public float rightSpace;
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
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(size * num + leftSpace + rightSpace - size, content.GetComponent<RectTransform>().rect.height);
        this.num = num;
        RefreshData();
    }
    private void RefreshData()
    {
        count = (int)Mathf.Ceil(scrollView.GetComponent<RectTransform>().sizeDelta.x / size);
        int minNum = Mathf.Max((int)((-content.GetComponent<RectTransform>().anchoredPosition.x - leftSpace + size / 2) / size) , 0);
        var maxNum = Mathf.Min(minNum + count,num - 1);
        for (int i = minNum; i <= maxNum; i++)
        {
            GameObject item ;
            if (! itemList.ContainsKey(i))
            {
                item = Instantiate(temp, content.transform);
                itemList[i] = item;
            }
            else
            {
                item = itemList[i];
            }
            item.SetActive(true);
            item.transform.localPosition = new Vector3(size * i + leftSpace, 0, 0);
            action.Invoke(i, item);
        }
        this.maxIndx = count;
        UpdateScale();
    }

    public void onUpdate(Vector2 v)
    {
        int minNum = Mathf.Max((int)((-content.GetComponent<RectTransform>().anchoredPosition.x - leftSpace + size / 2) / size),0);
        var maxNum = Mathf.Min(minNum + count,num - 1);
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
            if (Mathf.Abs(pos.x) < 300.0f)
            {
                var x = Mathf.Lerp(1.2f, 1.0f, Mathf.Abs(pos.x) / 300.0f);
                item.transform.localScale = new Vector3(x, x, 1);
            }
            else
            {
                item.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
    public void AddRefreshEvent(Action<int,GameObject> action) 
    {
        this.action += action;
    }
}
