using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public BagData BagData;
    public PlayerEquipData PlayerEquipData;
    public PlayerValueData PlayerValueData;
    public PlayerTaskData PlayerTaskData;

    void Start()
    {
        SvaeDataClick();
    }

    public void SvaeDataClick()
    {
        ToJosn("BagData", BagData);
        ToJosn("PlayerEquipData", PlayerEquipData);
        ToJosn("PlayerValueData", PlayerValueData);
        ToJosn("PlayerTaskData", PlayerTaskData);
        Debug.Log(Application.persistentDataPath);
        Debug.Log("保存数据成功");
    }

    public void DeleteDataClick()
    {
        var path = Path.Combine(Application.persistentDataPath, "Data");
        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            File.Delete(file);
        }
        Debug.Log("删除数据成功");
    }
    void ToJosn<T>(string fileName,T data)
    {
        string path = Path.Combine(Application.persistentDataPath, "Data", fileName + ".txt");
        var json = JsonUtility.ToJson(data);
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(path, json);
    }
}
