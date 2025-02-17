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
    [Header("�������ֵ")]
    public float MaxHp;
    [Header("��ǰ����")]
    public float Hp;
    [Header("������")]
    public float AttackPower;
    [Header("����")]
    public float Exp;
    [Header("�ȼ�")]
    public float Lv;
    [Header("�������")]
    public float AttackInterval;
    [Header("��Ծ����")]
    public float JumpNum;
    [Header("����")]
    public float Armor;
    [Header("�����ٶ�")]
    public float walkSpeed;
    [Header("�ܲ��ٶ�")]
    public float runSpeed;
    [Header("��Ծ�ٶ�")]
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
