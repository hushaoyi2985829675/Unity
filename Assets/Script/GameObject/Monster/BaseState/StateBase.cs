using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.HeroEditor.Common.CharacterScripts;

public enum State
{
    Idle = 0,
    Walk = 1,
    Pursuit = 2,
    Attack = 3,
    Patrol = 4,
    ReadyFight = 5,
    Hit = 6,
    Deanth = 7,
    Victory = 8,
}

public abstract class StateBase
{
    public virtual void onEnter()
    {
    }

    public virtual void onExit()
    {
    }

    public virtual void onUpdate()
    {
    }

    public virtual void onFixedUpdate()
    {
    }
}