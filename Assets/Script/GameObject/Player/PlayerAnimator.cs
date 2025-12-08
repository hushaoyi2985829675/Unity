using Assets.HeroEditor.Common.CharacterScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using HeroEditor.Common.Enums;
using UnityEditor;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    Player player;
    Character character;
    PlayerLocalValueData playerLocalValueData;

    private void Awake()
    {
        EventManager.Instance.AddEvent(GameEventType.WearEquipEvent, new object[] {(Action<EquipmentPart>) RefreshAttackSpeed});
    }

    void Start()
    {
        playerLocalValueData = GameDataManager.Instance.GetPlayerLocalValueInfo();
        player = GetComponent<Player>();
        character = GetComponent<Character>();
        RefreshAttackSpeed(EquipmentPart.Armor);
    }
    
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
                if (character.GetState() != CharacterState.Run)
                {
                    character.SetState(CharacterState.Run);
                }
            }
            else
            {
                if (character.GetState() != CharacterState.Idle)
                {
                    character.SetState(CharacterState.Idle);
                }
            }
        }
        else
        {
            if (character.GetState() != CharacterState.Jump)
            {
                character.SetState(CharacterState.Jump);
            }
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

    public void SetBoolValue(string name, bool value)
    {
        animator.SetBool(name, value);
    }

    public void SetIntValue(string name, int value)
    {
        animator.SetInteger(name, value);
    }

    public void PlayDeanth(Vector2 pos)
    {
        var dir = transform.position.x - pos.x;
        if (dir > 0 && transform.localScale.x == 1 || dir < 0 && transform.localScale.x == -1)
        {
            animator.SetInteger("State", (int)CharacterState.DeathF);
        }
        else
        {
            animator.SetInteger("State", (int)CharacterState.DeathB);
        }
    }

    public void RefreshAttackSpeed(EquipmentPart part)
    {
        animator.SetFloat("AttackSpeed", playerLocalValueData.AttackSpeed);
    }
}
