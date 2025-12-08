using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSettingLayer : PanelBase
{
    [SerializeField] Button returnMainBtn;

    public override void onEnter(params object[] data)
    {
        returnMainBtn.onClick.AddListener(ReturnMainClick);
    }

    public override void onShow(params object[] data)
    {
        Ui.Instance.SetGray(returnMainBtn.image, SceneManager.GetActiveScene().name == "MainScene");
    }

    private void ReturnMainClick()
    {
        gameObject.SetActive(false);
        UIManager.Instance.LoadScene("MainScene", 8);
    }

    public override void onExit()
    {
    }
}