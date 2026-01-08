using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CommonScripts;
using UnityEngine;
using UnityEngine.UI;

public class Stairs : MonoBehaviour
{
    [SerializeField]
    private Button stairsButton;

    [SerializeField]
    private Image maskImage;

    [SerializeField]
    bool isStairs = false;

    private void Awake()
    {
        stairsButton.onClick.AddListener(() =>
        {
            isStairs = !isStairs;
            maskImage.SetActive(!isStairs);
            EventManager.Instance.PostEvent(GameEventType.StairsEvent, new object[] {isStairs});
        });
    }

    void Start()
    {
        maskImage.SetActive(!isStairs);   
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    
}
