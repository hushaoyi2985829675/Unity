using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingredient
{

	[System.Serializable]
	public class MaterialInfo
	{
		public int material;
		public string name;
	}

	public class IngredientConfig: ScriptableObject
	{
		public List<MaterialInfo> materialInfoList = new List<MaterialInfo>();
	}
}
