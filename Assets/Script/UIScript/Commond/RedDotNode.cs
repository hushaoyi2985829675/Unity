using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RedDotNode : PanelBase
{
    [SerializeField] private Transform redDot;
    [SerializeField] private float aniTime = 0.5f;
    private Sequence sequence;

    public override void onEnter(params object[] data)
    {
    }

    public override void onShow(params object[] data)
    {
        ShowRedDot();
    }

    private void ShowRedDot()
    {
        sequence = DOTween.Sequence();
        sequence.Append(redDot.DOLocalMove(new Vector3(0, 25, 0), aniTime).SetEase(Ease.InOutExpo));
        sequence.Append(redDot.DOLocalMove(new Vector3(0, 0, 0), aniTime).SetEase(Ease.InOutExpo));
        sequence.AppendInterval(1f);
        sequence.SetLoops(-1);
    }

    public void HideRedDot()
    {
        sequence.Kill();
        redDot.DOKill();
        redDot.gameObject.SetActive(false);
    }

    public override void onExit()
    {
        sequence.Kill();
    }
}