using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    private int num;
    private float mapWidth;
    private float width;
    private GameObject mainCamera;
    private void Start()
    {
        mainCamera = Camera.main.gameObject;
        width = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        num = transform.parent.childCount;
        mapWidth = num * width;
    }

    private void Update()
    {
        if (mainCamera.transform.position.x > transform.position.x + mapWidth / 2)
        {
            transform.position = new Vector2( transform.position.x + mapWidth, transform.position.y);
        }
        else if (mainCamera.transform.position.x < transform.position.x - mapWidth / 2)
        {
            transform.position = new Vector2(transform.position.x - mapWidth, transform.position.y);
        }
    }
}
