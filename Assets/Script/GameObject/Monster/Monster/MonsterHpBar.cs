using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MonsterNs;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBar : MonoBehaviour
{
    [Header("跟随的物体")]
    [SerializeField]
    Transform monster;

    MonsterInfo monsterInfo;
    private int curHp;
    private float maxWidth;

    [SerializeField]
    private RectTransform fillParentTrans;

    [SerializeField]
    private RectTransform barTrans;

    [SerializeField]
    private RectTransform effBarTrans;

    [SerializeField]
    private Text barText;

    private Tweener tweener;

    public void Start()
    {
        maxWidth = fillParentTrans.rect.width;
        barTrans.offsetMin = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector2(monster.localPosition.x, transform.localPosition.y);
    }

    public void InitHp(int monsterId)
    {
        monsterInfo = Ui.Instance.GetMonsterValue(monsterId);
        curHp = monsterInfo.maxHp;
        barText.text = string.Format("{0}/{1}", curHp, monsterInfo.maxHp);
    }

    public void RefreshHp(float hp)
    {
        Vector2 startOffset = effBarTrans.offsetMin;
        float curWidth = maxWidth - hp / monsterInfo.maxHp * maxWidth;
        barTrans.offsetMin = new Vector2(curWidth, 0);
        barText.text = string.Format("{0}/{1}", hp, monsterInfo.maxHp);
        TimerManage.Instance.AddDelayCallback(() =>
        {
            tweener = DOTween.To(() => startOffset.x, x =>
            {
                effBarTrans.offsetMin = new Vector2(x, 0);
            }, curWidth, 0.2f);
        }, 0.1f);
    }

    public void RemoveAllDoTween()
    {
        tweener.Kill();
    }
}