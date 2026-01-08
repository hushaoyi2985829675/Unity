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
    private Rigidbody2D playerRd;

    [SerializeField]
    private SkillDemoMonster monster;

    Animator monsterAnimator;
    private Rigidbody2D monsterRd;
    Character character;
    PlayerEquip playerEquip;
    private int skillId;
    public override void onEnter(params object[] data)
    {
        playerSKill = player.GetComponent<PlayerSKill>();
        character = player.GetComponent<Character>();
        playerEquip = player.GetComponent<PlayerEquip>();
        playerRd = player.GetRigidbody();
        playerAnimator = player.GetComponent<PlayerAnimator>();
        monsterAnimator = monster.GetAnimator();
        monsterRd = monster.GetComponent<Rigidbody2D>();
    }

    public override void onShow(params object[] data)
    {
        character.SetState(CharacterState.Idle);
        playerEquip.RemoveAllEquip();
        skillId = (int) data[0];
        PlaySkill(skillId);
    }

    public void PlaySkill(int skillId)
    {
        this.skillId = skillId;
        RemoveAllScheduleAndDelay();
        ResetPos();
        if (skillId == 1)
        {
            ChargeAttack();
        }

        if (skillId == 2)
        {
            Dodge();
        }
    }

    public void ResetPos()
    {
        playerRd.velocity = Vector2.zero;
        playerAnimator.SetBoolValue("Action", false);
        playerAnimator.SetIntValue("State", (int) State.Idle);
        monsterAnimator.SetBool("Action", false);
        monsterAnimator.SetInteger("State", (int) State.Idle);
        monsterRd.velocity = Vector2.zero;
        if (skillId == 1)
        {
            player.SetPlayerPos(new Vector2(-5033.92f, -128.5953f));
            monster.SetMonsterPos(new Vector2(-5031.18f, -129.8094f));
        }

        if (skillId == 2)
        {
            player.SetPlayerPos(new Vector2(-5033.92f, -128.5953f));
            monster.SetMonsterPos(new Vector2(-5031.53f, -129.8094f));
        }
    }

    public override void onExit()
    {
    }

    private void Dodge()
    {
        AddScheduleCallback(() =>
        {
            ResetPos();
            AddDelayCallback(() =>
            {
                monster.Attack();
                AddDelayCallback(() =>
                {
                    playerSKill.PlaySkill(skillId);
                }, 0.3f);
            }, 0.5f);
        }, 2f);
    }

    private void ChargeAttack()
    {
        AddScheduleCallback(() =>
        {
            ResetPos();
            AddDelayCallback(() =>
            {
                playerSKill.PlaySkill(skillId);
            }, 0.5f);
        }, 3f);
    }
}