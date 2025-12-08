using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.HeroEditor.Common.CharacterScripts;
using Cinemachine;
using MonsterNs;

public class PursuitState : StateBase
{
    Animator animator;
    AnimationEvents animetonEvent;
    Monster monster;
    MonsterInfo monsterInfo;
    float dir;

    public PursuitState(Monster monster)
    {
        this.monster = monster;
        animator = monster.transform.Find("Animation").GetComponent<Animator>();
        animetonEvent = monster.transform.Find("Animation").GetComponent<AnimationEvents>();
        monsterInfo = monster.GetMonsterInfo();
    }

    public override void onEnter()
    {
        animator.SetInteger("State", (int) CharacterState.Run);
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
            SetMonsterDic();
            if (Math.Abs(dir) > monsterInfo.attackDistance)
            {
                Run();
            }
            else
            {
                monster.ChangeState(State.Attack);
            }
        }
        else
        {
            monster.ChangeState(State.Idle);
        }
    }

    public void Run()
    {
        monster.SetVelocityX(monsterInfo.runSpeed * monster.GetDirection());
    }

    private void SetMonsterDic()
    {
        dir = monster.GetMonsterTargetDic();
        if (dir >= 0)
        {
            monster.SetDirection(-1);
        }
        else
        {
            monster.SetDirection(1);
        }
    }
}