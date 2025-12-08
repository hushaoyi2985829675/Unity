using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResNode : MonoBehaviour
{
    private int id;
    public Image image;
    public Text text;
    private PlayerResData resData;

    public void Awake()
    {
        resData = Resources.Load<PlayerResData>("GameData/PlayerData/PlayerResData");
    }

    public void InitData(int id)
    {
        this.id = id;
        image.sprite = Ui.Instance.GetGoodIcon((int) GoodsType.Resource, id);
        RefreshResFunc();
        EventManager.Instance.AddEvent(GameEventType.ResEvent, new object[] {id, (Action) RefreshResFunc});
    }

    private void RefreshResFunc()
    {
        text.text = GameDataManager.Instance.GetResNum(id).ToString();
    }
}