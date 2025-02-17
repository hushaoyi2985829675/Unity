using Assets.HeroEditor.Common.CommonScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TalkLayer : PanelBase, IPointerDownHandler, IPointerUpHandler
{
    [Header("�Ի�����")]
    public TalkData TalkData;
    [Header("����ͷ��Ԥ����")]
    public GameObject playerHeadRef;
    [Header("����ͷ�����")]
    public RawImage playerHeadImg;
    [Header("��Ҫ�򿪵Ľ���")]
    public GameObject talkEndLayer;
    GameObject playerHeadObject;
    TalkObject TalkObject;
    [Header("Text���")]
    public Text Text;
    int index;
    bool Touch;
    Vector2 startPos;
    TalkHead playerHead;
    public override void onEnter(object[] data)
    {
        var npcName = data[0] as string;
        TalkObject = TalkData.TalkInfo.Find(info => info.name == npcName);
        if (TalkData == null)
        {
            Debug.Log("û�ҵ�" + npcName + "��TalkInfo");
            return;
        }
        StartCoroutine(ShowTalk());
    }

    void Update()
    {

    }
    public IEnumerator ShowTalk()
    {
        foreach (TalkInfo talkInfo in TalkObject.talkList)
        {
            createHead(talkInfo);
            foreach (string str in talkInfo.talkStr)
            {
                Text.text = "";
                StartTalk(talkInfo.talkType);
                for (int i = 0; i < str.Length; i++)
                {
                    Text.text += str[i];
                    yield return new WaitForSeconds(0.1f);
                }
                StopTalk(talkInfo.talkType);
                yield return new WaitUntil(() => Touch);
                Touch = false;
            }
        }       
        yield return null;
        //�ж��Ƿ���Ҫ�򿪽���
        UIManager.Instance.OpenLayer(talkEndLayer);
    }
    void createHead(TalkInfo talkInfo)
    {
        if (talkInfo.talkType == TalkType.Player)
        {
            if (playerHead == null)
            {
                //����player
                playerHeadObject = Instantiate(playerHeadRef);
                var player = GameObject.FindGameObjectWithTag("Player");
                var head = Instantiate(Tool.FindChlidTransform(player, "Head"));
                head.transform.SetParent(playerHeadObject.transform);
                head.transform.localPosition = Vector3.zero;
                playerHead = playerHeadObject.GetComponent<TalkHead>();
                playerHead.playerHead = head;
                playerHead.Enter();
            }
            playerHeadImg.SetActive(true);
        }
        else
        {
            if (playerHeadImg.IsActive())
            {
                playerHeadImg.SetActive(false);
            }
        }
    }

    void StartTalk(TalkType talkType)
    {
        if (talkType == TalkType.Player)
        {
            playerHead.StartTalk();
        }
        else
        { 
            
        }
    }
    void StopTalk(TalkType talkType)
    {
        if (talkType == TalkType.Player)
        {
            playerHead.StopTalk();
        }
        else
        {

        }
    }
    public override void onExit()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {        
        if (Vector2.Distance(startPos, eventData.position) > 100)
        {
            return;
        }
        Touch = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = eventData.position;
    }
}
