using System;
using System.Collections;
using System.Collections.Generic;
using EquipNs;
using HeroEditor.Common.Enums;
using PlayerLvAttrNs;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "PlayerLocalValueData", menuName = "GameData/PlayerLocalValueData")]
public class PlayerLocalValueData : ScriptableObject
{
    [Header("最大血量")]
    [SerializeField]
    public float MaxHp;

    [Header("血量")]
    [SerializeField]
    public float CurHp;

    [Header("攻击力")]
    [SerializeField]
    public float AttackPower;

    [Header("等级")]
    [SerializeField]
    public float Lv;

    [Header("攻击速度")]
    [SerializeField]
    public float AttackSpeed;

    [Header("跳跃次数")]
    [SerializeField]
    public float JumpNum;

    [Header("护甲")]
    [SerializeField]
    public float Armor;

    [Header("行走速度")]
    [SerializeField]
    public float MoveSpeed;

    [Header("暴击率")]
    [SerializeField]
    public float CritRate;

    [Header("暴击伤害")]
    [SerializeField]
    public float CritDamage;

    [Header("闪避率")]
    [SerializeField]
    public float DodgeRate;

    [Header("跳跃高度")]
    [SerializeField]
    public float jumpSpeed;

    public void InitPlayerValue()
    {
        PlayerLvInfo playerLvInfo = Ui.Instance.GetPlayerLvAttr();
        MaxHp = playerLvInfo.health;
        CurHp = GameDataManager.Instance.GetPlayerHp();
        Lv = GameDataManager.Instance.GetPlayerLv();
        AttackPower = playerLvInfo.attack;
        JumpNum = 1;
        Armor = playerLvInfo.armor;
        MoveSpeed = playerLvInfo.moveSpeed;
        jumpSpeed = 10;
        CritRate = playerLvInfo.critRate;
        CritDamage = playerLvInfo.critDamage;
        AttackSpeed = playerLvInfo.attackSpeed;
        DodgeRate = playerLvInfo.dodgeRate;
        AddEquipValue();
    }

    private void AddEquipValue()
    {
        foreach (EquipmentPart part in Enum.GetValues(typeof(EquipmentPart)))
        {
            EquipData equipData = GameDataManager.Instance.GetPlayerEquipData(part);
            if (equipData.id != -1)
            {
                EquipInfo equipInfo = Ui.Instance.GetEquipInfo(equipData.id);
                List<AttrClass> attrList = Ui.Instance.FormatAttrStr(equipInfo.attr);
                foreach (AttrClass attrClass in attrList)
                {
                    SetPlayerAttrValue(attrClass.attrType, attrClass.value);
                }
            }
        }
    }
    // public override void Clear()
    // {
    //     PlayerInfo = new PlayerInfo();
    // }

    public int GetPlayerLv()
    {
        return (int) Lv;
    }

    public float GetPlayerAttrValue(AttrType attrType)
    {
        switch (attrType)
        {
            case AttrType.Attack:
                return AttackPower;
            case AttrType.MaxHealth:
                return MaxHp;
            case AttrType.MoveSpeed:
                return MoveSpeed;
            case AttrType.Armor:
                return Armor;
            case AttrType.CritRate:
                return CritRate;
            case AttrType.CritDamage:
                return CritDamage;
            case AttrType.AttackSpeed:
                return AttackSpeed;
            case AttrType.DodgeRate:
                return DodgeRate;
            case AttrType.CurHealth:
                return CurHp;
            default:
                return -1;
        }
    }

    public void SetPlayerAttrValue(AttrType attrType, float value)
    {
        switch (attrType)
        {
            case AttrType.Attack:
                AttackPower = (AttackPower * 1000 + value * 1000) / 1000;
                break;
            case AttrType.MaxHealth:
                MaxHp = (MaxHp * 1000 + value * 1000) / 1000;
                break;
            case AttrType.MoveSpeed:
                MoveSpeed = (MoveSpeed * 1000 + value * 1000) / 1000;
                break;
            case AttrType.Armor:
                Armor = (Armor * 1000 + value * 1000) / 1000;
                break;
            case AttrType.CritRate:
                CritRate = (CritRate * 1000 + value * 1000) / 1000;
                break;
            case AttrType.CritDamage:
                CritDamage = (CritDamage * 1000 + value * 1000) / 1000;
                break;
            case AttrType.AttackSpeed:
                AttackSpeed = (AttackSpeed * 1000 + value * 1000) / 1000;
                break;
            case AttrType.DodgeRate:
                DodgeRate += (DodgeRate * 1000 + value * 1000) / 1000;
                break;
            case AttrType.CurHealth:
                CurHp += (DodgeRate * 1000 + value * 1000) / 1000;
                break;
        }
    }
}