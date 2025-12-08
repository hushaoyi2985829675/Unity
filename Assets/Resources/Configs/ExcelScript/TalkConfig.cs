using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TalkNs
{

	[System.Serializable]
	public class TalkInfo
	{
		public int talk;
		public string text;
	}

	public class TalkConfig: ScriptableObject
	{
		public List<TalkInfo> talkInfoList = new List<TalkInfo>();
	}
}
