using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FlutterWindowsLayer : PanelBase
{
    public Text Text;
    private float time = 1.5f;
    private float aniTime = 1.0f;
    public override void onEnter(params object[] data)
    {
        Text.text = data[0].ToString();
        transform.localPosition = new Vector3(0, 600, 0);
        Move();
    }

    void Move()
    {
        transform.DOLocalMove(new Vector3(0, 400, 0), aniTime).SetEase(Ease.OutBack);
        DOVirtual.DelayedCall(time, () =>
        {
            transform.DOLocalMove(new Vector3(0, 600, 0), aniTime).SetEase(Ease.InBack).OnComplete(callback);
        });
    }

    void callback()
    {
        UIManager.Instance.CloseLayer();
    }

    public override void onExit()
    {
        
    }
}
