using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MapNs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLayer : PanelBase
{
    public Slider slider;
    public enum LayerType 
    {
        MainLayer,
        FightLayer
    }
    public override void onEnter(object[] data)
    {
       
    }

    public override void onShow(object[] data)
    {
        slider.value = 0;
        StartScene((string) data[0], (int) data[1]);
    }
    public override void onExit()
    {
        slider.DOKill();
    }

    void Start()
    {
       
    }

    public void StartScene(string sceneName, int mapId)
    {
        StartCoroutine(LoadScene(sceneName, mapId));
    }

    public IEnumerator LoadScene(string sceneName, int mapId)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);  
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            slider.value = operation.progress * 50;

            if (operation.progress >= 0.9f)
            {             
                slider.value = 50;           
                operation.allowSceneActivation = true;           
            }
            yield return null; 
        }

        AddMap(mapId);
        slider.DOValue(100, 1.5f);
        UIManager.Instance.CloseLayer(gameObject);
    }

    public void AddMap(int mapId)
    {
        UIManager.Instance.ResetMapGrid();
        UIManager.Instance.AddMap(mapId);
    }
}
