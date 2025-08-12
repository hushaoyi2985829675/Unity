using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnScript : MonoBehaviour
{
    private GameObject talkLayer;
    public GameObject talkLayerRef;
    public int npcId;
    private Button btn;
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(CreateTalkUI);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void CreateTalkUI()
    {
        talkLayer = Instantiate(talkLayerRef);
        talkLayer.transform.SetParent(GameObject.Find("Canvas").transform);
        talkLayer.transform.localPosition = new Vector3(0, 0, 0);
    }
}
