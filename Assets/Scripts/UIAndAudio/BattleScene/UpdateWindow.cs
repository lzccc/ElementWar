using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateWindow : MonoBehaviour
{

    private Image windowBg;            //升级窗口背景
    private Button close;              //关闭按钮
    private Button upgrade;            //升级确认按钮
    private Button[] secondElements;   //二级元素按钮
    private int secondEleNum = 4;      //二级元素个数
    private Queue<int> queue = new Queue<int>(); //存储选中的二级元素
    private Dictionary<int, Image> elements = new Dictionary<int, Image>(); //技能button和遮罩，遮罩的alpha值为0表示已经升级

    private Image notEnoughHint;  //碎片不足的提示
    private Text[] cannotUPHints = new Text[4];  //已满级文本

    // Use this for initialization
    void Start()
    {
        //按钮：升级、关闭、二级元素
        upgrade = transform.Find("upgrade").GetComponent<Button>();
        close = transform.Find("close").GetComponent<Button>();
        secondElements = transform.Find("elements").GetComponentsInChildren<Button>();
        notEnoughHint = transform.Find("patchNotEnough").GetComponent<Image>();
        notEnoughHint.gameObject.SetActive(false);

        //按钮事件：确认升级、关闭升级窗口
        upgrade.onClick.AddListener(UpdateElement);
        close.onClick.AddListener(CloseUpdateWindow);
        for (int i = 0; i < secondEleNum; i++)
        {
            secondElements[i] = transform.Find("elements").GetChild(i).GetComponent<Button>();
            cannotUPHints[i] = secondElements[i].transform.Find("cannotUP").GetComponent<Text>();
            cannotUPHints[i].gameObject.SetActive(false);
            elements.Add(i + 1, secondElements[i].transform.Find("shadow").GetComponent<Image>());
            int ii = i;
            secondElements[i].onClick.AddListener(delegate () { this.SelectElement(ii + 1); });  //二级技能ID ii+1 = 1 2 3 4

            //每次打开升级页面，首先从历史数据中获取每个元素的等级，若是2级，就高亮，并设为不可用
            if (PlayerController.player.skillLv[i]==2)
            {
                Global.ChangeShadowColor(elements[i + 1], true);
                secondElements[i].enabled = false;
                cannotUPHints[i].gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 判断碎片是否足够
    /// </summary>
    /// <returns></returns>
    private bool IsPatchEnough()
    {
        return BasePlayerAttribute.instance.CanUpSkill();
    }

    /// <summary>
    /// 点击选中二级元素（碎片不足时，提示碎片不足）
    /// </summary>
    /// <param name="eleName"></param>
    private void SelectElement(int eleID)
    {
        if (!IsPatchEnough())
        {
            notEnoughHint.gameObject.SetActive(true);
            return;
        }
        if (queue.Count >= 1)
        {
            Global.ChangeShadowColor(elements[queue.Dequeue()], false);
        }
        queue.Enqueue(eleID);
        Global.ChangeShadowColor(elements[eleID], true);
    }

    /// <summary>
    /// 确认升级二级元素，并返回战斗界面（碎片不足时，提示碎片不足）
    /// </summary>
    private void UpdateElement()
    {
        if (!(BasePlayerAttribute.instance.nowSkillLvNum >= 4))
        {
            if (!IsPatchEnough())
            {
                notEnoughHint.gameObject.SetActive(true);
                return;
            }
            if (queue.Count > 0)
            {
                secondElements[queue.Peek() - 1].enabled = false;
                UpdateEleCore(queue.Dequeue());
            }
        }
        notEnoughHint.gameObject.SetActive(false);
        CloseUpdateWindow();
        Time.timeScale = 1;
    }

    /// <summary>
    /// 具体的升级表现
    /// </summary>
    /// <param name="selectedEle"></param>
    private void UpdateEleCore(int skillID)
    {
        PlayerController.player.SkillLevelUp(skillID);
        cannotUPHints[skillID - 1].gameObject.SetActive(true);
    }

    /// <summary>
    /// 关闭升级元素窗口
    /// </summary>
    private void CloseUpdateWindow()
    {
        notEnoughHint.gameObject.SetActive(false);
        gameObject.SetActive(false);
        EventManager.AllEvent.OnEasyTouchSet(true);//开启轮盘
        Time.timeScale = 1;
    }
}
