using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.HeroEditor.Common.CharacterScripts;
public class PursuitState : StateBase
{
    Monster monster;
    public PursuitState(Monster monster)
    {
        this.monster = monster;
    }
    public override void onEnter()
    {
        monster.animator = monster.transform.Find("Animation").GetComponent<Animator>();
        monster.animator.SetInteger("State", (int)CharacterState.Run);
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

    public override void onFixedUpdate()
    {

    }
}
