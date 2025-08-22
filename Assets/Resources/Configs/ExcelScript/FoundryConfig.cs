using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foundry
{

	[System.Serializable]
	public class FoundryInfo
	{
		public int foundry;
	}

	public class FoundryConfig: ScriptableObject
	{
		public List<FoundryInfo> foundryInfoList = new List<FoundryInfo>();
	}
}
