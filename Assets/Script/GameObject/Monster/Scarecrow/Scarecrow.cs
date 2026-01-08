using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : Monster
{
    // Start is called before the first frame update

    new void Start()
    {
        base.Start();
    }

    public override void InitState(out Dictionary<State, StateBase> stateList)
    {
        stateList = new Dictionary<State, StateBase>();
    }

    // Update is called once per frame
    new void Update()
    {
    }
}