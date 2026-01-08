using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TipPopNode : PanelBase
{
    [SerializeField]
    Text text;

    [SerializeField]
    Transform bgTrans;

    private Sequence sequence;

    public override void onEnter(params object[] data)
    {
    }

    public override void onShow(params object[] data)
    {
        text.text = (string) data[0];
        if (sequence != null)
        {
            bgTrans.transform.localPosition = new Vector3(0, -158, 0);
            sequence.Kill();
        }

        sequence = DOTween.Sequence();
        sequence.Append(bgTrans.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.1f));
        sequence.Append(bgTrans.DOScale(new Vector3(1f, 1f, 1f), 0.1f));
        sequence.Append(bgTrans.DOLocalMoveY(0, 1.3f));
        sequence.OnComplete(() =>
        {
            sequence.Kill();
            UIManager.Instance.CloseUINode(gameObject);
            bgTrans.transform.localPosition = new Vector3(0, -158, 0);
        });
    }

    public override void onExit()
    {
    }
}