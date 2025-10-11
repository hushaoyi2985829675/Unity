using Assets.HeroEditor.Common.CommonScripts;
using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum LayerAction
{
    Scale,
    BlackScreen,
    AllAction,
}

public class UINodeClass
{
    //当前显示Node
    public PanelBase panelNode;
    public Dictionary<string, PanelBase> cacheDict;

    public UINodeClass()
    {
        panelNode = null;
        cacheDict = new Dictionary<string, PanelBase>();
    }
}

public class UIManager : Singleton<UIManager>
{
    public GameObject ResourceNode;
    private Dictionary<int, PanelBase> LayerList;
    private Dictionary<int, PanelBase> PopLayerList;

    private Dictionary<int, int> instanceToPrefabGidList;

    //private Dictionary<string, PanelBase> UINodeDict;
    private Dictionary<string,GameObject> MapList;
    private GameObject CurMap;
    private Transform layerCanvas;
    private Transform popLayerCanvas;
    private PanelBase curActPanelNode;

    private void Awake()
    {
        LayerList = new Dictionary<int, PanelBase>();
        MapList = new Dictionary<string, GameObject>();
        PopLayerList = new Dictionary<int, PanelBase>();
        instanceToPrefabGidList = new Dictionary<int, int>();
        layerCanvas = GameObject.FindWithTag("LayerCanvas").transform;
        popLayerCanvas = GameObject.FindWithTag("PopLayerCanvas").transform;
    }

    private void Start()
    {
        AddUINode(ResourceNode, layerCanvas);
    }
    public PanelBase OpenLayer(GameObject layerRef, params object[] data)
    {
        Debug.Log("打开页面:" + layerRef.name);
        PanelBase layer = AddLayer(ref LayerList, layerRef, layerCanvas.transform, data);
        return layer;
    }

    private PanelBase AddLayer(ref Dictionary<int, PanelBase> layerList, GameObject layerRef,
        Transform parent = null, params object[] data)
    {
        PanelBase layerScript;
        int gid = layerRef.GetInstanceID();
        if (layerList.ContainsKey(gid))
        {
            layerScript = layerList[gid];
        }
        else
        {
            if (layerRef == null)
            {
                Debug.Log("界面预制体是空");
                return null;
            }

            GameObject layer = Instantiate(layerRef, parent);
            layer.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            layer.name = layerRef.name;
            layerScript = layer.GetComponent<PanelBase>();
            layerScript.onEnter(data);
            layerList[gid] = layerScript;
            int instanceGid = layer.GetInstanceID();
            instanceToPrefabGidList[instanceGid] = gid;
        }

        layerScript.SetActive(true);
        layerScript.transform.SetSiblingIndex(parent.childCount);
        layerScript.onShow(data);
        return layerScript;
    }

    // public PanelBase OpenLayer(GameObject layerRef, LayerAction action = 0, params object[] data)
    // {
    //     if (LayerList.ContainsKey(layerRef.name))
    //     {
    //         LayerList[layerRef.name].SetActive(true);
    //         LayerList[layerRef.name].onShow(data);
    //         return LayerList[layerRef.name];
    //     }
    //     else
    //     {
    //         if (layerRef == null)
    //         {
    //             return null;
    //         }
    //
    //         var layer = GameObject.Instantiate(layerRef, layerCanvas);
    //         layer.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    //         PanelBase layerScript = layer.GetComponent<PanelBase>();
    //         layerScript.transform.localPosition = new Vector3(0, 0, 0);
    //         layerScript.onEnter(data);
    //         LayerList[layerRef.name] = layerScript;
    //         return layerScript;
    //     }
    // }
    public void CloseLayer(GameObject layer, params object[] data)
    {
        int instanceGid = layer.GetInstanceID();
        int gid = instanceToPrefabGidList[instanceGid];
        PanelBase curLayer = LayerList[gid];
        curLayer.transform.SetSiblingIndex(0);
        curLayer.SetActive(false);
        curLayer.Hide();
        curLayer.onExit();
    }

    public PanelBase AddPopLayer(GameObject layerRef, Vector2 pos, params object[] data)
    {
        PanelBase layer = AddLayer(ref PopLayerList, layerRef, popLayerCanvas, data);
        layer.transform.localPosition = pos;
        return layer; 
    }

    public PanelBase AddUINode(GameObject layerRef, Transform parent, params object[] data)
    {
        var layer = Instantiate(layerRef, parent);
        layer.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        layer.name = layerRef.name;
        PanelBase layerScript = layer.GetComponent<PanelBase>();
        layerScript.onEnter(data);
        layerScript.transform.SetSiblingIndex(parent.childCount);
        layerScript.onShow(data);
        return layerScript;
    }

    public PanelBase AddUILayer(ref Dictionary<int, PanelBase> layerList, GameObject layerRef,
        Transform parent, params object[] data)
    {
        PanelBase layer = AddLayer(ref layerList, layerRef, parent, data);
        return layer;
    }

    public void ClosePopLayer(GameObject layer)
    {
        CloseUINode(layer);
    }

    public void CloseUINode(GameObject layer)
    {
        PanelBase panelBase = layer.GetComponent<PanelBase>();
        panelBase.transform.SetSiblingIndex(0);
        panelBase.SetActive(false);
        panelBase.onExit();
        panelBase.Hide();
    }

    public void SetShowHide(PanelBase layerScript, bool show)
    {
        if (show)
        {
        }
    }

    public void AddMap(GameObject mapLayer, Vector2 position, string name)
    {
        if (CurMap != null)
        {
            CurMap.SetActive(false);
        }

        if (MapList.ContainsKey(mapLayer.name))
        {
            CurMap = MapList[mapLayer.name];
            CurMap.SetActive(true);
        }
        else
        {
            //从最开始游戏进来就打开
            if (CurMap == null)
            {
                CurMap = GameObject.Find("MainMap");
                MapList.Add(CurMap.name, CurMap);
            }

            CurMap.SetActive(false);
            var layer = Instantiate(mapLayer, GameObject.Find("Grid").transform);
            MapList.Add(mapLayer.name, layer);
            CurMap = layer;
        }

        SetPlayerPos(position);
    }
    public void SetPlayerPos(Vector2 pos)
    {
        var player = GameObjectManager.instance.GetPlayer();
        player.SetPlayerPos(new Vector2(pos.x,pos.y));
    }
    
    public Transform getCanvas()
    {
        return GameObject.Find("InTurnCanvas").transform;
    }
    public GameObject getMonsterHpBar()
    {
        var hpBarRef = Resources.Load<GameObject>("Ref/GameObject/Monster/HpBar");
        return hpBarRef;
    }

    public void LoadScene(string sceneName, Action<Slider> callback)
    {
        LoadLayer layer = OpenLayer(Resources.Load<LoadLayer>("Ref/LayerRef/UIRef/LoadLayer/LoadLayer").gameObject)
            .GetComponent<LoadLayer>();
        layer.StartScene(sceneName, callback);
    }

    public void LoadMainScene()
    {
        LoadScene("MainScene", (slider) => { });
    }
}
