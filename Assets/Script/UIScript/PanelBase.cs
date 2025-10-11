using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HeroEditor.Common.Enums;
using UnityEngine;

public abstract class PanelBase : MonoBehaviour
{
    // private Dictionary<int, UINodeClass> UINodeDict = new Dictionary<int, UINodeClass>();
    Dictionary<int, PanelBase> UINodeDict = new Dictionary<int, PanelBase>();
    Dictionary<int, PanelBase> UILayerDict = new Dictionary<int, PanelBase>();
    public abstract void onEnter(params object[] data);

    public abstract void onShow(params object[] data);

    protected T AddUINode<T>(GameObject layerRef, Transform parent, params object[] data)
        where T : PanelBase
    {
        PanelBase layer = UIManager.Instance.AddUINode(layerRef, parent, data);
        int gId = layer.gameObject.GetInstanceID();
        UINodeDict[gId] = layer;
        return layer as T;
    }

    protected PanelBase AddUINode(GameObject layerRef, Transform parent, params object[] data)
    {
        return AddUINode<PanelBase>(layerRef, parent, data);
    }

    protected PanelBase AddUILayer(GameObject layerRef, Transform parent, params object[] data)
    {
        PanelBase layer = UIManager.Instance.AddUILayer(ref UILayerDict, layerRef, parent, data);
        return layer;
    }

    protected void CloseUILayer(GameObject layer)
    {
        UIManager.Instance.CloseUINode(layer);
    }

    protected void CloseUINode(Transform parent)
    {
        int gId = parent.gameObject.GetInstanceID();
        UINodeDict.Remove(gId);
        Destroy(parent.gameObject);
    }

    protected void CloseNodeAllUINode(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            CloseUINode(child);
        }
    }

    public void Hide()
    {
        foreach (PanelBase uiNode in UINodeDict.Values)
        {
            UIManager.Instance.CloseUINode(uiNode.gameObject);
            Destroy(uiNode.gameObject);
        }

        foreach (PanelBase uiNode in UILayerDict.Values)
        {
            UIManager.Instance.CloseUINode(uiNode.gameObject);
            Destroy(uiNode.gameObject);
        }

        UINodeDict.Clear();
        UILayerDict.Clear();
    }

    public abstract void onExit();
}
