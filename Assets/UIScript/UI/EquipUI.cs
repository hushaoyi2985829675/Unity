using Assets.HeroEditor.FantasyInventory.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Linq;
using UnityEditor;

public class EquipUI : MonoBehaviour
{
    public EquipCharate equipCharate;
    public SpriteCollection spriteCollection;
    public Transform parent;
    void Start()
    {
        createEquipRef();
    }

    void Update()
    {
        
    }

    void createEquipRef()
    {
        float x = 0;
        foreach (EquipData data in equipCharate.equipDatas)
        { 
            GameObject item = Resources.Load<GameObject>("GameObjectRef/EquipRef");
            Vector2 pos = new Vector2(x,0);
            item.transform.localPosition = pos;
            x += 5;
            item.GetComponent<EquipItemScript>().Id = data.Id;
            item.GetComponent<EquipItemScript>().part = data.Part;
            Instantiate(item, parent);
        }
    }
}
