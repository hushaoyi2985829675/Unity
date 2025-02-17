using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character Data/PlayerData")]
public class PlayerCharater : ScriptableObject
{
    public float walkSpeed;
    public float runSpeed;
    public int Hp;
    public float AttackPower;
    public float AttackInterval;
    public float Armor;
}

