using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterEquip
{

	[System.Serializable]
	public class EquipType
	{
		public string equipType;
		public List<Order> orderList;
	}

	[System.Serializable]
	public class Order
	{
		public string order;
		public List<Name> nameList;
	}

	[System.Serializable]
	public class Name
	{
		public string name;
		public string id;
	}

	public class MonsterEquipConfig: ScriptableObject
	{
		public List<EquipType> equipTypeList = new List<EquipType>();
	}
}
