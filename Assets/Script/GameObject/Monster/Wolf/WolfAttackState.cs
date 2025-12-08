using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using MonsterNs;
using UnityEngine;

public class WolfAttackState : StateBase
{
    Monster monster;
    MonsterInfo monsterInfo;
    Animator animator;
    AnimationEvents animationEvents;
    float dir;
    bool isAttack;
    float attackInterval;

    public WolfAttackState(Monster monster)
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
            case "Attack":
                Attack();
                break;
            case "Default":
                isAttack = false;
                break;
        }
    }

    public override void onExit()
    {
        attackInterval = monsterInfo.attackInterval;
        animator.SetInteger("State", (int) State.Idle);
    }

    public override void onUpdate()
    {
        if (monster.GetPlayerTriggerZone())
        {
            SetMonsterDic();
            if (Math.Abs(dir) < monsterInfo.attackDistance)
            {
                if (attackInterval <= 0)
                {
                    animator.SetInteger("State", (int) State.Attack);
                    isAttack = true;
                    attackInterval = monsterInfo.attackInterval;
                }
                else
                {
                    if (!isAttack)
                    {
                        animator.SetInteger("State", (int) State.Idle);
                    }

                    attackInterval -= Time.deltaTime;
                }
            }
            else
            {
                if (!isAttack)
                {
                    monster.ChangeState(State.Pursuit);
                }
            }
        }
        else
        {
            if (!isAttack)
            {
                monster.ChangeState(State.Idle);
            }
        }
    }

    private void Attack()
    {
        var hit = Tool.OverlapCircle(monster.GetEdgeTrans().position, 0.2f, LayerMask.GetMask("Player"));
        AudioManager.Instance.PlayAudio(monster.gameObject, AudioType.Attack, "WolfAttack");
        if (hit)
        {
            var player = hit.GetComponent<Player>();
            float power = monsterInfo.attackPower;
            //暴击
            if (Ui.Instance.IsCriticalAttack(monsterInfo.critRate))
            {
                power *= monsterInfo.critDamage;
            }

            //player.Hit(power, monster);
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