using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject MapLayer;
    private bool isPlayer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OpenMapLayer()
    {
        UIManager.Instance.OpenLayer(MapLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerUI.Instance.ShowTalkBtn("地图", OpenMapLayer);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerUI.Instance.HideTalkBtn();
    }
}
