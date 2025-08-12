using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnTest : MonoBehaviour
{
    public GameObject LayerPrefab;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            UIManager.Instance.OpenLayer(LayerPrefab,new object[] {"少时诵诗书少时诵诗书"});
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
