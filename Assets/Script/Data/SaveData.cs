using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public BagData BagData;
    public PlayerEquipData PlayerEquipData;
    public PlayerValueData PlayerValueData;

    public void SvaeDataClick()
    {
        ToJosn("BagData", BagData);
        ToJosn("PlayerEquipData", PlayerEquipData);
        ToJosn("PlayerValueData", PlayerValueData);
        Debug.Log("�������ݳɹ�");
    }

    public void DeleteDataClick()
    {
        var path = Path.Combine(Application.persistentDataPath, "Data");
        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            File.Delete(file);
        }
        Debug.Log("ɾ�����ݳɹ�");
    }
    void ToJosn<T>(string fileName,T data)
    {
        var json = JsonUtility.ToJson(data);
        File.WriteAllText(Path.Combine(Application.persistentDataPath,"Data", fileName + ".txt"), json);
    }
}
