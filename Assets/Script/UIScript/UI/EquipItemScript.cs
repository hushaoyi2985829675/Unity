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
    
    private PolygonCollider2D collider;
    private SpriteRenderer spriteRenderer;
        
    private string id;
    private EquipmentPart part;
    SpriteGroupEntry Weapon;
    ItemIcon WeaponIcon;
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // 减小物体的质量
            rb.mass = 2f;
            // 增大冲量的大小
            rb.AddForce(new Vector2(2f, 10f), ForceMode2D.Impulse);
            rb.mass = 1f;
        }
    }

    void Refresh()
    {
        switch (part)
        {
            case EquipmentPart.MeleeWeapon1H:
                Weapon = SpriteCollection.MeleeWeapon1H.Find(data => data.Id ==id );
                break;
            case EquipmentPart.Armor:
                Weapon = SpriteCollection.Armor.Find(data => data.Id ==id);
                break;
        }
        //找Icon
        if (Weapon != null)
        {
            WeaponIcon = IconCollection.Icons.Find(data => data.Id ==id);
            if (WeaponIcon != null)
            {
                spriteRenderer.sprite = WeaponIcon.Sprite;
            }
            else
            {
                Debug.Log("没找到武器图标" + Weapon.Name);
            }
        }
        else
        {
            Debug.Log("没找到武器" +id);
        }
        collider.SetPath(0,spriteRenderer.sprite.vertices);
    }

    void Update()
    {
        
    }
    
    public void InitData(string id, EquipmentPart part)
    {
        this.id = id;
        this.part = part;
        collider = GetComponent<PolygonCollider2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Refresh();
    }

    private void OnCollisionEnter2D (Collision2D collision)
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
