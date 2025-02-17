using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class ReadTable 
{
    [MenuItem("Tools/Run Table Import")]
    public static void RunTableImport()
    {
        //string path = Application.dataPath + "/Editor/MapConfig.xlsx";
        //FileInfo fileInfo = new FileInfo(path);

        //// 尝试加载现有的 CharaterMapData 资产
        //CharaterMapData charaterMapData = AssetDatabase.LoadAssetAtPath<CharaterMapData>("Assets/NumericValue/ScripttableObject/MapConfig.asset");

        //// 如果资产不存在，则创建新的 CharaterMapData
        //if (charaterMapData == null)
        //{
        //    charaterMapData = (CharaterMapData)ScriptableObject.CreateInstance<CharaterMapData>();
        //    AssetDatabase.CreateAsset(charaterMapData, "Assets/NumericValue/ScripttableObject/MapConfig.asset");
        //}

        //// 清空原有数据，避免重复数据
        //charaterMapData.mapList.Clear();

        //// 使用 EPPlus 读取 Excel 文件
        //using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        //{
        //    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["MapConfig"];
        //    if (worksheet == null)
        //    {
        //        Debug.LogError("没有找到名为 'MapConfig' 的工作表！");
        //        return;
        //    }

        //    // 读取 Excel 数据并更新 CharaterMapData
        //    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
        //    {
        //        MapInfo mapInfo = new MapInfo();
        //        mapInfo.mapName = worksheet.GetValue(i, 1).ToString();
        //        mapInfo.Id = 2;
        //        mapInfo.img = worksheet.GetValue(i, 3).ToString();

        //        var pos = worksheet.GetValue(i, 4).ToString().Split(',');
        //        Debug.Log(pos);
        //        float x = float.Parse(pos[0]);
        //        float y = float.Parse(pos[1]);
        //        mapInfo.pos = new Vector2(x, y);

        //        // 将更新后的 MapInfo 添加到 mapList 中
        //        charaterMapData.mapList.Add(mapInfo);
        //    }
        //}

        //// 保存更新后的数据到资产文件
        //EditorUtility.SetDirty(charaterMapData);
        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();

        //Debug.Log("数据已更新并保存！");
    }
}
