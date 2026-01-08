using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SkillNs;
using UnityEngine;
using Object = System.Object;

public class SkillEquipLayer : PanelBase
{
    [SerializeField]
    private Transform skillEquipParent;

    [SerializeField]
    private Transform localSkillImgParent;

    [SerializeField]
    private Transform unEquipSkillParent;

    [SerializeField]
    private List<GameObject> unEquipMaskImgList;

    private GameObject skillEquipNode;
    private GameObject skillNode;
    private List<int> skillList;
    private List<SkillEquipInfo> equipSkillList;
    private List<SkillEquipNode> skillEquipNodeList;

    public override void onEnter(params object[] data)
    {
        skillEquipNode = Ui.Instance.GetLayerRef("SkillEquipLayer/SkillEquipNode");
        skillNode = Ui.Instance.GetLayerRef("SkillEquipLayer/SkillUnEquipNode");
    }

    public override void onShow(params object[] data)
    {
        skillEquipNodeList = new List<SkillEquipNode>();
        OnUpEquipMaskImgAction(false);
        InitData();
        RefreshEquip();
    }

    void InitData()
    {
        equipSkillList = GameDataManager.Instance.GetPlayerSkillEquipList();
        skillList = GameDataManager.Instance.GetPlayerSkillList().ToList();
        foreach (var equipSkillInfo in equipSkillList)
        {
            skillList.Remove(equipSkillInfo.skillId);
        }

        skillList.Sort((a, b) =>
        {
            SkillInfo aSkillInfo = Ui.Instance.GetSkillInfoById(a);
            SkillInfo bSkillInfo = Ui.Instance.GetSkillInfoById(b);
            return aSkillInfo.colorLv > bSkillInfo.colorLv ? -1 : 1;
        });
    }

    private void InitSkillInfo()
    {
    }

    public override void onExit()
    {
    }

    private void RefreshEquip()
    {
        for (int i = 0; i < 3; i++)
        {
            int pos = i;
            SkillEquipNode skillEquip = AddUINode<SkillEquipNode>(skillEquipNode, skillEquipParent, new object[]
            {
                i, skillEquipNodeList, localSkillImgParent, (Action<bool>) OnUpEquipMaskImgAction, (Action<int>) ((targetPos) =>
                {
                    OnUnEquipEndDragAction(pos, targetPos);
                }),
                (Action<int, int>) OnRemoveSkillAction, unEquipMaskImgList[1]
            });
            skillEquipNodeList.Add(skillEquip);
        }

        for (int i = 0; i < skillList.Count; i++)
        {
            PanelBase panelBase = null;
            int id = skillList[i];
            panelBase = AddUINode(skillNode, unEquipSkillParent, new object[]
            {
                id, skillEquipNodeList, (Action<bool>) OnEquipMaskImgAction, (Action<int>) ((pos) =>
                {
                    OnEquipSkillAction(pos, id, panelBase);
                })
            });
        }
    }

    private void RefreshEquipNode(int i)
    {
        skillEquipNodeList[i].RefreshUI();
    }

    //未装备
    private void OnEquipSkillAction(int pos, int id, PanelBase panelBase)
    {
        CloseUINode(panelBase.transform);
        skillList.Remove(id);
        SkillEquipInfo skillInfo = equipSkillList.Find((data) => data.pos == pos);
        if (skillInfo != null && skillInfo.skillId != 0)
        {
            OnRemoveSkillAction(skillInfo.pos, skillInfo.skillId);
        }

        GameDataManager.Instance.SetPlayerSkillEquip(pos, id);
        RefreshEquipNode(pos);
        EventManager.Instance.PostEvent(GameEventType.PlayerSkillUpdateEvent, new object[] {pos});
    }

    //已装备
    private void OnUnEquipEndDragAction(int pos, int targetPos)
    {
        SkillEquipInfo skillInfo = equipSkillList.Find((data) => data.pos == pos);
        SkillEquipInfo targetSkillInfo = equipSkillList.Find((data) => data.pos == targetPos);
        int id = skillInfo.skillId;
        GameDataManager.Instance.SetPlayerSkillEquip(pos, targetSkillInfo?.skillId ?? 0);
        GameDataManager.Instance.SetPlayerSkillEquip(targetPos, id);
        RefreshEquipNode(pos);
        RefreshEquipNode(targetPos);
        //交换技能
        EventManager.Instance.PostEvent(GameEventType.PlayerSkillUpdateEvent, new object[] {pos});
        EventManager.Instance.PostEvent(GameEventType.PlayerSkillUpdateEvent, new object[] {targetPos});
    }

    private void OnUpEquipMaskImgAction(bool active)
    {
        foreach (var unEquipMaskImg in unEquipMaskImgList)
        {
            unEquipMaskImg.SetActive(active);
        }
    }

    private void OnEquipMaskImgAction(bool active)
    {
        unEquipMaskImgList[0].SetActive(active);
    }

    //卸下技能
    private void OnRemoveSkillAction(int targetPos, int targetId)
    {
        SkillInfo skillInfo = Ui.Instance.GetSkillInfoById(targetId);
        int pos = skillList.Count;
        for (int i = 0; i < skillList.Count; i++)
        {
            int id = skillList[i];
            SkillInfo oldSkillInfo = Ui.Instance.GetSkillInfoById(id);
            if (skillInfo.colorLv > oldSkillInfo.colorLv)
            {
                pos = i;
                break;
            }
            else if (skillInfo.colorLv == oldSkillInfo.colorLv)
            {
                if (skillInfo.skill > oldSkillInfo.skill)
                {
                    pos = i;
                }
                else
                {
                    pos = i + 1;
                }

                break;
            }
        }

        skillList.Insert(pos, targetId);
        PanelBase panelBase = null;
        panelBase = AddUINode(skillNode, unEquipSkillParent, new object[]
        {
            targetId, skillEquipNodeList, (Action<bool>) OnEquipMaskImgAction, (Action<int>) ((pos) =>
            {
                OnEquipSkillAction(pos, targetId, panelBase);
            })
        });
        panelBase.transform.SetSiblingIndex(pos);
        GameDataManager.Instance.SetPlayerSkillEquip(targetPos, 0);
        RefreshEquipNode(targetPos);
        EventManager.Instance.PostEvent(GameEventType.PlayerSkillUpdateEvent, new object[] {targetPos});
    }
}