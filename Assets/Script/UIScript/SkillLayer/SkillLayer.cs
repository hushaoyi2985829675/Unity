using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    Text haveNum;

    [SerializeField]
    List<int> skillList;

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
        skillConfig.Sort((a, b) =>
        {
            return a.colorLv > b.colorLv ? 1 : -1;
        });
    }

    public override void onShow(params object[] data)
    {
        selIndex = 0;
        RefreshUI();
        RefreshSkillDemo();
        RefreshHaveNum();
        tableView.SetNum(skillConfig.Count);
    }

    public void RefreshSkillNode(int index, GameObject obj)
    {
        SkillInfo skillInfo = skillConfig[index];
        SkillNode skillNode = obj.GetComponent<SkillNode>();
        skillNode.InitData(skillInfo, skillConfig[selIndex].id, (id) =>
        {
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
        SkillInfo skillInfo = skillConfig[selIndex];
        skillName.text = skillInfo.name;
        skillDesc.text = skillInfo.desc;
    }

    private void RefreshHaveNum()
    {
        haveNum.text = string.Format("{0}/{1}", skillList.Count, skillConfig.Count);
    }

    public override void onExit()
    {
    }
}