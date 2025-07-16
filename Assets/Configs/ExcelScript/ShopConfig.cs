using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShopConfig
{

	[System.Serializable]
	public class Type
	{
		public string type;
		public Id id;
	}


	[System.Serializable]
	public class Id
	{
		public string id;
		public Name name;
	}


	[System.Serializable]
	public class Name
	{
		public string name;
		public bool open;
	}


	class ShopConfig : ScriptableObject
	{
		public List<Type> data = new List<Type>();
	}
}
