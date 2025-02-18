using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

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

    public override void onExit()
    {      
        Destroy(gameObject);
    }

    void Start()
    {
       
    }
    public void StartScene(string sceneName, Func<Slider, IEnumerator> loadData)
    {
        StartCoroutine(LoadScene(sceneName, loadData));
    }
    public IEnumerator LoadScene(string sceneName,Func<Slider,IEnumerator> loadData)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);  // 激活当前的游戏对象
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            slider.value += operation.progress * 50;

            if (operation.progress >= 0.9f)
            {             
                slider.value = 50;           
                operation.allowSceneActivation = true;           
            }
            yield return null; 
        }
        yield return StartCoroutine(loadData.Invoke(slider));
        Debug.Log("关闭页面");
        UIManager.Instance.CloseLayer();
    }
}
