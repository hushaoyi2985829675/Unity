using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadData : MonoBehaviour
{
    public BagData BagData;
    public PlayerEquipData PlayerEquipData;
    public PlayerValueData PlayerValueData;
    void Start()
    {
        UIManager.Instance.LoadScene("MainScene",loadData);
    }

    IEnumerator loadData(Slider slider)
    {
        var num = 50 / 3;
        LoadPlayerData("BagData",BagData);
        slider.value += num;
        LoadPlayerData("PlayerEquipData", PlayerEquipData);
        slider.value += num;
        LoadPlayerData("PlayerValueData", PlayerValueData);
        slider.value += num;
        yield return null;
    }

    void LoadPlayerData<T>(string FileName,T data)where T : ScriptableBase
    {
        var path = Path.Combine(Application.persistentDataPath, "Data" ,FileName + ".txt");
        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }
        data.Clear();
        var json = File.ReadAllText(path);
        JsonUtility.FromJsonOverwrite(json, data);
    }
}
