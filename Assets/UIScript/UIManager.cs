using Assets.HeroEditor.Common.CommonScripts;
using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum LayerAction
{
    Scale,
    BlackScreen,
    AllAction,
}

public class UIManager 
{
   private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if ( _instance == null)
            {
                _instance = new UIManager();
            }          
            return _instance;
        }
    }
    private Dictionary<GameObject,PanelBase> LayerList;
    private Stack<PanelBase> LayerStack;
    private PanelBase CurLayer;
    GameObject NpcCamera;
    GameObject PlayerCamera;
    CinemachineBlenderSettings CinemachineBlenderSettings;
    UIManager()
    { 
        LayerList = new Dictionary<GameObject, PanelBase>();
        CinemachineBlenderSettings = AssetDatabase.LoadAssetAtPath<CinemachineBlenderSettings>("Assets/CameraBlends/CameraBlends.asset");
        LayerStack = new Stack<PanelBase>();
        NpcCamera = GameObject.FindWithTag("NpcCamera");
        PlayerCamera = GameObject.FindWithTag("PlayerCamera");
    }
    public GameObject openLayer(string name)
    {
        var layerRef = Resources.Load("Panle/" + name) as GameObject;
        Debug.Log(layerRef);
        if (layerRef != null)
        {
            var layer = GameObject.Instantiate(layerRef, GameObject.Find("LayerCanvas").transform);
            PanelBase layerScript = layer.GetComponent<PanelBase>();
            //if (layerStack == null)
            //{
            //    layerStack = new Stack<PanelBase>();
            //}
            layerScript.onEnter();
            layerScript.StartCoroutine(CallOnEnter(layerScript));
            LayerList[layerRef] = layerScript;
        }
        else
        {
            Debug.Log("不存在" + name);
        }
        return layerRef;
    }
    public PanelBase OpenLayer(GameObject layerRef, params object[] data)
    {
        if (LayerList.ContainsKey(layerRef))
        {
            LayerList[layerRef].SetActive(true);
            LayerList[layerRef].onEnter(data);
            LayerStack.Push(LayerList[layerRef]);
            CurLayer = LayerList[layerRef];
            return LayerList[layerRef];
        }
        else
        {
            if (layerRef == null)
            {
                Debug.Log("加载的layer为空");
                return null;
            }
            var LayerCanvas = GameObject.FindWithTag("LayerCanvas");
            if (LayerCanvas == null)
            {
                Debug.Log("场景中没有Tag为LayerCanvas的物体");
                return null;
            }
            var layer = GameObject.Instantiate(layerRef, LayerCanvas.transform);
            PanelBase layerScript = layer.GetComponent<PanelBase>();
            layerScript.transform.localPosition = new Vector3(0, 0, 0);
            layerScript.onEnter(data);
            LayerList[layerRef] = layerScript;
            LayerStack.Push(layerScript);
            CurLayer = layerScript;
            return layerScript;
        }
    }
    public PanelBase OpenLayer(GameObject layerRef, LayerAction action = 0, params object[] data)
    {
        if (LayerList.ContainsKey(layerRef))
        {
            LayerList[layerRef].SetActive(true);
            LayerList[layerRef].onEnter(data);
            LayerStack.Push(LayerList[layerRef]);
            CurLayer = LayerList[layerRef];
            return LayerList[layerRef];
        }
        else
        {
            if (layerRef == null)
            {
                Debug.Log("加载的layer为空");
                return null;
            }
            var LayerCanvas = GameObject.FindWithTag("LayerCanvas");
            if (LayerCanvas == null)
            {
                Debug.Log("场景中没有Tag为LayerCanvas的物体");
                return null;
            }
            var layer = GameObject.Instantiate(layerRef, LayerCanvas.transform);
            PanelBase layerScript = layer.GetComponent<PanelBase>();
            layerScript.transform.localPosition = new Vector3(0, 0, 0);
            layerScript.onEnter(data);
            LayerList[layerRef] = layerScript;
            LayerStack.Push(layerScript);
            CurLayer = layerScript;
            return layerScript;
        }
    }
    public void addMap(string mapName)
    {
        var layerRef = Resources.Load("Panle/" + mapName) as GameObject;
        if (layerRef != null)
        {
            GameObject.Instantiate(layerRef, GameObject.Find("Grid").transform);      
        }
        else
        {
            Debug.Log("不存在" + mapName);
        }
    }
    public void setPlayerPos(Vector2 pos)
    {
        var player = GameObject.Find("Player");
        player.transform.position =new Vector2(pos.x,pos.y);
    }
    private IEnumerator CallOnEnter(PanelBase layer)
    {
        yield return null;
        layer.onEnter();
    }
    public void colseLayer()
    {
        //if (LayerList.Count > 0)
        //{
        //    var layer = layerStack.Pop();
        //    layer.transform.SetActive(false);          
        //    layer.onExit();
        //}
    }
    public void OpenTalkLayer(GameObject Npc)
    {
        GameObject layer = Resources.Load<GameObject>("LayerRef/TalkLayer");
        //摄像机过渡动画信息           
        NpcCamera.GetComponent<CameraScript>().SetFollowTarget(Npc.transform);
        PlayerCamera.GetComponent<CameraScript>().HideCamera(() =>
        {
            OpenLayer(layer,Npc.name);
        });
    }
   
    public void CloseLayer()
    {
        LayerStack.Pop();
        CurLayer.SetActive(false);
        CurLayer.GetComponent<PanelBase>().onExit();
        CurLayer = null;
    }
    public Transform getCanvas()
    {
        return GameObject.Find("InTurnCanvas").transform;
    }
    public GameObject getMonsterHpBar()
    {
        var hpBarRef = Resources.Load<GameObject>("Monster/HpBar");
        return hpBarRef;
    }
    public void LoadScene(string sceneName, Func<Slider, IEnumerator> loadDataClick)
    {
        var layer = OpenLayer(Resources.Load("Panle/LoadLayer") as GameObject).GetComponent<LoadLayer>();
        layer.StartScene(sceneName,loadDataClick);
    }
}
