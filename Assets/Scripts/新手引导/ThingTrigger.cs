using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingTrigger : MonoBehaviour {
    
    //对应id的场景
    //1:第一张图的过桥触发器
    //2:第一张图的三只小怪
    //3:第二张图的入口
    //4：第二张图的三只小怪
    //5：第三张图的入口
    //21，22，23，2开头表示障碍墙,空气墙
    //99统一表示剧情关卡的空气墙，需要一个刷怪点物体脚本判断波数

    public int triggerId;
    public StorySummonTerrainController sstc;
    private Transform thisParent;
    private GameObject characterManager;
    [Header("通关传送阵")]
    public GameObject portal;
    [Header("传送阵生成地点")]
    public GameObject portalTarget;
    public GameObject CharacterManager
    {
        get
        {
            if (characterManager == null)
            {
                characterManager = GameObject.Find("CharacterManager");
            }return characterManager;
        }
    }
    private void Awake()
    {
        thisParent = transform.parent;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (triggerId == 21)
        {
            EventManager.AllEvent.OnMesShowEventUse("击败所有四脚鸡才能继续往前");
        }
        else if (triggerId == 22)
        {
            EventManager.AllEvent.OnMesShowEventUse("击败所有敌人才能继续往前");
        }
        else if (triggerId == 23)
        {
            EventManager.AllEvent.OnMesShowEventUse("前方的挑战较为艰险,请体验并学会并使用觉醒技能后再前进!");
        }else if (triggerId == 99)
        {
            if (CharacterManager.transform.childCount <= 1)//表示清光怪了
            {
                if (sstc.nowNum >= sstc.summonNum)
                {
                    //PanelMgr.instance.OpenPanel<SuccessPanel>("", true);
                    BattlePanel.Instance.MaskFadeIn();
                    Destroy(gameObject);
                }else
                {
                    EventManager.AllEvent.OnMesShowEventUse("击败所有敌人才能继续往前");
                }
            }
            else
            {
                EventManager.AllEvent.OnMesShowEventUse("击败所有敌人才能继续往前");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (triggerId == 1)
            {
                PlayerLearnController.learnController.FirstLearn();
            } 
            else if (triggerId == 3)
            {
                PlayerLearnController.learnController.ThirdLearn();
            }
            else if (triggerId == 5)
            {
                PlayerLearnController.learnController.SeventhLearn();
            } else if (triggerId == 6)
            {
                PlayerLearnController.learnController.TenthLearn();
            } 
        }
    }
    public void OnLearnEnemyDeath()
    {
        if (triggerId == 2)
        {
            if (thisParent.childCount > 1)
            {
                PlayerLearnController.learnController.SecondLearn0();
            }
            else
            {
                PlayerLearnController.learnController.SecondLearn1();
                Destroy(transform.parent.gameObject);
            }
        }
        else if (triggerId == 4)
        {
            if (thisParent.childCount > 1)
            {
                PlayerLearnController.learnController.FourthLearn0(thisParent.childCount-1);
            }
            else
            {
                PlayerLearnController.learnController.FourthLearn1();
                Destroy(transform.parent.gameObject);
            }
        }else if (triggerId == 10)//表示boss
        {
            PlayerLearnController.learnController.BossDeath();
        }
        
    }
}
