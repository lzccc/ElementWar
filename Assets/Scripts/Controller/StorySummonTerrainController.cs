using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySummonTerrainController : MonoBehaviour {

    public bool isLearn=false;
    [Header("刷怪的波数")]
    public int summonNum;
    [HideInInspector]
    public int nowNum = 0;
    [Header("刷怪的间隔")]
    public float summonCool;
    private float summonCooler;
    private bool summonCoolTrigger = false;
    private float updateCounter = 2;//每两秒判断一次是否杀完怪
    StorySummonTerrain[] sst;
    private Transform characterManager;
    private bool isClearTrigger=false;//判断是否清空敌人的开关
    private BoxCollider boxCollider;
    private bool isEnd = false;
    private void Awake()
    {
        sst = GetComponentsInChildren<StorySummonTerrain>();
        summonCooler = summonCool;
        characterManager = GameObject.Find("CharacterManager").transform;
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (isEnd) return;
        if (isClearTrigger)
        {
            updateCounter -= Time.deltaTime;
            if (updateCounter <= 0)
            {
                updateCounter = 1;
                if (!summonCoolTrigger)
                {
                    if (characterManager.childCount <= 1)
                    {
                        if (nowNum != 0)
                        {
                            if(nowNum < summonNum)
                            {
                                EventManager.AllEvent.OnMesShowEventUse("敌人已被消灭,新的敌人即将出现");
                            }
                            else
                            {
                                EventManager.AllEvent.OnMesShowEventUse("敌人已经全部消灭,你可以继续前进了");
                            }
                            summonCoolTrigger = true;
                        }
                    }
                }
            }
        }
        if (summonCoolTrigger)//召唤冷却时间，击杀所有敌人后开启
        {
            if (summonCooler > 0)
            {
                summonCooler -= Time.deltaTime;
                if (summonCooler <= 0)
                {
                    StartSummon();
                    summonCooler = summonCool;
                    summonCoolTrigger = false;
                }
            }
        }
    }

    public void StartSummon()
    {
        if (nowNum >= summonNum)//判断是否已经超出波数
        {
            if (isLearn)
            {
                PlayerLearnController.learnController.FinishALearn();//完成这一轮的新手教程
            }
            isEnd = true;
        }
        else
        {
            for (int i = 0; i < sst.Length; i++)
            {
                sst[i].StartSummon(characterManager, nowNum);
            }
            nowNum++; isClearTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(CharacterType.Player.ToString()))
        {
            boxCollider.enabled = false;
            StartSummon();
        }
    }

}
