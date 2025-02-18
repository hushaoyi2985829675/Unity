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
    void Start()
    {
        mainCamera = Camera.main.gameObject;
        lastPosition = mainCamera.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera.transform.localPosition.x - lastPosition.x != 0)
        {
            var pos = new Vector2(mainCamera.transform.localPosition.x - lastPosition.x,0);
            farBackGround.transform.localPosition += new Vector3(pos.x , 0, 0);
            middleBackGround.transform.localPosition += new Vector3(pos.x * 0.7f, 0, 0);
            nearBackGround.transform.localPosition += new Vector3(pos.x * 0.5f, 0, 0);
            lastPosition = mainCamera.transform.localPosition;
        }
    }
}
