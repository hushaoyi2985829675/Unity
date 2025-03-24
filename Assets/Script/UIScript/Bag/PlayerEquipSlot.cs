using Assets.HeroEditor.Common.CommonScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEquipSlot : MonoBehaviour
{
    public List<GameObject> ItemList;

    public void RefreshAllEquip()
    {
        foreach (GameObject item in ItemList)
        {
            var equipInfo = transform.GetComponentInParent<BagLayer>().PlayerEquipData.EquipInfo.Find(data => data.name == item.name);
            if (equipInfo != null && equipInfo.SpriteGroupEntry != null && equipInfo.SpriteGroupEntry.Id != "" && equipInfo.SpriteGroupEntry.Id != null)
            {
                var IconInfo = item.GetComponentInParent<BagLayer>().IconCollection.Icons.Find(data => data.Id == equipInfo.SpriteGroupEntry.Id);
                if (IconInfo != null)
                {
                    item.transform.Find("EquipSpr").SetActive(true);
                    item.transform.Find("EquipSpr").GetComponent<Image>().sprite = IconInfo.Sprite;
                    item.transform.Find("DefalutSpr").SetActive(false);
                }
                else
                {
                    item.transform.Find("DefalutSpr").SetActive(true);
                    item.transform.Find("EquipSpr").SetActive(false);
                }
            }
            else
            {
                item.transform.Find("DefalutSpr").SetActive(true);
                item.transform.Find("EquipSpr").SetActive(false);
            }
        }
    }
}
