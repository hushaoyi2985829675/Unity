using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    public GameObject MainLayer;
    GameObject curLayer;
    List<GameObject> LayerList;
    void Start()
    {    
        LayerList = new List<GameObject>();
        if (LayerList.Count == 0)
        { 
            LayerList.Add(MainLayer);
        }
    }
    public void OpenLayer(GameObject layer)
    {
        if (layer == null)
        {
            Debug.Log("Layer¿Õ");
            return;
        }
        MainLayer.SetActive(false);
        if (LayerList.Contains(layer))
        {
            curLayer = LayerList.Find(gameObject => gameObject == layer);
        }
        else
        {
            Instantiate(layer, gameObject.transform);
            LayerList.Add(layer);
            curLayer = layer;
        }
        curLayer.SetActive(true);
    }

    public void CloseLayer(GameObject layer)
    {
        curLayer.SetActive(false);
        MainLayer.SetActive(true);
        curLayer = MainLayer;
    }
}
