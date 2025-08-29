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
using Unity.VisualScripting;
using UnityEditor.Compilation;
using UnityEngine.Tilemaps;
using Object = System.Object;

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
    private static void ReadConfig()
    {
        FileInfo fileInfo = new FileInfo(path);
        using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            for (int i = 1; i <= excelPackage.Workbook.Worksheets.Count; i++)
            {
                worksheet = excelPackage.Workbook.Worksheets[i];
                StringBuilder scriptFIle = new StringBuilder();
                headType = "";   
                List<StringBuilder> classStrList = new List<StringBuilder>();
                //先把第一行循环一遍创建出数据结构
                for (int k = 1; k <= worksheet.Dimension.Columns; k++)
                {
                     StringBuilder sb = Create(worksheet,k);
                     classStrList.Add(sb);
                }
                scriptFIle.AppendLine("using HeroEditor.Common;\nusing HeroEditor.Common.Enums;\nusing System.Collections;\nusing System.Collections.Generic;\nusing UnityEngine;");
                scriptFIle.AppendLine();
                scriptFIle.AppendLine("namespace " + worksheet.Name);
                scriptFIle.AppendLine("{");
                foreach (var classStr in classStrList )
                {
                    scriptFIle.Append(classStr.ToString());
                }
                scriptFIle.AppendLine();
                //创建ScriptTable
                scriptFIle.AppendLine("\tpublic class " + worksheet.Name + "Config: ScriptableObject");
                scriptFIle.AppendLine("\t{");
                string headName = char.ToLower(headType[0]) + headType.Substring(1) + "List";
                scriptFIle.AppendLine(string.Format("\t\tpublic List<{0}> {1} = new List<{2}>();",headType,headName,headType));
                scriptFIle.AppendLine("\t}");
                scriptFIle.AppendLine("}");
                string filePath = Application.dataPath + "/Resources/Configs/ExcelScript/" + worksheet.Name + "Config.cs";
                // 创建文件并写入内容，若文件已存在则会覆盖
                if (!File.Exists(filePath)) 
                {
                    FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                    fileStream.Close();
                }
                AssetDatabase.Refresh();
                File.WriteAllText(filePath, scriptFIle.ToString());
                string fullTypeName = string.Format("{0}.{1}, Assembly-CSharp", worksheet.Name, worksheet.Name + "Config");
                Type t = Type.GetType(fullTypeName);
                if (t == null)
                {
                    Debug.LogError("创建,重新导入");
                }
                else
                {
                    string dataPath = "Assets/Resources/Configs/Data/" + worksheet.Name + "Config.asset";
                    // 创建并保存ScriptableObject
                    ScriptableObject script = ScriptableObject.CreateInstance(t);
                    AssetDatabase.CreateAsset(script, dataPath);
                    AssetDatabase.Refresh();
                    RefreshData(script);
                    EditorUtility.SetDirty(script);
                    AssetDatabase.SaveAssets();
                }
            }
            
            Debug.Log("导入成功");
            AssetDatabase.Refresh();
        }
    }

    private static StringBuilder Create(ExcelWorksheet worksheet, int k)
    {
        string title = worksheet.Cells[2, k].Value.ToString();
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
            string type = GetType(worksheet.Cells[3, k].Value.ToString());
            string result = getSubInfoString(title);
            string loweTitle = char.ToLower(result[0]) + result.Substring(1);
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
        string title = worksheet.Cells[2, k].Value.ToString();
        if (title.StartsWith("#"))
        {
            title = title.Substring(1);
            title = char.ToLower(title[0]) + title.Substring(1);
            string type = char.ToUpper(title[0]) + title.Substring(1);
            sb.AppendLine($"\t\tpublic List<{type}> {title}List;");
        }
        else
        {
            title = char.ToLower(title[0]) + title.Substring(1);
            string type = GetType(worksheet.Cells[3, k].Value.ToString());
            sb.AppendLine($"\t\tpublic {type} {title};");
            CreateClass(worksheet, k + 1,sb);
        }
    }

    static void RefreshData(ScriptableObject script)
    {
        for (int i = 4; i <= worksheet.Dimension.Rows; i++)
        {
            Assignment(i,1,script);
        }
    }

    static void Assignment(int i, int k,object obj)
    {
        if (k > worksheet.Dimension.Columns)
        {
            return;
        }
        string valueName = worksheet.Cells[2, k].Value.ToString();
        //默认值
        string value = getDefaultValue(k);
        if (worksheet.Cells[i, k].Value != null)
        {
            value = worksheet.Cells[i, k].Value.ToString();
        }
        Type objType = obj.GetType();
        if (valueName.StartsWith("#"))
        {
            valueName = valueName.Substring(1);
            valueName = char.ToLower(valueName[0]) + valueName.Substring(1);
            FieldInfo listInfo = objType.GetField(valueName + "List");
            if (listInfo == null)
            {
                Debug.Log(string.Format("在类{0}没有{1}",objType.Name,valueName + "List"));
            }
            Type listType = listInfo.FieldType;
            MethodInfo findFunc = listType.GetMethod("Find");
            Type elementType = listType.GetGenericArguments()[0];
            Type predicateType = typeof(Predicate<>).MakeGenericType(elementType);
            object listInstance = listInfo.GetValue(obj);
            if (listInstance == null)
            {
                // 创建 List 实例（如 new List<Item>()）
                listInstance = Activator.CreateInstance(listType);
                listInfo.SetValue(obj, listInstance);
            }
            Delegate func = GetDelegate(predicateType,valueName, value);
            object target = findFunc.Invoke(listInstance, new object[]{func});
            if (target == null)
            {
                target = Activator.CreateInstance(elementType);
                
                string result = getSubInfoString(elementType.Name);
                string name = char.ToLower(result[0]) + result.Substring(1);
                object v = GetValue(k,value);
                target.GetType().GetField(name).SetValue(target, v);
                MethodInfo addFunc = listType.GetMethod("Add");
                addFunc.Invoke(listInstance, new object[] {target});
            }
            Assignment(i,k + 1,target);
        }
        else
        {
            FieldInfo fieldInfo = objType.GetField(char.ToLower(valueName[0]) + valueName.Substring(1));
            object valueObj = GetValue(k,value);
            fieldInfo.SetValue(obj,valueObj);
            Assignment(i, k + 1, obj);
        }
    }
    
    public static Delegate GetDelegate(Type delegateType,string valueName ,string value)
    {
        valueName = getSubInfoString(valueName);
        Func<object, bool> func = (obj) =>
        {
            if (obj.GetType().GetField(valueName) == null)
            {
                Debug.Log(string.Format("在类{0}中没找到{1}", obj.GetType().Name, valueName));
                return false;
            }
            object v = obj.GetType().GetField(valueName).GetValue(obj);
            return v.ToString().Equals(value);
        };
        return Delegate.CreateDelegate(delegateType,func.Target,func.Method);
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
        string type = worksheet.Cells[3, k].Value.ToString();
        switch (type)
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

    public static string getDefaultValue(int k)
    {
        string valueName = worksheet.Cells[3, k].Value.ToString();
        switch (valueName)
        {
            case "int":
            case "float":
            case "bool":
                return "0";
        }
        return ""; 
    }

    public static string getSubInfoString(string name)
    {
        int index = name.IndexOf("Info");
        string result = name;
        if (index != -1) 
        {
            result  = result.Substring(0, index);
        }

        return result;
    }
}

