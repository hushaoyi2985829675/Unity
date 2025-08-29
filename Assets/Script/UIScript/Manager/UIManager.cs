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

public class UIManager : Singleton<UIManager>
{
    public GameObject ResourceNode;
    private Dictionary<string,PanelBase> LayerList;
    private Dictionary<string, PanelBase> PopLayerList;
    private Dictionary<string, PanelBase> UINodeList;
    private Dictionary<string,GameObject> MapList;
    private GameObject CurMap;
    private Transform layerCanvas;
    GameObject NpcCamera;
    GameObject PlayerCamera;
    CinemachineBlenderSettings CinemachineBlenderSettings;

    private void Awake()
    {
        LayerList = new Dictionary<string, PanelBase>();
        MapList = new Dictionary<string, GameObject>();
        //CinemachineBlenderSettings = AssetDatabase.LoadAssetAtPath<CinemachineBlenderSettings>("Assets/CameraBlends/CameraBlends.asset");
        PopLayerList = new Dictionary<string, PanelBase>();
        UINodeList = new Dictionary<string, PanelBase>();
        NpcCamera = GameObject.FindWithTag("NpcCamera");
        PlayerCamera = GameObject.FindWithTag("PlayerCamera");
        layerCanvas = GameObject.FindWithTag("LayerCanvas").transform;
    }

    private void Start()
    {
        AddUINode(ResourceNode, layerCanvas);
    }
    public PanelBase OpenLayer(GameObject layerRef, params object[] data)
    {
        PanelBase layer = AddLayer(ref LayerList, layerRef, layerCanvas.transform, data);
        return layer;
    }

    private PanelBase AddLayer(ref Dictionary<string, PanelBase> layerList, GameObject layerRef,
        Transform parent = null, params object[] data)
    {
        if (layerList.ContainsKey(layerRef.name))
        {
            PanelBase layer = layerList[layerRef.name];
            if (layer.gameObject.activeSelf)
            {
                layer.onExit();
            }
            layer.SetActive(true);
            //调整排序位置
            layer.transform.SetAsLastSibling();
            layer.onEnter(data);
            return layer;
        }
        else
        {
            if (layerRef == null)
            {
                Debug.Log("界面预制体是空");
                return null;
            }

            var layer = GameObject.Instantiate(layerRef, parent);
            layer.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            layer.name = layerRef.name;
            PanelBase layerScript = layer.GetComponent<PanelBase>();
            layerScript.transform.SetSiblingIndex(parent.childCount - 2);
            layerScript.onEnter(data);
            layerList[layerRef.name] = layerScript;
            return layerScript;
        }
    }
    public PanelBase OpenLayer(GameObject layerRef, LayerAction action = 0, params object[] data)
    {
        if (LayerList.ContainsKey(layerRef.name))
        {
            LayerList[layerRef.name].SetActive(true);
            LayerList[layerRef.name].onEnter(data);
            return LayerList[layerRef.name];
        }
        else
        {
            if (layerRef == null)
            {
                return null;
            }

            var layer = GameObject.Instantiate(layerRef, layerCanvas);
            layer.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            PanelBase layerScript = layer.GetComponent<PanelBase>();
            layerScript.transform.localPosition = new Vector3(0, 0, 0);
            layerScript.onEnter(data);
            LayerList[layerRef.name] = layerScript;
            return layerScript;
        }
    }
    public void CloseLayer(String name)
    {
        PanelBase curLayer = LayerList[name];
        curLayer.transform.SetSiblingIndex(0);
        curLayer.SetActive(false);
        curLayer.onExit();
    }

    public PanelBase AddPopLayer(GameObject layerRef, Vector2 pos, params object[] data)
    {
        PanelBase layer = AddLayer(ref PopLayerList, layerRef, layerCanvas, data);
        layer.transform.localPosition = pos;
        return layer; 
    }

    public PanelBase AddUINode(GameObject layerRef, Transform parent, params object[] data)
    {
        PanelBase layer = AddLayer(ref UINodeList, layerRef, parent, data);
        return layer;
    }

    public void ClosePopLayer(string name)
    {
        PanelBase curLayer = PopLayerList[name];
        curLayer.transform.SetSiblingIndex(0);
        curLayer.SetActive(false);
        curLayer.onExit();
    }

    public void CloseUINode(string name)
    {
        PanelBase curLayer = UINodeList[name];
        curLayer.transform.SetSiblingIndex(0);
        curLayer.SetActive(false);
        curLayer.onExit();
    } 
    public void AddMap(GameObject mapLayer,Vector2 position)
    {
        CameraManager.Instance.ChangeMapAction(() =>
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
                    MapList.Add(CurMap.name,CurMap);
                }
                CurMap.SetActive(false);
                var layer = GameObject.Instantiate(mapLayer, GameObject.Find("Grid").transform);
                MapList.Add(mapLayer.name,layer);
                CurMap = layer;
            }
            SetPlayerPos(position);
        });
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
    public void LoadScene(string sceneName, Func<Slider, IEnumerator> loadDataClick)
    {
        var layer = OpenLayer(Resources.Load("Ref/LayerRef/UIRef/LoadLayer") as GameObject).GetComponent<LoadLayer>();
        layer.StartScene(sceneName,loadDataClick);
    }
}
