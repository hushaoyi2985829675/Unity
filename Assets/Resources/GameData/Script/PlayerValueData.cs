using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character Data/PlayerValueData")]
public class PlayerValueData : ScriptableBase
{
    public PlayerInfo PlayerInfo;

    public override void Clear()
    {
        PlayerInfo = new PlayerInfo();
    }
}

[System.Serializable]
public class PlayerInfo
{
    [Header("????????")] public float MaxHp;
    [Header("???????")] public float Hp;
    [Header("??????")] public float AttackPower;
    [Header("????")] public float Exp;
    [Header("???")] public float Lv;
    [Header("???????")] public float AttackInterval;
    [Header("???????")] public float JumpNum;
    [Header("????")] public float Armor;
    [Header("???????")] public float walkSpeed;
    [Header("??????")] public float runSpeed;
    [Header("??????")] public float jumpSpeed;
    [Header("?????")] public float LuckValue;

    public PlayerInfo()
    {
        MaxHp = 100;
        Hp = 100;
        AttackPower = 1;
        Armor = 0;
        Exp = 0;
        Lv = 0;
        AttackInterval = 1;
        JumpNum = 1;
        walkSpeed = 4;
        runSpeed = 0;
        jumpSpeed = 10;
        LuckValue = 0;
    }
}