using Assets.HeroEditor.Common.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryState : StateBase
{
    Monster monster;
    private float dir;
    public VictoryState(Monster monster)
    {
        this.monster = monster;
    }
    public override void onEnter()
    {
        monster.animator.SetBool("Victory",true);
    }
    public void AnimationEvent(string name)
    {
        if (name == "Death")
        {
            monster.CreateDeathEff();
        }
    }
}
