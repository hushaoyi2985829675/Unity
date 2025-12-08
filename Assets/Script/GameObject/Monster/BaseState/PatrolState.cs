using Assets.HeroEditor.Common.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using MonsterNs;
using UnityEngine;

public class PatrolState : StateBase
{
    Animator animator;
    AnimationEvents animetonEvent;
    Monster monster;
    MonsterInfo monsterInfo;
    private float time = 1;
    private bool idle = true;

    public PatrolState(Monster monster)
    {
        this.monster = monster;
        monsterInfo = monster.GetMonsterInfo();
        animator = monster.GetAnimator();
    }

    public override void onEnter()
    {
        animator.SetInteger("State", (int) State.Walk);
    }

    public override void onExit()
    {
    }

    public override void onUpdate()
    {
        if (animator != null)
        {
        }
    }

    public override void onFixedUpdate()
    {
        if (monster.GetPlayerTriggerZone())
        {
            monster.ChangeState(State.ReadyFight);
            return;
        }

        if (monster.IsNeedTurn() && idle)
        {
            if (time <= 0)
            {
                time = 1;
                idle = false;
                monster.ChangeState((int) State.Idle);
            }
            else
            {
                time -= Time.deltaTime;
                monster.SetVelocityX(0);
                animator.SetInteger("State", (int) State.Idle);
            }
        }
        else
        {
            walk();
        }

        if (!monster.IsNeedTurn())
        {
            idle = true;
        }
    }

    public void walk()
    {
        monster.SetVelocityX(monsterInfo.walkSpeed * monster.GetDirection());
    }
}