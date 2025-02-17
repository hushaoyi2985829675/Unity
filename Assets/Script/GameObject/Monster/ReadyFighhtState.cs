using Assets.HeroEditor.Common.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyFightState : StateBase
{
    AnimationEvents animetonEvent;
    Monster monster;
    public ReadyFightState(Monster monster)
    {
        this.monster = monster;
    }
    public override void onEnter()
    {
        monster.animator = monster.transform.Find("Animation").GetComponent<Animator>();
        monster.animator.SetInteger("State", (int)CharacterState.Idle);
        monster.animator.SetBool("Ready", true);
        monster.rd.velocity = new Vector2(0, monster.rd.velocity.y);
    }
    public override void onExit()
    {
        monster.animator.SetBool("Ready", false);
    }
    public override void onUpdate()
    {
        if (monster.awaitTime > 0)
        {
            monster.awaitTime -= Time.deltaTime;
        }
        else
        {
            if (monster.detectPlayer)
            {
                monster.awaitTime = monster.monsterValue.awaitTime;
                monster.ChangeState(State.Run);
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
