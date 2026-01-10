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

public enum PlayerState
{
    walk,
    jump,
}

public partial class Player : MonoBehaviour
{
    private PlayerLocalValueData playerLocalValueData;

    // [Header("玩家等级加成")]
    // public PlayerLvData PlayerLvData;
    [Header("是否监听事件")]
    public bool isListenEvent;
    
    [Header("移动参数")]
    public float speed;
    public float jumpHeight;
    
    [Header("状态相关")]
    [SerializeField]
    private bool isSkill;
    
    private bool isDodge;
    RaycastHit2D leftFoot;
    RaycastHit2D rightFoot;
    RaycastHit2D stairsHit;
    public bool isStairs;
    public bool isStairsModel;
    private bool isUpStairs;
    private Vector2 slopeDirection;
    private float slopeAngle;
    AnimationEvents animationEvent;
    PlayerAnimator playerAnimator;
    PlayerSKill playerSKill;
    Attacked attacked;
    Vector2 scale;
    private bool isJumpKey;
    public float velocityX;
    public bool isGround;
    public float jumpNum;
    public bool isWounded;
    public bool isDeath;
    public float localSpeed;
    Rigidbody2D rd;
    CapsuleCollider2D coll;
    Action PlayerEvent;
    public ButtonState attackState;

    private Vector2 gravity;

    //禁止状态
    private bool allowJump = true;
    private bool allowWalk = true;
    #region 初始化

    private void Awake()
    {
        gravity = Physics2D.gravity;
        // 初始化组件引用
        rd = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        attacked = GetComponent<Attacked>();
        playerAnimator = GetComponent<PlayerAnimator>();
        playerSKill = GetComponent<PlayerSKill>();
        animationEvent = transform.Find("Animation").GetComponent<AnimationEvents>();
        //注册移动事件
        EventManager.Instance.AddEvent(GameEventType.PlayerMoveEvent, new object[]
        {
            (Action<int>) ((direction) =>
            {
                velocityX = direction;
            })
        });
        //注册跳跃事件
        EventManager.Instance.AddEvent(GameEventType.PlayerJumpEvent, new object[]
        {
            (Action) (() =>
            {
                if (!isGround && jumpNum > 0)
                {
                    Debug.Log("跳跃Bug");
                }

                if (jumpNum > 0 && !isWounded && !isDeath && !isStairs && allowJump)
                {
                    isJumpKey = true;
                }
            })
        });
        //注册楼梯模式
        EventManager.Instance.AddEvent(GameEventType.StairsEvent, new object[]
        {
            (Action<bool>) ((isStairsModel) =>
            {
                this.isStairsModel = isStairsModel;
            })
        });
    }

    void Start()
    {
        playerLocalValueData = GameDataManager.Instance.GetPlayerLocalValueInfo();
        // 注册动画事件回调
        animationEvent.OnCustomEvent += AttackEvent;
        // 初始化玩家数据
        scale = transform.localScale;
        speed = playerLocalValueData.MoveSpeed;
        jumpHeight = playerLocalValueData.jumpSpeed;
        jumpNum = playerLocalValueData.JumpNum;
    }
    #endregion
    
    #region 更新循环
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpNum > 0 && !isWounded && !isDeath && !isStairs && allowJump)
        { 
            isJumpKey = true;
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
        velocityX = Input.GetAxisRaw("Horizontal");
       
        
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
            //判断上坡
            if ((leftFoot && !rightFoot) || (!leftFoot && rightFoot))
            {
                isUpStairs = false;
            }
            else
            {
                isUpStairs = true;
            }
            if (velocityX * slopeDirection.x > 0)
            {
                isStairs = true;
                rd.gravityScale = 0;
            }

            if (rd.velocity.y < 0)
            {
                isStairs = true;
                rd.velocity = new Vector2(rd.velocity.x, 0);
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
        if (isWounded || isDeath || !allowWalk)
        {
            return;
        }
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
            localSpeed = speed;
            if (attacked.GetAttackState())
            {
                localSpeed *= 0.3f;
            }

            playerAnimator.SetFloatValue("RunSpeed", localSpeed);
            // 平地移动
            rd.velocity = new Vector2(velocityX * localSpeed, rd.velocity.y);
        }
    }
    
    void JumpAction()
    {
        // 处理跳跃输入
        if (isJumpKey)
        {
            rd.velocity = new Vector2(rd.velocity.x, 0);
            rd.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            jumpNum -= 1;
            isJumpKey = false;
        }
        
        // 如果在斜坡上，不应用额外的重力效果
        if (isStairs)
        {
            return;
        }
        
        // 调整跳跃物理
        if (rd.velocity.y < 0) // 下落时
        {
            rd.velocity += Vector2.up * (Physics2D.gravity.y * 10f * Time.deltaTime);
        }
        else if (rd.velocity.y > 0) // 上升时
        {
            rd.velocity += Vector2.up * (Physics2D.gravity.y * 3f * Time.deltaTime);
        }
        
    }
    
    void DeathState(Vector2 attackerPosition)
    {
        isDeath = true;
        playerAnimator.PlayDeanth(attackerPosition);
    }

    public void Hit(float attackPower, Monster monster)
    {
        if (isDeath)
        {
            return;
        }
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

    public Rigidbody2D GetRigidbody()
    {
        return rd;
    }

    public void SetPlayerState(PlayerState state, bool allow)
    {
        switch (state)
        {
            case PlayerState.walk:
                allowWalk = allow;
                break;
            case PlayerState.jump:
                allowJump = allow;
                break;
        }
    }

    public void SetSkillState(bool isSkill)
    {
        this.isSkill = isSkill;
    }

    public void SetDodgeState(bool isDodge)
    {
        this.isDodge = isDodge;
    }
}

