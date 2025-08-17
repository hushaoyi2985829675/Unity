using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnTest : MonoBehaviour
{
    public GameObject TalkLayerPrefab;
    public GameObject TaskLayerRef;
    void Start()
    {
      GetComponent<Button>().onClick.AddListener(() =>
      {
          UIManager.Instance.OpenLayer(TaskLayerRef);
      });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
