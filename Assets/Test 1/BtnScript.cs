using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnScript : MonoBehaviour
{
    public GameObject talkLayerRef;
    public int npcId;
    private Button btn;
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            UIManager.Instance.OpenLayer(talkLayerRef);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
