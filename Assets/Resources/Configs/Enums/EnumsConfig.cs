using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 视图方向
public enum DirectionType
{
    Horizontal,
    Vertical
}

//道具类型
public enum GoodsType
{
    Equip = 1,
    Good = 2,
    Ingredient = 3,
    Resource = 4,
}

//任务类型
public enum TaskType
{
    Kill = 1,
    PickUp
}

//怪物类型
public enum MonsterType
{
    Wolf = 1, //狼
}

//TabItem类型
public enum TabItemType
{
    Weapon = 0,
    Helmet,
    Armor,
}

//属性
public enum AttrType
{
    Attack = 0, //攻击力
    Health = 1, //生命值
    MoveSpeed = 2, //移速
    AttackSpeed = 3, //攻速
    Armor = 4, //护甲
    CritRate = 5, //暴击率
    CritDamage = 6, //暴击伤害
    DodgeRate = 9, //闪避率 0-1
}
