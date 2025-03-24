using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character Data/MonsterValueCharate")]
public class MonsterValue : ScriptableObject
{      
    [Header("�ƶ�����")]
    public float walkSpeed;
    public float runSpeed;
    [Header("�ƶ��ȴ�ʱ��")]
    public float awaitTime;
    [Header("�������")]
    public float MaxHp;
    [Header("����")]
    public float Hp;
    [Header("������")]
    public float AttackPower;
    [Header("��������")]
    public float AttackDitance;
    [Header("�������")]
    public float AttackInterval;
    [Header("����")]
    public float Armor;
    [Header("����ֵ")]
    public float Exp;
    [Header("基础掉落概率")]
    public float DropProbability;
}
