using System;
using System.Collections;
using System.Collections.Generic;
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
    }
    public override void onExit()
    {      
        Destroy(gameObject);
    }

    void Start()
    {
       
    }

    public void StartScene(string sceneName, Action<Slider> callback)
    {
        StartCoroutine(LoadScene(sceneName, callback));
    }

    public IEnumerator LoadScene(string sceneName, Action<Slider> callback)
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

        callback.Invoke(slider);
        //yield return new WaitForSeconds(1f);
        UIManager.Instance.CloseLayer(gameObject);
    }
}
