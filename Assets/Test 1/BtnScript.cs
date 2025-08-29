using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnScript : MonoBehaviour
{
    public GameObject talkLayerRef;
    public GameObject foundryNode;
    public int npcId;
    private Button btn;
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            //UIManager.Instance.OpenLayer(talkLayerRef, new object[] {1, foundryNode});
            GameDataManager.Instance.AddRes(1, 10);
            GameDataManager.Instance.AddRes(2, 10);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
