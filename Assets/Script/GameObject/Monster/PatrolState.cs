using Assets.HeroEditor.Common.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PatrolState : StateBase
{
    Animator animator;
    AnimationEvents animetonEvent;
    Monster monster;
    public PatrolState(Monster monster)
    {
        this.monster = monster;
    }
    public override void onEnter()
    {
        animator = monster.transform.Find("Animation").GetComponent<Animator>();
        animetonEvent = monster.transform.Find("Animation").GetComponent<AnimationEvents>();
        animator.SetInteger("State", (int)State.Walk);
        monster.transform.localScale = new Vector2(monster.direction,1);
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
            monster.ChangeState(State.ReadyFight);
            return;
        }
        if (monster.direction == 1)
        {
            if (monster.leftWall)
            {
                monster.direction = -1;
                monster.ChangeState(State.Idle);
            }
            else
            {
                walk();
            }
        }
        else
        {
            if (monster.rightWall)
            {
                monster.direction = 1;
                monster.ChangeState(State.Idle);
            }
            else
            {
                walk();
            }
        }
    }

    public void walk()
    {
        monster.rd.velocity = new Vector2(monster.walkSpeed * monster.direction, monster.rd.velocity.y);
    }
}
