using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class B0004 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region C#自带的文件夹操作类

        //类名：Directory
        //命名空间：System.IO
        //功能：创建文件夹

        //判断文件夹是否存在
        if (!Directory.Exists(Application.dataPath + "Assets/Scripts/B0004_文件夹"))
        {
            //创建文件夹
            Directory.CreateDirectory(Application.dataPath + "Assets/Scripts/B0004_文件夹");
            Debug.Log("文件夹创建成功");
        }
        else
        {
            Debug.Log("文件夹已存在");
        }

        //创建文件夹
        Directory.CreateDirectory(Application.dataPath + "Assets/Scripts/B0004_文件夹/文件夹1");

        //删除文件夹
        //参数1：文件夹路径
        //参数2：是否删除非空文件夹 如果为true则删除整个目录，如果为false则只删除空目录(默认为false)
        Directory.Delete(Application.dataPath + "Assets/Scripts/B0004_文件夹/文件夹1");

        //查找文件夹和指定文件
        //得到指定路径下的所有文件夹
        string[] dirs = Directory.GetDirectories(Application.dataPath + "Assets/Scripts/B0004_文件夹");
        foreach (string dir in dirs)
        {
            Debug.Log(dir);
        }

        //得到指定路径下的所有文件
        string[] files = Directory.GetFiles(Application.dataPath + "Assets/Scripts/B0004_文件夹");
        foreach (string file in files)
        {
            Debug.Log(file);
        }

        //移动文件夹
        //参数1：源文件夹路径
        //参数2：目标文件夹路径
        //如果第二个参数是已经存在的文件夹，则会抛出异常
        Directory.Move(Application.dataPath + "Assets/Scripts/B0004_文件夹/文件夹1", Application.dataPath + "Assets/Scripts/B0004_文件夹/文件夹2");

        #endregion

        #region DirectortInfo类 和 FileInfo类

        //DirectoryInfo目录信息类
        //可以获取文件夹的更多信息
        //主要出现在两个地方
        //1.创建文件夹的返回值
        DirectoryInfo dirInfo = Directory.CreateDirectory(Application.dataPath + "Assets/Scripts/B0004_文件夹/文件夹3");
        //全路径
        Debug.Log(dirInfo.FullName);
        //文件夹名称
        Debug.Log(dirInfo.Name);

        //2.查找上级文件夹的信息
        dirInfo = Directory.GetParent(Application.dataPath + "Assets/Scripts/B0004_文件夹/文件夹3");
        //全路径
        Debug.Log(dirInfo.FullName);
        //文件夹名称
        Debug.Log(dirInfo.Name);

        //重要方法
        //得到指定路径下的所有文件夹
        DirectoryInfo[] dirsInfo = dirInfo.GetDirectories();

        //FileInfo文件信息类
        //可以获取文件的更多信息
        FileInfo[] fileInfos = dirInfo.GetFiles();
        foreach (FileInfo fileInfo in fileInfos)
        {
            //全路径
            Debug.Log(fileInfo.FullName);
            //文件名称
            Debug.Log(fileInfo.Name);
            //文件大小
            Debug.Log(fileInfo.Length);
            //文件创建时间
            Debug.Log(fileInfo.CreationTime);
            //文件最后修改时间
            Debug.Log(fileInfo.LastWriteTime);
        }

        #endregion
    }
}
