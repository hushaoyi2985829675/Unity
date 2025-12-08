using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterNs
{

	[System.Serializable]
	public class MonsterInfo
	{
		public int monster;
		public string name;
		public int exp;
		public int maxHp;
		public int walkSpeed;
		public int runSpeed;
		public int awaitTime;
		public int attackInterval;
		public int attackPower;
		public int armor;
		public float attackDistance;
		public float critRate;
		public float critDamage;
	}

	public class MonsterConfig: ScriptableObject
	{
		public List<MonsterInfo> monsterInfoList = new List<MonsterInfo>();
	}
}
