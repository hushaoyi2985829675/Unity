using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroEditor.Common;
using Assets.HeroEditor.Common.CharacterScripts;
using HeroEditor.Common.Enums;
using Assets.HeroEditor.Common.CommonScripts;

public class EquipItemScript : MonoBehaviour
{
    public SpriteCollection SpriteCollection;
    public IconCollection IconCollection;
    public BagData BagData;
    public string Id;
    public EquipmentPart part;
    SpriteGroupEntry Weapon;
    ItemIcon WeaponIcon;
    void Start()
    {
        switch (part)
        {
            case EquipmentPart.MeleeWeapon1H:
                Weapon = SpriteCollection.MeleeWeapon1H.Find(data => data.Id == Id );
                break;
            case EquipmentPart.Armor:
                Weapon = SpriteCollection.Armor.Find(data => data.Id == Id);
                break;
        }
        if (Weapon != null)
        {
            WeaponIcon = IconCollection.Icons.Find(data => data.Id == Id);
            if (WeaponIcon != null)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = WeaponIcon.Sprite;
            }
            else
            {
                Debug.Log("没找到武器图标" + Weapon.Name);
            }
        }
        else
        {
            Debug.Log("没找到武器" + Id);
        }
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {           
            collision.gameObject.GetComponent<Player>().CheckoutEquip(Weapon,part);
            //放入背包
            BagData.AddEquip(Weapon, part);
            Destroy(gameObject);
        }
    }


}
