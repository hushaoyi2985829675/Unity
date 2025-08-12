using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NpcTalkTask
{

	[System.Serializable]
	public class NpcIdInfo
	{
		public int npcId;
		public List<PlayerLvInfo> playerLvInfoList;
	}

	[System.Serializable]
	public class PlayerLvInfo
	{
		public int playerLv;
		public string ordinaryTaskLst;
		public string beforeTaskcompletion;
		public int taskId;
		public string inTaskTalkList;
		public string taskcompletionList;
		public List<ChoiceInfo> choiceInfoList;
	}

	[System.Serializable]
	public class ChoiceInfo
	{
		public int choice;
		public string title;
	}

	public class NpcTalkTaskConfig: ScriptableObject
	{
		public List<NpcIdInfo> npcIdInfoList = new List<NpcIdInfo>();
	}
}
