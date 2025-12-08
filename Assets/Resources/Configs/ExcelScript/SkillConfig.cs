using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillNs
{

	[System.Serializable]
	public class SkillInfo
	{
		public int skill;
		public int id;
		public string name;
		public int lockLv;
		public string price;
		public float duration;
		public float cd;
		public int buff;
		public string skillImg;
		public int colorLv;
		public string desc;
	}

	public class SkillConfig: ScriptableObject
	{
		public List<SkillInfo> skillInfoList = new List<SkillInfo>();
	}
}
