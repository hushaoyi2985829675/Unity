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
    Attacked attacked;
    Character character;
    PlayerLocalValueData playerLocalValueData;
    private int eventId;
    private void Awake()
    {
        attacked = GetComponent<Attacked>();
        eventId = EventManager.Instance.AddEvent(GameEventType.WearEquipEvent, new object[] {(Action<EquipmentPart>) RefreshAttackSpeed});
    }

    void Start()
    {
        playerLocalValueData = GameDataManager.Instance.GetPlayerLocalValueInfo();
        player = GetComponent<Player>();
        character = GetComponent<Character>();
        RefreshAttackSpeed(EquipmentPart.MeleeWeapon1H);
    }
    
    void Update()
    {
        if (player.isDeath)
        {
            return;
        }

        if (player.isGround || player.isStairs)
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
        if (attacked.GetPlayerAttack())
        {
            SetFloatValue("RunSpeed", 0.3f);
        }
        else
        {
            SetFloatValue("RunSpeed", 1f);
        }
       
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

    public void SetFloatValue(string name, float value)
    {
        animator.SetFloat(name, value);
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
        if (part == EquipmentPart.MeleeWeapon1H)
        {
            animator.SetFloat("AttackSpeed", playerLocalValueData.AttackSpeed);
        }
    }

    private void OnDestroy()
    {
        EventManager.Instance?.RemoveEvent(GameEventType.WearEquipEvent, eventId);
    }
}
