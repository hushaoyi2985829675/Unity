using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.CommonScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DeathState : StateBase
{
    Monster monster;
    Animator animator;
    AnimationEvents animationEvents;
    private float dir;

    public DeathState(Monster monster)
    {
        this.monster = monster;
        animator = monster.GetComponent<Animator>();
        animationEvents = monster.GetAnimationEvents();
        animationEvents.OnCustomEvent += AnimationEvent;
    }

    public override void onEnter()
    {
        dir = monster.GetMonsterTargetDic();
        float direction = monster.GetDirection();
        if (dir > 0 && direction == 1 || dir < 0 && direction == -1)
        {
            animator.SetInteger("State", (int) CharacterState.DeathF);
        }
        else
        {
            animator.SetInteger("State", (int) CharacterState.DeathB);
        }

        monster.transform.localScale = new Vector2(direction, 1);
    }

    public void AnimationEvent(string name)
    {
        if (name == "Death")
        {
            monster.CreateDeathEff();
        }
    }
}