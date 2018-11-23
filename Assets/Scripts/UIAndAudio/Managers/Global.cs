using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Global {

    //在这里记录当前切换场景的名称
    public static string loadName;
    public static bool beginingAnimPlay = false;//开场动画播放过
    public static bool newGame=true;

    /// <summary>
    /// 控制按钮上阴影的亮暗，表现为按钮的选中和未选中
    /// </summary>
    /// <param name="image"></param>
    /// <param name="isHighlited"></param>
    public static void ChangeShadowColor(Image image, bool isHighlited)
    {
        if (isHighlited)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        }
        else
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.53f);
        }
    }


}
