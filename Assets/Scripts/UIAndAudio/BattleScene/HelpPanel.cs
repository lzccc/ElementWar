using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanel : PanelBase {

    private Image helpBg;

    public override void Init(params Object[] args)
    {
        base.Init(args);
        //初始化预设体路径和面板层级
        skinPath = "HelpPage";
        layer = PanelLayer.Tips;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
    }
}
