using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerLvAttrNs
{

	[System.Serializable]
	public class PlayerLvInfo
	{
		public int playerLv;
		public int attack;
		public int health;
		public float moveSpeed;
		public float attackSpeed;
		public int armor;
		public float critRate;
		public float critDamage;
		public float dodgeRate;
		public int exp;
	}

	public class PlayerLvAttrConfig: ScriptableObject
	{
		public List<PlayerLvInfo> playerLvInfoList = new List<PlayerLvInfo>();
	}
}
