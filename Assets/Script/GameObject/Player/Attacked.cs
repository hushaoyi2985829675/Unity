using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Attacked : MonoBehaviour
{
    [Header("攻击距离")]
    [SerializeField]
    private Transform edge;

    Player player;
    PlayerLocalValueData PlayerLocalValueData;
    PlayerAnimator playerAnimator;
    PlayerSKill playerSKill;
    GameObject npcObj;
    bool npcObjOpen;

    [SerializeField]
    private bool isAttack;

    AnimationEvents animationEvent;
    private int attackState = 1;
    private float attackTime = 0.15f;
    private float attackChangeTime;
    private int eventId;
    [SerializeField]
    private bool isCombo; //连击

    void Start()
    {
        playerSKill = GetComponent<PlayerSKill>();
        edge = Ui.Instance.GetChild(transform, "Edge").transform;
        player = GetComponent<Player>();
        playerAnimator = GetComponent<PlayerAnimator>();
        PlayerLocalValueData = GameDataManager.Instance.GetPlayerLocalValueInfo();
        // PlayerLvData = player.PlayerLvData;
        animationEvent = transform.Find("Animation").GetComponent<AnimationEvents>();
        animationEvent.OnCustomEvent += AttackEvent;
        SceneManager.sceneLoaded += UpdateState;
        //InitAttackState();
        //注册攻击事件
        eventId = EventManager.Instance.AddEvent(GameEventType.PlayerAttackEvent, new object[]
        {
            (Action) (() =>
            {
                AttackInput();
            })
        });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            AttackInput();
        }
        if (isCombo)
        {
            if (attackChangeTime >= 0)
            {
                attackChangeTime -= Time.deltaTime;
            }
            else
            {
                isCombo = false;
            }
        }
    }

    public bool GetAttackState()
    {
        return isAttack;
    }

    void AttackInput()
    {
        if (isAttack || player.isWounded || player.isDeath)
        {
            return;
        }

        if (isCombo)
        {
            attackState += 1;
            attackState = attackState > 3 ? 1 : attackState;
        }
        else
        {
            attackState = 1;
        }

        if (attackState == 1)
        {
            playerAnimator.SetIntValue("WeaponType", 0);
            playerAnimator.PlayTrigger("Slash");
        }
        else if (attackState == 2)
        {
            playerAnimator.SetIntValue("WeaponType", 0);
            playerAnimator.PlayTrigger("Jab");
        }
        else if (attackState == 3)
        {
            playerAnimator.SetIntValue("WeaponType", 1);
            playerAnimator.PlayTrigger("Slash");
            attackChangeTime = attackTime;
            isCombo = false;
        }

        isAttack = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("NPC"))
        {
            npcObj = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("NPC"))
        {
            npcObj = null;
        }        
    }
    public void Attack()
    {
        float distance = Mathf.Abs(edge.position.x - player.GetPlayerBoxPosX());
        float center = distance / 2;
        Collider2D[] hits = Tool.OverlapBox(new Vector3(edge.position.x - center * player.GetDirection(), edge.position.y), new Vector2(distance, 2.5f), LayerMask.GetMask("Monster"));
        if (hits.Length > 0)
        {
            bool isCritRate = Ui.Instance.IsCriticalAttack(PlayerLocalValueData.CritRate);
            float power = PlayerLocalValueData.AttackPower * (1 + attackState / 10f);
            for (int i = 0; i < hits.Length; i++)
            {
                Collider2D hit = hits[i];
                Monster monster = hit.GetComponent<Monster>();
                PlayEff(monster);
                //暴击
                if (isCritRate)
                {
                    power *= PlayerLocalValueData.CritDamage;
                }
                // bool isDeath = monster.Hit(power);
                // if (isDeath)
                // {
                //     var exp = monster.GetExp();
                //     //AddExp(exp);
                // }
            }

            if (isCritRate)
            {
                CameraManager.Instance.ShakeCamera(0.25f, 0.5f);
                AudioManager.Instance.PlayAudio(gameObject, AudioType.Attack, "CriticalAttack");
            }
            else
            {
                AudioManager.Instance.PlayAudio(gameObject, AudioType.Attack, "BasicAttack");
            }
        }
        else
        {
            AudioManager.Instance.PlayAudio(gameObject, AudioType.Attack, "NullAttack");
        }
    }

    void AttackEvent(string eventName)
    {
        switch (eventName)
        {
            case "Hit":
                Attack();
                break;
            case "HitEnd":
                isAttack = false;
                attackChangeTime = attackTime;
                isCombo = true;
                break;
        }
    }
    // void AddExp(float exp)
    // {
    //     if (PlayerLocalValueData.Lv == PlayerLvData.PlayerLvDatas.Count)
    //     {
    //         //满级
    //         return;
    //     }
    //     PlayerLocalValueData.Exp += exp; 
    //     while (true)
    //     {
    //         var curLvInfo = PlayerLvData.PlayerLvDatas.Find(info => info.Lv == PlayerLocalValueData.Lv);
    //         //升级
    //         if (PlayerLocalValueData.Exp >= curLvInfo.Exp)
    //         {
    //             var n = curLvInfo.Lv;
    //             PlayerLocalValueData.Exp -= curLvInfo.Exp;
    //             PlayerLocalValueData.Lv++;
    //             PlayerLocalValueData.AttackPower += curLvInfo.AttackPower;
    //             PlayerLocalValueData.MaxHp += curLvInfo.Hp;
    //             PlayerLocalValueData.Armor += curLvInfo.Armor;
    //         }
    //         else
    //         {
    //             //更新UI
    //             player.RefreshUI();
    //             break;
    //         }
    //     }
    // }

    public bool GetPlayerAttack()
    {
        return isAttack;
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        Gizmos.color = Color.red;
        float distance = Mathf.Abs(edge.position.x - player.GetPlayerBoxPosX());
        float center = distance / 2;
        Gizmos.DrawWireCube(new Vector3(edge.position.x - center * player.GetDirection(), edge.position.y), new Vector2(2.5f, 2.5f));
    }
    void Talk(GameObject npc)
    {
        // UIManager.Instance.OpenTalkLayer(npc);
    }

    // void InitAttackState()
    // {
    //     string sceneName = SceneManager.GetActiveScene().name;
    //     if (sceneName == "MainScene")
    //     {
    //         player.attackState = ButtonState.Talk;
    //     }
    //     else if (sceneName == "FightScene")
    //     {
    //         player.attackState = ButtonState.Attack;
    //     }
    // }

    void UpdateState(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
        {
            player.attackState = ButtonState.Talk;
        }
        else if (scene.name == "FightScene")
        {
            player.attackState = ButtonState.Attack;
        }
    }

    void PlayEff(Monster monster)
    {
        Vector2 pos = monster.GetHitPos(edge);
        EffectManager.Instance.PlayEff(EffectType.CritRate, pos);
    }

    private void OnDestroy()
    {
        EventManager.Instance?.RemoveEvent(GameEventType.WearEquipEvent, eventId);
    }
}
