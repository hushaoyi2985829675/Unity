using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.FantasyHeroes.TestRoom.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using MonsterNs;
using UnityEditor.UI;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [Header("是否是展示")]
    [SerializeField]
    private bool isShow;
    //数值
    public int monsterId = 0;

    //血量
    private float hp;
    private MonsterInfo monsterInfo;

    //检测
    private bool playerTriggerZone;
    private bool selfTriggerZone;
    private GameObject triggerTarget;
    private float direction;
    private bool leftFoot;
    private bool rightFoot;
    private bool leftWall;
    private bool rightWall;
    private BoxCollider2D coll;
    private Rigidbody2D rd;
    protected Animator animator;
    private MonsterEquipManager monsterEquipManager;
    private AnimationEvents animationEvents;
    MonsterUI monsterUI;
    Dictionary<State, StateBase> stateList = new Dictionary<State, StateBase>();
    StateBase curState;
    State stateId;

    [Header("攻击距离")]
    [SerializeField]
    private Transform edge;

    [Header("特效节点")]
    [SerializeField]
    private Transform effTrans;

    [Header("血条")]
    [SerializeField]
    private MonsterHpBar monsterHpBar;

    public void Awake()
    {
        direction = transform.localScale.x;
        rd = GetComponent<Rigidbody2D>();
        animationEvents = transform.Find("Animation")?.GetComponent<AnimationEvents>();

        coll = GetComponent<BoxCollider2D>();
        animator = transform.Find("Animation")?.GetComponent<Animator>();
    }

    public void Start()
    {
        InitMonster(monsterId);
        InitState(out stateList);
        ChangeState(State.Idle);
        if (monsterHpBar)
        {
            monsterHpBar.InitHp(monsterId);
        }
        // Tool.AddPlayerEvent(() =>
        // {
        //     ChangeState(State.Victory);
        // });
    }

    abstract public void InitState(out Dictionary<State, StateBase> stateList);


    public void InitMonster(int id)
    {
        if (id == 0)
        {
            return;
        }

        monsterId = id;
        monsterInfo = Ui.Instance.GetMonsterValue(id);
        hp = monsterInfo.maxHp;
    }

    public void RefreshDirection()
    {
        transform.localScale = new Vector2(direction, 1);
    }

    public void SetDirection(float dic)
    {
        direction = dic;
        transform.localScale = new Vector2(direction, 1);
    }

    public MonsterInfo GetMonsterInfo()
    {
        return monsterInfo;
    }

    public AnimationEvents GetAnimationEvents()
    {
        return animationEvents;
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public float GetMonsterTargetDic()
    {
        float dir = transform.position.x - triggerTarget.gameObject.transform.position.x;
        return dir;
    }

    public bool GetPlayerTriggerZone()
    {
        return playerTriggerZone;
    }

    public float GetDirection()
    {
        return direction;
    }

    public Transform GetEdgeTrans()
    {
        return edge;
    }

    public void SetPlayerTriggerZone(GameObject triggerObj, bool trigger)
    {
        triggerTarget = triggerObj;
        playerTriggerZone = trigger;
    }

    public void SetSelfTriggerZone(bool trigger)
    {
        selfTriggerZone = trigger;
    }

    private void OnEnable()
    {
        //monsterInfo.Hp = monsterValue.MaxHp;
    }

    public void Update()
    {
        if (curState != null)
        {
            curState.onUpdate();
        }
        
        leftFoot = Tool.Raycast(new Vector2(-coll.size.x / 2 + coll.offset.x, -coll.size.y / 2 + coll.offset.y), Vector2.down, 0.15f, LayerMask.GetMask("Ground"), transform.position);
        rightFoot = Tool.Raycast(new Vector2(coll.size.x / 2 + coll.offset.x, -coll.size.y / 2 + coll.offset.y), Vector2.down, 0.15f, LayerMask.GetMask("Ground"), transform.position);
        leftWall = Tool.Raycast(new Vector2(coll.size.x / 2 + coll.offset.x, -coll.size.y / 2 + coll.offset.y + 0.5f), Vector2.right, 0.3f, LayerMask.GetMask("Ground"), transform.position);
        rightWall = Tool.Raycast(new Vector2(-coll.size.x / 2 + coll.offset.x, -coll.size.y / 2 + coll.offset.y + 0.5f), Vector2.left, 0.3f, LayerMask.GetMask("Ground"), transform.position);
    }

    public void FixedUpdate()
    {
        curState?.onFixedUpdate();
    }

    public void ChangeState(State id)
    {
        curState?.onExit();
        if (stateList.ContainsKey(id))
        {
            curState = stateList[id];
            curState?.onEnter();
            stateId = id;
        }
    }

    public State getCurState()
    {
        return stateId;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(edge.position, 0.2f);
    }

    public void SetVelocityX(float x)
    {
        rd.velocity = new Vector2(x, rd.velocity.y);
    }

    public void SetVelocityY(float y)
    {
        rd.velocity = new Vector2(rd.velocity.x, y);
    }

    public bool IsNeedTurn()
    {
        if (direction == 1)
        {
            if (rightWall || !rightFoot)
            {
                direction = -1;
            }
        }
        else
        {
            if (leftWall || !leftFoot)
            {
                direction = 1;
            }
        }

        return leftWall || rightWall || !leftFoot || !rightFoot;
    }

    //受伤
    public bool Hit(float attackPower)
    {
        if (isShow)
        {
            HitShow();
            return false;
        }
        ChangeState(State.Hit);
        var harm = Math.Max(0, attackPower - monsterInfo.armor);
        hp = Math.Max(0, hp - harm);
        monsterHpBar?.RefreshHp(hp);
        if (hp == 0)
        {
            ChangeState(State.Deanth);
            return true;
        }
        else
        {
            ChangeState(State.Hit);
        }

        return false;
    }

    void HitShow()
    {
        animator.SetInteger("State", (int) State.Hit);
    }

    public void CreateDeathEff()
    {
        Instantiate(Resources.Load("Effect/怪物死亡/Death Particles"), effTrans.position, effTrans.rotation);
        TimerManage.Instance.AddDelayCallback(() =>
        {
            monsterHpBar.RemoveAllDoTween();
            Destroy(transform.parent.gameObject);
        }, 0.3f);
    }

    public Vector2 GetHitPos(Transform edge)
    {
        Vector2 pos = new Vector2();
        float maxX = transform.TransformPoint(new Vector2(coll.size.x / 2, 0)).x;
        float minX = transform.TransformPoint(new Vector2(-coll.size.x / 2, 0)).x;
        pos.x = Mathf.Clamp(edge.position.x, minX, maxX);
        float maxY = transform.TransformPoint(new Vector2(0, coll.size.y)).y;
        float minY = transform.position.y;
        pos.y = Mathf.Clamp(edge.position.y, minY, maxY);
        return pos;
    }

    public float GetExp()
    {
        return monsterInfo.exp;
    }
}