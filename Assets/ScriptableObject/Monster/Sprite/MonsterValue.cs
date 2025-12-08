using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character Data/MonsterValueCharate")]
public class MonsterValue : ScriptableObject
{      
    public float walkSpeed;
    public float runSpeed;
    public float awaitTime;
    public float MaxHp;
    public float Hp;
    public float AttackPower;
    public float AttackDitance;
    public float AttackInterval;
    public float Armor;
    public float Exp;
    [Header("基础掉落概率")]
    public float DropProbability;
}
