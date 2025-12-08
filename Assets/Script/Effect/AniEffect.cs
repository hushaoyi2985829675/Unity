using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniEffect : EffectBase
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
    }

    public override void Play(Vector2 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
        animator.Play("Animation");
    }

    public override void onDelect()
    {
        gameObject.SetActive(false);
    }
}