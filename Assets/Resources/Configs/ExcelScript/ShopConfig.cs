using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShopNs
{

	[System.Serializable]
	public class ShopInfo
	{
		public int shop;
		public int goodType;
		public int id;
		public int num;
		public string price;
		public int limitedBuy;
	}

	public class ShopConfig: ScriptableObject
	{
		public List<ShopInfo> shopInfoList = new List<ShopInfo>();
	}
}
