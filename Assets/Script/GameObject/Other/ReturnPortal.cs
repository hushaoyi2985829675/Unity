using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPortal : MonoBehaviour
{
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OpenMapLayer()
    {
        UIManager.Instance.LoadScene("MainScene", 8);
        PlayerUI.Instance.HideTalkBtn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerUI.Instance.ShowTalkBtn("返回村庄", OpenMapLayer);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerUI.Instance.HideTalkBtn();
    }
}