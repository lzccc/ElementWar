using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 颜色透明的闪烁的帮助类
/// </summary>
public class HideControl : MonoBehaviour {
    [Tooltip("一次无到有的时间")]
    public float time = 1;
    [Tooltip("是文本还是图片")]
    public bool isImg;
    [Tooltip("初始颜色")]
    public Color color;
    private Image img;
    private Text text;
    private bool addTrigger;
    private void Awake()
    {
        if (isImg)
        {
            img = this.GetComponent<Image>();
            img.color = color;
        }
        else
        {
            text = this.GetComponent<Text>();
            text.color = color;
        }
    }

    private void Update()
    {
        if (addTrigger)
        {
            if (isImg)
            {
                img.color +=new Color(0,0,0 ,Time.deltaTime / time);
                if (img.color.a >= 1)
                {
                    addTrigger = false;
                }
            }
            else
            {
                text.color += new Color(0, 0, 0, Time.deltaTime / time);
                if (text.color.a >= 1)
                {
                    addTrigger = false;
                }
            }
        }
        else
        {
            if (isImg)
            {
                img.color -= new Color(0, 0, 0, Time.deltaTime / time);
                if (img.color.a <=0 )
                {
                    addTrigger = true;
                }
            }
            else
            {
                text.color -= new Color(0, 0, 0, Time.deltaTime / time);
                if (text.color.a <=0 )
                {
                    addTrigger = true;
                }
            }
        }
    }

}
