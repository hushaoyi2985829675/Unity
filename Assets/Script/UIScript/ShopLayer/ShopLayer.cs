using System.Collections;
using System.Collections.Generic;
using Shop;
using UnityEngine;
using UnityEngine.UI;

public class ShopLayer : PanelBase
{
    [SerializeField] private Transform parent;
    [SerializeField] private ToggleTableView tableVie;
    [Header("页面")] [SerializeField] private ShopNode shopNode;
    [SerializeField] private SellNode sellNode;
    List<ShopInfo> ShopConfig;
    Dictionary<int, List<ShopInfo>> ShopConfigDic;
    private PanelBase panel;
    private PanelBase selPanelRef;

    public override void onEnter(params object[] data)
    {
        ShopConfig = Resources.Load<ShopConfig>("Configs/Data/ShopConfig").shopInfoList;
        Init();
        tableVie.InitTabView(TabChange);
    }

    public override void onShow(object[] data)
    {
        tableVie.SetSelTag(0);
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
        if (panel != null)
        {
            CloseUILayer(panel.gameObject);
            panel = null;
        }

        object[] data = new object[1];
        if (idx == 0)
        {
            selPanelRef = shopNode;
            data[0] = ShopConfigDic[(int) GoodsType.Equip];
        }
        else if (idx == 1)
        {
            selPanelRef = sellNode;
            data[0] = ShopConfigDic[(int) GoodsType.Equip];
        }

        OpenLayer(data);
    }

    private void OpenLayer(object[] data)
    {
        panel = AddUILayer(selPanelRef.gameObject, parent, data);
    }

    public override void onExit()
    {
      
    }
}