using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.HeroEditor.Common.CharacterScripts;

public class WoundedState : StateBase
{
    AnimationEvents animetonEvent;
    AnimationEvents animationEvents;
    Animator animator;
    Monster monster;

    public WoundedState(Monster monster)
    {
        this.monster = monster;
        animator = monster.GetAnimator();
        animator = monster.transform.Find("Animation").GetComponent<Animator>();
        animationEvents = monster.GetAnimationEvents();
        animationEvents.OnCustomEvent += AnimationEvent;
    }

    public override void onEnter()
    {
        animator.SetTrigger("Hit");
    }

    public override void onExit()
    {
    }

    public override void onUpdate()
    {
    }

    void AnimationEvent(string name)
    {
        if (name == "Wounded")
        {
            Debug.Log("�л�");
            monster.ChangeState(State.Patrol);
        }
    }
}