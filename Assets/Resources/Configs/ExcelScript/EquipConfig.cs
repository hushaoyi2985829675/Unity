using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Equip
{

	[System.Serializable]
	public class EquipInfo
	{
		public int equip;
		public string name;
		public string id;
		public int part;
		public int lv;
		public string buyPrice;
		public string sellPrice;
		public string attr;
		public string synthesisRoute;
		public string desc;
	}

	public class EquipConfig: ScriptableObject
	{
		public List<EquipInfo> equipInfoList = new List<EquipInfo>();
	}
}
