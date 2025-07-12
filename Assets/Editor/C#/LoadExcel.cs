using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using OfficeOpenXml;
using UnityEngine;
using System.Dynamic;
using PlasticPipe.PlasticProtocol.Messages;

class ConfigeBase : ScriptableObject
{
    public dynamic items = new ExpandoObject();
}

class Data
{
    
}

public class LoadExcel
{
    private static string path;
    [MenuItem("Tools/导表")]
    private static void importExcel()
    {
        if (Selection.objects.Length <= 0 || Selection.objects.Length > 1)
        {
            Debug.LogError("请选择一个文件");
            return;
        }

        var file = Selection.objects[0];
        path = AssetDatabase.GetAssetPath(file);
        if(!string.Equals(Path.GetExtension(path),".xlsx"))
        {
            Debug.LogError("请选择一个表");
            return;
        }

        ReadConfig();
    }

    private static void ReadConfig()
    {
        string className = "";
        bool isHead = true;
        FileInfo fileInfo = new FileInfo(path);
        using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];
            for (int k = 1; k <= worksheet.Dimension.Columns; k++)
            {
                Debug.Log(worksheet.Cells[1,k].Value.ToString());
            }
            for (int i = 3; i <= worksheet.Dimension.Rows; i++)
            {
                string headName = "";
                for (int k = 1; k <= worksheet.Dimension.Columns; k++)
                {
                    ConfigeBase dataConfig = ScriptableObject.CreateInstance<ConfigeBase>();
                    string title = worksheet.Cells[1, k].Value.ToString();
                    if (title.StartsWith("#"))
                    {
                        string name = worksheet.Cells[3, k].Value.ToString();
                        if (isHead)
                        {
                            dataConfig.items[name] = new Dictionary<string,dynamic>();
                            isHead = false;
                            headName = name;
                        }
                        else
                        {
                            CreatePath(dataConfig.items[headName],worksheet,headName,i);
                        }
                    }
                    else
                    {
                        
                    }
                }
            }
        }
        
    }
    //递归创建
    public static void CreatePath(Dictionary<string,dynamic> data, ExcelWorksheet worksheet,string headName,int rows)
    {
        dynamic temp = data[headName];
        for (int i = 2; i <= worksheet.Dimension.Columns; i++)
        {
            string name = worksheet.Cells[rows,i].Value.ToString();
            string title = worksheet.Cells[1, i].Value.ToString();
            if (title.StartsWith("#"))
            {
                if (data.ContainsKey(name))
                {
                    temp = data[name];
                }
                else
                {
                    temp[name] = new Dictionary<string,dynamic>();
                    return;
                }
            }
        }

        
    }

}
