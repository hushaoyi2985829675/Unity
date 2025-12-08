using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NpcNs
{

	[System.Serializable]
	public class NpcIdInfo
	{
		public int npcId;
		public string name;
	}

	public class NpcConfig: ScriptableObject
	{
		public List<NpcIdInfo> npcIdInfoList = new List<NpcIdInfo>();
	}
}
