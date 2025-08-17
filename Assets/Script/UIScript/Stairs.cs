using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    public float direction = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Input.GetKeyDown(KeyCode.W);
    }
}
