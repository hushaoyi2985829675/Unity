using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Option
{

	[System.Serializable]
	public class OptionInfo
	{
		public string option;
		public string optionText;
		public List<TaskEnumInfo> taskEnumInfoList;
	}

	[System.Serializable]
	public class TaskEnumInfo
	{
		public string taskEnum;
		public string answers;
	}

	public class OptionConfig: ScriptableObject
	{
		public List<OptionInfo> optionInfoList = new List<OptionInfo>();
	}
}
