using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using UnityEditor.Compilation;
using UnityEngine.Tilemaps;
using Object = System.Object;

class ConfigeBase : ScriptableObject
{
    public Dictionary<string,string> items = new Dictionary<string,string>();
}

public class LoadExcel
{
    private static string path;
    private static string headType;
    private static ExcelWorksheet worksheet;
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
        if (!string.Equals(Path.GetExtension(path), ".xlsx"))
        {
            Debug.LogError("请选择一个表");
            return;
        }
        ReadConfig();
    }
    private static void OnCompilationFinished(object obj)
    {
        // 取消注册，避免重复调用
        CompilationPipeline.compilationFinished -= OnCompilationFinished;
        
        Debug.Log("资源刷新及编译已完成！");
    }
    private static void ReadConfig()
    {
        FileInfo fileInfo = new FileInfo(path);
        using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
           
           // bool isHead = true;
          
                List<StringBuilder> classStrList = new List<StringBuilder>();
                StringBuilder scriptFIle = new StringBuilder();
                headType = "";
                worksheet = excelPackage.Workbook.Worksheets[1];
                //先把第一行循环一遍创建出数据格式
                for (int k = 1; k <= worksheet.Dimension.Columns; k++)
                {
                    StringBuilder sb = Create(worksheet,k);
                    classStrList.Add(sb);
                }
                scriptFIle.AppendLine("using HeroEditor.Common;\nusing HeroEditor.Common.Enums;\nusing System.Collections;\nusing System.Collections.Generic;\nusing UnityEngine;");
                scriptFIle.AppendLine();
                scriptFIle.AppendLine("namespace " + worksheet.Name + "Config");
                scriptFIle.AppendLine("{");
                foreach (var classStr in classStrList )
                {
                    scriptFIle.AppendLine(classStr.ToString());
                }
                
                //创建ScriptTable
                scriptFIle.AppendLine("\tpublic class " + worksheet.Name + " : ScriptableObject");
                scriptFIle.AppendLine("\t{");
                scriptFIle.AppendLine(string.Format("\t\tpublic List<{0}> data = new List<{1}>();",headType,headType));
                scriptFIle.AppendLine("\t}");
                scriptFIle.AppendLine("}");
                string filePath = Application.dataPath + "/Configs/ExcelScript/" + worksheet.Name + "Config.cs";
                // 创建文件并写入内容，若文件已存在则会覆盖
                if (!File.Exists(filePath))
                {
                    FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                    fileStream.Close();
                }
                File.WriteAllText(filePath, scriptFIle.ToString());
                string fullTypeName = string.Format("{0}.{1}, Assembly-CSharp", worksheet.Name + "Config", worksheet.Name);
                Type t = Type.GetType(fullTypeName);
                    
                if (t == null)
                {
                        
                    Debug.LogError($"无法找到类型：{fullTypeName}，请检查生成的C#脚本是否有语法错误或命名空间正确");
                }
                else
                {
                    // 创建并保存ScriptableObject
                    ScriptableObject script = ScriptableObject.CreateInstance(t);
                    string dataPath = @"Assets\Configs\Data\" + worksheet.Name + "Config.asset";
                    AssetDatabase.CreateAsset(script, dataPath);
                        
                    // 填充数据
                    Debug.Log(t);
                    RefreshData(t, script);
                        
                    Debug.Log(worksheet.Name + ".cs已刷新");
                }
               
                AssetDatabase.Refresh();
        }
    }

    private static StringBuilder Create(ExcelWorksheet worksheet, int k)
    {
        string title = worksheet.Cells[1, k].Value.ToString();
        StringBuilder sb = new StringBuilder();
        if (title.StartsWith("#"))
        {
            title = title.Substring(1);
            if (string.IsNullOrEmpty(headType))
            {
                headType = title;
            }
            string classType = char.ToUpper(title[0]) + title.Substring(1);
            sb.AppendLine();
            sb.AppendLine("\t[System.Serializable]");
            sb.AppendLine($"\tpublic class {classType}");
            sb.AppendLine("\t{");
            string type = GetType(worksheet.Cells[2, k].Value.ToString());
            string loweTitle = char.ToLower(title[0]) + title.Substring(1);
            sb.AppendLine($"\t\tpublic {type} {loweTitle};");
            CreateClass(worksheet,k + 1,sb);
            sb.AppendLine("\t}");
        }
        return sb;
    }

    private static void CreateClass(ExcelWorksheet worksheet,int k,StringBuilder sb)
    {
        if (k > worksheet.Dimension.Columns)
        {
            return;
        }
        string title = worksheet.Cells[1, k].Value.ToString();
        if (title.StartsWith("#"))
        {
            title = title.Substring(1);
            title = char.ToLower(title[0]) + title.Substring(1);
            string type = char.ToUpper(title[0]) + title.Substring(1);
            sb.AppendLine($"\t\tpublic {type} {title};");
        }
        else
        {
            string type = GetType(worksheet.Cells[2, k].Value.ToString());
            sb.AppendLine($"\t\tpublic {type} {title};");
            CreateClass(worksheet, k + 1,sb);
        }
    }

    static void RefreshData(Type data,ScriptableObject script)
    {
        FieldInfo fieldInfo = data.GetField("data");
        IList list = (IList)fieldInfo.GetValue(script);
        for (int i = 3; i <= worksheet.Dimension.Rows; i++)
        {
            Type type = Type.GetType(string.Format("{0}.{1}, Assembly-CSharp",worksheet.Name + "Config",headType));
            object obj = Activator.CreateInstance(type);
            Assignment(i,1,type,obj);
            list.Add(obj);
        }
    }

    static void Assignment(int i, int k,Type type,object obj)
    {
        if (k > worksheet.Dimension.Columns)
        {
            return;
        }
        string value = worksheet.Cells[i, k].Value.ToString();
        string valueName = worksheet.Cells[1, k].Value.ToString();
        bool isClass = false;
        bool isType = false;
        if (valueName.StartsWith("#"))
        {
            isClass = true;
            valueName = valueName.Substring(1);
        }
        valueName = char.ToLower(valueName[0]) + valueName.Substring(1);
        FieldInfo fieldInfo = type.GetField(valueName);
        if (fieldInfo == null)
        {
            Debug.Log(valueName + "在表" + type.Name + "中不存在");
            return;
        }
        var str = char.ToUpper(valueName[0]) + valueName.Substring(1);
        if (isClass && !type.Name.Equals(str))
        {
            Type fieldType = fieldInfo.FieldType;
            object fieldTypeInstance = Activator.CreateInstance(fieldType);
            fieldInfo.SetValue(obj,fieldTypeInstance);
            Assignment(i,k,fieldType,fieldTypeInstance);
        }
        else
        {
            object valueObj = GetValue(k,value);
            fieldInfo.SetValue(obj,valueObj);
            Assignment(i,k + 1,type,obj); 
        }
    }

    static string GetType(string str)
    {
        switch (str)
        {
            case "int":
                return "int";
            case "float":
                return "float";
            case "bool":
                return "bool";
        }
        return "string"; 
    }

    static object GetValue(int k,string value)
    {
        string valueName = worksheet.Cells[2, k].Value.ToString();
        switch (valueName)
        {
            case "int":
                return int.Parse(value);
            case "float":
                return float.Parse(value);
            case "bool":
                if (value.Equals("false") || value.Equals("False"))
                {
                    return false;
                }
                return true;
        }
        return value; 
    }
}

