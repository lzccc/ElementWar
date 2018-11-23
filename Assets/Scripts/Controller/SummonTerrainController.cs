using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 召唤怪物的地块的控制器
/// 绑定在地块的父物体上
/// </summary>
public class SummonTerrainController : MonoBehaviour
{
    [Header("无尽模式")]
    public bool isInfinite;
    [Header("允许使用的次数(无尽下无用)")]
    public int useCount = 2;
    [Header("使用间隔")]
    public float useCoolTime = 15;
    private float useCoolTimer;
    private bool useTimerTrigger = false;
    private BoxCollider boxcollider;
    SummonMonsterTerrain[] smt;
    private Transform characterManager;
    private float updateCounter = 2;//每两秒判断一次是否杀完怪

    public int nextFlowNum;
    //波数
    private int counter;
    private void Awake()
    {
        useCoolTimer = useCoolTime;
        boxcollider = GetComponent<BoxCollider>();
        smt = GetComponentsInChildren<SummonMonsterTerrain>();
        characterManager = GameObject.Find("CharacterManager").transform;
        nextFlowNum = 5;
    }

    private void Update()
    {
        updateCounter -= Time.deltaTime;
        if (updateCounter <= 0)
        {
            updateCounter = 2;
            if (!useTimerTrigger)
            {
                if(isInfinite )//无尽
                {
                    if (characterManager.childCount <= 1)
                    {
                        if (counter != 0)
                        {
                            EventManager.AllEvent.OnMesShowEventUse("本波怪物已经消灭尽,休息" + useCoolTime + "秒后将出现第" + (counter + 1) + "波");
                            useTimerTrigger = true;
                        }
                    }
                }
                else//剧情模式
                {
                    if (characterManager.childCount <= 1)
                    {
                        if (counter != 0)
                        {
                            EventManager.AllEvent.OnMesShowEventUse("敌人已经全部消灭!");
                            useTimerTrigger = true;
                        }
                    }
                }

            }
        }
        if (useTimerTrigger)//开启休息时间的计数
        {
            useCoolTimer -= Time.deltaTime;
            if (useCoolTimer <= 0)
            {
                useTimerTrigger = false;
                counter++;
                if (counter % 5 == 0)
                {
                    StartBoss();//满足5波一次boss
                }
                else
                {
                    StartSummon();
                }
                useCoolTimer = useCoolTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EventManager.AllEvent.OnMesShowEventUse("第一波敌人已经出现");
        boxcollider.enabled = false;
        counter++;
        if (isInfinite)
            useTimerTrigger = false;//这是无尽模式的开关
        StartSummon();
    }

    public void StartSummon()
    {
        if (isInfinite)
        {
            int summonedNum = 0;
            while (summonedNum < nextFlowNum)
            {
                for (int i = 0; i < smt.Length; i++)
                {
                    smt[i].StartSummon2(characterManager);
                }
                summonedNum += smt.Length;
            }
            nextFlowNum = 5 * (counter + 1);
        }
        
    }
    public void StartBoss()
    {
        EventManager.AllEvent.OnMesShowEventUse("Boss已经出现");

        for (int i = 0; i < smt.Length; i++)
        {
            smt[i].StartBoss(characterManager);
        }
    }
}
