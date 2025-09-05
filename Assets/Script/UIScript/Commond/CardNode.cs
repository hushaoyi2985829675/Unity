using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardNode : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private GameObject checkmark;
    [SerializeField] private Text numText;
    [SerializeField] private Text nameText;
    [SerializeField] private Button button;
    [SerializeField] private GameObject UINode;
    [SerializeField] private GameObject descLayer;
    private Action callback;
    private int id;
    private GoodsType type;

    private void Awake()
    {
        callback = DefaultClick;
        button.onClick.AddListener(() => { callback(); });
    }

    public void SetCardData(GoodsType goodsType, int id, int num = -1)
    {
        RefreshUI(goodsType, id, num);
    }

    public void SetCardData(GoodsType goodsType, ResClass resClass)
    {
        RefreshUI(goodsType, resClass.resourceId, resClass.num);
    }

    private void RefreshUI(GoodsType goodsType, int id, int num = -1)
    {
        Sprite sprite = Ui.Instance.GetGoodIcon((int) goodsType, id);
        icon.sprite = sprite;
        if (num > 0)
        {
            numText.gameObject.SetActive(true);
            numText.text = num.ToString();
        }
        else
        {
            numText.gameObject.SetActive(false);
        }

        this.id = id;
        this.type = goodsType;
    }
    

    public void ShowName()
    {
        string name = Ui.Instance.GetGoodName((int) type, id);
        nameText.gameObject.SetActive(true);
        nameText.text = name;
    }

    public void SetClick(Action action)
    {
        callback = action;
    }

    public void SetSelState(bool isSel)
    {
        checkmark.SetActive(isSel);
    }

    public void SetIsNull(bool isNull)
    {
        UINode.SetActive(isNull);
        if (!isNull)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    public void DefaultClick()
    {
        UIManager.Instance.OpenLayer(descLayer, new object[] {type, id});
    }
}