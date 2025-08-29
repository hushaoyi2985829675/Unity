using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnTest : MonoBehaviour
{
    public GameObject TalkLayerPrefab;
    public PlayerResData resData;

    void Start()
    {
      GetComponent<Button>().onClick.AddListener(() =>
      {
          UIManager.Instance.OpenLayer(TalkLayerPrefab, new object[] {2});
      });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
