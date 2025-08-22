using System.Collections;
using System.Collections.Generic;
using System;
using Npc;
using Task;
using UnityEngine;
using UnityEngine.UI;

public class TaskLayer : PanelBase
{
    public Button closeBtn;
    public TableView tabView;
    public PlayerTaskData PlayerTaskData;
    public List<NpcIdInfo> NpcConfig;
    public Button abandonButton;
    public Text progress;
    public Text npcName;
    public Text taskDes;
    private List<TaskInfo> taskList;
    private TaskInfo playerTaskInfo;
    private List<Task.TaskInfo> TaskConfig;
    private Task.TaskInfo taskInfo;
    public override void onEnter(params object[] data)
    {
        InitData();
        abandonButton.onClick.AddListener(AbandonTaskClick);
        closeBtn.onClick.AddListener(CloseClick);
        tabView.AddRefreshEvent(CreateItem);
        tabView.SetNum(PlayerTaskData.TaskList.Count);
        playerTaskInfo = taskList[0];
        RefreshTaskInfo(0);
    }

    private void InitData()
    {
        TaskConfig = Resources.Load<TaskConfig>("Configs/Data/TaskConfig").taskInfoList;
        NpcConfig = Resources.Load<NpcConfig>("Configs/Data/NpcConfig").npcIdInfoList;
        taskList = PlayerTaskData.TaskList;
    }

    private void CreateItem(int i, GameObject item)
    {
        taskInfo = TaskConfig.Find((obj) => obj.task == taskList[i].taskId);
        item.GetComponent<TaskItem>().InitData(i, taskInfo.name, RefreshTaskInfo);
    }

    private void RefreshTaskInfo(int i)
    {
        playerTaskInfo = taskList[i];
        //Npc名字
        NpcIdInfo npcIdInfo = NpcConfig.Find((obj) => obj.npcId == playerTaskInfo.npcId);
        npcName.text = npcIdInfo.name;
        //目标
        progress.text = string.Format("{0}/{1}", playerTaskInfo.curNum, playerTaskInfo.needNum);
    }

    private void CloseClick()
    {
        UIManager.Instance.CloseLayer(gameObject.name);
    }

    private void AbandonTaskClick()
    {
        PlayerTaskData.RemoveTask(playerTaskInfo.taskId);
        tabView.SetNum(PlayerTaskData.TaskList.Count);
        playerTaskInfo = taskList[0];
        RefreshTaskInfo(0);
    }

    public override void onExit()
    {
        
    }
}
