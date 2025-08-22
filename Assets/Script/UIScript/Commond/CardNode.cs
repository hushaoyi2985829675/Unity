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
        Sprite sprite = Ui.Instance.GetEquipIcon(goodsType, id);
        icon.sprite = sprite;
        if (num != -1)
        {
            numText.text = num.ToString();
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