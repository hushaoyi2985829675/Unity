using System.Collections;
using System.Collections.Generic;
using PlayerLvAttrNs;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLocalValueData", menuName = "GameData/PlayerValueData")]
public class PlayerValueData : ScriptableBase
{
    [Header("血量")]
    [SerializeField]
    private int curHp;

    [Header("经验")]
    [SerializeField]
    private int exp;

    [Header("等级")]
    public int lv;

    [Header("已解锁技能")]
    public List<int> skillIdList = new List<int>(); 

    public override void Create()
    {
        exp = 0;
        lv = 1;
        PlayerLvInfo playerLvInfo = Ui.Instance.GetPlayerLvAttr();
        curHp = playerLvInfo.health;
    }

    public int GetPlayerLv()
    {
        return lv;
    }

    public void SetPlayerLv(int lv)
    {
        this.lv = lv;
    }

    public int GetPlayerExp()
    {
        return exp;
    }

    public void SetPlayerExp(int exp)
    {
        this.exp = exp;
    }

    public int GetPlayerHp()
    {
        return curHp;
    }

    public void SetPlayerHp(int curHp)
    {
        this.curHp = curHp;
    }

    public List<int> GetPlayerSkill()
    {
        return skillIdList;
    }

    public void AddPlayerSkill(int id)
    {
        skillIdList.Add(id);
        skillIdList.Sort();
    }

    public override void Clear()
    {
    }
}
