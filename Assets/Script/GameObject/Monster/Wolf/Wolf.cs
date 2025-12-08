using System;
using System.Collections;
using System.Collections.Generic;
using MonsterNs;
using UnityEngine;

public class Wolf : Monster
{
    new void Start()
    {
        base.Start();
    }

    public override void InitState(out Dictionary<State, StateBase> stateList)
    {
        stateList = new Dictionary<State, StateBase>();
        stateList.Add(State.Idle, new IdleState(this));
        stateList.Add(State.Patrol, new WolfPatrolState(this));
        stateList.Add(State.Pursuit, new PursuitState(this));
        stateList.Add(State.Attack, new WolfAttackState(this));
        stateList.Add(State.Hit, new WolfHit(this));
        stateList.Add(State.Deanth, new WolfDeath(this));
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }


    new void FixedUpdate()
    {
        base.FixedUpdate();
    }
}