using System;
using System.Collections;
using System.Collections.Generic;
using SkillNs;
using UnityEngine;

public class PlayerSKill : MonoBehaviour
{
    PlayerAnimator playerAnimator;

    private void Awake()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    public void PlaySkill(int id)
    {
        SkillInfo skillInfo = Ui.Instance.GetSkillInfoById(id);
        if (skillInfo.id == 2)
        {
            Dodge(skillInfo);
        }
    }

    //闪避
    private void Dodge(SkillInfo skillInfo)
    {
        playerAnimator.SetIntValue("SkillId", skillInfo.id);
        playerAnimator.PlayTrigger("PlaySkill");
    }
}