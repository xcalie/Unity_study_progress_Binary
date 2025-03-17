using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class B0001 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //1.各类型转换为字节数组
        byte[] bytes = BitConverter.GetBytes(123);

        //2.字节数组转换为各类型
        int value = BitConverter.ToInt32(bytes, 0);


        //游戏常用编码UTF-8 GBK中文 ASCII英文
        byte[] bytes1 = System.Text.Encoding.UTF8.GetBytes("你好");
        byte[] b1 = Encoding.UTF8.GetBytes("你好");

        byte[] bytes2 = System.Text.Encoding.GetEncoding("GBK").GetBytes("你好");
        byte[] b2 = Encoding.GetEncoding("GBK").GetBytes("你好");

        byte[] bytes3 = System.Text.Encoding.ASCII.GetBytes("hello");
        byte[] b3 = Encoding.ASCII.GetBytes("hello");


        //字节数组转换为字符串
        string str1 = System.Text.Encoding.UTF8.GetString(bytes1);
        string s1 = Encoding.UTF8.GetString(b1);
    }
}
