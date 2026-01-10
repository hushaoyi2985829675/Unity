using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapLayerInfo
{
    public int mapId;
    public GameObject mapLayer;
    public GameObject cameraBoundary;
    public Vector2 playerPos;
    public List<AudioClip> MusicList;
    public AudioClip BossMusic;

    public AudioClip GetMusicClip()
    {
        return MusicList[Random.Range(0, MusicList.Count)];
    }
}

[CreateAssetMenu(fileName = "MapLayerData", menuName = "GameStaticData/MapLayerData")]
public class MapLayerData : ScriptableObject
{
    public List<MapLayerInfo> mapInfoList = new List<MapLayerInfo>();
}