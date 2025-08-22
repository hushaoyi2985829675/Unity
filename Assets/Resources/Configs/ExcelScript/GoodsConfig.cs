using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goods
{

	[System.Serializable]
	public class GoodInfo
	{
		public int good;
		public string name;
		public string icon;
		public string image;
		public string desc;
	}

	public class GoodsConfig: ScriptableObject
	{
		public List<GoodInfo> goodInfoList = new List<GoodInfo>();
	}
}
