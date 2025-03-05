using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Attacked : MonoBehaviour
{
    [Header("����")]
    public Transform edge;
    Player player;
    PlayerInfo PlayerValueData;
    PlayerLvData PlayerLvData;
    PlayerAnimator playerAnimator;
    float attackInterval;
    bool isNpc;
    bool isNpcOpen;
    void Start()
    {
        player = GetComponent<Player>();
        playerAnimator = GetComponent<PlayerAnimator>();
        PlayerValueData = player.PlayerValueData.PlayerInfo;
        PlayerLvData = player.PlayerLvData;
        attackInterval = 0;
        SceneManager.sceneLoaded += UpdateState;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && isNpc)
        {
            isNpcOpen = true;
        }
      
        switch (player.attackState)
        { 
            case ButtonState.Attack:
                Attack(); 
            break;
            case ButtonState.Talk:
                
            break;
        }
        attackInterval -= Time.deltaTime;
    }
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.J) && attackInterval <= 0 && !player.isWounded)
        {
            playerAnimator.PlayTrigger("Slash");
            attackInterval = PlayerValueData.AttackInterval;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("NPC"))
        {
            isNpc = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("NPC"))
        {
            if (isNpcOpen)
            {
                Talk(collision.gameObject);
                isNpcOpen = false;
            }   
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("NPC"))
        {
            isNpc = false;
        }        
    }
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("NPC"))
    //    {
    //        Talk();
    //    }
    //}
    public void Hit()
    {
        var hit = Tool.OverlapCircle(edge.position, 0.2f, LayerMask.GetMask("Monster"));
        if (hit)
        {
            var monster = hit.GetComponent<Monster>();
            bool isDeath = monster.Attacked(PlayerValueData.AttackPower);
            if (isDeath)
            {
                var exp = monster.GetExp();
                Debug.Log(exp);
                AddExp(exp);
            }
        }
    }

    void AddExp(float exp)
    {
        if (PlayerValueData.Lv == PlayerLvData.PlayerLvDatas.Count)
        {
            //满级
            return;
        }
        PlayerValueData.Exp += exp; 
        while (true)
        {
            var curLvInfo = PlayerLvData.PlayerLvDatas.Find(info => info.Lv == PlayerValueData.Lv);
            //升级
            if (PlayerValueData.Exp >= curLvInfo.Exp)
            {
                var n = curLvInfo.Lv;
                PlayerValueData.Exp -= curLvInfo.Exp;
                PlayerValueData.Lv++;
                PlayerValueData.AttackPower += curLvInfo.AttackPower;
                PlayerValueData.MaxHp += curLvInfo.Hp;
                PlayerValueData.Armor += curLvInfo.Armor;
            }
            else
            {
                //更新UI
                player.RefreshUI();
                break;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(edge.position, 0.2f);
    }
    void Talk(GameObject npc)
    {
         UIManager.Instance.OpenTalkLayer(npc);
    }

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
}
