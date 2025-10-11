using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gray : MonoBehaviour
{
    Material originalMaterial;
    private Material material;
    private Image image;
    private Button button;

    private void Awake()
    {
        originalMaterial = Resources.Load<Material>("Shader/Gray");
        material = new Material(originalMaterial);
        image = GetComponent<Image>();
        image.material = material;
        button = GetComponent<Button>();
        ColorBlock colors = button.colors;
        colors.disabledColor = Color.white;
        button.colors = colors;
    }

    public void SetGray(bool isGray)
    {
        material.SetFloat("_Grayscale", isGray ? 1f : 0f);
        image.enabled = false;
        image.enabled = true;
        button.interactable = !isGray;
    }
}