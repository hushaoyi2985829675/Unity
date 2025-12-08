using Assets.HeroEditor.Common.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Serialization;
using MonsterNs;

public class AttackState : StateBase
{
    Monster monster;
    Animator animator;
    AnimationEvents animationEvents;
    MonsterInfo monsterInfo;
    float dir;
    bool isAttack;
    float attackInterval;

    public AttackState(Monster monster)
    {
        this.monster = monster;
        animator = monster.GetAnimator();
        monsterInfo = monster.GetMonsterInfo();
        animationEvents = monster.GetAnimationEvents();
        animationEvents.OnCustomEvent += AttackEvent;
    }

    public override void onEnter()
    {
        monster.SetVelocityX(0);
        attackInterval = 0;
        animator.SetInteger("State", (int) State.Attack);
        isAttack = false;
    }

    public void AttackEvent(string name)
    {
        switch (name)
        {
            case "Default":
                isAttack = false;
                break;
        }
    }

    public override void onExit()
    {
        attackInterval = monsterInfo.attackInterval;
        animator.SetInteger("State", 0);
    }

    public override void onUpdate()
    {
        if (monster.GetPlayerTriggerZone())
        {
            if (!isAttack)
            {
                if (attackInterval <= 0)
                {
                    SetMonsterDic();
                    if (Math.Abs(dir) < monsterInfo.attackDistance)
                    {
                        animator.SetTrigger("Slash");
                        isAttack = true;
                        attackInterval = monsterInfo.attackInterval;
                    }
                    else
                    {
                        monster.ChangeState(State.Pursuit);
                    }
                }
                else
                {
                    attackInterval -= Time.deltaTime;
                }
            }
        }
        else
        {
            monster.ChangeState(State.Idle);
        }
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

    public override void onFixedUpdate()
    {
    }
}