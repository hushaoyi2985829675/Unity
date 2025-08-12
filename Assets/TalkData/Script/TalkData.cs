using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TalkType
{ 
    Player,
    Npc
}
[CreateAssetMenu(fileName = "New Data", menuName = "Character Data/TalkData")]
public class TalkData : ScriptableObject
{
    public List<TalkObject> TalkInfo;
}
[System.Serializable]
public class TalkObject
{
    public string name;
    public List<TalkInfo> talkLists;
}
[System.Serializable]
public class TalkInfo
{   
    public TalkType talkType;
    public List<string> talkStr;
}
