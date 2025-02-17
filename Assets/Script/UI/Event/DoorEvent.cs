using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEvent : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onClose()
    {
        animator.SetBool("isOpen", false);
    }
    public void onOpen()
    {
        animator.SetBool("isOpen",true);
    }
}
