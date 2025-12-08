using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using SkillNs;
using UnityEngine;

public class SkillDemo : PanelBase
{
    [SerializeField]
    private Player player;

    PlayerSKill playerSKill;
    PlayerAnimator playerAnimator;
    Character character;
    PlayerEquip playerEquip;
    private int id;

    public override void onEnter(params object[] data)
    {
        playerSKill = player.GetComponent<PlayerSKill>();
        character = player.GetComponent<Character>();
        playerEquip = player.GetComponent<PlayerEquip>();
    }

    public override void onShow(params object[] data)
    {
        character.SetState(CharacterState.Idle);
        playerEquip.RemoveAllEquip();
        playerEquip.isUpdateEquip = false;
        id = 0;
        PlaySkill((int) data[0]);
    }

    public void PlaySkill(int skillId)
    {
        if (id != 0)
        {
            TimerManage.Instance.RemoveScheduleCallback(id);
        }

        id = TimerManage.Instance.AddScheduleCallback(() =>
        {
            playerSKill.PlaySkill(skillId);
        }, 3f);
    }

    public override void onExit()
    {
        TimerManage.Instance.RemoveScheduleCallback(id);
    }
}