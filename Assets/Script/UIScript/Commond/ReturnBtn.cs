using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnBtn : MonoBehaviour
{
    private Button btn;
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => {
            UIManager.Instance.CloseLayer(gameObject.name);
        });
    }
}
