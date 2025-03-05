using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("血量条")]
    public Slider hpSlider;
    [Header("经验条")]
    public Slider expSlider;
    [Header("等级")]
    public Text LvText;
    private Text hpText;
    private Text expText;
    Player player;

    private void Start()
    {
        player = GetComponent<Player>();
        hpText = hpSlider.transform.Find("HpText").GetComponent<Text>();
        expText = expSlider.transform.Find("ExpText").GetComponent<Text>();
    }
    public void InitUI()
    {
        hpSlider.maxValue = player.PlayerValueData.PlayerInfo.MaxHp;
        hpSlider.value = player.PlayerValueData.PlayerInfo.Hp;
        hpText.text = string.Format(hpSlider.value.ToString() + "/" + hpSlider.maxValue.ToString());
        //经验
        var curLvInfo = player.PlayerLvData.PlayerLvDatas.Find(data => data.Lv == player.PlayerValueData.PlayerInfo.Lv);
        expSlider.maxValue = curLvInfo.Exp;
        expSlider.value = player.PlayerValueData.PlayerInfo.Exp;
        expText.text = string.Format(expSlider.value.ToString() + "/" + expSlider.maxValue.ToString());
        //等级
        LvText.text = string.Format("等级：{0}", player.PlayerValueData.PlayerInfo.Lv);
    }
    public void setHp(float hp)
    { 
        hpSlider.value = hp;
        hpText.text = string.Format(hp + "/" + hpSlider.maxValue.ToString());
    }
}
