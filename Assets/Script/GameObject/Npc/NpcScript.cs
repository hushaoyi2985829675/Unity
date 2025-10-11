using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScript : MonoBehaviour
{
    [SerializeField] private int npcId;
    GameObject talkLayer;
    [SerializeField] private GameObject npcLayer;

    void Start()
    {
        talkLayer = Resources.Load<GameObject>("Ref/LayerRef/UIRef/TalkLayer/TalkLayer");
    }

    // Update is called once per frame

    void Talk()
    {
        UIManager.Instance.OpenLayer(talkLayer, new object[] {npcId, npcLayer});
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerUI.Instance.ShowTalkBtn("交谈", Talk);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerUI.Instance.HideTalkBtn();
        }
    }
}