using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ui : MonoBehaviour
{
    private static Ui _instance;
    public static Ui Instance
    {
        get
        {
            if ( _instance == null)
            {
                _instance = FindObjectOfType<Ui>();
            }          
            return _instance;
        }
    }
    private GameObject flutteViewRef;
    void Start()
    {
        flutteViewRef = Resources.Load<GameObject>("Ref/LayerRef/UIRef/FlutterWindowsLayer");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowFlutterView(string text)
    {
        UIManager.Instance.AddUINode(flutteViewRef, new Vector2(0, 0), new object[] {text});
    }
}
