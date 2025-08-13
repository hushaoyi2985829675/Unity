using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScript : NpcBase
{
    public int npcId = 1;
    GameObject talkLayer;
    void Start()
    {
        talkLayer = Resources.Load<GameObject>("Ref/LayerRef/UIRef/TalkLayer");
        Debug.Log(talkLayer);
        boxCollider =  GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer && Input.GetKeyDown(KeyCode.K))
        {
            Talk();
        }
    }

    void Talk()
    {
        UIManager.Instance.OpenLayer(talkLayer,new object[] {this.npcId});
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayer = false;
        }
    }
}
