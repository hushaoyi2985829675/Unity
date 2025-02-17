using Assets.HeroEditor.Common.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IdleState : StateBase
{   
    AnimationEvents animetonEvent;
    Monster monster;
    public IdleState(Monster monster)
    { 
        this.monster = monster;
    }
    public override void onEnter()
    {
        monster.animator = monster.transform.Find("Animation").GetComponent<Animator>();
        monster.animator.SetInteger("State", (int)State.Idle);
        monster.rd.velocity = new Vector2(0, monster.rd.velocity.y);
    }
    public override void onExit()
    {
        
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
