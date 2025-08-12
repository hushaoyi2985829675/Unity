using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBase : MonoBehaviour
{
    public GameObject npcLayer;
    protected BoxCollider2D boxCollider;
    public bool isPlayer = false;
    public GameObject GetNpcLayer()
    {
        return npcLayer;
    }
}
