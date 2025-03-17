using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public class BinaryDataManager
{
    private static BinaryDataManager instance = new BinaryDataManager();
    public static BinaryDataManager Instance => instance;

    /// <summary>
    /// 用于存储表格数据的容器
    /// </summary>
    private Dictionary<string, object> tableDic = new Dictionary<string, object>();

    /// <summary>
    /// 存储路径
    /// </summary>
    private static string SAVE_PATH = Application.persistentDataPath + "/Data/";

    /// <summary>
    /// 二进制数据路径
    /// </summary>
    public static string DATA_BINARY_PATH = Application.streamingAssetsPath + "/Binary/";

    private BinaryDataManager() { }

    public void initData()
    {
        //LoadTable<XXXContainer, XXXInfo>();
    }

    /// <summary>
    /// 加载在2进制文件中的表格数据
    /// </summary>
    /// <typeparam name="T">容器类名</typeparam>
    /// <typeparam name="K">数据结构体类名</typeparam>
    public void LoadTable<T, K>()
    {
        //读取 excel表对应的2进制文件
        using (FileStream fs = File.Open(DATA_BINARY_PATH + typeof(K).Name + ".xc", FileMode.Open, FileAccess.Read))
        {
            byte[] btyes = new byte[fs.Length];
            fs.Read(btyes, 0, btyes.Length);
            fs.Close();
            //用于记录当前读取的位置
            int index = 0;

            //读取多少行的数据
            int count = BitConverter.ToInt32(btyes, index);
            index += 4;

            //读取主键的变量类型
            int keyNameLength = BitConverter.ToInt32(btyes, index);
            index += 4;
            string keyName = Encoding.UTF8.GetString(btyes, index, keyNameLength);
            index += keyNameLength;

            //创建容器类对象
            Type containerType = typeof(T);
            object containerObj = Activator.CreateInstance(containerType);
            //得到数据结构体类的Type
            Type classType = typeof(K);
            //得到容器类的数据结构体 所有的字段
            FieldInfo[] infos = classType.GetFields();

            //读取每一行的数据
            for (int i = 0; i < count; i++)
            {
                //实例化数据结构体对象
                object dataObj = Activator.CreateInstance(classType);
                foreach (FieldInfo info in infos)
                {
                    if (info.FieldType == typeof(int))
                    {
                        info.SetValue(dataObj, BitConverter.ToInt32(btyes, index));
                        index += 4;
                    }
                    else if (info.FieldType == typeof(long))
                    {
                        info.SetValue(dataObj, BitConverter.ToInt64(btyes, index));
                        index += 8;
                    }
                    else if (info.FieldType == typeof(float))
                    {
                        info.SetValue(dataObj, BitConverter.ToSingle(btyes, index));
                        index += 4;
                    }
                    else if (info.FieldType == typeof(double))
                    {
                        info.SetValue(dataObj, BitConverter.ToDouble(btyes, index));
                        index += 8;
                    }
                    else if (info.FieldType == typeof(bool))
                    {
                        info.SetValue(dataObj, BitConverter.ToBoolean(btyes, index));
                        index += 1;
                    }
                    else if (info.FieldType == typeof(string))
                    {
                        int length = BitConverter.ToInt32(btyes, index);
                        index += 4;
                        info.SetValue(dataObj, Encoding.UTF8.GetString(btyes, index, length));
                        index += length;
                    }
                }

                //将数据结构体对象存入容器类对象
                object dicObject = containerType.GetField("dataDic").GetValue(containerObj);
                //通过反射调用Add方法
                MethodInfo mInfo = dicObject.GetType().GetMethod("Add");
                //获取主键的值
                object keyValue = classType.GetField(keyName).GetValue(dataObj);
                mInfo.Invoke(dicObject, new object[] { keyValue, dataObj });
            }

            //将容器类对象存入字典
            tableDic.Add(typeof(T).Name, containerObj);

            fs.Close();
        }
    }

    /// <summary>
    /// 获取表格数据
    /// </summary>
    /// <typeparam name="T">容器类名</typeparam>
    /// <returns></returns>
    public T GetTable<T>() where T : class
    {
        string tableName = typeof(T).Name;
        if (tableDic.ContainsKey(tableName))
        {
            return tableDic[tableName] as T;
        }
        return null;
    }

    /// <summary>
    /// 存储类对象数据
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="filename"></param>
    public void Save(object obj, string filename)
    {
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        using (FileStream fs = new FileStream(SAVE_PATH + filename + ".cx", FileMode.OpenOrCreate, FileAccess.Write))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, obj);
            fs.Close();
        }
    }


    /// <summary>
    /// 读取2进制文件转换为类对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filename"></param>
    /// <returns></returns>
    public T Load<T>(string filename) where T : class
    {
        //如果文件不存在 返回默认这
        if (!File.Exists(SAVE_PATH + filename + ".xc"))
        {
            return default(T);
        }

        T obj;
        using (FileStream fs = new FileStream(SAVE_PATH + filename + ".xc", FileMode.Open, FileAccess.Read))
        {
            BinaryFormatter bf = new BinaryFormatter();
            obj = bf.Deserialize(fs) as T;
            fs.Close();
        }

        return obj;
    }
}
