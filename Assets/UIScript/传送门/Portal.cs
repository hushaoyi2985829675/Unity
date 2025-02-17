using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject MapLayer;
    private bool isPlayer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer && Input.GetKeyDown(KeyCode.J))
        {
            UIManager.Instance.OpenLayer(MapLayer);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (MapLayer != null)
        {
            isPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (MapLayer != null)
        {
            isPlayer = false;
        }
    }
}
