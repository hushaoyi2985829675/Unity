using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Attacked : MonoBehaviour
{
    [Header("¹¥»÷")]
    public Transform edge;
    Player player;
    PlayerInfo PlayerValueData;
    PlayerAnimator playerAnimator;
    float attackInterval;
    bool isNpc;
    bool isNpcOpen;
    void Start()
    {
        player = GetComponent<Player>();
        playerAnimator = GetComponent<PlayerAnimator>();
        PlayerValueData = player.PlayerValueData.PlayerInfo;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && isNpc)
        {
            isNpcOpen = true;
        }
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            player.attackState = ButtonState.Talk;
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
        if (Input.GetKeyDown(KeyCode.J) && PlayerValueData.AttackInterval <= 0 && !player.isWounded)
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
            hit.gameObject.GetComponent<Monster>().Attacked(PlayerValueData.AttackPower);
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
}
