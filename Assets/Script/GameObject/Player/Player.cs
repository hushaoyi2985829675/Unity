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
    private PlayerLocalValueData playerLocalValueData;

    // [Header("玩家等级加成")]
    // public PlayerLvData PlayerLvData;
    
    [Header("移动参数")]
    public float speed;
    public float jumpHeight;
    
    [Header("状态相关")]
    Rigidbody2D rd;
    CapsuleCollider2D coll;
    RaycastHit2D leftFoot;
    RaycastHit2D rightFoot;
    RaycastHit2D stairsHit;
    public bool isStairs;
    private Vector2 slopeDirection;
    private float slopeAngle;
    AnimationEvents animationEvent;
    PlayerAnimator playerAnimator;
    PlayerSKill playerSKill;
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
    #region 初始化
    void Start()
    {
        playerLocalValueData = GameDataManager.Instance.GetPlayerLocalValueInfo();
        // 初始化组件引用
        rd = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        attacked = GetComponent<Attacked>();
        
        // 初始化动画相关组件
        animationEvent = transform.Find("Animation").GetComponent<AnimationEvents>();
        playerAnimator = GetComponent<PlayerAnimator>();
        playerSKill = GetComponent<PlayerSKill>();
        // 注册动画事件回调
        animationEvent.OnCustomEvent += AttackEvent;
        
        // 初始化玩家数据
        scale = transform.localScale;
        speed = playerLocalValueData.MoveSpeed;
        jumpHeight = playerLocalValueData.jumpSpeed;
        jumpNum = playerLocalValueData.JumpNum;
        
        // 更新装备
        //  UpdateEquip();
    }
    #endregion
    
    #region 更新循环
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
    
    #region 物理检测
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
        
        // 检测地面
        leftFoot = Tool.Raycast(new Vector2(-coll.size.x / 2 + 0.3f, -coll.size.y / 2), Vector2.down, 0.15f, LayerMask.GetMask("Ground"), transform.position
        );
        
        rightFoot = Tool.Raycast(new Vector2(coll.size.x / 2 - 0.3f, -coll.size.y / 2), Vector2.down, 0.15f, LayerMask.GetMask("Ground"), transform.position
        );
        
        isGround = leftFoot || rightFoot;
        
        if (isGround)
        {
            jumpNum = playerLocalValueData.JumpNum;
        }
        else
        {
            isJump = false;
        }
        
        // 控制朝向
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
            // 斜坡移动
            rd.velocity = new Vector2(
                -velocityX * speed * slopeDirection.x,
                -velocityX * speed * slopeDirection.y
            );
        }
        else
        {
            float localSpeed = speed;
            if (attacked.GetAttackState())
            {
                localSpeed *= 0.3f;
            }
            // 平地移动
            rd.velocity = new Vector2(velocityX * localSpeed, rd.velocity.y);
        }
    }
    
    void JumpAction()
    {
        // 处理跳跃输入
        if (isJumpKey && jumpNum > 0)
        {
            rd.velocity = new Vector2(rd.velocity.x, 0);
            rd.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            jumpNum -= 1;
            isJumpKey = false;
            isJump = true;
        }
        
        // 如果在斜坡上，不应用额外的重力效果
        if (isStairs)
        {
            return;
        }
        
        // 调整跳跃物理
        if (rd.velocity.y < 0) // 下落时
        {
            rd.velocity += Vector2.up * Physics2D.gravity.y * (6f) * Time.deltaTime;
        }
        else if (rd.velocity.y > 0) // 上升时
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

    public void Hit(float attackPower, Monster monster)
    {
        Vector2 attackerPosition = monster.transform.position;
        float harm = Math.Max(0, attackPower - playerLocalValueData.Armor);
        GameDataManager.Instance.SetPlayerHp((int) Math.Max(0, playerLocalValueData.CurHp - harm));
        EventManager.Instance.PostEvent(GameEventType.PlayerUIStateEvent);
        if (playerLocalValueData.CurHp == 0)
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
        AudioManager.Instance.PlayAudio(gameObject, AudioType.Attack, "Hit");
        playerAnimator.PlayTrigger("Hit");
    }
    #endregion
    
    void AttackEvent(string eventName)
    {
        switch (eventName)
        {
            case "Wounded":
                isWounded = false;
                break;
            case "Death":
                break;
        }
    }
    
    public void SetPlayerPos(Vector2 position)
    {
        transform.localPosition = position;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Equip"))
        {
            // 处理装备拾取
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Stairs"))
        {
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Equip"))
        {
            // 处理装备碰撞
        }
    }

    public float GetPlayerBoxPosX()
    {
        float dis = transform.localScale.x > 0 ? 1 : -1;
        return transform.position.x + dis;
    }

    public int GetDirection()
    {
        return transform.localScale.x > 0 ? 1 : -1;
    }
    // 其他方法
    // private void UpdateEquip()
    // {
    //     // 更新装备逻辑
    // }
}

