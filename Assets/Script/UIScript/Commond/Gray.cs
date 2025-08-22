using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gray : MonoBehaviour
{
    Material originalMaterial;

    private void Awake()
    {
        originalMaterial = Resources.Load<Material>("Shader/Gray");
        Material material = new Material(originalMaterial);
        GetComponent<Image>().material = material;
    }
}