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
    
    private PolygonCollider2D polygonCollider;
    private SpriteRenderer spriteRenderer;
        
    private string id;
    private int equipId;
    private EquipmentPart part;
    SpriteGroupEntry Weapon;
    ItemIcon WeaponIcon;
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // ��С���������
            rb.mass = 2f;
            // ��������Ĵ�С
            rb.AddForce(new Vector2(2f, 10f), ForceMode2D.Impulse);
            rb.mass = 1f;
        }
    }

    void Refresh()
    {
        Weapon = Ui.Instance.GetEquipEntry(part, id);
        //��Icon
        if (Weapon != null)
        {
            WeaponIcon = IconCollection.Icons.Find(data => data.Id ==id);
            if (WeaponIcon != null)
            {
                spriteRenderer.sprite = WeaponIcon.Sprite;
            }
            else
            {
                Debug.Log("û�ҵ�����ͼ��" + Weapon.Name);
            }
        }
        else
        {
            Debug.Log("û�ҵ�����" +id);
        }
        polygonCollider.SetPath(0,spriteRenderer.sprite.vertices);
    }

    void Update()
    {
        
    }

    public void InitData(string id, int equipId)
    {
        this.id = id;
        this.equipId = equipId;
        part = (EquipmentPart) Ui.Instance.GetEquipInfo(equipId).part;
        polygonCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Refresh();
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {   
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {           
            collision.gameObject.GetComponent<Player>().CheckoutEquip(Weapon,part);
            //���뱳��
            BagData.AddEquip(Weapon, equipId);
            Destroy(gameObject);
        }
    }
}
