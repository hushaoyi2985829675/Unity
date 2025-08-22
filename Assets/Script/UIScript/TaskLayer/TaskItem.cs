using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour
{
    public Text text;
    public Button button;
    public Image image;
    public Material Material;

    public void InitData(int id, string title, Action<int> callback)
    {
        //image.material = new Material(Material);
        //image.material.SetFloat("_isGray", 0.5f);
        text.text = title;
        button.onClick.AddListener(() =>
        {
            // image.material.SetFloat("_isGray", 1f);
            // image.SetMaterialDirty();
            callback(id);
        });
    }
}