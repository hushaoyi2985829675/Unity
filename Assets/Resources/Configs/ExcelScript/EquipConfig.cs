using HeroEditor.Common;
using HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Equip
{
	[System.Serializable]
	public class EquipInfo
	{
		public int equip;
		public string name;
		public string id;
		public int part;
		public string synthesisRoute;
		public string desc;

		// 重写ToString()，在Inspector列表中显示装备名称
		public override string ToString()
		{
			// 可以更详细地显示，比如包含部位信息
			return $"{name} (部位:{part})";
		}
	}

	// 添加创建菜单，方便在Project窗口创建配置文件
	[CreateAssetMenu(
		fileName = "EquipConfig",
		menuName = "配置/装备配置",
		order = 100)]
	public class EquipConfig : ScriptableObject
	{
		// 序列化列表，在Inspector中可见
		[SerializeField] 
		public List<EquipInfo> equipInfoList = new List<EquipInfo>();

		// 可选：添加验证方法，确保数据合法性
		private void OnValidate()
		{
			// 例如：检查name不为空
			foreach (var info in equipInfoList)
			{
				if (string.IsNullOrEmpty(info.name))
				{
					Debug.LogWarning("存在未命名的装备信息", this);
				}
			}
		}

		// 可选：添加获取装备信息的工具方法
		public EquipInfo GetEquipInfoById(string targetId)
		{
			return equipInfoList.Find(info => info.id == targetId);
		}
	}
}
