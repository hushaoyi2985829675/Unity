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
    Armor = 1 << EquipmentPart.Armor,
    Helmet = 1 << EquipmentPart.Helmet,
    Pauldrons = 1 << EquipmentPart.Pauldrons,
    Vest = 1 << EquipmentPart.Vest,
    Gloves = 1 << EquipmentPart.Gloves,
    Belt = 1 << EquipmentPart.Belt,
    Boots = 1 << EquipmentPart.Boots,
    MeleeWeapon1H = 1 << EquipmentPart.MeleeWeapon1H,
    MeleeWeapon2H = 1 << EquipmentPart.MeleeWeapon2H,
    MeleeWeaponPaired = 1 << EquipmentPart.MeleeWeaponPaired,
    Bow = 1 << EquipmentPart.Bow,
    Firearm1H = 1 << EquipmentPart.Firearm1H,
    Firearm2H = 1 << EquipmentPart.Firearm2H,
    Shield = 1 << EquipmentPart.Shield,
    Earrings = 1 << EquipmentPart.Earrings,
    Cape = 1 << EquipmentPart.Cape,
    Back = 1 << EquipmentPart.Back,
    Glasses = 1 << EquipmentPart.Glasses,
    Mask = 1 << EquipmentPart.Mask
}
public class MonsterEquipManager : MonoBehaviour
{
    public MonsterEquip MonsterEquipData;
    public SpriteCollection SpriteCollection;
    private Character monsterCharacter;
    public MonsterPart Parts;
    public Monster monster;
    void Start()
    {
        monsterCharacter = GetComponent<Character>();
        monster = GetComponent<Monster>();
        //处理装备
        RefreshEquip();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RefreshEquip()
    {
        MonsterEquipData.InitMonsterEquipData();
        foreach (MonsterPart part in Enum.GetValues(typeof(MonsterPart)))
        {
            if ((Parts & part) != 0)
            {
                EquipmentPart p =  (EquipmentPart)Math.Log((int)part, 2);
                Equip(p);
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
            case EquipmentPart.Helmet:
                
                break;
            
        }
        monsterCharacter.Equip(spriteGroupEntry,part);
        MonsterEquipData.SetEquip(part,id);
    }

    public void EquipDrop()
    {
        var player = GameObjectManager.instance.GetPlayer();
        var value = monster.monsterValue.DropProbability + player.PlayerValueData.PlayerInfo.LuckValue;
        EquipData equipData = MonsterEquipData.GetEquipDrop(value);
        if (equipData != null)
        {
            GameObject item = Resources.Load<GameObject>("GameObjectRef/EquipItemRef");
            item.transform.localPosition = monster.transform.localPosition;
            item.GetComponent<EquipItemScript>().InitData(equipData.Id,equipData.Part);
            Instantiate(item);
        }
    }

    private void OnGUI()
    {
        Parts = (MonsterPart)EditorGUILayout.EnumMaskField("Selected Parts", (MonsterPart)Parts);
    }
}
