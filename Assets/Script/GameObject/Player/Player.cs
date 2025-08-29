using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.FantasyHeroes.TestRoom.Scripts;
using HeroEditor.Common.Enums;
using HeroEditor.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;

public enum ButtonState
{
    Attack,
    Talk,
    Stairs
}

public partial class Player : MonoBehaviour
{
    [Header("�����ֵ")]
    public PlayerValueData PlayerValueData;
    
    [Header("��ҵȼ��ӳ�")]
    public PlayerLvData PlayerLvData;
    
    [Header("�ƶ�����")]
    public float speed;
    public float jumpHeight;
    
    [Header("״̬���")]
    Rigidbody2D rd;
    CapsuleCollider2D coll;
    RaycastHit2D leftFoot;
    RaycastHit2D rightFoot;
    RaycastHit2D stairsHit;
    public bool isStairs;
    private Vector2 slopeDirection;
    private float slopeAngle;
    PlayerUI playerUI;
    AnimationEvents animationEvent;
    PlayerAnimator playerAnimator;
    Attacked attacked;
    Vector2 scale;
    public bool isJumpKey;
    public bool isJump;
    public float velocityX;
    public float velocityY;
    public bool isGround;
    public float jumpNum;
    public bool isWounded;
    public bool isDeath;
    Action PlayerEvent;
    public ButtonState attackState;
    private Vector3 groundPos;
    #region ��ʼ��
    void Start()
    {
        // ��ʼ���������
        rd = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        attacked = GetComponent<Attacked>();
        playerUI = GetComponent<PlayerUI>();
        
        // ��ʼ������������
        animationEvent = transform.Find("Animation").GetComponent<AnimationEvents>();
        playerAnimator = GetComponent<PlayerAnimator>();
        
        // ע�ᶯ���¼��ص�
        animationEvent.OnCustomEvent += AttackEvent;
        
        // ��ʼ���������
        scale = transform.localScale;
        speed = PlayerValueData.PlayerInfo.walkSpeed;
        jumpHeight = PlayerValueData.PlayerInfo.jumpSpeed;
        jumpNum = PlayerValueData.PlayerInfo.JumpNum;
        
        // ��ʼ��UI
        playerUI.InitUI();
        
        // ע����Ҷ���
        GameObjectManager.instance.SetPlayer(this);
        
        // ����װ��
        UpdateEquip();
    }
    #endregion
    
    #region ����ѭ��
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpNum > 0 && !isWounded && !isDeath && !isStairs)
        { 
            isJumpKey = true;
        }

        if (stairsHit && Input.GetKeyDown(KeyCode.K) && !isJumpKey)
        {
            isStairs = true;
            rd.gravityScale = 0;
        }
    }
    
    void FixedUpdate()
    {
        PhysicsCheck();
        SlopeCheck();
        MoveAction();
        JumpAction();
    }
    #endregion
    
    #region ������
    void PhysicsCheck()
    {
        if (!isWounded && !isDeath)
        {
            velocityX = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            velocityX = 0;
        }
        
        // ������
        leftFoot = Tool.Raycast(
            new Vector2(-coll.size.x / 2 + 0.3f, -coll.size.y / 2),
            Vector2.down,
            0.15f,
            LayerMask.GetMask("Ground"),
            transform.localPosition
        );
        
        rightFoot = Tool.Raycast(
            new Vector2(coll.size.x / 2 - 0.3f, -coll.size.y / 2),
            Vector2.down,
            0.15f,
            LayerMask.GetMask("Ground"),
            transform.localPosition
        );
        
        isGround = leftFoot || rightFoot;
        
        if (isGround)
        {
            groundPos = transform.localPosition;
            jumpNum = PlayerValueData.PlayerInfo.JumpNum;
        }
        else
        {
            isJump = false;
        }
        
        // ���Ƴ���
        if (velocityX > 0)
        {
            transform.localScale = new Vector2(scale.x, transform.localScale.y);
        }
        
        if (velocityX < 0)
        {
            transform.localScale = new Vector2(-scale.x, transform.localScale.y);
        }
    }
    
    void SlopeCheck()
    {
        stairsHit = Tool.Raycast(
            new Vector2(0, -coll.size.y / 2 + 0.2f),
            Vector2.down,
            0.4f,
            LayerMask.GetMask("Stairs"),
            transform.localPosition
        );
        
        if (stairsHit)
        {
            Vector2 normal = stairsHit.normal;
            slopeDirection = Vector2.Perpendicular(normal).normalized;
            Debug.DrawRay(stairsHit.point, normal, Color.blue);
            Debug.DrawRay(stairsHit.point, slopeDirection * 50, Color.magenta);
            if (-velocityX * slopeDirection.y < 0)
            {
                isStairs = true;
                rd.gravityScale = 0;
            }
        }
        else
        {
            isStairs = false;
            rd.gravityScale = 1;
        }
    }
    
    void MoveAction()
    {
        if (isStairs)
        {
            // б���ƶ�
            rd.velocity = new Vector2(
                -velocityX * speed * slopeDirection.x,
                -velocityX * speed * slopeDirection.y
            );
        }
        else
        {
            // ƽ���ƶ�
            rd.velocity = new Vector2(velocityX * speed, rd.velocity.y);
        }
    }
    
    void JumpAction()
    {
        // ������Ծ����
        if (isJumpKey && jumpNum > 0)
        {
            rd.velocity = new Vector2(rd.velocity.x, 0);
            rd.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            jumpNum -= 1;
            isJumpKey = false;
            isJump = true;
        }
        
        // �����б���ϣ���Ӧ�ö��������Ч��
        if (isStairs)
        {
            return;
        }
        
        // ������Ծ����
        if (rd.velocity.y < 0) // ����ʱ
        {
            rd.velocity += Vector2.up * Physics2D.gravity.y * (6f) * Time.deltaTime;
        }
        else if (rd.velocity.y > 0) // ����ʱ
        {
            rd.velocity += Vector2.up * Physics2D.gravity.y * (1.5f) * Time.deltaTime;
        }
        
        velocityY = rd.velocity.y;
    }
    
    void DeathState(Vector2 attackerPosition)
    {
        isDeath = true;
        playerAnimator.PlayDeanth(attackerPosition);
    }
    
    public void Attacked(float attackPower, Vector2 attackerPosition)
    {
        float harm = Math.Max(0, attackPower - PlayerValueData.PlayerInfo.Armor);
        PlayerValueData.PlayerInfo.Hp = Math.Max(0, PlayerValueData.PlayerInfo.Hp - harm);
        playerUI.setHp(PlayerValueData.PlayerInfo.Hp);
        
        if (PlayerValueData.PlayerInfo.Hp == 0)
        {
            DeathState(attackerPosition);
        }
        else
        {
            HitState();
        }
    }
    
    void HitState()
    {
        isWounded = true;
        playerAnimator.PlayTrigger("Hit");
    }
    #endregion
    
    void AttackEvent(string eventName)
    {
        switch (eventName)
        {
            case "Hit":
                attacked.Hit();
                break;
            case "Wounded":
                isWounded = false;
                break;
            case "Death":
                Tool.onPlayerEvent();
                break;
        }
    }
    
    public void RefreshUI()
    {
        playerUI.InitUI();
    }
    
    public void SetPlayerPos(Vector2 position)
    {
        transform.localPosition = position;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Equip"))
        {
            // ����װ��ʰȡ
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Stairs"))
        {
            
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Equip"))
        {
            // ����װ����ײ
        }
    }
    
    // ��������
    // private void UpdateEquip()
    // {
    //     // ����װ���߼�
    // }
}

