using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CloseTips : MonoBehaviour
{
    public Text text;
    public Button button;

    private void OnEnable()
    {
        text?.DOKill();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(text.DOFade(0, 1f).From(1f));
        sequence.SetLoops(-1, LoopType.Yoyo);
        sequence.SetEase(Ease.InOutSine);
        button.onClick.AddListener(() => { UIManager.Instance.CloseLayer(transform.parent.gameObject); });
    }
}