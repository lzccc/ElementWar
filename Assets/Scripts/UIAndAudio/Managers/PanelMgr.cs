using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//页面类型
public enum PanelLayer { Panel, Tips };

/// <summary>
/// 面板管理类：初始化、打开、关闭
/// </summary>
public class PanelMgr : MonoBehaviour
{

    public static PanelMgr instance;                       //单例
    private GameObject canvas;                             //画布
    public Dictionary<string, PanelBase> dict;             //存放已打开的面板
    public Dictionary<PanelLayer, Transform> layerDict;    //存放各个层级所对应的父物体



    private void Awake()
    {
        instance = this;
        InitLayer();
        dict = new Dictionary<string, PanelBase>();
    }

    /// <summary>
    /// 初始化各类型面板的transform，作为面板子类的父对象
    /// </summary>
    private void InitLayer()
    {
        //在场景中找到画布
        canvas = GameObject.Find("Canvas");
        if (canvas == null)
            Debug.Log("panelMgr.InitLayer fail, canvas is null");

        //初始化各个层级
        layerDict = new Dictionary<PanelLayer, Transform>();
        foreach (PanelLayer layer in Enum.GetValues(typeof(PanelLayer)))
        {
            string name = layer.ToString();
            Transform transform = canvas.transform.Find(name);
            layerDict.Add(layer, transform);
        }
    }

    /// <summary>
    /// 打开指定预设体的页面
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="skinPath"></param>
    /// <param name="fade"> fade=true 淡入，false直接出现 </param>
    /// <param name="arg"></param>
    public void OpenPanel<T>(string skinPath, bool fadeIn = false, params object[] arg) where T : PanelBase
    {

        //获取面板对应的名字
        string name = typeof(T).ToString();
        if (dict.ContainsKey(name))
            return;

        //面板脚本，挂到画布上
        PanelBase panel = canvas.AddComponent<T>();

        panel.Init();
        panel.fadeIn = fadeIn;
        dict.Add(name, panel);

        //加载面板T的预设体
        skinPath = (skinPath != "" ? skinPath : panel.skinPath);
        GameObject skin = Resources.Load<GameObject>("WXY/Prefabs/Panels/" + skinPath);
        if (skin == null)
            Debug.Log("panelMgr.OpenPanel fail, skin is null, skinPath = " + skinPath);
        panel.skin = (GameObject)Instantiate(skin);

        //坐标
        Transform skinTrans = panel.skin.transform;
        PanelLayer layer = panel.layer;
        Transform parent = layerDict[layer];

        skinTrans.SetParent(parent, false);

        //panel生命周期 预留的面板动画，可自行实现
        panel.OnShowing();
        panel.OnShowed();
    }

    /// <summary>
    /// 关闭面板
    /// </summary>
    /// <param name="name"></param>
    public void ClosePanel(string name)
    {
        PanelBase panel = (PanelBase)dict[name]; //根据名字，从字典中获取面板对象
        if (panel == null)
            return;

        panel.OnClosing();
        dict.Remove(name);
        panel.OnClosed();

        GameObject.Destroy(panel.skin);
        Component.Destroy(panel);
    }
}
