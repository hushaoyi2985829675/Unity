using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CommonScripts;
using DG.Tweening;
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
    [Header("资源栏")]
    private GameObject ResourceNode;

    [Header("地图Gird")]
    [SerializeField]
    private Transform mapGrid;

    [Header("Layer遮罩")]
    private GameObject maskLayerRef;
    
    private Dictionary<int, PanelBase> LayerList;
    private Dictionary<PanelBase, MaskLayer> maskLayerList;
    private Dictionary<int, PanelBase> PopLayerList;
    private Dictionary<int, int> instanceToPrefabGidList;
    private Dictionary<int, GameObject> MapList;
    private GameObject curMap;
    private Transform layerCanvas;
    private Transform popLayerCanvas;
    private PanelBase curActPanelNode;
    private void Awake()
    {
        ResourceNode = Ui.Instance.GetLayerRef("ResourceNode/ResourceNode");
        maskLayerRef = Ui.Instance.GetLayerRef("Common/MaskLayer");
        LayerList = new Dictionary<int, PanelBase>();
        MapList = new Dictionary<int, GameObject>();
        PopLayerList = new Dictionary<int, PanelBase>();
        maskLayerList = new Dictionary<PanelBase, MaskLayer>();
        instanceToPrefabGidList = new Dictionary<int, int>();
        layerCanvas = GameObject.FindWithTag("LayerCanvas").transform;
        popLayerCanvas = GameObject.FindWithTag("PopLayerCanvas").transform;
    }

    private void Start()
    {
        {
            //测试代码
            curMap = GameObject.Find("MainMap");
            MapLayerInfo mapLayerInfo = Ui.Instance.GetMapLayerInfo(8);
            GameObject mapLayer = GameObject.Find("MainMap");
            int gid = mapLayerInfo.mapLayer.GetInstanceID();
            MapList.Add(gid, mapLayer);
        }
        //AddUINode(ResourceNode, layerCanvas);
    }
    public PanelBase OpenLayer(GameObject layerRef, params object[] data)
    {
        PanelBase layer = AddLayer(ref LayerList, layerRef, layerCanvas.transform, data);
        MaskLayer maskLayer;
        if (maskLayerList.ContainsKey(layer))
        {
            maskLayer = maskLayerList[layer];
            maskLayer.SetActive(true);
        }
        else
        {
            maskLayer = Instantiate(maskLayerRef, layerCanvas.transform).GetComponent<MaskLayer>();
            maskLayer.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            maskLayer.SetPanel(layer);
            maskLayer.SetActive(true);
            maskLayerList[layer] = maskLayer;
        }

        maskLayer.transform.SetSiblingIndex(layerCanvas.transform.childCount);
        layer.transform.SetSiblingIndex(layerCanvas.transform.childCount);
        // EditorApplication.isPaused = true;
        layer.transform.localScale = new Vector3(0.01f, 0.01f, 1);
        layer.transform.DOScale(new Vector3(1, 1, 1), 0.3f).SetEase(Ease.OutCirc);
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

        layerScript.gameObject.SetActive(true);
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
    public void CloseLayer(GameObject layer)
    {
        int instanceGid = layer.GetInstanceID();
        int gid = instanceToPrefabGidList[instanceGid];
        PanelBase curLayer = LayerList[gid];
        curLayer.Hide();
        MaskLayer maskLayer = maskLayerList[curLayer];
        maskLayer.SetActive(false);
        layer.transform.DOScale(new Vector3(0.01f, 0.01f, 1), 0.15f).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            layer.SetActive(false);
            layer.transform.DOKill();
        });
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
        panelBase.transform.SetSiblingIndex(1);
        panelBase.gameObject.SetActive(false);
        panelBase.onExit();
        panelBase.Hide();
    }


    public void AddMap(int mapId, bool isAction = false)
    {
        Action<int> action = (mapId) =>
        {
            MapLayerInfo mapLayerInfo = Ui.Instance.GetMapLayerInfo(mapId);
            if (curMap != null)
            {
                curMap.transform.SetActive(false);
            }

            int gid = mapLayerInfo.mapLayer.GetInstanceID();
            if (MapList.ContainsKey(gid))
            {
                curMap = MapList[gid];
                curMap.transform.SetActive(true);
            }
            else
            {
                var layer = Instantiate(mapLayerInfo.mapLayer, mapGrid);
                MapList.Add(gid, layer);
                curMap = layer;
            }

            CameraManager.Instance.ChangeBoundary(mapLayerInfo.cameraBoundary);
            //设置人物位置
            GameObjectManager.Instance.SetPlayerPos(mapLayerInfo.playerPos);
            //延迟调用等待黑屏结束
            DOVirtual.DelayedCall(1f, () =>
            {
                Ui.Instance.ShowFlutterView(Ui.Instance.GetMapInfo(mapId).name);
                AudioManager.Instance.PlayBGM(mapId);
            });
        };
        if (isAction)
        {
            CameraManager.Instance.ChangeMapAction(() =>
            {
                action(mapId);
            });
        }
        else
        {
            action(mapId);
        }
    }
    
    public void ResetMapGrid()
    {
        curMap = null;
        Ui.Instance.RemoveAllChildren(mapGrid);
        MapList.Clear();
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

    public void LoadScene(string sceneName, int mapId)
    {
        CameraManager.Instance.ChangeMapAction(() =>
        {
            OpenLayer(Resources.Load<LoadLayer>("Ref/LayerRef/UIRef/LoadLayer/LoadLayer").gameObject, new object[] {sceneName, mapId}).GetComponent<LoadLayer>();
        });
        
    }
    
    public void LoadMainScene()
    {
        //LoadScene("MainScene", (slider) => { });
    }
}
