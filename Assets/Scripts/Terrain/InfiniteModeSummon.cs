using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 召唤怪物的地块的控制器
/// 绑定在地块的父物体上
/// 无尽模式专用
/// </summary>
public class InfiniteModeSummon : MonoBehaviour
{
    public static InfiniteModeSummon instance;
    [Header("使用间隔")]
    public float useCoolTime = 9;
    private float useCoolTimer;
    private bool useTimerTrigger = false;
    private BoxCollider boxcollider;
    SummonMonsterTerrain[] smt;
    private Transform characterManager;
    private float updateCounter = 0.5f;
    public int monsterWithBossNum = 10;
    public GameObject DirectionalLight;

    public int nextFlowNum;
    //波数
    private int counter;
    private void Awake()
    {
        instance = this;
        useCoolTimer = useCoolTime;
        boxcollider = GetComponent<BoxCollider>();
        smt = GetComponentsInChildren<SummonMonsterTerrain>();
        characterManager = GameObject.Find("CharacterManager").transform;
        nextFlowNum = 4;
    }

    void chooseBuff() {
        DirectionalLight.GetComponent<FadeLight>().UpGrade(2);

        //Time.timeScale = 0;        
        BattlePanel.Instance.MaskFadeIn();
    }

    private void Update()
    {
        updateCounter -= Time.deltaTime;
        if (updateCounter <= 0)
        {
            updateCounter = 0.5f;
            if (!useTimerTrigger)
            {
                if (characterManager.childCount <= 1)
                {
                    if (counter != 0)
                    {
                        useCoolTimer = useCoolTime;
                        useTimerTrigger = true;
                        StartCoroutine(waittime());
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
                    
                    if (counter == 10) {
                        StartSummon(monsterWithBossNum);
                        StartBoss();
                    }
                    if (counter == 15) {
                        StartBoss();
                        StartBoss();
                    }
                }
                else
                {
                    StartSummon(nextFlowNum);
                }
                useCoolTimer = useCoolTime;
            }
        }
    }
    IEnumerator waittime()
    {
        yield return new WaitForSeconds(1);
        chooseBuff(); //每次打boss前选择增益
        yield return new WaitForSeconds(3);
        EventManager.AllEvent.OnMesShowEventUse((useCoolTime - 4) + "秒后将出现第" + (counter + 1) + "波");
    }

    private void OnTriggerEnter(Collider other)
    {
        EventManager.AllEvent.OnMesShowEventUse("第一波怪物已经出现");
        boxcollider.enabled = false;
        counter++;
        useTimerTrigger = false;//这是无尽模式的开关
        StartSummon(nextFlowNum);
    }

    public void StartSummon(int Num)
    {
        int summonedNum = 0;
        while (summonedNum < Num)
        {
            for (int i = 0; i < smt.Length; i++)
            {
                smt[i].StartSummon2(characterManager);
            }
            summonedNum += smt.Length;
            Debug.Log(summonedNum);
        }
        nextFlowNum = 4 * (counter + 1);
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
