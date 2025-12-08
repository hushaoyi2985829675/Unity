using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public Player Player;
    private Button button;
    public Wolf wolf;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            Player.Hit(20, wolf);
        });
    }

    // Update is called once per frame
    void Update()
    {
    }
}