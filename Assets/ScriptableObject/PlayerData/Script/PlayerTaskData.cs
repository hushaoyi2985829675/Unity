using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerTaskData", menuName = "CharacterData/Player/PlayerTaskData")]
public class PlayerTaskData : ScriptableObject
{
    public List<TaskInfo> TaskList; 
}

[System.Serializable]
public class TaskInfo
{
    public int taskId;
    public bool isComplete;
    public int TaskType;
    public int needNum;
    public int curNum;
}
