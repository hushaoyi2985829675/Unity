using System.Collections;
using System.Collections.Generic;
using HeroEditor.Common.Enums;
using UnityEngine;

//场景
public enum SceneType
{
    MainScene = 1,
    FightScene = 2
}

//道具分类
public enum GoodsCategory
{
    Potion = 0, //药水
    Bow = 1, //箭 
    Food = 2, //食物
}

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


//装备类型
public enum EquipType
{
    Weapon = 7,
    Helmet = 1,
    Armor = 0,
}

//属性
public enum AttrType
{
    Attack = 0, //攻击力
    MaxHealth = 1, //生命值
    MoveSpeed = 2, //移速 百分比
    AttackSpeed = 3, //攻速
    Armor = 4, //护甲
    CritRate = 5, //暴击率
    CritDamage = 6, //暴击伤害
    DodgeRate = 7, //闪避率 0-1
    CurHealth = 8, //当前生命值
    Exp = 9, //经验
}

//颜色
public class MyColor
{
    public static readonly Color Green = new Color(0.2f, 0.9f, 0.25f, 1f);
    public static readonly Color LightGreen = new Color(0.15f, 0.7f, 0.2f, 1f);
    public static readonly Color Red = new Color(0.95f, 0.2f, 0.25f, 1f);

    public static readonly Color LightRed = new Color(0.85f, 0.15f, 0.15f, 1f);

    // 扩展：临界状态（可选，数量接近不足时用）
    public static readonly Color Critical = new Color(1f, 0.6f, 0.1f, 1f);
    public static readonly Color LightCritical = new Color(0.9f, 0.5f, 0f, 1f);
}
