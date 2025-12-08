using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BagAttrNs
{

	[System.Serializable]
	public class BagAttr
	{
		public int bagAttr;
		public int type;
	}

	public class BagAttrConfig: ScriptableObject
	{
		public List<BagAttr> bagAttrList = new List<BagAttr>();
	}
}
