using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResourceNs
{

	[System.Serializable]
	public class ResourceInfo
	{
		public int resource;
		public string name;
		public string icon;
		public string desc;
		public string ways;
	}

	public class ResourceConfig: ScriptableObject
	{
		public List<ResourceInfo> resourceInfoList = new List<ResourceInfo>();
	}
}
