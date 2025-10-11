using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Map;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapLayer : PanelBase
{
    public TableView tableView;
    private List<MapInfo> mapInfoList;
    private MapInfo curData;
    Dictionary<int, MapLayerInfo> mapLayerInfoList;
    private int mapId;

    public override void onEnter(params object[] data)
    {
        mapInfoList = Ui.Instance.GetMapInfoList().Values.ToList();
        tableView.AddRefreshEvent(RefreshItem);
        tableView.AddScaleEvent(RefreshScaleItem);
        mapLayerInfoList = Ui.Instance.GetMapLayerInfoList();
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
        CameraManager.Instance.ChangeMapAction(() => { UIManager.Instance.LoadScene("FightScene", InitMap); });
    }

    void InitMap(Slider slider)
    {
        string name = Ui.Instance.GetMapInfo(this.mapId).name;
        UIManager.Instance.AddMap(mapLayerInfoList[mapId].mapLayer, mapInfoList[mapId].playerPos, name);
        slider.value = 90;
    }

    public override void onExit()
    {
        tableView.Clear();
    }
}
