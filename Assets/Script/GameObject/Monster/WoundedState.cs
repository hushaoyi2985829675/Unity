using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.HeroEditor.Common.CharacterScripts;
public class WoundedState : StateBase
{
    AnimationEvents animetonEvent;
    Monster monster;
    public WoundedState(Monster monster)
    {
        this.monster = monster;
    }
    public override void onEnter()
    {
        monster.animator = monster.transform.Find("Animation").GetComponent<Animator>();
        monster.animationEvent.OnCustomEvent += AnimationEvent;
        monster.animator.SetTrigger("Hit");
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
            Debug.Log("ÇÐ»»");
            monster.ChangeState(State.Patrol);
        }
    }
}
