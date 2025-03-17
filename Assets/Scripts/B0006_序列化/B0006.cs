using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class B0006 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 序列化类

        // 需要序列化的类 一定要加上 [System.Serializable] 标签

        #endregion

        #region 将对象序列化为2进制文件

        //方法一：使用内存流得到二进制文件
        //主要用于得到字节数组 可以用于网络传输
        //1.内存流对象 
        //类名 MemoryStream
        //using System.IO;

        //2.格式化2进制对象
        //类名 BinaryFormatter
        //using System.Runtime.Serialization.Formatters.Binary;
        //主要方法 Serialize(内存流对象,对象)

        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter bf = new BinaryFormatter();
            Person p = new Person();
            p.name = "张三";
            p.age = 18;
            p.id = 1001;
            bf.Serialize(ms, p);

            byte[] bytes = ms.GetBuffer();
            //将字节数组保存到本地文件
            File.WriteAllBytes(Application.dataPath + "/Person.bytes", bytes);

            ms.Close();
        }

        //方法二：使用文件流得到二进制文件
        //主要用于保存到本地文件

        using (FileStream fs = new FileStream(Application.dataPath + "/Person2.bytes", FileMode.Create))
        {
            BinaryFormatter bf = new BinaryFormatter();
            Person p = new Person();
            p.name = "李四";
            p.age = 20;
            p.id = 1002;
            bf.Serialize(fs, p);

            fs.Close();
        }

        #endregion
    }
}

// 序列化类 就要加上这个标签
[System.Serializable]
public class Person
{
    public string name;
    public int age; 
    public int id;
}