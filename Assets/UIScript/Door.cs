using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator animator;
    public GameObject Layer;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            animator.SetTrigger("Open");          
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                OpenLayer();
            }
        } 
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            animator.SetTrigger("Close");
        }
    }
    void OpenLayer()
    {
        if (Layer == null)
        {
            Debug.Log("当前门打开的Layer为空");
            return;
        }
        GameObject.Find("LayerManager").GetComponent<LayerManager>().OpenLayer(Layer);
    }
}
