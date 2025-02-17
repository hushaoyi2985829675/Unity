using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data",menuName = "Character Data/PlayerValueData")]
public class PlayerValueData : ScriptableObject,ScriptableBase
{
    public PlayerInfo PlayerInfo;

    public void Clear()
    {
        PlayerInfo = new PlayerInfo();
    }
}
[System.Serializable]
public class PlayerInfo
{
    [Header("最大生命值")]
    public float MaxHp;
    [Header("当前生命")]
    public float Hp;
    [Header("攻击力")]
    public float AttackPower;
    [Header("经验")]
    public float Exp;
    [Header("等级")]
    public float Lv;
    [Header("攻击间隔")]
    public float AttackInterval;
    [Header("跳跃次数")]
    public float JumpNum;
    [Header("护甲")]
    public float Armor;
    [Header("步行速度")]
    public float walkSpeed;
    [Header("跑步速度")]
    public float runSpeed;
    [Header("跳跃速度")]
    public float jumpSpeed;
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
    }
}
