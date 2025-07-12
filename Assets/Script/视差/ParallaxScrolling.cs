using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    [Header("远层")]
    public GameObject farBackGround;
    [Header("中层")]
    public GameObject middleBackGround;
    [Header("近层")]
    public GameObject nearBackGround;
    private Vector3 lastPosition;
    private  GameObject mainCamera;
    private float scaleX = 0;
    void Start()
    {
        scaleX = transform.localScale.x;
        mainCamera = Camera.main.gameObject;
        lastPosition = mainCamera.transform.localPosition;
        transform.position = new Vector3(mainCamera.transform.position.x,transform.position.y,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera.transform.localPosition.x - lastPosition.x != 0)
        {
            var pos = new Vector2((mainCamera.transform.localPosition.x - lastPosition.x) / scaleX * 1,0);
            farBackGround.transform.localPosition += new Vector3(pos.x , 0, 0);
            middleBackGround.transform.localPosition += new Vector3(pos.x * 0.6f, 0, 0);
            nearBackGround.transform.localPosition += new Vector3(pos.x * 0.4f, 0, 0);
            lastPosition = mainCamera.transform.localPosition;
        }
    }
}
