using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroEditor.Common.Enums;

[CreateAssetMenu(fileName = "MonsterEquip", menuName = "Character Data/Monster/MonsterEquip")]
public class MonsterEquip : ScriptableObject
{
   public List<EquipList> EquipList;

   public List<EquipData> MonsterEquipData = new List<EquipData>();
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

   public void SetEquip(EquipmentPart part, string id)
   {
      var equipinfo =  MonsterEquipData.Find(data => data.Part == part);
      if (equipinfo == null)
      {
         equipinfo = new EquipData();
         MonsterEquipData.Add(equipinfo);
      }
      equipinfo.Part = part; 
      equipinfo.Id = id;
      equipinfo.name = Tool.GetEquipPartName(part);
   }

   public void InitMonsterEquipData()
   {
      MonsterEquipData = new List<EquipData>();
   }
}

[System.Serializable]
public class EquipList
{
   public string name;
   public EquipmentPart Part;
   public string[] EquipDatas;
}

