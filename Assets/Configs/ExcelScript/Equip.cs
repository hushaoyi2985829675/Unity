using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Equip
{

	[System.Serializable]
	public class EquipType
	{
		public string equipType;
	public string Id;
	public int Num;
	public Has has;
	}




	[System.Serializable]
	public class Has
	{
		public string has;
	}

	class Equip : ScriptableObject
	{
		public List<EquipType> data = new List<EquipType>();
	}
}
