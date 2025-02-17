using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Slider hpSlider;
    public Text hpText;
    Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }
    public void initHp()
    {
        hpSlider.maxValue = player.PlayerValueData.PlayerInfo.MaxHp;
        hpSlider.value = player.PlayerValueData.PlayerInfo.Hp;
    }
    public void setHp(float hp)
    { 
        hpSlider.value = hp;
        hpText.text = string.Format("ÑªÁ¿: " + hp);
    }
}
