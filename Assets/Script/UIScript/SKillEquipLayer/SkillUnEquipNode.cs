using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CommonScripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillUnEquipNode : PanelBase
{
    [SerializeField]
    private Image skillImg;

    [SerializeField]
    private Image lockImg;

    [SerializeField]
    private Text skillName;

    private int skillId;
    private EventTrigger skillTrigger;
    RectTransform rectTransform;
    private List<SkillEquipNode> skillEquipNodeList;
    private Action<int> action;
    private Action<bool> maskAction;
    private bool isTarget;
    private int targetIdx;
    Vector3 oldPosition;

    public override void onEnter(params object[] data)
    {
        rectTransform = transform as RectTransform;
        skillTrigger = GetComponent<EventTrigger>();
        EventTrigger.Entry dragEntry = new EventTrigger.Entry();
        dragEntry.eventID = EventTriggerType.Drag;
        dragEntry.callback.AddListener(OnDrag);
        skillTrigger.triggers.Add(dragEntry);
        EventTrigger.Entry endDragEntry = new EventTrigger.Entry();
        endDragEntry.eventID = EventTriggerType.EndDrag;
        endDragEntry.callback.AddListener(OnEndDrop);
        skillTrigger.triggers.Add(endDragEntry);
        EventTrigger.Entry beginDragEntry = new EventTrigger.Entry();
        beginDragEntry.eventID = EventTriggerType.BeginDrag;
        beginDragEntry.callback.AddListener(OnBeginDrag);
        skillTrigger.triggers.Add(beginDragEntry);
    }

    public override void onShow(params object[] data)
    {
        skillId = (int) data[0];
        skillEquipNodeList = (List<SkillEquipNode>) data[1];
        maskAction = (Action<bool>) data[2];
        action = (Action<int>) data[3];
        RefreshUI(skillId);
    }

    public void RefreshUI(int id)
    {
        skillId = id;
        lockImg.SetActive(id == 0);
        skillImg.SetActive(id != 0);
        if (id == 0)
        {
            return;
        }

        Sprite icon = Ui.Instance.GetGoodIcon((int) GoodsType.Skill, id);
        skillImg.sprite = icon;
        skillName.text = Ui.Instance.GetSkillInfoById(id).name;
    }

    private void OnBeginDrag(BaseEventData baseEventData)
    {
        maskAction(true);
        oldPosition = transform.localPosition;
    }

    private void OnDrag(BaseEventData baseEventData)
    {
        PointerEventData eventData = baseEventData as PointerEventData;
        Vector2 pos = new Vector2(rectTransform.anchoredPosition.x + eventData.delta.x, rectTransform.anchoredPosition.y + eventData.delta.y);
        rectTransform.anchoredPosition = pos;
        for (int i = 0; i < skillEquipNodeList.Count; i++)
        {
            SkillEquipNode skillEquipNode = skillEquipNodeList[i];

            if (Vector2.Distance(transform.position, skillEquipNode.transform.position) < 100f)
            {
                transform.localScale = new Vector3(1.3f, 1.3f, 1);
                isTarget = true;
                targetIdx = i;
                break;
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
                isTarget = false;
            }
        }
    }

    private void OnEndDrop(BaseEventData baseEventData)
    {
        if (isTarget)
        {
            action(targetIdx);
        }
        else
        {
            transform.localPosition = oldPosition;
        }

        maskAction(false);
    }


    public override void onExit()
    {
    }
}