using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ReturnBtn : MonoBehaviour
{
    private Button btn;
    [SerializeField] private GameObject parent;
    [SerializeField] private bool isLayer = true; 
    void Start()
    {
        btn = GetComponent<Button>();
        if (btn == null)
        {
            btn = gameObject.AddComponent<Button>();
        }

        if (isLayer)
        {
            btn.onClick.AddListener(() => { UIManager.Instance.CloseLayer(parent); });
        }
        else
        {
            btn.onClick.AddListener(() => { UIManager.Instance.CloseUINode(parent); });
        }
    }
}
