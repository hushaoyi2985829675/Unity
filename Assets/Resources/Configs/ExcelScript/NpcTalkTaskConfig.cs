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
		public string name;
		public List<PlayerTaskLvInfo> playerTaskLvInfoList;
	}

	[System.Serializable]
	public class PlayerTaskLvInfo
	{
		public int playerTaskLv;
		public int taskId;
		public int taskLocation;
		public int taskRequirement;
		public string taskReward;
		public int ordinaryTask;
		public int beforeTaskCompletion;
		public int inTaskTalk;
		public int taskCompletion;
		public List<ChoiceInfo> choiceInfoList;
	}

	[System.Serializable]
	public class ChoiceInfo
	{
		public int choice;
		public string title;
		public int answer;
	}

	public class NpcTalkTaskConfig: ScriptableObject
	{
		public List<NpcIdInfo> npcIdInfoList = new List<NpcIdInfo>();
	}
}
