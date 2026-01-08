using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CommonScripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillEquipNode : PanelBase
{
    [SerializeField]
    private Transform skillNodeParent;

    [SerializeField]
    private Image skillImg;

    [SerializeField]
    private Image lockImg;

    [SerializeField]
    private Text skillName;

    [SerializeField]
    private EventTrigger skillTrigger;

    private Transform skillImgParent;
    private int skillId;
    RectTransform rectTransform;
    private bool isTarget;
    private bool isRemove;
    private int targetIdx;
    Vector3 oldPosition;
    private int pos;
    private List<SkillEquipInfo> skillEquipList;
    private GameObject skillUnEquipNode;
    private SkillUnEquipNode skillNode;
    private List<SkillEquipNode> skillEquipNodeList;
    private Action<int> endAction;
    private Action<bool> maskAction;
    private Action<int, int> removeAction;
    private SkillEquipNode targetSkillNode;
    private RectTransform removeMaskTrans;

    [SerializeField]
    Text skillNodeName;

    public override void onEnter(params object[] data)
    {
        skillUnEquipNode = Ui.Instance.GetLayerRef("SkillEquipLayer/SkillUnEquipNode");
        skillEquipList = GameDataManager.Instance.GetPlayerSkillEquipList();
        rectTransform = skillImg.transform as RectTransform;
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
        skillEquipNodeList = new List<SkillEquipNode>();
        pos = (int) data[0];
        skillEquipNodeList = (List<SkillEquipNode>) data[1];
        skillImgParent = (Transform) data[2];
        maskAction = (Action<bool>) data[3];
        endAction = (Action<int>) data[4];
        removeAction = (Action<int, int>) data[5];
        GameObject removeMaskObj = (GameObject) data[6];
        removeMaskTrans = (RectTransform) removeMaskObj.transform;
        skillNodeName.text = string.Format("技能栏{0}", pos + 1);
        RefreshUI();
    }

    public override void onExit()
    {
    }

    public void RefreshUI()
    {
        SkillEquipInfo skillEquipInfo = skillEquipList.Find((data) => data.pos == pos);
        int id = skillEquipInfo?.skillId ?? 0;
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
        if (skillId == 0)
        {
            return;
        }

        maskAction(true);
        oldPosition = skillImg.transform.localPosition;
        skillImg.transform.SetParent(skillImgParent);
    }

    private void OnDrag(BaseEventData baseEventData)
    {
        if (skillId == 0)
        {
            return;
        }

        PointerEventData eventData = baseEventData as PointerEventData;
        Vector2 newPos = new Vector2(rectTransform.anchoredPosition.x + eventData.delta.x / transform.parent.localScale.x, rectTransform.anchoredPosition.y + eventData.delta.y / transform.parent.localScale.y);
        rectTransform.anchoredPosition = newPos;
        for (int i = 0; i < skillEquipNodeList.Count; i++)
        {
            SkillEquipNode skillEquipNode = skillEquipNodeList[i];
            if (skillEquipNode.gameObject == gameObject)
            {
                continue;
            }

            skillEquipNode.SetSkillImgScale(1f);
            isTarget = false;
            if (Vector2.Distance(skillImg.transform.position, skillEquipNode.transform.position) < 100f)
            {
                skillEquipNode.SetSkillImgScale(1.3f);
                isTarget = true;
                targetSkillNode = skillEquipNode;
                targetIdx = i;
                break;
            }
        }

        isRemove = false;
        Vector2 maskPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(removeMaskTrans, skillImg.transform.position, null, out maskPos))
        {
            if (removeMaskTrans.rect.Contains(maskPos))
            {
                SetSkillImgScale(1.3f);
                isRemove = true;
            }
        }
    }

    private void OnEndDrop(BaseEventData baseEventData)
    {
        if (skillId == 0)
        {
            return;
        }

        if (isTarget)
        {
            targetSkillNode.SetSkillImgScale(1f);
            endAction(targetIdx);
        }

        if (isRemove)
        {
            SetSkillImgScale(1f);
            removeAction(pos, skillId);
        }

        maskAction(false);
        skillImg.transform.SetParent(skillNodeParent);
        skillImg.transform.localPosition = oldPosition;
    }

    public void SetSkillImgScale(float scale)
    {
        skillImg.transform.localScale = new Vector3(scale, scale, 1);
    }
}