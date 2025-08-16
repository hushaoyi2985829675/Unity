using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TaskLayer : PanelBase
{
    public Button closeBtn;
    public TableView tabView;
    public PlayerTaskData PlayerTaskData;
    public override void onEnter(params object[] data)
    {
        closeBtn.onClick.AddListener(CloseClick);
        tabView.SetNum(20);
    }

    private void CloseClick()
    {
        UIManager.Instance.CloseLayer(gameObject.name);
    }

    public override void onExit()
    {
        
    }
}
