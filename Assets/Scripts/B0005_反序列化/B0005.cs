using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class B0005 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 序列化

        //主要类
        // FileStream文件流类
        // BinaryFormatter二进制格式化类
        //主要方法
        // Deserialize反序列化
        //通过文件流打开指定2进制文件    
        using (FileStream fs = new FileStream(Application.dataPath + "/Resources/序列化文件.txt", FileMode.Open, FileAccess.Read))
        {
            //创建二进制格式化类
            BinaryFormatter bf = new BinaryFormatter();
            //反序列化
            //参数1：文件流
            //返回值：反序列化后的对象
            List<int> list = (List<int>)bf.Deserialize(fs);
            foreach (int i in list)
            {
                Debug.Log(i);
            }

            fs.Close();
        }

        #endregion

        #region 网络传输

        //主要类
        // MemoryStream内存流类
        // BinaryFormatter二进制格式化类
        //主要方法
        // Serialize序列化
        //通过内存流写入序列化对象

        //假设这是从网络接收到的数据
        byte[] data = File.ReadAllBytes(Application.dataPath + "/Resources/序列化文件.txt");
        //创建内存流
        using (MemoryStream ms = new MemoryStream(data))
        {
            //创建二进制格式化类
            BinaryFormatter bf = new BinaryFormatter();
            //反序列化
            //参数1：内存流
            //返回值：反序列化后的对象
            List<int> list = (List<int>)bf.Deserialize(ms);
            foreach (int i in list)
            {
                Debug.Log(i);
            }
            ms.Close();
        }


        #endregion
    }
}
