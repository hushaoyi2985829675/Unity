using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MapNs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapLayer : PanelBase
{
    public TableView tableView;
    private List<MapInfo> mapInfoList;
    private MapInfo curData;
    private int mapId;

    public override void onEnter(params object[] data)
    {
        mapInfoList = Ui.Instance.GetMapInfoList().Values.ToList().Where(mapInfo => mapInfo.scene == (int) SceneType.FightScene).ToList();
        tableView.AddRefreshEvent(RefreshItem);
        tableView.AddScaleEvent(RefreshScaleItem);
    }

    public override void onShow(object[] data)
    {
        tableView.SetNum(mapInfoList.Count);
    }

    public void RefreshItem(int i, GameObject item)
    {
        MapInfo mapInfo = mapInfoList[i];
        MapNode mapNode = item.GetComponent<MapNode>();
        mapNode.RefreshUI(mapInfo, MapClick);
    }

    public void RefreshScaleItem(float scale, GameObject item)
    {
        MapNode mapNode = item.GetComponent<MapNode>();
        mapNode.RefreshScale(scale);
    }

    private void MapClick(int mapId)
    {
        this.mapId = mapId;
        UIManager.Instance.CloseLayer(gameObject);
        UIManager.Instance.LoadScene("FightScene", mapId);
    }
    
    public override void onExit()
    {
        tableView.Clear();
    }
}
