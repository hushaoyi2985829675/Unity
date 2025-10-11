using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapLayerInfo
{
    public int mapId;
    public GameObject mapLayer;
}

[CreateAssetMenu(fileName = "MapLayerData", menuName = "GameData/MapLayerData")]
public class MapLayerData : ScriptableObject
{
    public List<MapLayerInfo> mapInfoList = new List<MapLayerInfo>();
}