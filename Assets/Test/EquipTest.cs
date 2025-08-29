using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTest : MonoBehaviour
{
    public EquipData data;
    private GameObject item;
    void Start()
    {
        GameObject item2 = Resources.Load<GameObject>("GameObjectRef/EquipItemRef");
        item = Instantiate(item2);
        item.transform.localPosition = transform.localPosition;
        //  item.GetComponent<EquipItemScript>().InitData(data.Id,data.Part);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
