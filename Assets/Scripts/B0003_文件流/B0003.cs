using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class B0003 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 打开或创建文件

        //1.打开或创建文件
        //方法1 new FileStream(文件路径, FileMode.OpenOrCreate)
        //参数1：文件路径  
        //参数2：打开或创建文件的方式
        // FileMode.CreateNew：创建一个新文件，如果文件已存在，则引发异常
        // FileMode.Create：创建一个新文件，如果文件已存在，则覆盖
        // FileMode.Open：打开一个文件，如果文件不存在，则引发异常
        // FileMode.OpenOrCreate：打开一个文件，如果文件不存在，则创建
        // FileMode.Truncate：打开一个文件，如果文件不存在，则引发异常
        // FileMode.Append：打开一个文件，如果文件不存在，则创建，将写入内容追加到文件末尾
        //参数3：访问模式
        // FileAccess.Read：只读
        // FileAccess.Write：只写
        // FileAccess.ReadWrite：读写
        //参数4：文件共享方式(相对于其他程序)(可选)
        // FileShare.None：不共享
        // FileShare.Read：只读共享
        // FileShare.Write：只写共享
        // FileShare.ReadWrite：读写共享
        // FileShare.Delete：删除共享
        FileStream fs = new FileStream(Application.dataPath + "/test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

        //方法2 File.Create(文件路径)
        //参数1：文件路径
        //参数2：缓存区大小(可选)
        //参数3：文件选项(可选)
        // FileOptions.None：默认选项
        // FileOptions.Encrypted：加密文件
        // FileOptions.DeleteOnClose：关闭文件时删除
        // FileOptions.Asynchronous：异步操作
        // FileOptions.SequentialScan：从头到尾文件读取
        // FileOptions.RandomAccess：随机文件写入
        // FileOptions.WriteThrough：直接写入磁盘
        //返回值：FileStream
        FileStream fs2 = File.Create(Application.dataPath + "/test2.txt");


        //方法3 File.Open(文件路径, FileMode, FileAccess)
        //参数1：文件路径
        //参数2：打开或创建文件的方式
        FileStream fs3 = File.Open(Application.dataPath + "/test3.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);

        #endregion

        #region 文件属性

        FileStream fs4 = File.Open(Application.dataPath + "/test4.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        //文本字节长度
        Debug.Log(fs4.Length);
        //文件是否可读
        Debug.Log(fs4.CanRead);
        //文件是否可写
        Debug.Log(fs4.CanWrite);

        //将字节写入文件 写入后 一定执行一次
        fs.Flush();

        //关闭文件 当读写完毕后一定要关闭文件
        fs.Close();

        //缓存资源回收
        fs.Dispose();

        #endregion

        #region 写入文件

        //方法1 FileStream.Write(字节数组, 0, 字节数组长度)
        //参数1：字节数组
        //参数2：写入字节数组的起始位置
        //参数3：写入字节数组的长度
        byte[] bytes = BitConverter.GetBytes(999);
        fs.Write(bytes, 0, bytes.Length);

        //写入字符串
        bytes = Encoding.UTF8.GetBytes("Hello World");
        //先写入长度
        fs.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
        //再写入字符串
        fs.Write(bytes, 0, bytes.Length);

        //避免数据丢失 写入后 一定执行一次
        fs.Flush();
        //销毁缓存
        fs.Close();

        #endregion

        #region 读取文件

        #region 方法一 挨个读取

        //读取第一个整形
        byte[] bytes1 = new byte[4];

        //参数1：用于存储读取的字节数组
        //参数2：读取字节数组的起始位置
        //参数3：读取字节数组的长度
        //返回值：读取的字节数

        //读取整形
        int index = fs.Read(bytes1, 0, 4);
        int num = BitConverter.ToInt32(bytes1, 0);
        print("取出来的第一个整形：" + num);  
        print("索引向前移动的字节数：" + index);

        //读取第二个字符串
        //读取字符串字节数组长度
        index = fs.Read(bytes1, 0, 4);
        print("索引向前移动的字节数：" + index);
        int length = BitConverter.ToInt32(bytes1, 0);
        //根据长度读取字符串
        byte[] bytes2 = new byte[length];
        index = fs.Read(bytes2, 0, length);

        #endregion

        #region 方法二 一次性全部读取再处理

        //读取文件
        FileStream fs5 = File.Open(Application.dataPath + "/test5.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        //一开始就声明一个和文件大小一样的字节数组
        byte[] bytes3 = new byte[fs5.Length];
        int index2 = fs5.Read(bytes3, 0, (int)bytes3.Length);

        //读取整数
        print(BitConverter.ToInt32(bytes3, 0));
        //读取字符串
        int length2 = BitConverter.ToInt32(bytes3, 4);
        print(Encoding.UTF8.GetString(bytes3, 8, length2));

        #endregion

        #region 更加安全的读取方式

        //using 关键字 会自动释放资源
        using (FileStream fs6 = File.Open(Application.dataPath + "/test6.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            byte[] bytes4 = new byte[fs6.Length];
            fs6.Read(bytes4, 0, (int)fs6.Length);
            print(BitConverter.ToInt32(bytes4, 0));
            int length3 = BitConverter.ToInt32(bytes4, 4);
            print(Encoding.UTF8.GetString(bytes4, 8, length3));
        }

        #endregion

        #endregion
    }
}
