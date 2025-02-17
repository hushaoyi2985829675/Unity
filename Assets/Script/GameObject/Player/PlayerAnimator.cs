using Assets.HeroEditor.Common.CharacterScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    Player player;
    Character character;
    float attackInterval;
    void Start()
    {
        player = GetComponent<Player>();
        character = GetComponent<Character>();
        attackInterval = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isDeath)
        {
            return;
        }
        if (player.isGround)
        {
            if (Math.Abs(player.velocityX) > 0)
            {
                character.SetState(CharacterState.Run);
            }
            else
            {
                character.SetState(CharacterState.Idle);
            }
        }
        else
        {            
            character.SetState(CharacterState.Jump);        
        }

        //if (Input.GetKeyDown(KeyCode.J) && attackInterval <= 0 && !player.isWounded)
        //{
        //    animator.SetTrigger("Slash");
        //    attackInterval = player.PlayerValueData.PlayerInfo.AttackInterval;
        //}
        //else
        //{
        //    attackInterval -= Time.deltaTime;
        //}      
    }

    public void PlayTrigger(string name)
    {
        animator.SetTrigger(name);
    }

    public void PlayDeanth(Vector2 pos)
    {
        var dir = transform.localPosition.x - pos.x;
        if (dir > 0 && transform.localScale.x == 1 || dir < 0 && transform.localScale.x == -1)
        {
            animator.SetInteger("State", (int)CharacterState.DeathF);
        }
        else
        {
            animator.SetInteger("State", (int)CharacterState.DeathB);
        }
    }
}
