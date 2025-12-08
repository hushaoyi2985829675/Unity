using System;
using System.Collections;
using System.Collections.Generic;
using MapNs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapNode : PanelBase
{
    [SerializeField] Text nameText;
    [SerializeField] Image image;
    [SerializeField] Text lockText;
    [SerializeField] private Text bossText;
    [SerializeField] Button button;

    [SerializeField] private GameObject lockNode;

    public override void onEnter(params object[] data)
    {
    }

    public override void onShow(params object[] data)
    {
    }

    public void RefreshUI(MapInfo mapInfo, Action<int> action)
    {
        button.onClick.RemoveAllListeners();
        image.sprite = Ui.Instance.LoadSprite(mapInfo.image);
        nameText.text = mapInfo.name;
        button.onClick.AddListener(() => { action(mapInfo.map); });
        int lv = GameDataManager.Instance.GetPlayerLv();
        Ui.Instance.SetGray(button.image, lv < mapInfo.lockLv);
        bossText.text = string.Format("Boos: ");
        if (lv < mapInfo.lockLv)
        {
            lockText.text = string.Format("解锁等级: {0}", mapInfo.lockLv);
        }

        lockNode.SetActive(lv < mapInfo.lockLv);
    }

    public void RefreshScale(float scale)
    {
        button.transform.localScale = new Vector3(scale, scale, 1);
    }

    public override void onExit()
    {
    }
}