using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.CommonScripts;
using SkillNs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillLayer : PanelBase
{
    List<SkillInfo> skillConfig;

    [SerializeField]
    TableView tableView;

    [SerializeField]
    Text skillName;

    [SerializeField]
    TextMeshProUGUI skillDesc;

    [SerializeField]
    private Image skillIcon;
    [SerializeField]
    Text haveNum;

    List<int> skillList;

    [Header("ResText")]
    [SerializeField]
    private ResText resTextRef;

    [SerializeField]
    private Transform resTextTrans;

    ResText resText;

    [SerializeField]
    Button lockButton;

    [SerializeField]
    private Button skillEquipBtn;

    [SerializeField]
    Text lockText;
    [Header("技能演示父节点")]
    [SerializeField]
    Transform skillDemoTrans;

    [SerializeField]
    GameObject skillDemoRef;
   

    SkillDemo skillDemo;

    private SkillNode selSkillNode;
    private int selIndex;

    public override void onEnter(params object[] data)
    {
        skillConfig = Ui.Instance.GetSkillConfig().Values.ToList();
        tableView.AddRefreshEvent(RefreshSkillNode);
        skillList = GameDataManager.Instance.GetPlayerSkillList();
        skillConfig.Sort((a, b) => a.colorLv > b.colorLv ? 1 : -1);
        lockButton.onClick.AddListener(() =>
        {
            OnBuyClick();
        });
        skillEquipBtn.onClick.AddListener(() =>
        {
            OnSkillEquipClick();
        });
    }

    public override void onShow(params object[] data)
    {
        selIndex = 0;
        RefreshUI();
        RefreshSkillDemo();
        tableView.SetNum(skillConfig.Count);
    }

    private void RefreshSkillNode(int index, GameObject obj)
    {
        SkillInfo skillInfo = skillConfig[index];
        SkillNode skillNode = obj.GetComponent<SkillNode>();
        skillNode.InitData(skillInfo, skillConfig[selIndex].id, (id) =>
        {
            if (selIndex == index)
            {
                return;
            }
            if (selSkillNode != null)
            {
                selSkillNode.RefreshSelSate(false);
            }
            
            selIndex = index;
            selSkillNode = skillNode;
            selSkillNode.RefreshSelSate(true);
            RefreshUI();
            skillDemo.PlaySkill(skillConfig[selIndex].id);
        });
        if (selSkillNode == null && index == 0)
        {
            selSkillNode = skillNode;
            selSkillNode.RefreshSelSate(true);
        }
    }

    private void RefreshSkillDemo()
    {
        skillDemo = AddUINode<SkillDemo>(skillDemoRef, skillDemoTrans, new object[] {skillConfig[selIndex].id});
    }

    private void RefreshUI()
    {
        RefreshHaveNum();
        SkillInfo skillInfo = skillConfig[selIndex];
        skillName.text = skillInfo.name;
        skillDesc.text = skillInfo.desc;
        skillIcon.sprite = Ui.Instance.GetGoodIcon((int) GoodsType.Skill, skillInfo.id);
        bool isLock = skillList.Find((id) => id == skillInfo.id) != 0;
        if (!isLock)
        {
            if (resText == null)
            {
                resText = AddUINode<ResText>(resTextRef.gameObject, resTextTrans, new object[]
                {
                    skillInfo.price
                });
            }
            else
            {
                resText.RefreshUI(skillInfo.price);
            }

            resText.SetActive(true);
        }
        else
        {
            if (resText)
            {
                resText.SetActive(false);
            }
            //是否装备
        }
    }

    private void RefreshHaveNum()
    {
        haveNum.text = string.Format("{0}/{1}", skillList.Count, skillConfig.Count);
    }

    private void OnBuyClick()
    {
        SkillInfo skillInfo = skillConfig[selIndex];
        ResClass costRes = Ui.Instance.FormatResStr(skillInfo.price)[0];

        if (!Ui.Instance.GetResNumIsEnough(GoodsType.Resource, costRes.resourceId, costRes.num))
        {
            return;
        }

        Ui.Instance.ShowConfirmationLayer("解锁", () =>
        {
            GameDataManager.Instance.AddSkill(skillInfo.id);
            GameDataManager.Instance.DecreaseRes(costRes.resourceId, costRes.num);
            skillList = GameDataManager.Instance.GetPlayerSkillList();
            RefreshUI();
            selSkillNode.RefreshLockState();
            Ui.Instance.ShowFlutterView($"获得技能{skillInfo.name}");
        });
    }

    private void OnSkillEquipClick()
    {
        GameObject obj = Ui.Instance.GetLayerRef("SkillEquipLayer/SkillEquipLayer");
        UIManager.Instance.OpenLayer(obj);
    }

    public override void onExit()
    {
        tableView.Clear();
    }
}