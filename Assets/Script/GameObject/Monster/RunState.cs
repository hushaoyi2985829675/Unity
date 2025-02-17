using Assets.HeroEditor.Common.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Serialization;

public class RunState : StateBase
{
    Animator animator;
    AnimationEvents animetonEvent;
    Monster monster;
    float dir;
   
    public RunState(Monster monster)
    {
        this.monster = monster;
    }
    public override void onEnter()
    {
        animator = monster.transform.Find("Animation").GetComponent<Animator>();
        animetonEvent = monster.transform.Find("Animation").GetComponent<AnimationEvents>();     
        monster.transform.localScale = new Vector2(monster.direction, 1);
    }
    public override void onExit()
    {

    }
    public override void onUpdate()
    {
        if (animator != null) { }
    }
    public override void onFixedUpdate()
    {
        if (monster.detectPlayer)
        {
            setMosterDirection();
            if (Math.Abs(dir) > monster.attackDistance)
            {
                if (monster.getCurState() != State.Run)
                {
                    animator.SetInteger("State", (int)CharacterState.Run);
                }
                run();
            }
            else
            {
                monster.rd.velocity = new Vector2(0, monster.rd.velocity.y);
                monster.ChangeState(State.Attack);
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
    public void run()
    {
        animator.SetInteger("State",(int)CharacterState.Run);
        monster.rd.velocity = new Vector2(monster.runSpeed * monster.direction, monster.rd.velocity.y);
    }
}
