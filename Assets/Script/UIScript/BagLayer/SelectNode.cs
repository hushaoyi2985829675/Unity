using System;
using System.Collections;
using System.Collections.Generic;
using EquipNs;
using HeroEditor.Common.Enums;
using UnityEngine;
using UnityEngine.UI;

public class SelectNode : PanelBase
{
    private RectTransform parentRectTransform;
    [SerializeField] private GameObject descLayer;
    [SerializeField] private RectTransform UINode;
    [SerializeField] private Button equipBtn;
    [SerializeField] private Button showBtn;
    private Action callback;
    private Transform pos;
    private int id;
    private int type;

    public override void onEnter(params object[] data)
    {
        parentRectTransform = UINode.parent.GetComponent<RectTransform>();
        showBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.OpenLayer(descLayer, new object[] {type, id});
            UIManager.Instance.CloseUINode(gameObject);
        });
    }

    public override void onShow(params object[] data)
    {
        pos = (Transform) data[0];
        type = (int) data[1];
        id = (int) data[2];
        callback = (Action) data[3];
        RefreshPos();
        RefreshBtn();
    }

    public void RefreshPos()
    {
        //屏幕坐标
        Vector2 screenPos = Camera.main.WorldToScreenPoint(pos.position);
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPos,
            Camera.main, out localPos);
        UINode.anchoredPosition = localPos + new Vector2(45, 0);
    }

    public void RefreshBtn()
    {
        equipBtn.onClick.RemoveAllListeners();
        if (type == (int) GoodsType.Good)
        {
            equipBtn.transform.Find("Text").GetComponent<Text>().text = "使用";
        }

        equipBtn.onClick.AddListener(() =>
        {
            if (type == (int) GoodsType.Good)
            {
                equipBtn.transform.Find("Text").GetComponent<Text>().text = "使用";
            }
            else
            {
                //更新穿戴装备数据
                GameDataManager.Instance.SetPlayerEquipData(id);
                EquipInfo equipInfo = Ui.Instance.GetEquipInfo(id);
                //装备穿戴事件
                EventManager.Instance.PostEvent(GameEventType.WearEquipEvent, new object[] {(EquipmentPart) equipInfo.part});
                //刷新装备槽位
                callback();
            }

            UIManager.Instance.CloseUINode(gameObject);
        });
    }

    public override void onExit()
    {
    }
}