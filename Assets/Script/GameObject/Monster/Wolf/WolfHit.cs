using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;

public class WolfHit : StateBase
{
    AnimationEvents animetonEvent;
    AnimationEvents animationEvents;
    Animator animator;
    Monster monster;

    public WolfHit(Monster monster)
    {
        this.monster = monster;
        animator = monster.GetAnimator();
        animator = monster.transform.Find("Animation").GetComponent<Animator>();
        animationEvents = monster.GetAnimationEvents();
        animationEvents.OnCustomEvent += AnimationEvent;
    }

    public override void onEnter()
    {
        animator.SetInteger("State", (int) State.Hit);
    }

    public override void onExit()
    {
    }

    public override void onUpdate()
    {
    }

    void AnimationEvent(string name)
    {
        if (name == "HitEnd")
        {
            monster.ChangeState(State.Pursuit);
        }
    }
}