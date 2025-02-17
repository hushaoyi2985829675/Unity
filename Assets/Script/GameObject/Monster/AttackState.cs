using Assets.HeroEditor.Common.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Serialization;

public class AttackState : StateBase
{
    Monster monster;
    AnimationEvents animationEvent;
    float dir;
    bool isAttack;
    float attackInterval;

    public AttackState(Monster monster)
    {
        this.monster = monster;
    }
    public override void onEnter()
    {  
        animationEvent = monster.transform.Find("Animation").GetComponent<AnimationEvents>();
        attackInterval = monster.attackInterval;
        monster.animator.SetInteger("State", (int)CharacterState.Idle);
        isAttack = false;
        animationEvent.OnCustomEvent += AttackEvent;
    }
    public void AttackEvent(string name)
    {
        switch (name)
        {
            case "AttackEnd":
               isAttack = false;
                break;
        }
    }
    public override void onExit()
    {
        monster.animator.SetInteger("State", 0);
    }
    public override void onUpdate()
    {
        if (monster.animator != null) { }
    }
    public override void onFixedUpdate()
    {
        if (monster.detectPlayer)
        {
            if (!isAttack)
            {
                if (attackInterval < 0)
                {

                    setMosterDirection();
                    if (Math.Abs(dir) < monster.attackDistance)
                    {
                        monster.animator.SetTrigger("Slash");
                        isAttack = true;
                        attackInterval = monster.attackInterval;
                    }
                    else
                    {
                        monster.ChangeState(State.Run);
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

    public void setMosterDirection()
    {
        dir = monster.gameObject.transform.localPosition.x - monster.detectPlayer.gameObject.transform.localPosition.x;
        if (dir > 0)
        {
            monster.direction = -1;
            monster.transform.localScale = new Vector2(-1, monster.transform.localScale.y);
        }
        else
        {
            monster.direction = 1;
            monster.transform.localScale = new Vector2(1, monster.transform.localScale.y);
        }
    }
}
