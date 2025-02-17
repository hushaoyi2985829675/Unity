using Assets.HeroEditor.FantasyInventory.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabView : MonoBehaviour
{
    [Header("Ä£°å")]
    public GameObject Item;
    Action<int, GameObject> RefreshItemEvent;
    Dictionary<int,GameObject> ItemList = new Dictionary<int, GameObject>();
    public void SetNum(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if (!ItemList.ContainsKey(i))
            {
                var item = Instantiate(Item, transform);
                item.SetActive(true);
                RefreshItemEvent.Invoke(i, item);
                ItemList[i] = item;
            }   
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
