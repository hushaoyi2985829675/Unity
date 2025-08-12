using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Task
{

	[System.Serializable]
	public class TaskId
	{
		public int taskId;
		public string name;
		public string des;
		public int need;
	}

	public class TaskConfig: ScriptableObject
	{
		public List<TaskId> taskIdList = new List<TaskId>();
	}
}
