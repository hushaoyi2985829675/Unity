using System.Collections;
using System.Collections.Generic;
using Shop;
using UnityEngine;
using UnityEngine.UI;

public class ShopLayer : PanelBase
{
    [SerializeField] private Transform parent;
    [SerializeField] private Button closeBtn;
    [SerializeField] private ToggleTableView tableVie;
    [Header("页面")] [SerializeField] private ShopNode shopNode;
    [SerializeField] private SellNode sellNode;
    List<ShopInfo> ShopConfig;
    Dictionary<int, List<ShopInfo>> ShopConfigDic;
    private PanelBase panel;

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
        if (!(panel is null))
        {
            UIManager.Instance.CloseUINode(panel.name);
            panel = null;
        }

        object[] data = new object[1];
        if (idx == 0)
        {
            panel = shopNode;
            data[0] = ShopConfigDic[(int) GoodsType.Equip];
        }
        else if (idx == 1)
        {
            panel = sellNode;
            data[0] = ShopConfigDic[(int) GoodsType.Equip];
        }

        OpenLayer(data);
    }

    private void OpenLayer(object[] data)
    {
        UIManager.Instance.AddUINode(panel.gameObject, parent, data);
    }

    public override void onExit()
    {
        // if (panel != null)
        // {
        //     UIManager.Instance.CloseUINode(panel.name);
        //     panel = null;
        // }
    }
}