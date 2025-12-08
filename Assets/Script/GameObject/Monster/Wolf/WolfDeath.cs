using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;

public class WolfDeath : StateBase
{
    Monster monster;
    Animator animator;
    AnimationEvents animationEvents;
    private float dir;

    public WolfDeath(Monster monster)
    {
        this.monster = monster;
        animator = monster.GetAnimator();
        animationEvents = monster.GetAnimationEvents();
        animationEvents.OnCustomEvent += AnimationEvent;
    }

    public override void onEnter()
    {
        animator.SetInteger("State", (int) State.Deanth);
    }

    public void AnimationEvent(string name)
    {
        if (name == "Death")
        {
            monster.CreateDeathEff();
        }
    }
}