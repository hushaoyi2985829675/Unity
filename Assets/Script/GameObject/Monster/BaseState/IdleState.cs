using Assets.HeroEditor.Common.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using MonsterNs;
using UnityEngine;
using UnityEngine.Timeline;

public class IdleState : StateBase
{
    AnimationEvents animetonEvent;
    Monster monster;
    Animator animator;
    MonsterInfo monsterInfo;
    private float awaitTime;

    public IdleState(Monster monster)
    {
        this.monster = monster;
        monsterInfo = monster.GetMonsterInfo();
        animator = monster.GetAnimator();
    }

    public override void onEnter()
    {
        awaitTime = monsterInfo.awaitTime;
        animator.SetInteger("State", (int) State.Idle);
        monster.RefreshDirection();
        monster.SetVelocityX(0);
    }

    public override void onExit()
    {
        awaitTime = monsterInfo.awaitTime;
    }

    public override void onUpdate()
    {
        if (monster.GetPlayerTriggerZone())
        {
            monster.ChangeState(State.Pursuit);
            return;
        }

        if (awaitTime > 0)
        {
            awaitTime -= Time.deltaTime;
        }
        else
        {
            monster.ChangeState(State.Patrol);
        }
    }

    public override void onFixedUpdate()
    {
    }
}