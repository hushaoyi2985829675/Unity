using System.Collections;
using System.Collections.Generic;
using Equip;
using HeroEditor.Common.Enums;
using PlayerLvAttr;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    [Header("血量")] [SerializeField] public float MaxHp;
    [SerializeField] public float CurHp;
    [Header("攻击力")] [SerializeField] public float AttackPower;
    [Header("经验")] [SerializeField] public float Exp;
    [Header("等级")] [SerializeField] public float Lv;
    [Header("攻击速度")] [SerializeField] public float AttackSpeed;
    [Header("跳跃次数")] [SerializeField] public float JumpNum;
    [Header("护甲")] [SerializeField] public float Armor;
    [Header("行走速度")] [SerializeField] public float MoveSpeed;
    [Header("暴击率")] [SerializeField] public float CritRate;
    [Header("暴击伤害")] [SerializeField] public float CritDamage;
    [Header("闪避率")] [SerializeField] public float DodgeRate;
    [SerializeField] public float jumpSpeed;


    public void SetPlayerValue(PlayerLvInfo playerLvInfo)
    {
    }
}
[CreateAssetMenu(fileName = "New Data", menuName = "Character Data/PlayerValueData")]
public class PlayerValueData : ScriptableBase
{
    public PlayerInfo PlayerInfo;

    public override void Create()
    {
        PlayerLvInfo playerLvInfo = Ui.Instance.GetPlayerLvAttr();
        PlayerInfo.MaxHp = playerLvInfo.health;
        PlayerInfo.CurHp = playerLvInfo.health;
        PlayerInfo.Exp = 0;
        PlayerInfo.Lv = 1;
        PlayerInfo.AttackPower = playerLvInfo.attack;
        PlayerInfo.JumpNum = 1;
        PlayerInfo.Armor = playerLvInfo.armor;
        PlayerInfo.MoveSpeed = playerLvInfo.moveSpeed;
        PlayerInfo.jumpSpeed = 10;
        PlayerInfo.CritRate = playerLvInfo.critRate;
        PlayerInfo.CritDamage = playerLvInfo.critDamage;
        PlayerInfo.AttackSpeed = playerLvInfo.attackSpeed;
        PlayerInfo.DodgeRate = playerLvInfo.dodgeRate;
    }
    public override void Clear()
    {
        PlayerInfo = new PlayerInfo();
    }

    public int GetPlayerLv()
    {
        return (int) PlayerInfo.Lv;
    }

    public PlayerInfo GetPlayerValueInfo()
    {
        return PlayerInfo;
    }

    public float GetPlayerAttrValue(AttrType attrType)
    {
        switch (attrType)
        {
            case AttrType.Attack:
                return PlayerInfo.AttackPower;
            case AttrType.Health:
                return PlayerInfo.MaxHp;
            case AttrType.MoveSpeed:
                return PlayerInfo.MoveSpeed;
            case AttrType.Armor:
                return PlayerInfo.Armor;
            case AttrType.CritRate:
                return PlayerInfo.CritRate;
            case AttrType.CritDamage:
                return PlayerInfo.CritDamage;
            case AttrType.AttackSpeed:
                return PlayerInfo.AttackSpeed;
            case AttrType.DodgeRate:
                return PlayerInfo.DodgeRate;
            default:
                return -1;
        }
    }

    public void SetPlayerAttrValue(AttrType attrType, float value)
    {
        switch (attrType)
        {
            case AttrType.Attack:
                PlayerInfo.AttackPower = (PlayerInfo.AttackPower * 1000 + value * 1000) / 1000;
                break;
            case AttrType.Health:
                PlayerInfo.MaxHp = (PlayerInfo.MaxHp * 1000 + value * 1000) / 1000;
                break;
            case AttrType.MoveSpeed:
                PlayerInfo.MoveSpeed = (PlayerInfo.MoveSpeed * 1000 + value * 1000) / 1000;
                break;
            case AttrType.Armor:
                PlayerInfo.Armor = (PlayerInfo.Armor * 1000 + value * 1000) / 1000;
                break;
            case AttrType.CritRate:
                PlayerInfo.CritRate = (PlayerInfo.CritRate * 1000 + value * 1000) / 1000;
                break;
            case AttrType.CritDamage:
                PlayerInfo.CritDamage = (PlayerInfo.CritDamage * 1000 + value * 1000) / 1000;
                break;
            case AttrType.AttackSpeed:
                PlayerInfo.AttackSpeed = (PlayerInfo.AttackSpeed * 1000 + value * 1000) / 1000;
                break;
            case AttrType.DodgeRate:
                PlayerInfo.DodgeRate += (PlayerInfo.DodgeRate * 1000 + value * 1000) / 1000;
                break;
        }
    }

}



