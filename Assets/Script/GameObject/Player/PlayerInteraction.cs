using System;
using System.Collections;
using System.Collections.Generic;
using EquipNs;
using HeroEditor.Common;
using HeroEditor.Common.Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("移动")]
    [SerializeField]
    private EventTrigger leftTrigger;

    [SerializeField]
    private EventTrigger rightTrigger;

    [SerializeField]
    private EventTrigger attackTrigger;

    [Header("攻击")]
    [SerializeField]
    private Image weaponImg;

    [SerializeField]
    private Image colorImg;

    [Header("跳跃")]
    [SerializeField]
    private EventTrigger jumpTrigger;

    [Header("技能")]
    [SerializeField]
    private Transform skillParent;

    private GameObject skillNode;
    private Dictionary<int, SkillInteractionNode> skillNodeDict = new Dictionary<int, SkillInteractionNode>();
    List<SkillEquipInfo> skillEquipList;

    private void Awake()
    {
        skillNode = Ui.Instance.GetLayerRef("PlayerInteraction/SkillInteractionNode");
        BindTrigger(leftTrigger, -1);
        BindTrigger(rightTrigger, 1);
        Tool.BindTrigger(attackTrigger, EventTriggerType.PointerDown, (baseEventData) =>
        {
            EventManager.Instance.PostEvent(GameEventType.PlayerAttackEvent);
        });
        Tool.BindTrigger(jumpTrigger, EventTriggerType.PointerDown, (baseEventData) =>
        {
            EventManager.Instance.PostEvent(GameEventType.PlayerJumpEvent);
        });
        EventManager.Instance.AddEvent(GameEventType.WearEquipEvent, new object[] {(Action<EquipmentPart>) RefreshWeapon});
        //技能事件
        EventManager.Instance.AddEvent(GameEventType.PlayerSkillUpdateEvent, new object[] {(Action<int>) RefreshSkill});
    }

    void Start()
    {
        skillEquipList = GameDataManager.Instance.GetPlayerSkillEquipList();
        RefreshWeapon(EquipmentPart.MeleeWeapon1H);
        //刷新技能
        CreateSKill();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void BindTrigger(EventTrigger trigger, int direction)
    {
        Tool.BindTrigger(trigger, EventTriggerType.PointerDown, (baseEventData) =>
        {
            DirectionClick(direction);
        });
        Tool.BindTrigger(trigger, EventTriggerType.PointerUp, (baseEventData) =>
        {
            DirectionClick(0);
        });
    }

    void DirectionClick(int direction)
    {
        EventManager.Instance.PostEvent(GameEventType.PlayerMoveEvent, new object[] {direction});
    }

    void RefreshWeapon(EquipmentPart part)
    {
        EquipData equipData = GameDataManager.Instance.GetPlayerEquipData(part);

        if (part == EquipmentPart.MeleeWeapon1H)
        {
            if (equipData.id == -1)
            {
                return;
            }

            Sprite sprite = Ui.Instance.GetGoodIcon((int) GoodsType.Equip, equipData.id);
            weaponImg.sprite = sprite;
            EquipInfo equipInfo = Ui.Instance.GetEquipInfo(equipData.id);
            colorImg.color = Ui.Instance.GetColorByLv(equipInfo.lv);
        }
    }

    void CreateSKill()
    {
        for (int i = 0; i < 3; i++)
        {
            if (!skillNodeDict.ContainsKey(i))
            {
                GameObject obj = Instantiate(skillNode, skillParent.transform);
                skillNodeDict[i] = obj.GetComponent<SkillInteractionNode>();
            }

            RefreshSkill(i);
        }
    }

    void RefreshSkill(int pos)
    {
        SkillEquipInfo skillEquipInfo = skillEquipList.Find((data) => data.pos == pos);
        if (skillEquipInfo != null && skillEquipInfo.skillId != 0)
        {
            skillNodeDict[pos].RefreshUI(skillEquipInfo.skillId);
        }
        else
        {
            skillNodeDict[pos].RefreshUI(0);
        }
    }
}