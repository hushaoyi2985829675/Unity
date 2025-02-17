using Assets.HeroEditor.Common.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState1 : StateBase
{
    Monster monster;
    public DeathState1(Monster monster)
    {
        this.monster = monster;
    }
    public override void onEnter()
    {
        monster.animator = monster.transform.Find("Animation").GetComponent<Animator>();
        monster.animator.SetInteger("State", (int)CharacterState.Idle);
    }
    public override void onExit()
    {
        monster.animator.SetInteger("State", 0);
    }
    public override void onUpdate()
    {
        if (monster.awaitTime > 0)
        {
            monster.awaitTime -= Time.deltaTime;
        }
        else
        {
            monster.awaitTime = monster.monsterValue.awaitTime;
            monster.ChangeState(State.Patrol);
        }
    }
}
