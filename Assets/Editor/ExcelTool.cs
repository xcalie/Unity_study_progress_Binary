using Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ExcelTool
{
    /// <summary>
    /// Excel文件夹路径
    /// </summary>
    public static string EXCEL_PATH = Application.dataPath + "/ArtRes/Excel/";

    /// <summary>
    /// 数据结构类路径
    /// </summary>
    public static string DATA_CLASS_PATH = Application.dataPath + "/Scripts/ExcelData/DataClass/";

    /// <summary>
    /// 数据容器类路径
    /// </summary>
    public static string DATA_CONTAINER_PATH = Application.dataPath + "/Scripts/ExcelData/DataContainer/";

    /// <summary>
    /// 内容开始行号
    /// </summary>
    public static int BEGIN_INDEX = 4;

    [MenuItem("GameTool/GenerateExcelInfo")]
    private static void GenerateExcelInfo()
    {
        //记得在指定路径下放置Excel文件 用于生成对应的三个文件
        DirectoryInfo dInfo = Directory.CreateDirectory(EXCEL_PATH);
        //获取文件夹下所有文件
        FileInfo[] files = dInfo.GetFiles();
        //数据表容器
        DataTableCollection tableCollection;
        for (int i = 0; i < files.Length; i++)
        {
            //如果不是Excel文件则跳过
            if (!files[i].Name.EndsWith(".xlsx") && !files[i].Name.EndsWith(".xls"))
            {
                continue;
            }

            //打开Excel文件获得所有的表
            using (FileStream fs = files[i].Open(FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                tableCollection = excelReader.AsDataSet().Tables;
                fs.Close();
            }

            //遍历所有表的信息
            foreach (DataTable table in tableCollection)
            {
                Debug.Log("表名：" + table.TableName);
                //生成数据结构类
                GenerateExcelDataClass(table);
                //生成容器类
                GenerateExcelDataContainer(table);
                //生成2进制数据
                GenerateExcelBinary(table);
            }
        }
    }

    /// <summary>
    /// 生成Excel数据结构类
    /// </summary>
    /// <param name="table"></param>
    private static void GenerateExcelDataClass(DataTable table)
    {
        //字段名行
        DataRow rowName = GetVariableNameRow(table);
        //字段类型行
        DataRow rowType = GetVariableTypeRow(table);

        //判断路径是否存在 如果不存在则创建
        if (!Directory.Exists(DATA_CLASS_PATH))
        {
            Directory.CreateDirectory(DATA_CLASS_PATH);
        }
        //生成数据结构类 通过代码进行字符串拼接 存入文件
        string str = "public class " + table.TableName + "\n{\n";

        //变量进行字符串拼接
        for (int i = 0; i < table.Columns.Count; i++)
        {
            str += "    public " + rowType[i].ToString() + " " + rowName[i] + ";\n";
        }

        str += "}";

        //写入文件
        File.WriteAllText(DATA_CLASS_PATH + table.TableName + ".cs", str);

        //刷新资源
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 生成Excel数据容器类
    /// </summary>
    /// <param name="table"></param>
    private static void GenerateExcelDataContainer(DataTable table)
    {
        //获取主键索引
        int keyIndex = GetKeyIndex(table);
        //获取字段类型行
        DataRow rowType = GetVariableTypeRow(table);

        //判断路径是否存在 如果不存在则创建
        if (!Directory.Exists(DATA_CONTAINER_PATH))
        {
            Directory.CreateDirectory(DATA_CONTAINER_PATH);
        }

        string str = "using System.Collections.Generic;\n\n";

        str += "public class " + table.TableName + "Container\n{\n";

        str += "    public Dictionary<" + rowType[keyIndex] + ", " + table.TableName + "> dataDic = new Dictionary<" + rowType[keyIndex] + ", " + table.TableName + ">();\n";

        str += "}";

        //写入文件
        File.WriteAllText(DATA_CONTAINER_PATH + table.TableName + "Container.cs", str);

        //刷新资源
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 生成Excel二进制数据
    /// </summary>
    /// <param name="table"></param>
    private static void GenerateExcelBinary(DataTable table)
    {
        //如果不存在则创建
        if (!Directory.Exists(BinaryDataManager.DATA_BINARY_PATH))
        {
            Directory.CreateDirectory(BinaryDataManager.DATA_BINARY_PATH);
        }

        //创建二进制文件
        using (FileStream fs = new FileStream(BinaryDataManager.DATA_BINARY_PATH + table.TableName + ".xc", FileMode.OpenOrCreate, FileAccess.Write))
        {
            //存储具体excel的对应2进制数据
            //先存储数据的行数 方便读取
            //-4是因为前四行是字段名 字段类型 主键和数据ID
            fs.Write(BitConverter.GetBytes(table.Rows.Count - 4), 0, 4);
            //2.存储主键的变量类型
            string keyName = GetVariableNameRow(table)[GetKeyIndex(table)].ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(keyName);
            //存储字符串长度
            fs.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
            //存储字符串
            fs.Write(bytes, 0, bytes.Length);

            //遍历所有行 进行2进制数据存储
            DataRow row;
            //字段类型行
            DataRow rowType = GetVariableTypeRow(table);
            for (int i = BEGIN_INDEX; i < table.Rows.Count; i++)
            {
                row = table.Rows[i];
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    switch (rowType[j].ToString())
                    {
                        case "int":
                            fs.Write(BitConverter.GetBytes(int.Parse(row[j].ToString())), 0, 4);
                            break;
                        case "long":
                            fs.Write(BitConverter.GetBytes(long.Parse(row[j].ToString())), 0, 8);
                            break;
                        case "float":
                            fs.Write(BitConverter.GetBytes(float.Parse(row[j].ToString())), 0, 4);
                            break;
                        case "double":
                            fs.Write(BitConverter.GetBytes(double.Parse(row[j].ToString())), 0, 8);
                            break;
                        case "bool":
                            fs.Write(BitConverter.GetBytes(bool.Parse(row[j].ToString())), 0, 1);
                            break;
                        case "string":
                            bytes = Encoding.UTF8.GetBytes(row[j].ToString());
                            //存储字符串长度
                            fs.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
                            //存储字符串
                            fs.Write(bytes, 0, bytes.Length);
                            break;
                    }
                }
            }

            fs.Close();
        }

        //刷新资源
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 获取变量名所在行
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static DataRow GetVariableNameRow(DataTable table)
    {
        return table.Rows[0];
    }

    /// <summary>
    /// 获取变量类型所在行
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static DataRow GetVariableTypeRow(DataTable table)
    {
        return table.Rows[1];
    }

    /// <summary>
    /// 获取主键所在索引
    /// </summary>
    /// <param name="table"></param>
    private static int GetKeyIndex(DataTable table)
    {
        DataRow row = table.Rows[2];
        for (int i = 0; i < table.Columns.Count; i++)
        {
            if (row[i].ToString().Equals("key"))
            {
                return i;
            }
        }
        return 0;
    }
}
