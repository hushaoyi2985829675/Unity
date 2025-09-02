using System.Collections;
using System.Collections.Generic;
using Shop;
using UnityEngine;
using UnityEngine.UI;

public class ShopLayer : PanelBase
{
    public Transform parent;
    public Button closeBtn;
    public ToggleTableView tableVie;
    public ShopNode shopNode;
    List<ShopInfo> ShopConfig;
    Dictionary<int, List<ShopInfo>> ShopConfigDic;

    public override void onEnter(params object[] data)
    {
        ShopConfig = Resources.Load<ShopConfig>("Configs/Data/ShopConfig").shopInfoList;
        Init();
        tableVie.AddChangeEvent(TabChange);
        tableVie.InitData(0);
    }

    void Init()
    {
        ShopConfigDic = new Dictionary<int, List<ShopInfo>>();
        foreach (ShopInfo info in ShopConfig)
        {
            if (!ShopConfigDic.ContainsKey(info.goodType))
            {
                ShopConfigDic.Add(info.goodType, new List<ShopInfo>());
            }

            ShopConfigDic[info.goodType].Add(info);
        }
    }

    private void TabChange(int idx)
    {
        PanelBase panel = shopNode;
        object[] data = new object[1];
        if (idx == 0)
        {
            panel = shopNode;
            data[0] = ShopConfigDic[(int) GoodsType.Equip];
        }

        OpenLayer(panel, data);
    }

    private void OpenLayer(PanelBase panel, object[] data)
    {
        UIManager.Instance.AddUINode(panel.gameObject, parent, data);
    }

    public override void onExit()
    {
    }
}