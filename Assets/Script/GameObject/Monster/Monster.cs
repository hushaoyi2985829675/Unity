using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.FantasyHeroes.TestRoom.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;

public enum State
{
    Idle = 0,
    Walk,
    Run,
    Attack,
    Patrol,
    ReadyFight,
    Wounded,
    Deanth,
    Victory,
}
public class Monster : MonoBehaviour
{
    public MonsterValue monsterValue;
    public float walkSpeed;
    public float runSpeed;
    public float direction;
    public float awaitTime;
    public bool talking;
    public float attackPower;
    public float attackInterval;
    [Header("��������")]
    public float attackDistance;
    public Collider2D detectPlayer;
    public RaycastHit2D leftFoot;
    public RaycastHit2D rightFoot;
    public RaycastHit2D leftWall;
    public RaycastHit2D rightWall;
    public BoxCollider2D coll;
    public Rigidbody2D rd;
    public Animator animator;
    private MonsterEquipManager monsterEquipManager;
    public AnimationEvents animationEvent;
    MonsterUI monsterUI;
    Dictionary<State, StateBase> stateList = new Dictionary<State, StateBase>();
    StateBase curState;
    State stateId;
    [Header("����λ��")]
    public Transform edge;
    public void Start()
    {
        walkSpeed = monsterValue.walkSpeed;
        runSpeed = monsterValue.runSpeed;
        awaitTime = monsterValue.awaitTime;
        attackInterval = monsterValue.AttackInterval;
        attackPower = monsterValue.AttackPower;
        attackDistance = monsterValue.AttackDitance;
        monsterUI = GetComponent<MonsterUI>();
        direction = transform.localScale.x;
        rd = GetComponent<Rigidbody2D>();
        animationEvent = transform.Find("Animation").GetComponent<AnimationEvents>();
        animationEvent.OnCustomEvent += AttackEvent;
        animator = transform.Find("Animation").GetComponent<Animator>();
        monsterEquipManager = GetComponent<MonsterEquipManager>();
        coll = GetComponent<BoxCollider2D>();
        stateList.Add(State.Idle, new IdleState(this));
        stateList.Add(State.Patrol, new PatrolState(this));
        stateList.Add(State.ReadyFight, new ReadyFightState(this));
        stateList.Add(State.Run, new RunState(this));
        stateList.Add(State.Attack, new AttackState(this));
        stateList.Add(State.Wounded, new WoundedState(this));
        stateList.Add(State.Deanth, new DeathState(this));
        stateList.Add(State.Victory, new VictoryState(this));
        Tool.AddPlayerEvent(()=>{ ChangeState(State.Victory); });
        ChangeState(State.Idle);
    }

    private void OnEnable()
    {
        monsterValue.Hp = monsterValue.MaxHp;
    }

    public void Update()
    {
        curState.onUpdate();
        detectPlayer = Tool.BoxCast(new Vector2(0 + coll.offset.x, 0 + coll.offset.y), new Vector2(7f, coll.size.y), LayerMask.GetMask("Player"), transform.localPosition);
        leftFoot = Tool.Raycast(new Vector2(-coll.size.x / 2 + coll.offset.x, -coll.size.y / 2 + coll.offset.y), Vector2.down, 0.15f, LayerMask.GetMask("Ground"), transform.localPosition);
        rightFoot = Tool.Raycast(new Vector2(coll.size.x / 2 + coll.offset.x, -coll.size.y / 2 + coll.offset.y), Vector2.down, 0.15f, LayerMask.GetMask("Ground"), transform.localPosition);
        leftWall = Tool.Raycast(new Vector2(coll.size.x / 2 + coll.offset.x, -coll.size.y / 2 + coll.offset.y + 0.5f), Vector2.right, 0.3f, LayerMask.GetMask("Ground"), transform.localPosition);
        rightWall = Tool.Raycast(new Vector2(-coll.size.x / 2 + coll.offset.x, -coll.size.y / 2 + coll.offset.y + 0.5f), Vector2.left, 0.3f, LayerMask.GetMask("Ground"), transform.localPosition);
    }

    public void FixedUpdate()
    {
        curState.onFixedUpdate();
    }
    public void ChangeState(State id)
    {
        curState?.onExit();
        curState = stateList[id];
        curState.onEnter();
        stateId = id;
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
    public bool Attacked(float attackPower)
    {
        var harm = Math.Max(0, attackPower - monsterValue.Armor);
        monsterValue.Hp = Math.Max(0, monsterValue.Hp - harm);
        monsterUI.setHp(monsterValue.Hp);
        if (monsterValue.Hp == 0)
        {           
            ChangeState(State.Deanth);
            return true;
        }
        else
        {
            ChangeState(State.Wounded);
        }   
        return false;
    }
    void AttackEvent(string name)
    {
        switch (name)
        {
            case "Hit":
                var hit = Tool.OverlapCircle(edge.position, 0.2f, LayerMask.GetMask("Player"));
                if (hit)
                {
                    hit.gameObject.GetComponent<Player>().Attacked(monsterValue.AttackPower,transform.position);
                }
                break;
        }
    }
    public void CreateDeathEff()
    {
        StartCoroutine(MonsterDeathEff());
    }
    public IEnumerator MonsterDeathEff()
    {
        Instantiate(Resources.Load("Effect/Death Particles"), transform.position, transform.rotation);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        monsterUI.delectHpBar();
        //掉落物品
        monsterEquipManager.EquipDrop();
    }

    public float GetExp()
    {
        return monsterValue.Exp;
    }
}




