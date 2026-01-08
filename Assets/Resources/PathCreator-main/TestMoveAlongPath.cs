using System;
using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TestMoveAlongPath : MonoBehaviour
{
    public VertexPath vertexPath;
    public float speed = 1f;
    public Transform playerPos;
    public Transform sunParent;
    [Header("灯光")]
    public Light2D mlight;
    [Header("缓动")]
    public AnimationCurve curve;
    float time = 0f;
    private Vector3 distance;

    private void Start()
    {
        distance = playerPos.position - sunParent.transform.position;
    }

    void Update()
    {
        sunParent.transform.position = new Vector3((playerPos.transform.position - distance).x, sunParent.transform.position.y,0);
        //跟随主角
        transform.position = vertexPath.MoveConstantVelocity(speed, advanceForward: true, ref time);
        if (transform.localPosition.y > 0)
        {
            mlight.intensity = curve.Evaluate(transform.localPosition.y);
        }
        else
        {
            mlight.intensity = 0.05f;
        }
    }

}
