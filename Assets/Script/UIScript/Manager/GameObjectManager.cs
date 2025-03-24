using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager
{
    private Player player;
    static GameObjectManager _instance;
    public static GameObjectManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObjectManager();
            }

            return _instance;
        }
    }
    //装备对象池
    Stack<GameObject> equipObjStack = new Stack<GameObject>();
    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public  Player GetPlayer()
    {
        return this.player;
    }

    public GameObject GetEquipItemObj()
    {
        GameObject item;
        if (equipObjStack.Count > 0)
        {
            item =  equipObjStack.Pop();
        }
        else
        {
            item = Resources.Load<GameObject>("GameObjectRef/EquipItemRef");
        }
        item.transform.localPosition = new Vector3(0,0,0);
        return item;
    }

    public void RemoveObj(GameObject obj)
    {
        obj.SetActive(false);
        if (obj.CompareTag("Equip"))
        {
            equipObjStack.Push(obj);
        }
        else if (obj.CompareTag("Monster"))
        {
            
        }
    }
}
