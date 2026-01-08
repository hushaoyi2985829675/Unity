using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum CircleLayoutOption
{
    Clockwise, // 顺时针排列
    CounterClockwise, // 逆时针排列
    HalfWise, // 半圆布局
    FullCircle // 整圆布局
}

[ExecuteAlways]
public class CircleLayout : MonoBehaviour
{
    [Header("半径")]
    [SerializeField]
    private float radius;

    [Header("布局方式")]
    [SerializeField]
    private CircleLayoutOption option;

    [Header("间隔")]
    [SerializeField]
    private float spacing;

    [Header("偏移度数")]
    [SerializeField]
    private float offSetRadius;

    void Start()
    {
    }

    void Update()
    {
    }

    void OnTransformChildrenChanged()
    {
        StartCoroutine(CheckPositionAfterFrame());
    }

    IEnumerator CheckPositionAfterFrame()
    {
        yield return null;
        RefreshUI();
    }

    private void OnValidate()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        float offSetRadian = offSetRadius * Mathf.Deg2Rad;
        float angle = spacing;
        int num = transform.childCount;
        if (option == CircleLayoutOption.FullCircle && num > 0)
        {
            angle = 360 / num;
        }

        int direction = 1;
        if (option == CircleLayoutOption.Clockwise)
        {
            direction = -1;
        }

        float radian = angle * Mathf.Deg2Rad;
        if (option == CircleLayoutOption.HalfWise)
        {
            float startRadian = -(num - 1) * radian / 2;
            for (int i = 0; i < num; i++)
            {
                Transform child = transform.GetChild(i);
                float x = math.cos(startRadian + i * radian + offSetRadian) * direction * radius;
                float y = math.sin(startRadian + i * radian + offSetRadian) * direction * radius;
                child.localPosition = new Vector3(x, y, 0);
            }
        }
        else
        {
            for (int i = 0; i < num; i++)
            {
                Transform child = transform.GetChild(i);
                float x = math.cos((i * radian + offSetRadian) * direction) * radius;
                float y = math.sin((i * radian + offSetRadian) * direction) * radius;
                child.localPosition = new Vector3(x, y, 0);
            }
        }
    }
}