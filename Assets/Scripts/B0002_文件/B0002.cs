using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class B0002 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //文件流

        //1.判断文件是否存在
        if (File.Exists(Application.dataPath + "/Unity.xc"))
        {
            Debug.Log("文件存在");
        }
        else
        {
            Debug.Log("文件不存在");
        }

        //2.创建文件
        FileStream fs = File.Create(Application.dataPath + "/Unity.xc");

        //3.写入文件
        //将指定字节数组写入文件
        byte[] bytes = BitConverter.GetBytes(999);
        File.WriteAllBytes(Application.dataPath + "/Unity.xc", bytes);

        //将指定字符串数组写入文件 每个字符串占一行
        string[] str = { "Hello", "Unity" };
        File.WriteAllLines(Application.dataPath + "/Unity.xc", str);

        //将指定字符串写入文件
        File.WriteAllText(Application.dataPath + "/Unity.xc", "Hello Unity");


        //4.读取文件
        //读取文件所有字节
        byte[] bytes1 = File.ReadAllBytes(Application.dataPath + "/Unity.xc");

        //读取文件所有行
        string[] str1 = File.ReadAllLines(Application.dataPath + "/Unity.xc");

        //读取文件所有文本
        string str2 = File.ReadAllText(Application.dataPath + "/Unity.xc");

        //5.删除文件
        File.Delete(Application.dataPath + "/Unity.xc");//如果删除了打开的文件会报错

        //6.复制文件
        //参数1：源文件路径 需要是关闭着的文件
        //参数2：目标文件路径
        File.Copy(Application.dataPath + "/Unity.xc", Application.dataPath + "/Unity1.xc");

        //7.替换文件
        //参数1：源文件路径 需要是关闭着的文件
        //参数2：目标文件路径
        //参数3：备份文件路径
        File.Replace(Application.dataPath + "/Unity.xc", Application.dataPath + "/Unity1.xc", Application.dataPath + "/Unity2.xc");

        //8.以流的形式读取文件
        //参数1：文件路径
        //参数2：文件打开方式
        //参数3：文件访问权限
        FileStream fs1 = File.Open(Application.dataPath + "/Unity.xc", FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }
}
