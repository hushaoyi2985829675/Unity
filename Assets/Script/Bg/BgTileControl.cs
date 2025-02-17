using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgTileControl : MonoBehaviour
{
    public int bgTileNum = 4;
    float tileWidth;
    float mapWidth;
    GameObject cam;
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        tileWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        mapWidth = tileWidth * bgTileNum;
    }

    // Update is called once per frame
    void Update()
    {
        if (cam.transform.position.x > transform.position.x + mapWidth / 2)
        {
            transform.localPosition += new Vector3(mapWidth, 0);
        }
        else if (cam.transform.position.x < transform.position.x - mapWidth / 2)
        {
            transform.localPosition -= new Vector3(mapWidth, 0);
        }
    }
}
