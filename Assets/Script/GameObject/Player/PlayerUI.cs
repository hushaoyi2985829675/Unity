using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : Singleton<PlayerUI>
{
    [Header("血量条")]
    public Slider hpSlider;
    [Header("经验条")]
    public Slider expSlider;
    [Header("等级")]
    public Text LvText;

    [Header("对话按钮")] [SerializeField] public Button talkBtn;
    [Header("对话按钮")] [SerializeField] public Text talkText;
    private PlayerInfo playerValueInfo;
    private Text hpText;
    private Text expText;

    private void Awake()
    {
        hpText = hpSlider.transform.Find("HpText").GetComponent<Text>();
        expText = expSlider.transform.Find("ExpText").GetComponent<Text>();
    }

    private void Start()
    {
        HideTalkBtn();
        playerValueInfo = GameDataManager.Instance.GetPlayerValueInfo();
        InitUI();
    }
    public void InitUI()
    {
        hpSlider.maxValue = playerValueInfo.MaxHp;
        hpSlider.value = playerValueInfo.CurHp;
        hpText.text = string.Format(hpSlider.value.ToString() + "/" + hpSlider.maxValue.ToString());
        expSlider.value = playerValueInfo.Exp;
        expSlider.maxValue = playerValueInfo.Exp;
        expText.text = string.Format(expSlider.value.ToString() + "/" + expSlider.maxValue.ToString());
        //等级
        LvText.text = string.Format("等级：{0}", playerValueInfo.Lv);
    }
    public void setHp(float hp)
    { 
        hpSlider.value = hp;
        hpText.text = string.Format(hp + "/" + hpSlider.maxValue.ToString());
    }


    //交谈按钮
    public void ShowTalkBtn(string title, Action callback)
    {
        talkText.text = title;
        talkBtn.onClick.RemoveAllListeners();
        talkBtn.onClick.AddListener(() => { callback(); });
        talkBtn.gameObject.SetActive(true);
    }

    public void HideTalkBtn()
    {
        talkBtn.gameObject.SetActive(false);
    }
}
