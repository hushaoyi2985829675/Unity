using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapLayer : PanelBase
{
    public HorizontalView scrollView;
    public MapConfig config;
    private Player player;
    private MapData curData;
    public override void onEnter(params object[] data)
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        scrollView.AddRefreshEvent(RefreshItem);
        scrollView.SetNum(config.data.Count);
    }

    public void RefreshItem(int i,GameObject item)
    { 
        var data = config.data.Find(data => data.Id == i + 1);
        if (data != null)
        { 
            var btn = item.transform.Find("Button").GetComponent<Button>();
            btn.GetComponent<Image>().sprite = data.Sprite;
            item.transform.Find("Text").GetComponent<Text>().text = data.Name;
            //点击地图
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                curData = data;
                UIManager.Instance.CloseLayer();
                UIManager.Instance.LoadScene("FightScene",InitMap);
            });
        }
    }

    IEnumerator InitMap(Slider slider)
    {
        //加入对应地图到场景中
        if (curData.MapLayer == null)
        {
            Debug.Log("地图空");
        }
        else
        {
            UIManager.Instance.AddMap(curData.MapLayer,curData.PlayerPosition);
        }
        slider.value = 90;
        yield return null;
    }
    
    public override void onExit()
    {
        
    }
}
