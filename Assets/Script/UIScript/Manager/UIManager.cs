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
    private Dictionary<string,PanelBase> LayerList;
    private Dictionary<string,PanelBase> UINodeList;
    private Dictionary<string,GameObject> MapList;
    private GameObject CurMap;
    GameObject NpcCamera;
    GameObject PlayerCamera;
    CinemachineBlenderSettings CinemachineBlenderSettings;
    UIManager()
    {
        LayerList = new Dictionary<string, PanelBase>();
        MapList = new Dictionary<string, GameObject>();
        //CinemachineBlenderSettings = AssetDatabase.LoadAssetAtPath<CinemachineBlenderSettings>("Assets/CameraBlends/CameraBlends.asset");
        UINodeList =  new Dictionary<string,PanelBase>();
        NpcCamera = GameObject.FindWithTag("NpcCamera");
        PlayerCamera = GameObject.FindWithTag("PlayerCamera");
    }
    public PanelBase OpenLayer(GameObject layerRef, params object[] data)
    {
        PanelBase layer = AddLayer(ref LayerList, layerRef,data);
        return layer;
    }
    
    private PanelBase AddLayer(ref Dictionary<string,PanelBase> layerList,GameObject layerRef, params object[] data)
    {
        if (layerList.ContainsKey(layerRef.name))
        {
            PanelBase layer = layerList[layerRef.name];
            if (layer.gameObject.activeSelf)
            {
                layer.onExit();
            }
            layer.SetActive(true);
            layer.transform.SetAsLastSibling();
            layer.onEnter(data);
            return layer;
        }
        else
        {
            if (layerRef == null)
            {
                Debug.Log("���ص�layerΪ��");
                return null;
            }
            var LayerCanvas = GameObject.FindWithTag("LayerCanvas");
            if (LayerCanvas == null)
            {
                Debug.Log("������û��TagΪLayerCanvas������");
                return null;
            }
            var layer = GameObject.Instantiate(layerRef, LayerCanvas.transform);
            layer.name = layerRef.name;
            PanelBase layerScript = layer.GetComponent<PanelBase>();
            layerScript.transform.localPosition = new Vector3(0, 0, 0);
            layerScript.transform.SetAsLastSibling();
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
            var LayerCanvas = GameObject.FindWithTag("LayerCanvas");
            if (LayerCanvas == null)
            {
                return null;
            }
            var layer = GameObject.Instantiate(layerRef, LayerCanvas.transform);
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
        curLayer.SetActive(false);
        curLayer.onExit();
    }
    public PanelBase AddUINode(GameObject layerRef, Vector2 pos,params object[] data)
    {
        PanelBase layer = AddLayer(ref UINodeList, layerRef,data);
        layer.transform.localPosition = pos;
        return layer; 
    }
    public void CloseUINode(string name)
    {
        PanelBase curLayer = UINodeList[name];
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
