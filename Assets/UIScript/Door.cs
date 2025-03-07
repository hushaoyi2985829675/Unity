using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator animator;
    public GameObject Layer;
    private bool isPlayer;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && isPlayer)
        {
            animator.SetTrigger("Open");       
            OpenLayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayer = true;
        }
    }
    // private void OnTriggerStay2D(Collider2D collision)
    // {
    //     if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
    //     {
    //         
    //     } 
    // }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            animator.SetTrigger("Close");            
            isPlayer = false;
        }
    }
    void OpenLayer()
    {
        if (Layer == null)
        {
            Debug.Log("Layer为空");
            return;
        }
        GameObject.Find("LayerManager").GetComponent<LayerManager>().OpenLayer(Layer);
    }
}
