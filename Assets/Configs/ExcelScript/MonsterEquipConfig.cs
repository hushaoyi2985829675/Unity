using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterEquipConfig
{

	[System.Serializable]
	public class EquipType
	{
		public string equipType;
		public Name name;
	}


	[System.Serializable]
	public class Name
	{
		public string name;
		public string id;
	}


	public class MonsterEquip : ScriptableObject
	{
		public List<EquipType> data = new List<EquipType>();
	}
}
