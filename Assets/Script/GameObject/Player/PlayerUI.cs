using System;
using System.Collections;
using System.Collections.Generic;
using PlayerLvAttrNs;
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
    private PlayerLocalValueData playerLocalValueData;
    private Text hpText;
    private Text expText;
    private GameObject playerHead;
    private void Awake()
    {
        hpText = hpSlider.transform.Find("HpText").GetComponent<Text>();
        expText = expSlider.transform.Find("ExpText").GetComponent<Text>();
        playerHead = Resources.Load<GameObject>("Ref/GameObject/Players/PlayerHead");
        EventManager.Instance.AddEvent(GameEventType.PlayerUIStateEvent, new object[] {(Action) refreshUI});
    }

    private void Start()
    {
        HideTalkBtn();
        playerLocalValueData = GameDataManager.Instance.GetPlayerLocalValueInfo();
        Instantiate(playerHead);
        hpSlider.maxValue = 100;
        InitUI();
    }
    public void InitUI()
    {
        hpSlider.value = playerLocalValueData.CurHp / playerLocalValueData.MaxHp * 100;
        hpText.text = string.Format(hpSlider.value + "/" + hpSlider.maxValue);
        expSlider.value = GameDataManager.Instance.GetPlayerExp();
        PlayerLvInfo playerLvInfo = Ui.Instance.GetPlayerLvAttr();
        expSlider.maxValue = playerLvInfo.exp;
        expText.text = string.Format(expSlider.value + "/" + expSlider.maxValue);
        //等级
        LvText.text = string.Format("Lv.{0}", playerLocalValueData.Lv);
    }

    public void SetHp(float hp)
    { 
        hpSlider.value = hp;
        hpText.text = string.Format(hp + "/" + hpSlider.maxValue);
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

    private void refreshUI()
    {
        InitUI();
    }
}
