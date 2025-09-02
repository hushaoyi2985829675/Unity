using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Resource;
using UnityEngine;

[System.Serializable]
public class ResBase
{
    public int id;
    public int num;

    public ResBase(int id, int num)
    {
        this.id = id;
        this.num = num;
    }
}

[CreateAssetMenu(fileName = "PlayerResData", menuName = "GameData/PlayerResData")]
public class PlayerResData : ScriptableBase
{
    public List<ResBase> ResDataList = new List<ResBase>()
    {
        new ResBase(1, 0),
        new ResBase(2, 0),
    };

    public void AddResNum(int id, int num)
    {
        ResBase resBase = ResDataList.Find(x => x.id == id);
        if (resBase == null)
        {
            return;
        }

        resBase.num += num;
    }

    public void DecreaseResNum(int id, int num)
    {
        ResBase resBase = ResDataList.Find(x => x.id == id);
        if (resBase == null)
        {
            return;
        }

        resBase.num = Mathf.Max(resBase.num - num, 0);
    }

    public int GetResNum(int id)
    {
        ResBase resBase = ResDataList.Find(x => x.id == id);
        if (resBase == null)
        {
            return 0;
        }

        return resBase.num;
    }

    public override void Clear()
    {
        ResDataList = new List<ResBase>()
        {
            new ResBase(1, 0),
            new ResBase(2, 0),
        };
    }
}