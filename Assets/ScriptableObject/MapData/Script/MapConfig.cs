using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character Data/MapConfig")]
public class MapConfig : ScriptableObject
{
   public List<MapData> data = new List<MapData>();
}

[System.Serializable]
public class MapData
{
    public int Id;
    public string Name;
    public Sprite Sprite;
    [SerializeField]  public GameObject MapLayer;
}