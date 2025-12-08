using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TaskNs
{

	[System.Serializable]
	public class TaskInfo
	{
		public int task;
		public string name;
		public string des;
		public int taskType;
		public int targetType;
	}

	public class TaskConfig: ScriptableObject
	{
		public List<TaskInfo> taskInfoList = new List<TaskInfo>();
	}
}
