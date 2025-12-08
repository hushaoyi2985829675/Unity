using Assets.HeroEditor.Common.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryState : StateBase
{
    Monster monster;
    Animator animator;
    private float dir;

    public VictoryState(Monster monster)
    {
        this.monster = monster;
        animator = monster.GetAnimator();
    }

    public override void onEnter()
    {
        animator.SetBool("Victory", true);
    }

    public void AnimationEvent(string name)
    {
        if (name == "Death")
        {
            monster.CreateDeathEff();
        }
    }
}