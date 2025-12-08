using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.CommonScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BagAttrNs;
using EquipNs;
using HeroEditor.Common.Enums;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class BagLayer : PanelBase
{
    [Header("属性预制体")] [SerializeField] private GameObject attrTextRef;
    [Header("装备槽预制体")] [SerializeField] private GameObject equipSlotRef;
    [SerializeField] private GameObject selectNode;
    [SerializeField] private RectTransform selectNodeParent;
    [SerializeField] private TabView TabView;
    [SerializeField] private PlayerEquipData PlayerEquipData;
    [SerializeField] private IconCollection IconCollection;
    [SerializeField] private GameObject PlayerRef;
    [Header("装备槽节点")] [SerializeField] private Transform equipSlotNode;
    [Header("属性节点")] [SerializeField] private Transform attrNode;
    [Header("子页面父节点")] [SerializeField] private Transform viewNode;
    [Header("子页面")] [SerializeField] private GameObject bagEquipNode;
    [SerializeField] private GameObject bagGoodNode;
    private BagData BagData;
    Dictionary<int, EquipmentInfo> BagDataInfo;
    private GameObject BagPlayer;
    private List<GameObject> TabViewList;
    private string[] TabNameList;
    private int selTag;
    private GameObject layerRef;
    private PanelBase curLayer;
    private Dictionary<AttrType, AttrTextNode> attrTextDict;
    private Dictionary<EquipmentPart, EquipSlot> equipSlotDict;
    private Dictionary<int, BagAttr> bagAttrDict;

    public override void onEnter(object[] data)
    {
        attrTextDict = new Dictionary<AttrType, AttrTextNode>();
        equipSlotDict = new Dictionary<EquipmentPart, EquipSlot>();
        TabView.AddRefreshEvent(RefreshTabView);
        TabNameList = new string[] {"装备", "道具", "消耗品"};
        TabView.SetNum(TabNameList.Length, selTag);
        bagAttrDict = Resources.Load<BagAttrConfig>("Configs/Data/BagAttrConfig").bagAttrList.ToDictionary(key => key.type, value => value);
    }

    public override void onShow(object[] data)
    {
        curLayer = null;
        selTag = 0;
        attrTextDict.Clear();
        onChangeTitle();
        RefreshPlayer();
        RefreshAttr();
        CreateEquipSlotNode();
    }

    public void CreateEquipSlotNode()
    {
        foreach (EquipType equipType in Enum.GetValues(typeof(EquipType)))
        {
            EquipSlot equipSlot = AddUINode<EquipSlot>(equipSlotRef, equipSlotNode, new object[]
            {
                (EquipmentPart) equipType,
                (Action) RefreshAttr
            });
            equipSlotDict[(EquipmentPart) equipType] = equipSlot;
        }
    }

    //添加选择节点
    public void AddSelectNode(Transform item, GoodsType goodsType, int id, Action callback)
    {
        AddUINode(selectNode, selectNodeParent, new object[] {item, goodsType, id, callback});
    }

    //TabView点击
    public void RefreshTabView(int index, GameObject item)
    {
        var toggle = item.GetComponent<Toggle>();
        toggle.isOn = selTag == index;
        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (selTag == index || !isOn)
            {
                return;
            }

            selTag = index;

            onChangeTitle();
        });
        item.transform.Find("Background/Text").GetComponent<Text>().text = TabNameList[index];
    }

    private void onChangeTitle()
    {
        if (curLayer != null)
        {
            CloseUILayer(curLayer.gameObject);
        }

        layerRef = bagEquipNode;
        if (selTag == 0)
        {
            layerRef = bagEquipNode;
        }
        else if (selTag == 1)
        {
            layerRef = bagGoodNode;
        }

        curLayer = AddLayer(layerRef);
    }

    //添加子页面
    private PanelBase AddLayer(GameObject layerRef)
    {
        Action<GoodsType, int, GameObject, EquipmentPart> callback = ItemCallback;
        return AddUILayer(layerRef, viewNode, new object[] {callback});
    }

    //CardNode回调
    private void ItemCallback(GoodsType goodsType, int id, GameObject item, EquipmentPart part)
    {
        EquipInfo equipInfo = Ui.Instance.GetEquipInfo(id);
        AddSelectNode(item.transform, goodsType, id, () =>
        {
            //点击使用或装备
            if (goodsType == GoodsType.Equip)
            {
                equipSlotDict[(EquipmentPart) equipInfo.part].RefreshEquip();
                RefreshAttr();
            }

            //使用
            if (goodsType == GoodsType.Good)
            {
            }
        });
        //显示属性差异
        ShowAddAttr(equipInfo, part);
    }

    //显示属性差异
    private void ShowAddAttr(EquipInfo equipInfo, EquipmentPart part)
    {
        EquipData equip = GameDataManager.Instance.GetPlayerEquipData(part);
        List<AttrClass> newAttrList = Ui.Instance.FormatAttrStr(equipInfo.attr);
        Dictionary<AttrType, AttrClass> resList = newAttrList.ToDictionary(x => x.attrType, x => x);
        EquipInfo oldEquipInfo = Ui.Instance.GetEquipInfo(equip.id);
        if (oldEquipInfo != null)
        {
            List<AttrClass> oldAttrList = Ui.Instance.FormatAttrStr(oldEquipInfo.attr);

            foreach (AttrClass attrClass in newAttrList)
            {
                AttrClass oldAttrClass = oldAttrList.Find((obj) => obj.attrType == attrClass.attrType);
                if (oldAttrClass == null)
                {
                    resList[attrClass.attrType] = attrClass;
                }
                else
                {
                    resList[attrClass.attrType] =
                        new AttrClass(attrClass.attrType, attrClass.value - oldAttrClass.value);
                    oldAttrList.Remove(oldAttrClass);
                }
            }

            foreach (AttrClass attrClass in oldAttrList)
            {
                resList[attrClass.attrType] = new AttrClass(attrClass.attrType, 0 - attrClass.value);
            }
        }

        foreach (KeyValuePair<int, BagAttr> bagAttr in bagAttrDict)
        {
            AttrType attrType = (AttrType) bagAttr.Value.bagAttr;
            if (resList.ContainsKey(attrType))
            {
                attrTextDict[attrType].ShowAddText(resList[attrType].value);
            }
            else
            {
                //当时基础数值
                attrTextDict[attrType].RefreshUI();
            }
        }
    }

    //刷新属性
    private void RefreshAttr()
    {
        foreach (KeyValuePair<int, BagAttr> bagAttr in bagAttrDict)
        {
            AttrType attrType = (AttrType) bagAttr.Value.bagAttr;
            if (!attrTextDict.ContainsKey(attrType))
            {
                AttrTextNode attrTextNode = AddUINode<AttrTextNode>(attrTextRef, attrNode, new object[] {attrType});
                attrTextDict[attrType] = attrTextNode;
            }

            attrTextDict[attrType].RefreshUI();
        }
    }

    //刷新背包人物
    private void RefreshPlayer()
    {
        if (BagPlayer == null)
        {
            BagPlayer = Instantiate(PlayerRef);
            DontDestroyOnLoad(BagPlayer);
        }

        BagPlayer.SetActive(true);
    }

    public override void onExit()
    {
        BagPlayer.SetActive(false);
    }
}