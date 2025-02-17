using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.CommonScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DeathState : StateBase
{
    Monster monster;
    private float dir;
    public DeathState(Monster monster)
    {
        this.monster = monster;
    }
    public override void onEnter()
    {
        monster.animationEvent.OnCustomEvent += AnimationEvent;
        dir = monster.gameObject.transform.localPosition.x - monster.detectPlayer.gameObject.transform.localPosition.x;
        if (dir > 0 && monster.direction == 1 || dir < 0 && monster.direction == -1 )
        {
            monster.animator.SetInteger("State",(int)CharacterState.DeathF);
        }
        else
        {
            monster.animator.SetInteger("State", (int)CharacterState.DeathB);
        }       
        monster.transform.localScale = new Vector2(monster.direction, 1);
    }
    public void AnimationEvent(string name)
    {
        if (name == "Death")
        {
            monster.CreateDeathEff();          
        }
    }
}
