using System.Collections;
using System.Collections.Generic;
using Equip;
using Foundry;
using UnityEngine;
using UnityEngine.UI;

public class foundryNode : PanelBase
{
    public GridView gridView;
    public GameObject materialCardNode;
    public Transform materialNode;
    public Transform UINode;
    public Text nameText;
    public Text desText;
    public CardNode cardNode;
    public Button button;
    private List<FoundryInfo> FoundryList;
    private List<Equip.EquipInfo> equipInfoList;
    private CardNode oldCardNode;
    private int selId;

    public override void onEnter(params object[] data)
    {
        selId = 0;
        FoundryList = Resources.Load<FoundryConfig>("Configs/Data/FoundryConfig").foundryInfoList;
        equipInfoList = Resources.Load<EquipConfig>("Configs/Data/EquipConfig").equipInfoList;
        gridView.AddRefreshEvent(CreateItem);
        button.onClick.AddListener(() => { gridView.SetItemNumAndSpace(35, 4, 30, 30); });
        refreshUI();
    }

    private void refreshUI()
    {
        gridView.SetItemNumAndSpace(40, 4, 30, 30);
        // if (selId == 0)
        // {
        //     UINode.gameObject.SetActive(false);
        // }
    }

    private void CreateItem(int index, GameObject item)
    {
        // FoundryInfo foundryInfo = FoundryList[index];
        // // Equip.EquipInfo equip = equipInfoList.Find((obj) => foundryInfo.foundry == obj.equip);
        // CardNode cardNode = item.GetComponent<CardNode>();
        // cardNode.SetCardData(GoodsType.Equip, foundryInfo.foundry);
        // cardNode.SetSelState(selId == foundryInfo.foundry);
        // cardNode.SetClick(() =>
        // {
        //     if (selId == foundryInfo.foundry)
        //     {
        //         return;
        //     }
        //
        //     selId = foundryInfo.foundry;
        //     cardNode.SetSelState(true);
        //     if (oldCardNode != null)
        //     {
        //         oldCardNode.SetSelState(false);
        //     }
        //
        //     oldCardNode = cardNode;
        //     RefreshFoundry();
        // });
    }

    private void RefreshFoundry()
    {
        UINode.gameObject.SetActive(true);
        //刷新名字
        string name = Ui.Instance.GetGoodName((int) GoodsType.Equip, selId);
        nameText.text = name;
        string desc = Ui.Instance.GetGoodDes((int) GoodsType.Equip, selId);
        desText.text = desc;
        cardNode.SetCardData(GoodsType.Equip, selId, 1);
        //刷新材料
        CreateMaterial();
    }

    public void CreateMaterial()
    {
        Ui.Instance.RemoveAllChildren(materialNode);
        Equip.EquipInfo equip = Ui.Instance.GetEquipInfo(selId);
        List<ResClass> resList = Ui.Instance.FormatStr(equip.synthesisRoute);
        for (int i = 0; i < resList.Count; i++)
        {
            ResClass res = resList[i];
            GameObject cardNode = Instantiate(materialCardNode, materialNode);
            float x = -(resList.Count - 1) * 150 / 2 + i * 150;
            cardNode.transform.localPosition = new Vector3(x, 0, 0);
            if (i == resList.Count - 1)
            {
                GameObject horizontalLine = Ui.Instance.GetChild(cardNode.transform, "HorizontalLine");
                horizontalLine.SetActive(false);
            }
        }
    }

    public override void onExit()
    {
    }
}