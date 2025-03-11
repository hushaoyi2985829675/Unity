using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character Data/MonsterValueCharate")]
public class MonsterValue : ScriptableObject
{      
    [Header("移动参数")]
    public float walkSpeed;
    public float runSpeed;
    [Header("移动等待时间")]
    public float awaitTime;
    [Header("最大生命")]
    public float MaxHp;
    [Header("生命")]
    public float Hp;
    [Header("攻击力")]
    public float AttackPower;
    [Header("攻击距离")]
    public float AttackDitance;
    [Header("攻击间隔")]
    public float AttackInterval;
    [Header("护甲")]
    public float Armor;
    [Header("经验值")]
    public float Exp;
}
