using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerLearnController : MonoBehaviour {

    public static PlayerLearnController learnController;

    [Header("新手对话框")]
    public GameObject TextMask;
    private GameObject TextGo;
    public GameObject[] characterBg;
    [Header("第一个障碍物")]
    public GameObject firstObstacles;
    [Header("第二个障碍物")]
    public GameObject secondObstacles;
    [Header("第三个障碍物")]
    public GameObject thirdObstacles;
    [Header("第四个障碍物")]
    public GameObject fourthObstacles;
    [Header("第五个障碍物")]
    public GameObject fifthObstacles;
    [Header("第六个障碍物")]
    public GameObject sixthObstacles;
    [Header("第三个触发点的敌人数组")]
    public GameObject[] thirdTriggerEnemy;
    [Header("第五个触发点的物体，这是个刷怪点")]
    public GameObject fifthTrigger;
    [Header("第六个触发点的物体，这是怪物数组")]
    public GameObject[] sixthTrigger;
    [Header("最后一个触发点的物体，这是boss房间")]
    public GameObject lastTrigger;
    [Header("最后一个房间怪物的存放地点")]
    public GameObject lastAllEnemy;
    [Header("通关传送阵")]
    public GameObject portal;
    [Header("传送阵生成地点")]
    public GameObject portalTarget;
    private Text text;
    /// <summary>
    /// 完成觉醒
    /// </summary>
    private bool finishAwake=false;
    int index = 0;//新手教程的顺序
    private void Awake()
    {
        learnController = this;
    }
    private void Start()
    {
        //如果是新手关
        //加载mask
        TextGo=Instantiate(TextMask, transform.Find("LearnText"));
        text = TextGo.transform.Find("LearnText").GetComponent<Text>();
        text.text = "点击<color=orange>左下角空白区域</color>滑动轮盘进行移动，请尝试移动到河对岸";
        GameObject.Find("Canvas/Panel/BattlePage(Clone)/Elements/2").SetActive(false);//隐藏其他技能
        GameObject.Find("Canvas/Panel/BattlePage(Clone)/Elements/3").SetActive(false);//隐藏其他技能
        GameObject.Find("Canvas/Panel/BattlePage(Clone)/Elements/4").SetActive(false);//隐藏其他技能
        characterBg[0] = TextGo.transform.Find("Player").gameObject;
        characterBg[1] = TextGo.transform.Find("Split").gameObject;
        characterBg[2] = TextGo.transform.Find("Explosion").gameObject;
    }
    /// <summary>
    /// 完成一次新手关卡
    /// </summary>
    public void FinishALearn()
    {
        if (index == 5)//完成5号新手教程，允许进入第三个地图
        {
            text.text = "干得漂亮！你击杀了所有敌人,请通过<color=orange>右方的大桥</color>前往下个地图";
            //触发下一个点
            SixthLearn();
        }
    }
    /// <summary>
    /// 第一张图的桥头事件
    /// </summary>
    public void FirstLearn()
    {
        text.text = "<color=orange>拖动并按住</color>右下角的滑轮进行攻击，请尝试杀死前方的河对岸的<color=orange>三只</color>四脚鸡";
        index = 1;
    }
    /// <summary>
    /// 第一张图的三只新手怪
    /// </summary>
    public void SecondLearn0()
    {
        text.text = "你成功击杀了一只四脚鸡,击杀有几率<color=orange>爆落</color>回血球与技能碎片";
        index = 2;
    }
    /// <summary>
    /// 第一张图的三只新手怪死亡
    /// </summary>
    public void SecondLearn1()
    {
        text.text = "干得漂亮！你成功击杀了所有敌人,请前往<color=orange>右上方</color>的桥进入新的地图";
        Destroy(firstObstacles);//销毁障碍物
    }
    /// <summary>
    /// 第二张图的入口
    /// </summary>
    public void ThirdLearn()
    {
        text.text = "你解锁了3种新的攻击元素,<color=orange>点击右下角技能图标</color>切换你的攻击属性,尝试击杀火属性免疫的敌人吧";
        secondObstacles.GetComponent<BoxCollider>().enabled= false;
        GameObject.Find("Canvas/Panel/BattlePage(Clone)/Elements/2").SetActive(true);
        GameObject.Find("Canvas/Panel/BattlePage(Clone)/Elements/3").SetActive(true);
        GameObject.Find("Canvas/Panel/BattlePage(Clone)/Elements/4").SetActive(true);
        characterBg[0].SetActive(false);
        characterBg[1].SetActive(true);
        for (int i = 0; i < thirdTriggerEnemy.Length; i++)
        {
            thirdTriggerEnemy[i].GetComponent<EnemyAI>().enabled = true;
        }
        index = 3;
    }
    /// <summary>
    /// 第二张图的三只新手怪
    /// </summary>
    /// <param name="enemyNum"></param>
    public void FourthLearn0(int enemyNum)
    {
        text.text = "火属性免疫的敌人还剩下"+enemyNum+"个,加油";
    }
    /// <summary>
    /// 第二张图的三只新手怪死亡
    /// </summary>
    public void FourthLearn1()
    {
        text.text = "你成功击杀了所有的火属性免疫敌人!3秒后将出现新的敌人";
        index = 4;
        StartCoroutine(FifthLearnCoroutine());
    }
    IEnumerator FifthLearnCoroutine()
    {
        yield return new WaitForSeconds(3);
        text.text = "新的敌人已经出现，击杀所有的敌人后可继续前进";
        index = 5;
        //开始刷怪
        FifthLearn();
    }
    /// <summary>
    /// 第二张图的刷怪事件
    /// </summary>
    public void FifthLearn()
    {
        fifthTrigger.GetComponent<StorySummonTerrainController>().StartSummon();
    }
    /// <summary>
    /// 第二张图的击杀所有怪事件
    /// </summary>
    public void SixthLearn()
    {
        Destroy(thirdObstacles);//销毁障碍物
        index = 6;
    }
    /// <summary>
    /// 第三张图的入口
    /// </summary>
    public void SeventhLearn()
    {
        text.text = "你获得了新的能力,点击<color=orange>右侧的觉醒按钮</color>即可<color=orange>同时选择两种元素</color>属性,开启后你有30秒的持续时间";
        BaseCharacter.player.AngerValue = 99;
        BaseCharacter.player.angerIncrease = 4;
        BaseCharacter.player.angerIncreaseTime = 30;
        EventManager.AllEvent.OnPlayerDataChange(PlayerDataType.Anger);
        characterBg[1].SetActive(true);
        characterBg[2].SetActive(false);
        for (int i = 0; i < sixthTrigger.Length; i++)
        {
            sixthTrigger[i].GetComponent<EnemyAI>().enabled = true;
        }
        Destroy(fourthObstacles);
        index = 7;
    }
    /// <summary>
    /// 当点击觉醒按钮之后
    /// </summary>
    public void EighthLearn()
    {
        if (!finishAwake)
        {
            text.text = "选择两种元素后,你将同时获得两种元素的属性:如木+风=<color=orange>多重跟踪</color>";
            finishAwake = true;
            StartCoroutine(NinethLearn());
        }
        index = 8;
    }
    /// <summary>
    /// 点击觉醒按钮后开始计时
    /// </summary>
    /// <returns></returns>
    IEnumerator NinethLearn()
    {
        yield return new WaitForSeconds(30);//觉醒30秒持续时间结束
        index = 9;
        text.text = "觉醒时间结束,每次觉醒值满后使用将获得<color=orange>15秒觉醒时间</color>,现在前往下个地图进入最终战斗吧";
        //恢复觉醒怒气的属性
        BaseCharacter.player.angerIncreaseTime = 15;
        //关闭障碍阻挡
        Destroy(fifthObstacles);
        index = 9;
    }
    /// <summary>
    /// 最后一张地图的入口触发点
    /// </summary>
    public void TenthLearn()
    {
        AudioManager.Instance.ChangeMusic(AudioType.Battle);
        text.text = "请小心前方的牛魔王分身,虽然无法使用本体技能,但他依然十分强大";
        AI[] allAi = lastTrigger.GetComponentsInChildren<AI>();
        characterBg[2].SetActive(true);
        characterBg[0].SetActive(false);
        for (int i = 0; i < allAi.Length; i++)
        {
            allAi[i].GetComponent<AI>().enabled = true;
        }
        index = 10;
        Destroy(sixthObstacles);
        StartCoroutine(BossLearn());
    }
    /// <summary>
    /// 遇到boss后的部分提示
    /// </summary>
    /// <returns></returns>
    IEnumerator BossLearn()
    {
        yield return new WaitForSeconds(10);
        text.text = "注意牛魔王分身脚下的光环,它能<color=orange>免疫对应的元素攻击</color>！";
        yield return new WaitForSeconds(30);
        text.text = "";
        TextGo.SetActive(false);
    }

    public void BossDeath()
    {
        TextGo.SetActive(true);
        text.text = "恭喜你成功击杀了牛魔王分身,你获得了<color=orange>一次强化的机会</color>,进入传送门即可前往下一个世界";
        //召唤升级buff
        Destroy(lastAllEnemy);//销毁剩余怪物
        BattlePanel.Instance.MaskFadeIn();
        ShowPortal();
    }
    /// <summary>
    /// 出现传送门
    /// </summary>
    public void ShowPortal()
    {
        //保存数据
        SaveAndLoad.SaveGameData(BaseCharacter.player);
        SaveAndLoad.saveCurrentLevel(BasePlayerAttribute.instance.nowScene + 1);
        PlayerPrefs.SetInt("InfiniteTrigger", 1);//开启无尽模式
        //生成传送门
        Instantiate(portal, portalTarget.transform.position, portal.transform.rotation);
    }
}
