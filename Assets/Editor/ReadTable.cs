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

        //// ���Լ������е� CharaterMapData �ʲ�
        //CharaterMapData charaterMapData = AssetDatabase.LoadAssetAtPath<CharaterMapData>("Assets/NumericValue/ScripttableObject/MapConfig.asset");

        //// ����ʲ������ڣ��򴴽��µ� CharaterMapData
        //if (charaterMapData == null)
        //{
        //    charaterMapData = (CharaterMapData)ScriptableObject.CreateInstance<CharaterMapData>();
        //    AssetDatabase.CreateAsset(charaterMapData, "Assets/NumericValue/ScripttableObject/MapConfig.asset");
        //}

        //// ���ԭ�����ݣ������ظ�����
        //charaterMapData.mapList.Clear();

        //// ʹ�� EPPlus ��ȡ Excel �ļ�
        //using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        //{
        //    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["MapConfig"];
        //    if (worksheet == null)
        //    {
        //        Debug.LogError("û���ҵ���Ϊ 'MapConfig' �Ĺ�����");
        //        return;
        //    }

        //    // ��ȡ Excel ���ݲ����� CharaterMapData
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

        //        // �����º�� MapInfo ��ӵ� mapList ��
        //        charaterMapData.mapList.Add(mapInfo);
        //    }
        //}

        //// ������º�����ݵ��ʲ��ļ�
        //EditorUtility.SetDirty(charaterMapData);
        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();

        //Debug.Log("�����Ѹ��²����棡");
    }
}
