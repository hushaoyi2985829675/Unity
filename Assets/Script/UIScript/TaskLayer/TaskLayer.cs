using System.Collections;
using System.Collections.Generic;
using System;
using MapNs;
using NpcNs;
using TaskNs;
using UnityEngine;
using UnityEngine.UI;

public class TaskLayer : PanelBase
{
    [SerializeField] private GameObject UINode;
    [SerializeField] private GameObject CardNodeRef;
    [SerializeField] private TableView tabView;
    [SerializeField] private List<NpcIdInfo> NpcConfig;
    [SerializeField] private Button abandonButton;
    [SerializeField] private Image completionImg;
    [SerializeField] private Text taskDesText;
    [SerializeField] private Text progressText;
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private Text npcName;
    [SerializeField] private Transform taskRewardNode;
    [SerializeField] private List<TaskInfo> taskList;
    [SerializeField] private TaskInfo playerTaskInfo;

    [SerializeField]
    private List<TaskNs.TaskInfo> TaskConfig;

    [SerializeField]
    private TaskNs.TaskInfo taskInfo;
    public override void onEnter(params object[] data)
    {
        TaskConfig = Resources.Load<TaskConfig>("Configs/Data/TaskConfig").taskInfoList;
        NpcConfig = Resources.Load<NpcConfig>("Configs/Data/NpcConfig").npcIdInfoList;
       
    }

    public override void onShow(object[] data)
    {
        InitData();
        RefreshTaskInfo(0);
        tabView.SetNum(taskList.Count);
    }
    private void InitData()
    {
        taskList = GameDataManager.Instance.GetPlayerTaskData();
        abandonButton.onClick.AddListener(AbandonTaskClick);
        tabView.AddRefreshEvent(CreateItem);
        if (taskList.Count > 0)
        {
            playerTaskInfo = taskList[0];
        }

        taskList.Sort((a, b) =>
        {
            if (a.isComplete != b.isComplete)
            {
                return a.isComplete ? 1 : -1;
            }

            return 0;
        });
    }

    private void CreateItem(int i, GameObject item)
    {
        taskInfo = TaskConfig.Find((obj) => obj.task == taskList[i].taskId);
        item.GetComponent<TaskItem>().InitData(i, taskInfo.name, RefreshTaskInfo);
    }

    private void RefreshTaskInfo(int i)
    {
        if (taskList.Count == 0)
        {
            UINode.SetActive(false);
            return;
        }

        UINode.SetActive(true);

        playerTaskInfo = taskList[i];
        //Npc名字
        NpcIdInfo npcIdInfo = NpcConfig.Find((obj) => obj.npcId == playerTaskInfo.npcId);
        npcName.text = npcIdInfo.name;
        //目标
        List<string> paramsList = new List<string>();
        if (playerTaskInfo.taskLocation != 0)
        {
            MapInfo mapData = Ui.Instance.GetMapInfo(playerTaskInfo.taskLocation);
            paramsList.Add(mapData.name);
        }

        paramsList.Add(playerTaskInfo.needNum.ToString());
        string des = Ui.Instance.GetTaskDes(playerTaskInfo.taskId);
        des = string.Format(des, paramsList.ToArray());
        taskDesText.text = string.Format("{0}", des);
        progressText.text = string.Format("({0}/{1})", playerTaskInfo.curNum, playerTaskInfo.needNum);
        progressBar.SetMaxValue(playerTaskInfo.needNum);
        progressBar.SetValue(playerTaskInfo.curNum);
        //奖励
        CloseNodeAllUINode(taskRewardNode);
        foreach (ResClass resInfo in playerTaskInfo.rewardList)
        {
            CardNode cardNode = AddUINode<CardNode>(CardNodeRef, taskRewardNode);
            cardNode.SetCardData(resInfo);
        }

        //是否完成
        completionImg.gameObject.SetActive(playerTaskInfo.isComplete);
        abandonButton.gameObject.SetActive(!playerTaskInfo.isComplete);
    }

    private void CloseClick()
    {
        UIManager.Instance.CloseLayer(gameObject);
    }

    private void AbandonTaskClick()
    {
        GameDataManager.Instance.RemoveTask(playerTaskInfo.taskId);
        tabView.SetNum(taskList.Count);
        if (taskList.Count > 0)
        {
            playerTaskInfo = taskList[0];
        }
        RefreshTaskInfo(0);
    }

    public override void onExit()
    {
        
    }
}
