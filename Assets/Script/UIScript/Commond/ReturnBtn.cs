using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnBtn : MonoBehaviour
{
    private Button btn;
    public GameObject parent;
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => {
            UIManager.Instance.CloseLayer(parent.name);
        });
    }
}
