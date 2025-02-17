using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgCon : MonoBehaviour
{
    public Camera cam;
    Vector3 startPos;
    public Transform oneLayer;
    public Transform twoLayer;
    public Transform threeLayer;
    public Transform fourLayer;
    public Transform fiveLayer;
    void Start()
    {
        startPos = cam.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = cam.transform.localPosition - startPos;
        if (oneLayer != null)
        {
            oneLayer.localPosition += pos;
        }
        if (twoLayer != null)
        {
            twoLayer.localPosition += new Vector3(pos.x*0.8f,pos.y * 0.7f,0);
        }
        if (threeLayer != null)
        {
            threeLayer.localPosition += new Vector3(pos.x * 0.6f, pos.y * 0.7f, 0);
        }
        if (fourLayer != null)
        {
            fourLayer.localPosition += new Vector3(pos.x * 0.4f, pos.y * 0.7f, 0);
        }
        if (fiveLayer != null)
        {
            fiveLayer.localPosition += new Vector3(pos.x * 0.2f, pos.y * 0.7f, 0);
        }
        startPos = cam.transform.localPosition;
    }
}
