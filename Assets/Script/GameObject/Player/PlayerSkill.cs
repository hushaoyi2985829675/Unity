using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using SkillNs;
using UnityEngine;

public class PlayerSKill : MonoBehaviour
{
    [SerializeField]
    private Transform edge;
    PlayerAnimator playerAnimator;
    Player player;
    AnimationEvents animationEvent;
    private void Awake()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        player = GetComponent<Player>();
        animationEvent = transform.Find("Animation").GetComponent<AnimationEvents>();
        animationEvent.OnSkillEvent += AttackEvent;
    }

    public void PlaySkill(int id)
    {
        SkillInfo skillInfo = Ui.Instance.GetSkillInfoById(id);
        player.SetSkillState(true);
        playerAnimator.SetIntValue("SkillId", skillInfo.id);
        playerAnimator.PlayTrigger("PlaySkill");
        if (skillInfo.id == 1)
        {
            ChargeAttack();
        }
        else if (skillInfo.id == 2)
        {
            Dodge();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            PlaySkill(1);
        }
    }

    //闪避
    private void Dodge()
    {
        player.SetPlayerState(PlayerState.walk, false);
        player.SetPlayerState(PlayerState.jump, false);
        Rigidbody2D rd = player.GetRigidbody();
        int direction = player.GetDirection();
        rd.AddForce(new Vector2(25 * direction, 0), ForceMode2D.Impulse);
    }

    //重击
    private void ChargeAttack()
    {
        player.SetPlayerState(PlayerState.walk, false);
        player.SetPlayerState(PlayerState.jump, false);
    }

    private void ChargeHit()
    {
        Collider2D[] hits = Tool.OverlapBox(edge.position, new Vector2(2.5f, 2.5f), LayerMask.GetMask("Monster"));
        if (hits.Length > 0)
        {
            foreach (Collider2D hit in hits)
            {
                hit.gameObject.GetComponent<Monster>().Hit(0);
            }
        }
    }

    private void AttackEvent(string eventName)
    {
        switch (eventName)
        {
            case "DodgeEnd":
                player.SetPlayerState(PlayerState.walk, true);
                player.SetPlayerState(PlayerState.jump, true);
                player.SetDodgeState(false);
                player.SetSkillState(false);
                break;
            case "ChargeHit":
                ChargeHit();
                break;
            case "ChargeEnd":
                player.SetPlayerState(PlayerState.walk, true);
                player.SetPlayerState(PlayerState.jump, true);
                player.SetSkillState(false);
                break;
        }
    }
}