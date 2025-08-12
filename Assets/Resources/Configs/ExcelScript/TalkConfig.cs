using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Talk
{

	[System.Serializable]
	public class IdInfo
	{
		public int id;
		public string text;
	}

	public class TalkConfig: ScriptableObject
	{
		public List<IdInfo> idInfoList = new List<IdInfo>();
	}
}
