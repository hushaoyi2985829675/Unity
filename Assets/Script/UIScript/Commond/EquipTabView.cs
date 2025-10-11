using System;
using System.Collections;
using System.Collections.Generic;
using HeroEditor.Common.Enums;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class EquipTabInfo
{
    public int tag;
    public Sprite icon;
}

public class EquipTabView : MonoBehaviour
{
    [SerializeField] private GameObject TabItem;
    [Header("方向")] public DirectionType directionType = DirectionType.Horizontal;

    [Header("间距")] public int spacing = 40;
    [SerializeField] private List<EquipTabInfo> TabList;

    private int selTag;
    private Dictionary<int, Toggle> itemDic = new Dictionary<int, Toggle>();
    private Action<int> callback;

    public void InitTabView(Action<int> callback)
    {
        if (TabList == null)
        {
            return;
        }

        selTag = TabList[0].tag;
        AddLayoutGroup();
        foreach (EquipTabInfo info in TabList)
        {
            Toggle toggle = Instantiate(TabItem, transform).GetComponent<Toggle>();
            toggle.isOn = info.tag == selTag;
            toggle.group = transform.GetComponent<ToggleGroup>();
            toggle.transform.Find("Icon").GetComponent<Image>().sprite = info.icon;
            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn && selTag != info.tag)
                {
                    selTag = info.tag;
                    callback((int) selTag);
                }
            });
            itemDic[info.tag] = toggle;
        }

        //callback((int) selTag);
        this.callback = callback;
    }

    public void SetSelTag(int tag)
    {
        itemDic[tag].isOn = true;
        callback(tag);
    }

    private void AddLayoutGroup()
    {
        HorizontalOrVerticalLayoutGroup layoutGroup;
        if (directionType == DirectionType.Horizontal)
        {
            layoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
        }
        else
        {
            layoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
        }

        layoutGroup.padding = new RectOffset(0, 0, 0, 0);
        layoutGroup.spacing = spacing;
        layoutGroup.childAlignment = TextAnchor.MiddleCenter;
        layoutGroup.childControlHeight = false;
        layoutGroup.childControlWidth = false;
        layoutGroup.childForceExpandHeight = false;
        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childScaleHeight = false;
        layoutGroup.childScaleWidth = false;
    }
}