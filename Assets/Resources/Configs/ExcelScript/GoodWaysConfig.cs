using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodWaysNs
{

	[System.Serializable]
	public class GoodWay
	{
		public int goodWay;
		public int id;
		public string way;
	}

	public class GoodWaysConfig: ScriptableObject
	{
		public List<GoodWay> goodWayList = new List<GoodWay>();
	}
}
