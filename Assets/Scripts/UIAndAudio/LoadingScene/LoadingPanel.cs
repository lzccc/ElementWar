using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingPanel : PanelBase {

    public static Slider loading;     //进度条

    #region 生命周期
    public override void Init(params Object[] args)
    {
        base.Init(args);
        //初始化预设体路径和面板层级
        skinPath = "LoadingPage";
        layer = PanelLayer.Panel;
    }
    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
        loading = skinTrans.Find("Slider").GetComponent<Slider>();

    }
    #endregion
    
}
