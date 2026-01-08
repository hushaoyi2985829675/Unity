using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CommonScripts;
using SkillNs;
using UnityEngine;
using UnityEngine.UI;

public class SkillNode : PanelBase
{
    List<int> skillIdList;

    [SerializeField]
    private Button skillBtn;

    [SerializeField]
    private Image lockImg;

    [SerializeField]
    private Image selImg;

    public override void onEnter(params object[] data)
    {
    }

    public override void onShow(params object[] data)
    {
        skillIdList = GameDataManager.Instance.GetPlayerSkillList();
    }

    public void InitData(SkillInfo skillInfo, int selId, Action<int> action)
    {
        bool isLock = skillIdList.Find(skillId => skillId == skillInfo.id) != 0;
        Ui.Instance.SetGray(skillBtn.image, !isLock);
        lockImg.SetActive(!isLock);
        skillBtn.onClick.RemoveAllListeners();
        selImg.SetActive(selId == skillInfo.id);
        skillBtn.image.sprite = Ui.Instance.GetGoodIcon((int) GoodsType.Skill, skillInfo.id);
        skillBtn.onClick.AddListener(() =>
        {
            action?.Invoke(skillInfo.id);
        });
    }

    public void RefreshLockState()
    {
        Ui.Instance.SetGray(skillBtn.image, false);
        lockImg.SetActive(false);
    }

    public void RefreshSelSate(bool isShow)
    {
        selImg.SetActive(isShow);
    }

    public override void onExit()
    {
    }
}