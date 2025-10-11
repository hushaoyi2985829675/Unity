using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private RectTransform progressImg;
    private Vector2 size;
    private int maxValue;

    public void Awake()
    {
        size = GetComponent<RectTransform>().rect.size;
    }

    public void SetMaxValue(int value)
    {
        maxValue = value;
    }

    public void SetValue(float value)
    {
        progressImg.sizeDelta = new Vector2((value / maxValue) * size.x - size.x, 0);
    }
}