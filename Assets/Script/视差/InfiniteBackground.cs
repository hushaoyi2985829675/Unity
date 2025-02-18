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
        Debug.Log(mapWidth);
    }

    private void Update()
    {
        if (mainCamera.transform.localPosition.x > transform.localPosition.x + mapWidth / 2)
        {
            transform.localPosition = new Vector2( transform.localPosition.x + mapWidth / 2, transform.localPosition.y);
        }
        else if (mainCamera.transform.localPosition.x < transform.localPosition.x -mapWidth / 2)
        {
            transform.localPosition = new Vector2(transform.localPosition.x - mapWidth / 2, transform.position.y);
        }
    }
}
