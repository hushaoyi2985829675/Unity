using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.FantasyHeroes.TestRoom.Scripts;
using HeroEditor.Common.Enums;
using HeroEditor.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.WSA;

public enum ButtonState
{
    Attack,
    Talk,
}
public partial class Player : MonoBehaviour
{
    [Header("玩家数值")]
    public PlayerValueData PlayerValueData;
    [Header("玩家等级加成")]
    public PlayerLvData PlayerLvData;
    [Header("移动参数")]
    public float speed;
    public float jumpHeight;
    [Header("状态相关")]
    Rigidbody2D rd;
    BoxCollider2D coll;
    RaycastHit2D leftFoot;
    RaycastHit2D rightFoot;
    PlayerUI playerUI;
    AnimationEvents animationEvent;
    PlayerAnimator playerAnimator;
    Attacked attacked;
    Vector2 scale;
    public bool isJump;
    public bool jumpKey;
    public bool wall;
    public float velocityX;
    public float velocityY;
    public bool isGround;
    public float jumpNum;
    //受伤
    public bool isWounded;
    public bool isDeath;
    Action PlayerEvent;
    public ButtonState attackState;
    void Start()
    {
        //初始化UI
        playerUI = GetComponent<PlayerUI>();
        playerUI.InitUI();
        rd = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        attacked = GetComponent<Attacked>();
        scale = transform.localScale;
        speed = PlayerValueData.PlayerInfo.walkSpeed;
        jumpHeight = PlayerValueData.PlayerInfo.jumpSpeed;
        jumpNum = PlayerValueData.PlayerInfo.JumpNum;
        animationEvent = transform.Find("Animation").GetComponent<AnimationEvents>();
        playerAnimator = GetComponent<PlayerAnimator>();
        animationEvent.OnCustomEvent += AttackEvent;
        GameObjectManager.instance.SetPlayer(this);
        UpdateEquip();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpNum > 0 && !isWounded && !isDeath)
        { 
            isJump = true;
        }
    }

    void FixedUpdate()
    {
        PhysicsCheck();
        MoveAction();
        JumpAction();
    }
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
        leftFoot = Tool.Raycast(new Vector2(-coll.size.x /2 * transform.localScale.x, -coll.size.y / 2),Vector2.down,0.15f, LayerMask.GetMask("Ground"),transform.localPosition);
        rightFoot = Tool.Raycast(new Vector2(coll.size.x / 2 * transform.localScale.x, -coll.size.y / 2), Vector2.down, 0.15f, LayerMask.GetMask("Ground"),transform.localPosition);
        if (leftFoot || rightFoot)
        {
            isGround = true;
            jumpNum = PlayerValueData.PlayerInfo.JumpNum;
        }
        else
        { 
            isGround = false;
        }
        if (velocityX > 0)
        {
            transform.localScale = new Vector2(scale.x, transform.localScale.y);
        }
        if (velocityX < 0)
        {
            transform.localScale = new Vector2(-scale.x, transform.localScale.y);
        }
    }
    void MoveAction()
    {
        rd.velocity = new Vector2(velocityX * speed, rd.velocity.y);
    }
    void JumpAction()
    {
        if (isJump && jumpNum > 0)
        {
            rd.velocity = new Vector2(rd.velocity.x,0);
            rd.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            jumpNum -=  1;
            isJump = false;
        }

        if (rd.velocity.y < 0) //下落速度
        {
            rd.velocity += Vector2.up * Physics2D.gravity.y * (6f) * Time.deltaTime;
        }
        else if (rd.velocity.y > 0) //不按跳跃时减缓跳跃（如马里奥）
        {
            rd.velocity += Vector2.up * Physics2D.gravity.y * (1.5f) * Time.deltaTime;
        }
        velocityY = rd.velocity.y;
    }
    void DeathState(Vector2 pos)
    {
        isDeath = true;
        playerAnimator.PlayDeanth(pos);
        
    }
    void AttackEvent(string name)
    {
        switch (name)
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
    public void Attacked(float attackPower,Vector2 pos)
    { 
        var harm = Math.Max(0, attackPower - PlayerValueData.PlayerInfo.Armor);
        PlayerValueData.PlayerInfo.Hp = Math.Max(0, PlayerValueData.PlayerInfo.Hp - harm);
        playerUI.setHp(PlayerValueData.PlayerInfo.Hp);
        if (PlayerValueData.PlayerInfo.Hp == 0)
        {
            DeathState(pos);
        }
        else
        {
            HitState();
        }
        
    }
    public void HitState()
    {
        isWounded = true;
        playerAnimator.PlayTrigger("Hit");
    }

    public void RefreshUI()
    {
        playerUI.InitUI();
    }

    public void setPlayerPos(Vector2 pos)
    {
        transform.localPosition = pos;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Equip"))
        {
            Debug.Log("Player");
        }
    }
    
    private void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Equip"))
        {
            Debug.Log("Playerss");
        }
    }
}
