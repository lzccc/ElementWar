using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class begin : MonoBehaviour
{

    private GameObject panelMgr;
    private GameObject audioManager;

    public Image introduction; //黑幕背景
    public Text detail;        //文字
    public Image first;    //开场动画图
    private float time = 1.2f; //隐退出现的速度
    private float tmpColor = 0; //初始alpha值

    private bool active = false; //是否播放完
    private int flag = 0;

    private void Awake()
    {
        introduction.GetComponent<Button>().onClick.AddListener(delegate () { Next(introduction.name); });
        first.GetComponent<Button>().onClick.AddListener(delegate () { Next(first.name); });
    }

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;
        //可以放在游戏最开始时加载
        panelMgr = Resources.Load<GameObject>("WXY/Prefabs/PanelMgr");
        Instantiate(panelMgr);
        audioManager = Resources.Load<GameObject>("WXY/Prefabs/AudioManager");
        Instantiate(audioManager);

        HideImage(first);
        HideImage(introduction);
        detail.color = new Color(detail.color.r, detail.color.g, detail.color.b, 0);

        if (Global.beginingAnimPlay)
        {
            PanelMgr.instance.OpenPanel<BeginGamePanel>("");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Global.beginingAnimPlay)
        {
            if (flag < 2)
            {
                if (!active)
                {
                    ChangeImageColor(introduction, detail, true);
                }
                else
                {
                    ChangeImageColor(introduction, detail, false);
                }

            }
            else if (flag < 4)
            {
                if (!active)
                {
                    ChangeImageColor(first, null, true);
                }
                else
                {
                    ChangeImageColor(first, null, false);
                }
            }
            else
            {
                Destroy(first);
                Destroy(introduction);
                PanelMgr.instance.OpenPanel<BeginGamePanel>("");
                Global.beginingAnimPlay = true;
            }
        }
    }


    public void ChangeImageColor(Image img, Text text, bool isOccur)
    {
        if (img.name == "introduction")
            time = 5f;
        else
            time = 2f;
        if (isOccur)
        {
            tmpColor += Time.deltaTime / time;
            img.color = new Color(img.color.r, img.color.g, img.color.b, tmpColor);
            if (text != null)
                text.color = new Color(text.color.r, text.color.g, text.color.b, tmpColor);
            if (tmpColor >= 1)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
                if (text != null)
                    text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
                active = isOccur;
                flag++;
            }
        }
        else
        {
            tmpColor -= Time.deltaTime / time;
            img.color = new Color(img.color.r, img.color.g, img.color.b, tmpColor);
            if (text != null)
                text.color = new Color(text.color.r, text.color.g, text.color.b, tmpColor);
            if (tmpColor <= 0)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
                if (text != null)
                    text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
                active = isOccur;
                flag++;
            }
        }
    }


    private void HideImage(Image img)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
    }


    /// <summary>
    /// 点击直接切换到下一个页面
    /// </summary>
    /// <param name="v"></param>
    /// <param name="name"></param>
    private void Next(string name)
    {
        if (name == "introduction")
        {
            introduction.gameObject.SetActive(false);
            active = false;
            flag = 2;
        }
        else
        {
            flag = 4;
        }
    }
}
