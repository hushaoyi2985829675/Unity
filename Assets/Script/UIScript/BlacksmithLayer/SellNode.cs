using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Equip;
using Foundry;
using HeroEditor.Common;
using HeroEditor.Common.Enums;
using Ingredient;
using UnityEngine;
using UnityEngine.UI;


public class SellNode : PanelBase
{
    public GridView gridView;
    public GridView materialGridView;
    public Transform tabView;
    public Transform UINode;
    public Text nameText;
    public Text desText;
    public Button button;
    private BagData BagData;
    private Dictionary<int, MaterialInfo> IngredientConfig;
    private Dictionary<int, List<ResClass>> ingredientList;
    private Dictionary<int, List<EquipmentInfo>> EquipClassifyList;
    private List<EquipmentInfo> equipList;
    private Dictionary<int, Equip.EquipInfo> equipInfoList;
    private CardNode oldCardNode;
    private int selId;
    private int selTag;

    public override void onEnter(params object[] data)
    {
        AddListener();
        InitData();
        refreshUI();
    }

    private void AddListener()
    {
        gridView.AddRefreshEvent(CreateItem);
        materialGridView.AddRefreshEvent(CreateIngredientItem);
        button.onClick.AddListener(FoundClick);
    }

    private void InitData()
    {
        selId = 0;
        selTag = 0;
        BagData = Resources.Load<BagData>("GameData/PlayerData/BagData");
        equipInfoList = Resources.Load<EquipConfig>("Configs/Data/EquipConfig").equipInfoList
            .ToDictionary(key => key.equip, value => value);
        IngredientConfig = Resources.Load<IngredientConfig>("Configs/Data/IngredientConfig").materialInfoList
            .ToDictionary(key => key.material, value => value);
        ingredientList = new Dictionary<int, List<ResClass>>();
        EquipClassifyList = new Dictionary<int, List<EquipmentInfo>>();
    }

    private void refreshUI()
    {
        InitTabView();
        if (selId == 0)
        {
            UINode.gameObject.SetActive(false);
        }
    }

    private void InitTabView()
    {
        for (int i = 0; i < tabView.childCount; i++)
        {
            Toggle toggle = tabView.GetChild(i).GetComponent<Toggle>();
            toggle.isOn = selTag == i;
            int n = i;
            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (selTag == n)
                {
                    return;
                }

                ChangeTag(isOn, n);
            });
        }

        ChangeTag(true, selTag);
    }

    private void ChangeTag(bool isOn, int tag)
    {
        if (!isOn)
        {
            return;
        }

        selTag = tag;
        if (!EquipClassifyList.ContainsKey(tag))
        {
            switch ((EquipClassif) selTag)
            {
                case EquipClassif.Weapon:
                    EquipClassifyList[tag] = BagData.WeaponList;
                    break;
                case EquipClassif.Armor:
                    EquipClassifyList[tag] = BagData.ArmorList;
                    break;
                case EquipClassif.Helmet:
                    EquipClassifyList[tag] = BagData.HelmetList;
                    break;
                default:
                    break;
            }
        }

        equipList = EquipClassifyList.ContainsKey(tag) ? EquipClassifyList[tag] : null;
        if (equipList == null)
        {
            gridView.SetItemAndRefresh(0);
        }
        else
        {
            gridView.SetItemAndRefresh(equipList.Count);
        }
    }

    private void CreateItem(int index, GameObject item)
    {
        EquipmentInfo equipInfo = equipList[index];
        // Equip.EquipInfo equip = equipInfoList.Find((obj) => equipInfo.id == obj.equip);
        CardNode cardNode = item.GetComponent<CardNode>();
        cardNode.SetCardData(GoodsType.Equip, equipInfo.id);
        cardNode.SetSelState(selId == equipInfo.id);
        if (selId == equipInfo.id)
        {
            oldCardNode = cardNode;
        }

        cardNode.SetClick(() =>
        {
            if (selId == equipInfo.id)
            {
                return;
            }

            selId = equipInfo.id;
            cardNode.SetSelState(true);
            if (oldCardNode != null)
            {
                oldCardNode.SetSelState(false);
            }

            oldCardNode = cardNode;
            RefreshText();
            refreshIngredient();
        });
    }

    private void CreateIngredientItem(int index, GameObject item)
    {
        CardNode cardNode = item.GetComponent<CardNode>();
        cardNode.SetSelState(true);
        List<ResClass> ingredient = ingredientList[selId];
        cardNode.SetCardData(GoodsType.Ingredient, ingredient[index].resourceId, ingredient[index].num);
    }

    private void RefreshText()
    {
        UINode.gameObject.SetActive(true);
        //刷新名字
        string name = Ui.Instance.GetGoodName((int) GoodsType.Equip, selId);
        nameText.text = name;
        string desc = Ui.Instance.GetGoodDes((int) GoodsType.Equip, selId);
        desText.text = desc;
    }

    private void refreshIngredient()
    {
        InitIngredient();
        materialGridView.SetItemAndRefresh(ingredientList[selId].Count);
    }

    private void InitIngredient()
    {
        if (!ingredientList.ContainsKey(selId))
        {
            ingredientList[selId] = new List<ResClass>();
            Equip.EquipInfo equipInfo = Ui.Instance.GetEquipInfo(selId);
            List<ResClass> resList = Ui.Instance.FormatStr(equipInfo.synthesisRoute);
            foreach (var res in resList)
            {
                ingredientList[selId].Add(res);
            }
        }
    }

    public void FoundClick()
    {
        if (selId == 0)
        {
            return;
        }

        Equip.EquipInfo equipInfo = Ui.Instance.GetEquipInfo(selId);
        GameDataManager.Instance.AddEquip(selId);
        Ui.Instance.ShowFlutterView("获得" + equipInfo.name);
    }

    public override void onExit()
    {
    }
}