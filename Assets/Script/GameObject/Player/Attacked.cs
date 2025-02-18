using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Attacked : MonoBehaviour
{
    [Header("����")]
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

    void UpdateState(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
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
