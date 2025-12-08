using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectBase : MonoBehaviour
{
    protected EffectType effectType;

    public abstract void Play(Vector2 target);

    public abstract void onDelect();
}