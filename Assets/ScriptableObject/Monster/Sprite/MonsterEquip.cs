using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroEditor.Common.Enums;

[CreateAssetMenu(fileName = "MonsterEquip", menuName = "Character Data/Monster/MonsterEquip")]
public class MonsterEquip : ScriptableObject
{
   public List<EquipList> EquipList;
   public string GetEquip(EquipmentPart part)
   {
      var info = EquipList.Find(data => data.Part == part);
      if (info == null )
      {
         return "";
      }
      //随机
      var idx = Random.Range(0, info.EquipDatas.Length);
      return info.EquipDatas[idx];
   }
}

[System.Serializable]
public class EquipList
{
   public EquipmentPart Part;
   public string[] EquipDatas;
}
