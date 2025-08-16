using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goods
{

	[System.Serializable]
	public class GoodTypeInfo
	{
		public int goodType;
		public List<GoodInfo> goodInfoList;
	}

	[System.Serializable]
	public class GoodInfo
	{
		public int good;
		public string name;
		public string id;
		public int type;
		public string icon;
		public string image;
	}

	public class GoodsConfig: ScriptableObject
	{
		public List<GoodTypeInfo> goodTypeInfoList = new List<GoodTypeInfo>();
	}
}
