using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FlutterWindowsLayer : PanelBase
{
    public Text text;
    public Transform node;
    private float time = 1.5f;
    private float aniTime = 1.0f;
    private Tween aniTween;
    public override void onEnter(params object[] data)
    {
        text.text = data[0].ToString();
        node.transform.localPosition = new Vector3(0, 540, 0);
        Move();
    }

    void Move()
    {
        node.transform.DOLocalMove(new Vector3(0, 340, 0), aniTime).SetEase(Ease.OutBack);
        aniTween = DOVirtual.DelayedCall(time, () =>
        {
            node.transform.DOLocalMove(new Vector3(0, 540, 0), aniTime).SetEase(Ease.InBack).OnComplete(callback);
        });
    }

    void callback()
    {
        UIManager.Instance.ClosePopLayer(gameObject.name);
    }

    public override void onExit()
    {
        node.transform.DOKill();
        aniTween.Kill();
    }
}
