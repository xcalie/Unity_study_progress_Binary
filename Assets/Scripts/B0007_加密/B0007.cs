using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class B0007 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //加密 

        //简易加密 通过异或运算

        byte key = 0x07; //密钥

        //加密
        byte[] bytes = File.ReadAllBytes(Application.dataPath + "/Person.bytes");

        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)(bytes[i] ^ key);
        }

        File.WriteAllBytes(Application.dataPath + "/Person.bytes", bytes);

        //解密
        bytes = File.ReadAllBytes(Application.dataPath + "/Person.bytes");

        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)(bytes[i] ^ key);
        }

        File.WriteAllBytes(Application.dataPath + "/Person.bytes", bytes);

    }
}
