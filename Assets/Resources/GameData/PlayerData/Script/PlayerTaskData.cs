using System.Collections;
using System.Collections.Generic;
using NpcTalkTask;
using Task;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerTaskData", menuName = "CharacterData/Player/PlayerTaskData")]
public class PlayerTaskData : ScriptableObject
{
    public List<TaskInfo> TaskList;

    public void AddTask(int npcId, PlayerTaskLvInfo playerLvInfo)
    {
        TaskInfo taskInfo = new TaskInfo();
        taskInfo.npcId = npcId;
        taskInfo.taskId = playerLvInfo.taskId;
        taskInfo.curNum = 0;
        taskInfo.isComplete = false;
        taskInfo.needNum = playerLvInfo.taskRequirement;
        taskInfo.taskLocation = playerLvInfo.taskLocation;
        taskInfo.rewardList = Ui.Instance.FormatResStr(playerLvInfo.taskReward);
        TaskList.Add(taskInfo);
    }

    public void RemoveTask(int taskId)
    {
        TaskList.Remove(TaskList.Find((obj) => obj.taskId == taskId));
    }

    public void MarkTaskCompletion(int taskId)
    {
        TaskInfo taskInfo = GetTaskInfo(taskId);
        taskInfo.isComplete = true;
    }

    private TaskInfo GetTaskInfo(int taskId)
    {
        return TaskList.Find((obj) => obj.taskId == taskId);
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
    public List<ResClass> rewardList;
}


