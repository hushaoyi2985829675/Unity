using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheck : MonoBehaviour
{
    public Monster monster;

    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            monster.SetPlayerTriggerZone(other.gameObject, true);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            monster.SetSelfTriggerZone(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            monster.SetPlayerTriggerZone(other.gameObject, false);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            monster.SetSelfTriggerZone(false);
        }
    }
}