using System;
using System.Collections.Generic;
using DG.Tweening;
using Goods;
using Npc;
using NpcTalkTask;
using Option;
using Talk;
using Task;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NpcTalkInfo = NpcTalkTask.NpcIdInfo;
using NpcCfgInfo = Npc.NpcIdInfo;
using ConfigTaskInfo = Task.TaskInfo;

enum OperatorType
{
    NpcLayer,
    Task,
}

public class BaseOperator
{
    public int id;
    public string title;

    public BaseOperator()
    {
    }

    public BaseOperator(string title, int id)
    {
        this.title = title;
        this.id = id;
    }
}

public class NpcTalk : PanelBase
{
    public EventTrigger trigger;
    public PlayerTaskData PlayerTaskData;
    public GoodsConfig GoodsConfig;
    public Text talkText;

    public int playerLv = 1;

    //是否接取任务
    public bool isTasking;

    //是否完成任务
    public bool isTaskComplete;
    public GameObject operatorRef;
    private Transform operatorNode;
    private List<BaseOperator> operatorList = new();

    private List<BaseOperator> taskOperatorList = new();
    private List<string> taskTalkStrList = new();
    private int idx;
    private bool isCanTalk = true;
    private bool isPlayTaskTalk;

    private bool isTask;

    //触发任务选择对话
    private bool isTriggerTaskSel;
    private Map.MapConfig MapConfig;
    private NpcCfgInfo npcCfgInfo;
    private NpcConfig NpcConfig;
    private int npcId = 1;
    private NpcTalkInfo npcInfo;

    [Header("配置")] private NpcTalkTaskConfig NpcTalkTaskConfig;
    private OptionConfig OptionConfig;
    private PlayerLvInfo playerLvInfo;
    private TalkConfig TalkConfig;
    private List<string> talkStrList = new();
    private List<string> taskAcceptTalkList = new();
    private TaskConfig TaskConfig;
    private ConfigTaskInfo taskInfo;
    private List<string> taskingTalkStrList = new();

    [Header("任务页面")] private GameObject TaskLayer;
    private List<string> taskRefusetTalkList = new();
    private string[] taskStrList;
    private Tween textTween;

    private void Awake()
    {
    }

    private void Update()
    {
    }

    public override void onEnter(params object[] data)
    {
        npcId = (int) data[0];
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(TouchClick);
        trigger.triggers.Add(entry);
        taskStrList = new string[2];
        isTriggerTaskSel = false;
        operatorNode = GameObject.Find("OptionNode").transform;
        TaskLayer = Resources.Load<GameObject>("Ref/LayerRef/UIRef/TaskLayer/TaskLayer");
        NpcTalkTaskConfig = Resources.Load<NpcTalkTaskConfig>("Configs/Data/NpcTalkTaskConfig");
        TaskConfig = Resources.Load<TaskConfig>("Configs/Data/TaskConfig");
        TalkConfig = Resources.Load<TalkConfig>("Configs/Data/TalkConfig");
        NpcConfig = Resources.Load<NpcConfig>("Configs/Data/NpcConfig");
        OptionConfig = Resources.Load<OptionConfig>("Configs/Data/OptionConfig");
        MapConfig = Resources.Load<Map.MapConfig>("Configs/Data/MapConfig");
        npcInfo = NpcTalkTaskConfig.npcIdInfoList.Find(obj => obj.npcId == npcId);
        npcCfgInfo = NpcConfig.npcIdInfoList.Find(obj => obj.npcId == npcId);
        playerLvInfo = npcInfo.playerLvInfoList.Find(obj => obj.playerLv == playerLv);
        taskInfo = TaskConfig.taskInfoList.Find(obj => obj.task == playerLvInfo.taskId);
        InitTaskState();
        InitTalkList();
        InitBaseOperator();
        Talk();
    }

    private void InitTaskState()
    {
        if (playerLvInfo.taskId != 0)
        {
            isTask = true;
            var taskData = PlayerTaskData.TaskList.Find(taskInfo => taskInfo.taskId == playerLvInfo.taskId);
            if (taskData != null)
            {
                isTasking = true;
                isTaskComplete = taskData.isComplete;
            }
        }
    }

    private void InitTalkList()
    {
        string[] idstr;
        Talk.TalkInfo talkInfo;
        string[] strList;
        //处理任务对话
        if (isTask && !isTasking && !isTaskComplete)
        {
            var parms = new object[5];
            var i = 0;
            if (playerLvInfo.taskLocation != 0)
            {
                parms[i] = MapConfig.mapInfoList.Find(obj => obj.map == playerLvInfo.taskLocation)?.name;
                i++;
            }

            //目标名字
            parms[i] = Ui.Instance.GetTaskTargetName(taskInfo.taskType, taskInfo.targetType);
            i++;
            //数量
            parms[i] = playerLvInfo.taskRequirement;
            i++;
            talkInfo = TalkConfig.talkInfoList.Find(obj => obj.talk == playerLvInfo.beforeTaskCompletion);
            //奖励
            var rewardInfo = playerLvInfo.taskReward.Split(",");
            var goodInfo = Ui.Instance.GetGoodInfo(int.Parse(rewardInfo[0]), int.Parse(rewardInfo[1]));
            parms[i] = rewardInfo[2];
            i++;
            parms[i] = goodInfo.name;
            strList = string.Format(talkInfo.text, parms).Split("-");
            foreach (var str in strList) taskTalkStrList.Add(str);
        }

        //任务中
        taskingTalkStrList = GetTalkText(playerLvInfo.inTaskTalk);
        //普通对话
        talkStrList = GetTalkText(playerLvInfo.ordinaryTask);
        //接受了任务对话
        taskAcceptTalkList = GetTalkText(playerLvInfo.choiceInfoList[0].answer);
        //拒绝任务对话
        taskRefusetTalkList = GetTalkText(playerLvInfo.choiceInfoList[1].answer);
    }

    //处理基础选项
    private void InitBaseOperator()
    {
        //任务
        if (isTask && !isTaskComplete)
        {
            operatorList.Add(new BaseOperator("任务", (int) OperatorType.Task));
            foreach (var choiceInfo in playerLvInfo.choiceInfoList)
            {
                taskOperatorList.Add(new BaseOperator(choiceInfo.title, choiceInfo.choice));
            }
        }

        operatorList.Add(new BaseOperator(npcCfgInfo.name, (int) OperatorType.NpcLayer));
        Debug.Log(operatorList[0].title);
        Debug.Log(operatorList[1].title);
    }

    private List<string> GetTalkText(int id)
    {
        var textList = new List<string>();
        var talkInfo = TalkConfig.talkInfoList.Find(obj => obj.talk == id);
        var strList = talkInfo.text.Split("-");
        foreach (var str in strList)
        {
            textList.Add(str);
        }

        return textList;
    }

    private void TouchClick(BaseEventData eventData = null)
    {
        if (isCanTalk) Talk();
    }

    private void Talk()
    {
        //触发任务
        if (isPlayTaskTalk)
        {
            PlayTaskTalk();
            return;
        }

        //接受了任务
        if (isTriggerTaskSel)
        {
            PlayTaskSelTalk();
            return;
        }

        //有任务
        if (isTask && isTasking && !isTaskComplete)
        {
            //接了任务说的话
            PlayTaskingTalk();
        }
        else
        {
            PlayOrdinaryTalk();
        }
    }

    private void SetText(string text)
    {
        textTween.Kill();
        talkText.text = "";
        textTween = talkText.DOText(text, text.Length * 0.15f);
    }

    //创建选项
    private void createOption(List<BaseOperator> operatorList, Action<int> callback)
    {
        Ui.Instance.RemoveAllChildren(operatorNode);
        foreach (var operatorInfo in operatorList)
        {
            var option = Instantiate(operatorRef);
            option.transform.SetParent(operatorNode);
            option.GetComponent<OptionScript>().InitData(operatorInfo, callback);
        }
    }

    //播放普通对话
    private void PlayOrdinaryTalk()
    {
        if (idx >= talkStrList.Count)
        {
            isCanTalk = false;
            createOption(operatorList, id =>
            {
                Ui.Instance.RemoveAllChildren(operatorNode);
                isCanTalk = true;
                //任务对话
                if (id == (int) OperatorType.Task)
                {
                    idx = 0;
                    isPlayTaskTalk = true;
                    PlayTaskTalk();
                }
                else if (id == (int) OperatorType.NpcLayer)
                {
                    //打开NpcLayer
                    OpenNpcLayer();
                }
            });
        }

        var text = talkStrList[idx];
        SetText(text);
        idx += 1;
    }

    //播放任务中对话
    private void PlayTaskingTalk()
    {
        if (idx >= taskingTalkStrList.Count)
        {
            isCanTalk = false;
            createOption(operatorList, id =>
            {
                Ui.Instance.RemoveAllChildren(operatorNode);
                isCanTalk = true;
                //打开NpcLayer
                if (id == (int) OperatorType.NpcLayer)
                {
                    OpenNpcLayer();
                }
            });
            return;
        }

        string str = taskingTalkStrList[idx];
        SetText(str);
        idx++;
    }

    //播放任务对话
    private void PlayTaskTalk()
    {
        //任务
        if (idx >= taskTalkStrList.Count)
        {
            isPlayTaskTalk = false;
            isCanTalk = false;
            createOption(taskOperatorList, id =>
            {
                Ui.Instance.RemoveAllChildren(operatorNode);
                isCanTalk = true;
                //接受任务
                if (id == 1)
                {
                    PlayerTaskData.AddTask(npcId, playerLvInfo);
                    string str = string.Format("接受任务{0}", taskInfo.name);
                    Ui.Instance.ShowFlutterView(str);
                    isTasking = true;
                }

                idx = 0;
                isTriggerTaskSel = true;

                PlayTaskSelTalk();
            });
        }
        else
        {
            var text = taskTalkStrList[idx];
            SetText(text);
            idx++;
        }
    }

    //任务选项选择对话
    private void PlayTaskSelTalk()
    {
        var length = isTasking ? taskAcceptTalkList.Count : taskRefusetTalkList.Count;
        if (idx >= length)
        {
            CloseClick();
            return;
        }

        string text;
        if (isTasking)
        {
            text = taskAcceptTalkList[idx];
        }
        else
        {
            text = taskRefusetTalkList[idx];
        }

        SetText(text);
        idx++;
    }

    private void OpenNpcLayer()
    {
        //UIManager.Instance.OpenLayer(TaskLayer);
    }

    private void CloseClick()
    {
        UIManager.Instance.CloseLayer(gameObject.name);
    }

    public override void onExit()
    {
        isCanTalk = true;
        //触发任务选择对话
        isTriggerTaskSel = false;
        //是否接取任务
        isTasking = false;
        //是否完成任务
        isTaskComplete = false;
        idx = 0;
        isPlayTaskTalk = false;
        taskTalkStrList = new List<string>();
        taskingTalkStrList = new List<string>();
        talkStrList = new List<string>();
        operatorList = new List<BaseOperator>();
        taskOperatorList = new List<BaseOperator>();
        taskAcceptTalkList = new List<string>();
        taskRefusetTalkList = new List<string>();
    }
}