using System.Collections;
using System.Collections.Generic;
using NpcTalkTask;
using Task;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerTaskData", menuName = "CharacterData/Player/PlayerTaskData")]
public class PlayerTaskData : ScriptableObject
{
    public List<TaskInfo> TaskList;

    public void AddTask(int npcId, PlayerLvInfo playerLvInfo)
    {
        TaskInfo taskInfo = new TaskInfo();
        taskInfo.npcId = npcId;
        taskInfo.taskId = playerLvInfo.taskId;
        taskInfo.curNum = 0;
        taskInfo.isComplete = false;
        taskInfo.needNum = playerLvInfo.taskRequirement;
        taskInfo.taskLocation = playerLvInfo.taskLocation;
        taskInfo.rewardInfo = playerLvInfo.taskReward;
        TaskList.Add(taskInfo);
    }
}

[System.Serializable]
public class TaskInfo
{
    public int npcId;
    public int taskId;
    public bool isComplete;
    public int taskLocation;
    public int TaskType;
    public int needNum;
    public int curNum;
    public string rewardInfo;
}


