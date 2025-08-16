using Assets.HeroEditor.FantasyInventory.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

// 定义枚举（选项列表）
public enum Direction
{
    Horizontal,
    Vertical
}
public class TableView : MonoBehaviour
{
    [Header("方向")]
    [SerializeField]
    public Direction  direction =  Direction.Horizontal;
    [Header("是否中心缩放")]
    public bool isScale = false;
    [Header("节点")]
    public GameObject temp;
    [Header("����")]
    public RectTransform content;
    [Header("物体间的间隔")]
    public float tempSpace;
    [Header("左或上间隔")]
    public float leftTopSpace;
    [Header("右或下边间隔")]
    public float rightBottomSpace;
    private int num;
    private ScrollRect scrollView;
    Dictionary<int, GameObject> itemList = new Dictionary<int, GameObject>();
    int minIndx = 0;
    int maxIndx = 0;
    int oldMinIndx = 0;
    int oldMaxIndx = 0;
    Stack<GameObject> stack = new Stack<GameObject>();
    private int count;
    private Vector2 size;
    Action<int, GameObject> action;
    
    public void SetNum(int num)
    {
        scrollView = GetComponent<ScrollRect>();
        scrollView.horizontal = false;
        scrollView.vertical = false;
        scrollView.GetComponent<ScrollRect>().onValueChanged.AddListener(onUpdate);
        size = temp.GetComponent<RectTransform>().sizeDelta ;
        if (direction == Direction.Horizontal)
        {
            scrollView.horizontal = true;
            content.anchorMin = new Vector2(0,0);
            content.anchorMax = new Vector2(0,1);
            content.pivot = new Vector2(0,0.5f);
            content.sizeDelta = Vector2.zero;
            content.sizeDelta = new Vector2(num * (size.x + tempSpace) - tempSpace  + leftTopSpace + rightBottomSpace , content.GetComponent<RectTransform>().rect.height);
        }
        else
        {
            scrollView.vertical = true;
            content.anchorMin = new Vector2(0,1);
            content.anchorMax = new Vector2(1,1);
            content.pivot = new Vector2(0.5f,1);
            content.sizeDelta = Vector2.zero;
            content.sizeDelta = new Vector2(content.GetComponent<RectTransform>().rect.width , num * (size.y + tempSpace) - tempSpace +  leftTopSpace + rightBottomSpace);
        }
        this.num = num;
        RefreshData();
    }
    public void onUpdate(Vector2 v)
    {
        int minNum = 0;
        if (direction == Direction.Horizontal)
        {
            count = (int)Mathf.Ceil(scrollView.GetComponent<RectTransform>().sizeDelta.x / size.x); 
            minNum = Mathf.Max((int)((-content.GetComponent<RectTransform>().anchoredPosition.x - leftTopSpace) / size.x) , 0);
        }
        else
        {
            count = (int)Mathf.Ceil(scrollView.GetComponent<RectTransform>().sizeDelta.y / size.y);
            minNum = Mathf.Max((int)((content.GetComponent<RectTransform>().anchoredPosition.y - leftTopSpace) / size.y) , 0);
        }
         
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
            if (direction == Direction.Horizontal)
            {
                item.transform.localPosition = new Vector3((size.x + tempSpace) * i + leftTopSpace + size.x / 2, 0, 0);
            }
            else
            {
                item.transform.localPosition = new Vector3(0,(-size.y - tempSpace) * i - leftTopSpace - size.y / 2, 0);
            }
            
            action?.Invoke(i, item);
        }
        if (isScale)
        {
            UpdateScale();   
        }
    }
    private void RefreshData()
    {
        onUpdate(Vector2.zero);
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
