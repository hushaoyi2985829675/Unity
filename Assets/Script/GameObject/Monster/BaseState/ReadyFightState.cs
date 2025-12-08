using Assets.HeroEditor.Common.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using MonsterNs;
using UnityEngine;

public class ReadyFightState : StateBase
{
    AnimationEvents animetonEvent;
    MonsterInfo monsterInfo;
    private float awaitTime;
    Animator animator;
    Monster monster;

    public ReadyFightState(Monster monster)
    {
        this.monster = monster;
        monsterInfo = Ui.Instance.GetMonsterValue(monster.monsterId);
        animator = monster.GetAnimator();
        animator = monster.transform.Find("Animation").GetComponent<Animator>();
    }

    public override void onEnter()
    {
        awaitTime = monsterInfo.awaitTime;
        animator.SetInteger("State", (int) CharacterState.Idle);
        animator.SetBool("Ready", true);
        monster.SetVelocityX(0);
    }

    public override void onExit()
    {
        animator.SetBool("Ready", false);
    }

    public override void onUpdate()
    {
        if (awaitTime > 0)
        {
            awaitTime -= Time.deltaTime;
        }
        else
        {
            if (monster.GetPlayerTriggerZone())
            {
                // monster.awaitTime = monster.monsterInfo.awaitTime;
                monster.ChangeState(State.Pursuit);
            }
            else
            {
                monster.ChangeState(State.Patrol);
            }
        }
    }

    public override void onFixedUpdate()
    {
    }
}