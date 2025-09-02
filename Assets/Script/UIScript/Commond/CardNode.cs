using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardNode : MonoBehaviour
{
    public Image icon;
    public GameObject checkmark;
    public Text numText;
    public Text nameText;
    public Button button;
    private int id;
    private GoodsType type;

    public void SetCardData(GoodsType goodsType, int id, int num = -1)
    {
        Sprite sprite = Ui.Instance.GetGoodIcon(goodsType, id);
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
        button.onClick.AddListener(() => { action(); });
    }

    public void SetSelState(bool isSel)
    {
        checkmark.SetActive(isSel);
    }
}