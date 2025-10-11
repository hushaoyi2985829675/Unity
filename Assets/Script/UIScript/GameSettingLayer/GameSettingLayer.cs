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
    }

    private void ReturnMainClick()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            return;
        }

        UIManager.Instance.LoadScene("MainScene", (slider) => { });
    }

    public override void onExit()
    {
    }
}