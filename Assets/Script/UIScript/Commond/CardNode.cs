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
    public Button button;

    public void SetCardData(GoodsType goodsType, int id, int num = -1)
    {
        Sprite sprite = Ui.Instance.GetGoodIcon(goodsType, id);
        icon.sprite = sprite;
        if (num != -1)
        {
            numText.gameObject.SetActive(true);
            numText.text = num.ToString();
        }
        else
        {
            numText.gameObject.SetActive(false);
        }
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