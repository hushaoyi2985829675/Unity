using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PanelBase : MonoBehaviour
{
    public abstract void onEnter(params object[] data);

    public abstract void onShow(params object[] data);
    public abstract void onExit();
}
