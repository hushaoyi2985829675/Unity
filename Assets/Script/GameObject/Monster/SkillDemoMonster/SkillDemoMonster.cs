using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDemoMonster : Monster
{
    private new void Awake()
    {
        base.Awake();
    }

    new void Start()
    {
        base.Start();
    }

    public override void InitState(out Dictionary<State, StateBase> stateList)
    {
        stateList = new Dictionary<State, StateBase>();
    }

    public void Attack()
    {
        animator.SetTrigger("Slash");
    }


    public void SetMonsterPos(Vector2 position)
    {
        transform.localPosition = position;
    }
}