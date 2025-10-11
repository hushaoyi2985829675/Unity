using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableBase : ScriptableObject
{
    public abstract void Create();
    public abstract void Clear();
}