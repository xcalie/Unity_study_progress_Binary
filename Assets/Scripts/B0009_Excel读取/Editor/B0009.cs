using Excel;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;

public class B0009
{
    #region 打开Excel文件

    //1.FileStream读取Excel文件
    //2.IExcelDataReader读取Excel文件
    //3.DataSet读取Excel文件 将Excel文件转换成DataSet

    [MenuItem("B0009/打开Excel文件")]
    private static void OpenExcel()
    {
        using (FileStream fs = File.Open(Application.dataPath + "/ArtRes/Excel/PlayerInfo.xlsx", FileMode.Open, FileAccess.Read))
        {
            //通过文件流创建ExcelDataReader
            IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
            //将ExcelDataReader转换成DataSet 方便我们 获取Excel文件中的数据
            DataSet dataSet = excelDataReader.AsDataSet();
            //获取Excel文件中的数据
            for (int i = 0; i < dataSet.Tables.Count; i++)
            {
                DataTable dataTable = dataSet.Tables[i];
                for (int j = 0; j < dataTable.Rows.Count; j++)
                {
                    for (int k = 0; k < dataTable.Columns.Count; k++)
                    {
                        Debug.Log(dataTable.Rows[j][k]);
                    }
                }

                Debug.Log(dataTable.TableName);
                Debug.Log(dataTable.Rows);
                Debug.Log(dataTable.Columns);
            }
        }
    }

    #endregion


    #region 获得Excel文件中的数据信息

    //1.FileStream读取Excel文件
    //2.IExcelDataReader读取Excel文件
    //3.DataSet读取Excel文件 将Excel文件转换成DataSet
    //4.DataTable读取Excel文件 将Excel文件转换成DataTable
    //5.DataRow读取Excel文件 将Excel文件转换成DataRow
    [MenuItem("B0009/获得Excel文件中的数据信息")]
    private static void ReadExcel()
    {
        using (FileStream fs = File.Open(Application.dataPath + "/ArtRes/Excel/PlayerInfo.xlsx", FileMode.Open, FileAccess.Read))
        {
            //通过文件流创建ExcelDataReader
            IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
            //将ExcelDataReader转换成DataSet 方便我们 获取Excel文件中的数据
            DataSet dataSet = excelDataReader.AsDataSet();
            //获取Excel文件中的数据
            for (int i = 0; i < dataSet.Tables.Count; i++)
            {
                DataTable dataTable = dataSet.Tables[i];
                for (int j = 0; j < dataTable.Rows.Count; j++)
                {
                    DataRow dataRow = dataTable.Rows[j];
                    for (int k = 0; k < dataTable.Columns.Count; k++)
                    {
                        Debug.Log(dataRow[k]);
                    }
                }
            }
        }
    }

    #endregion
}
