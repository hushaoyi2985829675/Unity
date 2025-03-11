using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using HeroEditor.Common;
using HeroEditor.Common.Enums;
using UnityEditor;
using UnityEngine;
[System.Flags]
public enum MonsterPart
{
    Armor = 1 << 0,
    Helmet = 1 << 1,
    Pauldrons = 1 << 2,
    Vest = 1 << 3,
    Gloves = 1 << 4,
    Belt = 1 << 5,
    Boots = 1 << 6,
    MeleeWeapon1H = 1 << 7,
    MeleeWeapon2H = 1 << 8,
    MeleeWeaponPaired = 1 << 9,
    Bow = 1 << 10,
    Firearm1H = 1 << 11,
    Firearm2H = 1 << 12,
    Shield = 1 << 13,
    Earrings = 1 << 14,
    Cape = 1 << 15,
    Back = 1 << 16,
    Glasses = 1 << 17,
    Mask = 1 << 18
}
public class MonsterEquipManager : MonoBehaviour
{
    public MonsterEquip MonsterEquipData;
    public SpriteCollection SpriteCollection;
    private Character monsterCharacter;
    public MonsterPart Parts;
    void Start()
    {
        monsterCharacter = GetComponent<Character>();
        //处理装备
        RefreshEquip();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RefreshEquip()
    {
        foreach (MonsterPart part in Enum.GetValues(typeof(MonsterPart)))
        {
            if ((Parts & part) != 0)
            {
                switch (part)
                {
                   case MonsterPart.Armor:
                       Equip(EquipmentPart.Armor);
                       break;
                   case MonsterPart.MeleeWeapon1H:
                       Equip(EquipmentPart.MeleeWeapon1H);
                       break;
                }
            }
        }
    }

    void Equip(EquipmentPart part)
    {
        var id = MonsterEquipData.GetEquip(part);
        if (id == "")
        {
            return;
        }
        SpriteGroupEntry spriteGroupEntry = null;
        switch (part)
        {
            case EquipmentPart.Armor:
                spriteGroupEntry = SpriteCollection.Armor.Find(data => data.Id == id);
                break;
            case EquipmentPart.MeleeWeapon1H:
                spriteGroupEntry = SpriteCollection.MeleeWeapon1H.Find(data => data.Id == id);
                break;
        }
        monsterCharacter.Equip(spriteGroupEntry,part);
    }
    private void OnGUI()
    {
        Parts = (MonsterPart)EditorGUILayout.EnumMaskField("Selected Parts", (MonsterPart)Parts);
    }
}
