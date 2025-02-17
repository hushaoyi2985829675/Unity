using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartScript : MonoBehaviour
{
    void Start()
    {
        //加载第一个页面
        UIManager.Instance.openLayer("MainLayer");
    }

    void Update()
    {
        
    }
}
