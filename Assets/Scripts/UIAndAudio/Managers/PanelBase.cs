
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 面板基类
/// </summary>
public class PanelBase : MonoBehaviour
{

    public string skinPath;                  //预设体路径
    public GameObject skin;                  //预设体

    public PanelLayer layer;                 //面板的层级

    public Object[] args;                    //面板的参数


    public bool fadeIn = false;               //是否淡入
    public float alpha = 1;


    #region 生命周期
    //初始化
    public virtual void Init(params Object[] args)
    {
        this.args = args;
    }

    //开始面板前
    public virtual void OnShowing() { }

    //显示面板后
    public virtual void OnShowed() { }

    //帧更新
    public virtual void Update()
    {
    }

    //关闭前
    public virtual void OnClosing() { }

    //关闭后
    public virtual void OnClosed() { }
    #endregion

    protected virtual void Close()
    {
        string name = this.GetType().ToString();
        PanelMgr.instance.ClosePanel(name);
    }
}