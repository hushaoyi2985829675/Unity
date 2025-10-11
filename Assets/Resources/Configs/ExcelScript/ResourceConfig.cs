using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resource
{

	[System.Serializable]
	public class ResourceInfo
	{
		public int resource;
		public string name;
		public string icon;
		public string desc;
	}

	public class ResourceConfig: ScriptableObject
	{
		public List<ResourceInfo> resourceInfoList = new List<ResourceInfo>();
	}
}
