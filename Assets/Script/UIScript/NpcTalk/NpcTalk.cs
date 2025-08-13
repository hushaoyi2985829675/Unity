using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Npc;
using UnityEngine;
using NpcTalkTask;
using NUnit.Framework.Constraints;
using Talk;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using NpcTalkInfo = NpcTalkTask.NpcIdInfo;
using NpcCfgInfo = Npc.NpcIdInfo;

public class BaseOperator
{
    public string title;
    public int id;

    public BaseOperator() { }

    public BaseOperator(string title, int id)
    {
        this.title = title;
        this.id = id;
    }
}

public class NpcTalk : PanelBase
{
    public EventTrigger trigger;
    public NpcTalkTaskConfig NpcTalkTaskConfig;
    public PlayerTaskData PlayerTaskData;
    public TalkConfig TalkConfig;
    public NpcConfig  NpcConfig;
    public GameObject TaskLayer;
    public Text talkText;
    public int playerLv = 1;
    //是否接取任务
    public bool isTasking = false;
    //是否完成任务
    public bool isTaskComplete = false;
    private int npcId = 1;
    private NpcCfgInfo npcCfgInfo;
    private NpcTalkInfo npcInfo;
    private PlayerLvInfo playerLvInfo;
    public GameObject operatorRef;
    private int idx = 0;
    bool isPlayTaskTalk = false;
    private bool isTask;
    List<int> tasktalkIdList = new List<int>(); 
    List<int> talkIdList = new List<int>(); 
    List<BaseOperator> operatorList = new List<BaseOperator>();
    List<BaseOperator> taskOperatorList = new List<BaseOperator>();
    public override void onEnter(params object[] data)
    {
        this.npcId = (int)data[0];
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(TouchClick);
        trigger.triggers.Add(entry);
        TaskLayer = Resources.Load<GameObject>("Ref/LayerRef/UIRef/TaskLayer");
        NpcTalkTaskConfig = Resources.Load<NpcTalkTaskConfig>("Configs/Data/NpcTalkTaskConfig");
        TalkConfig = Resources.Load<TalkConfig>("Configs/Data/TalkConfig");
        NpcConfig =  Resources.Load<NpcConfig>("Configs/Data/NpcConfig");
        npcInfo = NpcTalkTaskConfig.npcIdInfoList.Find((obj) => obj.npcId == npcId);
        npcCfgInfo = NpcConfig.npcIdInfoList.Find((obj) => obj.npcId == npcId);
        playerLvInfo = npcInfo.playerLvInfoList.Find((obj)=>obj.playerLv == playerLv);
        InitTaskState();
        InitTalkList();
        InitBaseOperator();
        Talk();
    }

    void InitTaskState()
    {
        if (playerLvInfo.taskId != 0)
        {
            isTask = true;
            TaskInfo taskInfo = PlayerTaskData.TaskList.Find((taskInfo) => taskInfo.taskId == playerLvInfo.taskId);
            if (taskInfo != null)
            {
                isTasking = true;
                isTaskComplete = taskInfo.isComplete;
            }
        }
    }
    public override void onExit()
    {
        
    }
    void InitTalkList()
    {
        string[] idstr;
        //处理任务对话
        if (isTask && !isTasking && !isTaskComplete)
        {
            idstr = playerLvInfo.beforeTaskcompletion.Split(",");
            foreach (string id in idstr)
            {
                tasktalkIdList.Add(int.Parse(id));
            }
        }
       
        idstr = playerLvInfo.ordinaryTaskLst.Split(",");
        foreach (string id in idstr)
        {
            talkIdList.Add(int.Parse(id));
        }
    
    }

    //处理基础选项
    void InitBaseOperator()
    {
        if (isTask && !isTaskComplete)
        {
            operatorList.Add(new BaseOperator("任务", operatorList.Count + 1));
        }
        operatorList.Add(new BaseOperator(npcCfgInfo.name, operatorList.Count + 1));
        //任务
        foreach (var choiceInfo in playerLvInfo.choiceInfoList)
        {
            taskOperatorList.Add(new BaseOperator(choiceInfo.title, choiceInfo.choice));
        }
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    void TouchClick(BaseEventData eventData = null)
    {
        Talk();
    }

    void Talk()
    {
        //有任务
        if (isTask && !isTaskComplete)
        {
            
            if (isPlayTaskTalk) //触发
            {
                PlayTaskTalk();
            }
            else if (isTasking) //任务中
            {
               
            }
            else
            {
                Debug.Log(talkIdList.Count);
                //未接取状态
                if (idx >= talkIdList.Count)
                { 
                    createOption();
                    return;
                }
                int id = talkIdList[idx] - 1;
                Debug.Log(TalkConfig.idInfoList[id].text);
                talkText.DOText(TalkConfig.idInfoList[id].text,TalkConfig.idInfoList[id].text.Length * 0.15f);
                idx += 1;
            }
        }
        else
        {  
            if (idx >= talkIdList.Count)
            {
                createOption();
                return;
            }

            int id = talkIdList[idx] - 1;
            talkText.DOText(TalkConfig.idInfoList[id].text, TalkConfig.idInfoList[id].text.Length * 0.15f);
            idx += 1;
        }
    }

    void OpenTaskLayer()
    {
        UIManager.Instance.OpenLayer(TaskLayer);
    }

    void createOption()
    {
        foreach (var operatorInfo in operatorList)
        {
            GameObject option = Instantiate(operatorRef);
            option.transform.SetParent(GameObject.Find(("OptionNode")).transform);
            option.GetComponent<OptionScript>().InitData(operatorInfo, (id) =>
            {
                if (id == 1)
                {
                    idx = 0;
                    isPlayTaskTalk = true;
                    PlayTaskTalk();
                }
            });
        }
    }

    void PlayTaskTalk()
    {
        //任务
        if (idx >= tasktalkIdList.Count)
        {
            isPlayTaskTalk = false;  
            createTaskOption();
            return;
        }
        int id = tasktalkIdList[idx] - 1;
        talkText.DOText(TalkConfig.idInfoList[id].text,TalkConfig.idInfoList[id].text.Length * 0.15f);
        idx += 1;
    }
    
    void createTaskOption()
    {
        foreach (var operatorInfo in taskOperatorList)
        {
            GameObject option = Instantiate(operatorRef);
            option.transform.SetParent(GameObject.Find(("OptionNode")).transform);
            option.GetComponent<OptionScript>().InitData(operatorInfo, (id) =>
            {
                if (id == 1)
                {
                   //关闭页面打开任务
                   Debug.Log("关闭");
                   OpenTaskLayer();
                }
            });
        }
    }

    
}
