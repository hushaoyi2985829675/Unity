using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SkeletonScript : Monster
{
    private GameObject talkUI;
    private float talkTime;
    private new void Update()
    {
        base.Update();
        if (detectPlayer)
        {
            if (!talking)
            {
               // StartCoroutine(createTalkUI("?????????"));
                talking = true;
                talkTime = 5;
            }                  
        }
        if (talkUI != null && talkTime > 0 && talking)
        {
            var canvasPos = Camera.main.WorldToScreenPoint(new Vector2(transform.localPosition.x, transform.localPosition.y+1.7f));
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(UIManager.Instance.getCanvas().GetComponent<RectTransform>(), canvasPos, Camera.main, out localPos);
            talkUI.GetComponent<RectTransform>().localPosition = localPos;
            talkTime -= Time.deltaTime;
        }
    }

    // public IEnumerator createTalkUI(string talk)
    // {
    //     Debug.Log(talk);
    //     GameObject tablkRef = Resources.Load<GameObject>("UIRef/TalkRef");
    //     talkUI = Instantiate(tablkRef, UIManager.Instance.getCanvas().transform);
    //     string str = "";
    //     talkUI.GetComponent<TalkUI>().text.text = str;
    //     for (int i = 0; i < talk.Length; i++)
    //     {
    //         str += talk[i];
    //         talkUI.GetComponent<TalkUI>().text.text = str;
    //         yield return new WaitForSeconds(0.2f);
    //     }
    //     yield return new WaitForSeconds(5f);
    //     talkUI.GetComponent<RectTransform>().SetParent(null, false);
    // }
    public new IEnumerator MonsterDeathEff()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(base.MonsterDeathEff());
        Destroy(talkUI);
    }
}
