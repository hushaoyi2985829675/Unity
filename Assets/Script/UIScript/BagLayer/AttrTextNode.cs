using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AttrTextNode : PanelBase
{
    [SerializeField] private GameObject addNode;
    [SerializeField] private Text attrText;
    [SerializeField] private Text symbolText;
    [SerializeField] private Text addText;
    private AttrType attrType;


    public override void onEnter(params object[] data)
    {
        this.attrType = (AttrType) data[0];
    }

    public override void onShow(params object[] data)
    {
    }

    public void RefreshUI()
    {
        CloseAddText();
        float value = GameDataManager.Instance.GetPlayerAttrValue(attrType);
        string name = Ui.Instance.GetAttrName(attrType);
        attrText.text = string.Format("{0}: {1}", name, value);
    }

    public void CloseAddText()
    {
        addNode.SetActive(false);
    }

    public void ShowAddText(float value)
    {
        if (value == 0)
        {
            return;
        }
        else if (value > 0)
        {
            symbolText.text = "+";
            symbolText.color = MyColor.Green;
            addText.color = MyColor.Green;
        }
        else if (value < 0)
        {
            symbolText.text = "-";
            symbolText.color = MyColor.Red;
            addText.color = MyColor.Red;
        }

        addNode.SetActive(true);
        addText.text = Mathf.Abs(value).ToString();
        addText.transform.localScale = new Vector3(1, 1, 1);
        addText.transform.DOKill(true);
        addText.transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 1f).SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public override void onExit()
    {
        addText.transform.DOKill();
    }
}