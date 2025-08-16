using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{

	[System.Serializable]
	public class MapInfo
	{
		public int map;
		public string name;
		public string image;
		public string playerPos;
	}

	public class MapConfig: ScriptableObject
	{
		public List<MapInfo> mapInfoList = new List<MapInfo>();
	}
}
