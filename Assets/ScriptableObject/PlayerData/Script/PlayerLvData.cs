using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLvData", menuName = "CharacterData/Player/PlayerLvData")]
public class PlayerLvData : ScriptableObject
{
    public List<PlayerData> PlayerLvDatas = new List<PlayerData>();
}

[System.Serializable]
public class PlayerData
{
    [Header("等级")]
    public int Lv;
    [Header("需要经验")]
    public float Exp;
    [Header("血量")]
    public float Hp;
    [Header("攻击力")]
    public float AttackPower;
    [Header("护甲")]
    public float Armor;
}