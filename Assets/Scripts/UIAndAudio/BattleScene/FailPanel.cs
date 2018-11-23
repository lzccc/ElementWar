using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class FailPanel : PanelBase
{

    private Button returnToBeginScene;     //返回主界面

    private GameObject normal;
    private static Text killMonsterNum;    //击杀怪物数量

    private GameObject infinite;
    private Text thisKillNum;
    private Text thisBoshu;
    private Text historyKillNum;
    private Text historyBoshu;


    private void Start()
    {
        PanelMgr.instance.ClosePanel("BattlePanel");

        normal = GameObject.Find("normal").gameObject;
        infinite = GameObject.Find("infinite").gameObject;

        //如果是无尽模式，则normal不显示，否则infinite不显示
        if (BasePlayerAttribute.instance.inInfinite)
        {
            normal.SetActive(false);
            SaveAndLoad.SaveInfinityGameData(InfiniteModeSummon.instance.nextFlowNum, BasePlayerAttribute.instance.killNum);//存储数据
        }
        else
        {
            infinite.SetActive(false);
            //SaveAndLoad.SaveGameData(BaseCharacter.player);//存储数据
        }
    }

    public override void Init(params UnityEngine.Object[] args)
    {
        base.Init(args);
        //初始化预设体路径和面板层级
        skinPath = "FailPage";
        layer = PanelLayer.Tips;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;

        returnToBeginScene = skinTrans.Find("returnToBeginScene").GetComponent<Button>();
        returnToBeginScene.onClick.AddListener(Return);

        //正常模式
        killMonsterNum = GameObject.Find("killMonsterNum").GetComponent<Text>();
        killMonsterNum.text = BasePlayerAttribute.instance.killNum.ToString();

        //无尽模式
        thisKillNum = GameObject.Find("thisKillNum").GetComponent<Text>();
        thisBoshu = GameObject.Find("thisBoshu").GetComponent<Text>();
        historyKillNum = GameObject.Find("historyKillNum").GetComponent<Text>();
        historyBoshu = GameObject.Find("historyBoshu").GetComponent<Text>();

        if (BasePlayerAttribute.instance.inInfinite)
        {
            int[] data = SaveAndLoad.LoadInfinityData();
            //TODO 获取本地缓存数据
            thisKillNum.text = BasePlayerAttribute.instance.killNum.ToString();// 本次击杀怪物数
            thisBoshu.text = InfiniteModeSummon.instance.nextFlowNum.ToString(); //本次波数

            historyKillNum.text = data[1].ToString(); //历史击杀怪物数
            historyBoshu.text = data[0].ToString(); //历史波数
        }
    }

    private void CloseFailPanel()
    {
        PanelMgr.instance.ClosePanel("FailPanel");
    }

    private void Return()
    {
        AudioManager.Instance.PlaySound(AudioType.ButtonNormal);
        AudioManager.Instance.ChangeMusic(AudioType.BeginGame);
        SceneManager.LoadScene("BeginGame");
    }
}
