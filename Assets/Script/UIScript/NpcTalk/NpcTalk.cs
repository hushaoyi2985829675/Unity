using System;
using System.Collections.Generic;
using DG.Tweening;
using GoodsNs;
using MapNs;
using NpcNs;
using NpcTalkTaskNs;
using OptionNs;
using TalkNs;
using TaskNs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NpcTalkInfo = NpcTalkTaskNs.NpcIdInfo;
using NpcCfgInfo = NpcNs.NpcIdInfo;
using ConfigTaskInfo = TaskNs.TaskInfo;

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

    public BaseOperator(string title, int id, bool isTask = false)
    {
        this.title = title;
        this.id = id;
    }
}

public class NpcTalk : PanelBase
{
    public EventTrigger trigger;
    private List<TaskInfo> PlayerTaskList;
    private GoodsConfig GoodsConfig;
    [SerializeField] private Text talkText;
    [SerializeField] private GameObject touchNode;

    private int playerLv;
    //是否接取任务
    public bool isTasking;

    //是否领取任务
    public bool isTaskReceive;
    //是否完成任务
    public bool isTaskComplete;

    //是否可以领取
    public bool isCanTaskReceive;
    public GameObject operatorRef;
    private Transform operatorNode;
    private List<BaseOperator> operatorList;

    private List<BaseOperator> taskOperatorList;
    private List<string> taskTalkStrList;
    private int idx;
    private bool isCanTalk = true;
    private bool isPlayTaskTalk;

    private bool isTask;

    //触发任务选择对话
    private bool isTriggerTaskSel;
    private NpcCfgInfo npcCfgInfo;
    private NpcConfig NpcConfig;
    private int npcId = 1;
    private NpcTalkInfo npcInfo;

    //配置
    private NpcTalkTaskConfig NpcTalkTaskConfig;
    private OptionConfig OptionConfig;
    private PlayerTaskLvInfo PlayerTaskLvInfo;
    private TalkConfig TalkConfig;
    private List<string> talkStrList = new();
    private List<string> taskAcceptTalkList = new();
    private List<string> taskRefusetTalkList = new();
    private List<string> taskCompletionTalkList = new();
    private List<string> taskingTalkStrList = new();
    private TaskConfig TaskConfig;
    private ConfigTaskInfo taskInfo;
    //任务页面
    private GameObject TaskLayer;
    private GameObject NpcLayer;
    private Tween textTween;
    
    public override void onEnter(params object[] data)
    {
        NpcLayer = Ui.Instance.GetLayerRef("TalkLayer/TalkLayer");
        playerLv = GameDataManager.Instance.GetPlayerLv();
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(TouchClick);
        trigger.triggers.Add(entry);
        isTriggerTaskSel = false;
        operatorNode = transform.Find("OptionNode").transform;
        NpcTalkTaskConfig = Resources.Load<NpcTalkTaskConfig>("Configs/Data/NpcTalkTaskConfig");
        TaskConfig = Resources.Load<TaskConfig>("Configs/Data/TaskConfig");
        TalkConfig = Resources.Load<TalkConfig>("Configs/Data/TalkConfig");
        NpcConfig = Resources.Load<NpcConfig>("Configs/Data/NpcConfig");
        OptionConfig = Resources.Load<OptionConfig>("Configs/Data/OptionConfig");
        TaskLayer = Resources.Load<GameObject>("Ref/LayerRef/UIRef/TaskLayer/TaskLayer");
        PlayerTaskList = GameDataManager.Instance.GetPlayerTaskData();
    }

    public override void onShow(object[] data)
    {
        playerLv = GameDataManager.Instance.GetPlayerLv();
        npcId = (int) data[0];
        InitData();
        InitTaskState();
        InitTalkList();
        InitBaseOperator();
        Talk();
        TouchNodeAni();
    }

    private void TouchNodeAni()
    {
        touchNode.transform.localPosition = new Vector3(837, 25, 0);
        touchNode.SetActive(true);
        touchNode.transform.DOLocalMoveY(-25f, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    private void InitData()
    {
        isCanTalk = true;
        //触发任务选择对话
        isTriggerTaskSel = false;
        //是否接取任务
        isTasking = false;
        //是否完成任务
        isTaskReceive = false;
        idx = 0;
        isPlayTaskTalk = false;
        isCanTaskReceive = false;
        taskTalkStrList = new List<string>();
        taskingTalkStrList = new List<string>();
        talkStrList = new List<string>();
        operatorList = new List<BaseOperator>();
        taskOperatorList = new List<BaseOperator>();
        taskAcceptTalkList = new List<string>();
        taskRefusetTalkList = new List<string>();
        npcInfo = NpcTalkTaskConfig.npcIdInfoList.Find(obj => obj.npcId == npcId);
        npcCfgInfo = NpcConfig.npcIdInfoList.Find(obj => obj.npcId == npcId);
        PlayerTaskLvInfo = npcInfo.playerTaskLvInfoList.Find(obj => obj.playerTaskLv == playerLv);
        taskInfo = TaskConfig.taskInfoList.Find(obj => obj.task == PlayerTaskLvInfo.taskId);
    }

    private void InitTaskState()
    {
        if (PlayerTaskLvInfo.taskId != 0)
        {
            isTask = true;
            TaskInfo taskData = PlayerTaskList.Find(taskInfo => taskInfo.taskId == PlayerTaskLvInfo.taskId);
            if (taskData != null)
            {
                isTaskReceive = taskData.isComplete;
                if (!isTaskReceive)
                {
                    isTasking = true;
                }

                isTaskComplete = taskData.curNum >= taskData.needNum;
                isCanTaskReceive = !isTaskReceive && isTaskComplete;
            }
        }
    }

    private void InitTalkList()
    {
        //处理任务对话
        if (isTask && !isTaskReceive)
        {
            List<string> parmsList = new List<string>();
            string beforeString = Ui.Instance.GetTalkText(PlayerTaskLvInfo.beforeTaskCompletion);
            if (PlayerTaskLvInfo.taskLocation != 0)
            {
                MapInfo mapData = Ui.Instance.GetMapInfo(PlayerTaskLvInfo.taskLocation);
                parmsList.Add(mapData.name);
            }

            //目标名字
            string targetName = Ui.Instance.GetTaskTargetName(taskInfo.taskType, taskInfo.targetType);
            parmsList.Add(targetName);
            //任务数量
            parmsList.Add(PlayerTaskLvInfo.taskRequirement.ToString());
            //奖励数量
            List<ResClass> resList = Ui.Instance.FormatResStr(PlayerTaskLvInfo.taskReward);
            foreach (ResClass res in resList)
            {
                string goodName = Ui.Instance.GetGoodName((int) res.goodsType, res.resourceId);
                parmsList.Add(res.num.ToString());
                parmsList.Add(goodName);
            }

            beforeString = string.Format(beforeString, parmsList.ToArray());
            foreach (string str in beforeString.Split("-"))
            {
                taskTalkStrList.Add(str);
            }

            //领取任务奖励字符
            parmsList = new List<string>();
            string completionStr = Ui.Instance.GetTalkText(PlayerTaskLvInfo.taskCompletion);
            foreach (ResClass res in resList)
            {
                string goodName = Ui.Instance.GetGoodName((int) res.goodsType, res.resourceId);
                parmsList.Add(res.num.ToString());
                parmsList.Add(goodName);
            }

            completionStr = string.Format(completionStr, parmsList.ToArray());
            foreach (string str in completionStr.Split("-"))
            {
                taskCompletionTalkList.Add(str);
            }

            //任务中
            taskingTalkStrList = GetTalkText(PlayerTaskLvInfo.inTaskTalk);
            //接受了任务对话
            taskAcceptTalkList = GetTalkText(PlayerTaskLvInfo.choiceInfoList[0].answer);
            //拒绝任务对话
            taskRefusetTalkList = GetTalkText(PlayerTaskLvInfo.choiceInfoList[1].answer);
        }

        //普通对话
        talkStrList = GetTalkText(PlayerTaskLvInfo.ordinaryTask);
    }

    //处理基础选项
    private void InitBaseOperator()
    {
        //任务
        if (isTask && !isTaskReceive)
        {
            operatorList.Add(new BaseOperator("任务", (int) OperatorType.Task));
            foreach (var choiceInfo in PlayerTaskLvInfo.choiceInfoList)
            {
                taskOperatorList.Add(new BaseOperator(choiceInfo.title, choiceInfo.choice));
            }
        }

        operatorList.Add(new BaseOperator(npcCfgInfo.name, (int) OperatorType.NpcLayer));
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
        //领取任务对话
        if (isCanTaskReceive)
        {
            PlayCompletionTalk();
            return;
        }

        //任务描述对话
        if (isPlayTaskTalk)
        {
            PlayTaskTalk();
            return;
        }

        //接受或拒绝任务对话
        if (isTriggerTaskSel)
        {
            PlayTaskSelTalk();
            return;
        }

        //在任务中
        if (isTasking)
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
        textTween = talkText.DOText(text, text.Length * 0.1f).SetEase(Ease.Linear);
    }

    //创建选项
    private void createOption(List<BaseOperator> operatorList, Action<int> callback)
    {
        for (int i = 0; i < operatorList.Count; i++)
        {
            BaseOperator operatorInfo = operatorList[i];
            OptionScript option = AddUINode<OptionScript>(operatorRef, operatorNode,
                new object[] {operatorInfo, callback, isCanTaskReceive});
            
        }
    }

    //播放普通对话
    private void PlayOrdinaryTalk()
    {
        if (idx >= talkStrList.Count)
        {
            isCanTalk = false;
            touchNode.SetActive(false);
            createOption(operatorList, id =>
            {
                isCanTalk = true;
                touchNode.SetActive(true);
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

                CloseNodeAllUINode(operatorNode);
            });
            return;
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
            touchNode.SetActive(false);
            createOption(operatorList, id =>
            {
                isCanTalk = true;
                touchNode.SetActive(true);
                //打开NpcLayer
                if (id == (int) OperatorType.NpcLayer)
                {
                    OpenNpcLayer();
                }
                else if (id == (int) OperatorType.Task)
                {
                    if (isCanTaskReceive)
                    {
                        //领取任务
                        CloseNodeAllUINode(operatorNode);
                        PlayCompletionTalk();
                    }
                    else
                    {
                        OpenTaskLayer();
                        CloseClick();
                    }
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
            touchNode.SetActive(false);
            createOption(taskOperatorList, id =>
            {
                CloseNodeAllUINode(operatorNode);
                isCanTalk = true;
                touchNode.SetActive(true);
                //接受任务
                if (id == 1)
                {
                    //玩家增加任务
                    GameDataManager.Instance.AddTask(npcId, PlayerTaskLvInfo);
                    string str = string.Format("接受任务{0}", taskInfo.name);
                    Ui.Instance.ShowFlutterView(str);
                    isTasking = true;
                }
                else //拒绝任务
                {
                    isTasking = false;
                }

                idx = 0;
                isTriggerTaskSel = true;
                PlayTaskSelTalk();
            });
        }
        else
        {
            string text = taskTalkStrList[idx];
            SetText(text);
            idx++;
        }
    }

    //任务选项选择对话
    private void PlayTaskSelTalk()
    {
        int length = isTasking ? taskAcceptTalkList.Count : taskRefusetTalkList.Count;
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

    //播放完成任务对话
    private void PlayCompletionTalk()
    {
        //任务
        if (idx >= taskCompletionTalkList.Count)
        {
            isCanTaskReceive = false;
            //奖励数量
            List<ResClass> resList = Ui.Instance.FormatResStr(PlayerTaskLvInfo.taskReward);
            foreach (var res in resList)
            {
                //发放奖励
                GameDataManager.Instance.AddGood(res.goodsType, res.resourceId, res.num);
            }

            //标记任务已领取
            GameDataManager.Instance.MarkTaskCompletion(PlayerTaskLvInfo.taskId);
            Ui.Instance.ShowReward(resList);
            CloseClick();
        }
        else
        {
            string text = taskCompletionTalkList[idx];
            SetText(text);
            idx++;
        }
    }

    private void OpenNpcLayer()
    {
        CloseClick();
        UIManager.Instance.OpenLayer(NpcLayer);
    }

    private void OpenTaskLayer()
    {
        UIManager.Instance.OpenLayer(TaskLayer);
    }

    private void CloseClick()
    {
        UIManager.Instance.CloseLayer(gameObject);
    }

    public override void onExit()
    {
        talkText.DOKill();
        touchNode.transform.DOKill();
    }
}