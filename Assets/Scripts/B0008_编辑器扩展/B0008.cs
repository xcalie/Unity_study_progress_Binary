using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

public class B0008 : MonoBehaviour
{
    #region 为菜单栏添加一个按钮

    //可以通过UnityEditor.MenuItem来为菜单栏添加一个按钮
    //UnityEditor.MenuItem("GameTool/Test/生成Excel信息")表示在GameTool下的Test下添加一个生成Excel信息的按钮
    //这个按钮的功能是输出"生成Excel信息"


    //规则一 一定要是静态方法
    //规则二 菜单栏按钮必须有至少一个斜杠,不支持只有一个菜单栏入口
    [UnityEditor.MenuItem("GameTool/Test/")]
    private static void Test()
    {
        Debug.Log("生成Excel信息");

        #region 刷新project视图

        //AssetDatabase.Refresh(); //刷新project视图
        //命名空间：UnityEditor
        //方法：Refresh

        Directory.CreateDirectory(Application.persistentDataPath + "/测试文件夹");

        AssetDatabase.Refresh();

        #endregion
    }

    #endregion

    #region Editor 文件夹

    //编辑器相关的代码都要放在Editor文件夹下
    //在打包的时候,编辑器相关的代码不会被打包
    //如果不放在Editor文件夹下,在打包的时候会报错

    #endregion
}
