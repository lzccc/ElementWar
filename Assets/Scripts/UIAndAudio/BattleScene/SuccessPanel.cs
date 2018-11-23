using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class SuccessPanel : PanelBase
{

    private GameObject buff;
    private Image maxBuffHint;     //级数已满的提示
    private Button[] buffs;        //buff图标
    private Button backButton;     //背景图上的button
    private Button yes;            //确认buff按钮
    private Queue<int> queue = new Queue<int>(); //存储上一次点击到的技能
    private Image mask;
    public float speed = 2f;
    private GameObject JoystickManager;
    private void Start()
    {
        JoystickManager=GameObject.Find("JoystickManager");
        if(JoystickManager!=null)
        JoystickManager.SetActive(false);

        //实现页面的淡入淡出
        if (fadeIn)
        {
            mask = skin.transform.Find("mask").GetComponent<Image>();
            mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, 0.8f);
        }
        else {
            Time.timeScale = 0;
        }
    }

    public override void Update()
    {
        if (fadeIn)
        {
            alpha -= Time.deltaTime / speed;
            mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, alpha);
            if (alpha <= 0)
            {
                mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, 0);
                Time.timeScale = 0;
            }
        }
    }

    public override void Init(params UnityEngine.Object[] args)
    {
        base.Init(args);
        //初始化预设体路径和面板层级
        skinPath = "SuccessPage";
        layer = PanelLayer.Tips;

    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;

        buff = skinTrans.Find("buff").gameObject;

        maxBuffHint = skinTrans.Find("maxBuffHint").GetComponent<Image>(); //buff已满提示框
        maxBuffHint.gameObject.SetActive(false);

        buffs = buff.GetComponentsInChildren<Button>();
        backButton = skin.GetComponent<Button>();
        backButton.onClick.AddListener(BackButtonClick);
        yes = skinTrans.Find("yes").GetComponent<Button>();
        yes.onClick.AddListener(GetBuff);
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i].transform.Find("info").gameObject.SetActive(false);
            int ii = i;
            buffs[i].onClick.AddListener(delegate () { this.SkillUpUp(ii + 1); });  //buff ID = 1 2 3 4 5 6
        }
        //默认选择一个等级最低且未满级的技能，一开始就是亮的
        DisplayDefaultBuffInfo();
    }

    /// <summary>
    /// 高亮显示默认buff
    /// </summary>
    private void DisplayDefaultBuffInfo()
    {
        int id = SearchCanUpBuffID();
        if (id != 0)
            BuffInfoDisplay(id);
    }

    /// <summary>
    /// 搜索等级最低、且未满级的buff ID，并高亮显示信息
    /// 若搜索不到则不显示
    /// </summary>
    /// <returns></returns>
    private int SearchCanUpBuffID()
    {
        int minLevel = BasePlayerAttribute.instance.ExtraBuffLv[0];
        int id = 1;
        for (int i = 2; i < 7; i++)
        {
            if (BasePlayerAttribute.instance.ExtraBuffLv[i - 1] < minLevel)
            {
                if (i == 3 && BasePlayerAttribute.instance.ExtraBuffLv[i - 1] == 1)
                    continue;
                minLevel = BasePlayerAttribute.instance.ExtraBuffLv[i - 1];
                id = i;
            }
        }
        if (BasePlayerAttribute.instance.CanUpBuff(id))
            return id;
        return 0;
    }

    /// <summary>
    /// 点击Buff图标，选中buff
    /// </summary>
    /// <param name="text"></param>
    private void SkillUpUp(int buffID)
    {
        AudioManager.Instance.PlaySound(AudioType.SelectBuff);
        skin.transform.Find("hint").gameObject.SetActive(false); //隐藏“选择一个能力”
        ClearBuffSelect();

        //若已升级为4级，则弹出提示不能再升级！buff不再增益！
        if (!BasePlayerAttribute.instance.CanUpBuff(buffID))
        {
            maxBuffHint.transform.position = buffs[buffID - 1].transform.position;
            maxBuffHint.gameObject.SetActive(true);
        }
        else
        {
            BuffInfoDisplay(buffID);
        }
    }

    /// <summary>
    /// 点击buff图标后，入队、高亮、显示buff对应的等级信息
    /// </summary>
    /// <param name="id"></param>
    private void BuffInfoDisplay(int id)
    {
        //入队，信息和高亮同时出现
        queue.Enqueue(id);
        Transform info = buffs[id - 1].transform.Find("info");
        info.gameObject.SetActive(true);
        //显示对应等级的buff信息
        info.GetChild(1).GetComponent<Text>().text = BasePlayerAttribute.instance.GetExtraBuffUpInfo(id);
    }

    /// <summary>
    /// 点击背景图，则取消刚刚选中的技能
    /// </summary>
    private void BackButtonClick()
    {
        skin.transform.Find("hintBg").gameObject.SetActive(true); //显示“选择一个能力”
        ClearBuffSelect();
    }

    /// <summary>
    /// 取消选中的buff，信息和高亮隐藏
    /// </summary>
    private void ClearBuffSelect()
    {
        if (queue.Count > 0)
        {
            int buffId = queue.Dequeue();
            buffs[buffId - 1].transform.Find("info").gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 确认获得buff
    /// </summary>
    private void GetBuff()
    {
        AudioManager.Instance.PlaySound(AudioType.ButtonYes);
        //如果选中了buff，则获得buff，否则不会获得buff
        if (queue.Count > 0)
        {
            int id = queue.Dequeue();
            SkillGet(id);
            //将获得的buff显示再战斗界面左上角
            BattlePanel.Instance.DisplayMinibuff(id);
        }
        maxBuffHint.gameObject.SetActive(false);
        PanelMgr.instance.ClosePanel("SuccessPanel");
        Time.timeScale = 1;
        JoystickManager.SetActive(true);
    }

    /// <summary>
    /// 升级buff
    /// </summary>
    public void SkillGet(int buffID)
    {
        BasePlayerAttribute.instance.UpBuffLvForId(buffID);
    }

    /// <summary>
    /// 关闭“级数已满”的提示
    /// </summary>
    private void CloseHint()
    {
        maxBuffHint.gameObject.SetActive(false);
    }

    /// <summary>
    /// 进入下一关
    /// </summary>
    public void NextLevel()
    {
        SceneManager.LoadScene("BeginGame");
    }
}

