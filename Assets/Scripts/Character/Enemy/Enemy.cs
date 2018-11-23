using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour {
    [Header("是否是新手引导的怪物,是的话会调用死亡时触发的方法")]
    public bool IsLearnEnemy=false;
    [Header("精英怪开关")]
    public bool isMutatation = false;
    [Header("精英光环")]
    public GameObject areaEffect;
    [Header("精英怪的概率0-1")]
    public float mutatationProbability = 0.1f;
    [Header("生命值")]
    public int hp;
    [Header("最大生命值")]
    public int maxHp;
    [Header("攻击力")]
    public int attack;
    [Header("移动速度")]
    public float moveSpeed;
    [Header("攻击类型")]
    public EnemyAttackType attackType;
    [Header("攻击范围")]
    public float attackRange;
    [Header("攻击间隔")]
    public float attackCool;
    [Header("额外伤害")]
    public int extraHarm;
    [Header("buff列表")]
    public List<SkillResult> skillList = new List<SkillResult>();
    [Header("true为允许：0易伤,1灼烧,2减速")]
    public bool[] skillResultTrigger = new bool[]
    {
        true,true,true
    };
    [Header("元素属性")]
    public ElementAttributeType attributeType;
    [Header("回血球掉落概率")]
    public int hpBallDropRate=10;
    [Header("回血血球预制体")]
    public GameObject hpBall;
    [Header("碎片预制体")]
    public GameObject FragmentBall;
    [Header("碎片掉落个数")]
    public int fragmentNum;
    [Header("死亡特效(除了自爆怪)")]
    public GameObject deathEffect;
    [HideInInspector]
    public float attackCoolTimer;//攻击冷却的计数器
    [HideInInspector]
    public bool isInMarsh = false;//判断是否处于沼泽
    protected float buffTimer;//buff的计时器
    protected bool buffTrigger;//buff的启动开关
    protected float repelTimeCounter=0;//被击退时间计数
    protected float repelSpeed;//被击退速度
    protected bool repelTrigger = false;//被击退开关
    protected Vector3 forward;//被击退方向
    protected bool assimilateTrigger=false;//吸收伤害开关
    protected bool isDeathTrigger = false;
    private HpUIController hpUI;
    /// <summary>
    /// 存放爆落物的容器
    /// </summary>
    private GameObject dropThing;
    private TurnRed beHart;
    public HpUIController HpUI
    {
        get
        {
            if (hpUI == null)
            {
                if(attackType==EnemyAttackType.Boss)
                    hpUI = transform.Find("HpUI/Bg/Image").GetComponent<HpUIController>();
                else
                    hpUI = transform.Find("HpUI/Image").GetComponent<HpUIController>();
            }
            return hpUI;
        }
    }

    private NavMeshAgent agent;
    public NavMeshAgent Agent
    {
        get
        {
            if (agent == null)
            { 
                agent = GetComponent<NavMeshAgent>();
            }return agent;
        }
    }

    private AI ai;
    public AI AI
    {
        get
        {
            if (ai == null)
            {
                ai = GetComponent<AI>();
            }
            return ai;
        }
    }
    private ProcessBar processBar;
    /// <summary>
    /// 得到技能读条
    /// </summary>
    public ProcessBar PBar
    {
        get
        {
            if (processBar == null)
            {
                processBar = transform.Find("Process").GetComponent<ProcessBar>();
            }
            return processBar;
        }
    }

    public virtual void Start()
    {
        beHart = GetComponentInChildren<TurnRed>();
        HpUI.UpdateHp(hp, maxHp);//血条变化
        dropThing = GameObject.Find("场景/Terrain/Drop");
    }
    private void OnEnable()
    {
        //不在寻路范围内自动销毁
        if (agent != null && !(agent.isOnNavMesh))
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (isDeathTrigger) return;
        OnUpdate();
    }
    private SkillResult destroyGo;
    public void OnUpdate()
    {
        if (transform.position.y < -20)//如果穿透到地下就销毁
        {
            Destroy(gameObject);
        }
        if (attackCoolTimer > 0)//技能冷却时间
        {
            attackCoolTimer -= Time.deltaTime;
            if (attackCoolTimer <= 0)
                attackCoolTimer = 0;
        }
        if (buffTrigger)//buff的时间判断，如果每buff就会被归零
        {
            buffTimer += Time.deltaTime;
        }
        if (skillList.Count != 0)//判断身上的buff是否到期
        {
            if (buffTimer >= 1)//1秒计时器，执行一次buff伤害
            {
                for (int i = 0; i < skillList.Count; i++)
                {
                    skillList[i].HarmBuffSkill();
                }
                buffTimer = 0;
            }
            buffTrigger = true;
            for (int i = 0; i < skillList.Count; i++)//扣除流去的时间
            {
                if (skillList[i].SubtractTime(Time.deltaTime))
                {
                    destroyGo = skillList[i];
                    destroyGo.ReSkill();
                    skillList.Remove(skillList[i]);
                }
            }
        }
        else
        {
            buffTrigger = false; buffTimer = 0;
        }

        if (repelTrigger)//这里是被击退
        {
            repelTimeCounter -= Time.deltaTime;
            gameObject.transform.position += forward * Time.deltaTime * repelSpeed;
            if (repelTimeCounter <= 0)
            {
                repelTrigger = false;
            }
        }
    }
    
    /// <summary>
    /// 判断是否已经有该buff，有则刷新时间返回true
    /// </summary>
    /// <param name="id">技能buffid</param>
    /// <returns></returns>
    public void GetSkillById(int id)
    {
        lock (skillList)
        {
            for (int i = 0; i < skillList.Count; i++)
            {
                if (skillList[i].skillResultId == id)
                {
                    //skillList[i].skillKeepTime = skillList[i].skillKeepTime_Static;//如果存在技能则刷新时间
                    destroyGo = skillList[i];
                    destroyGo.ReSkill();
                    skillList.Remove(skillList[i]);
                }
            }
        }
        
    }

    /// <summary>
    /// 被击退方法
    /// </summary>
    /// <param name="forward">击退方向</param>
    /// <param name="speed">击退速度</param>
    /// <param name="time">击退时间</param>
    public void BeRepel(Vector3 forward, float speed, float time)
    {
        repelTimeCounter = time;
        repelSpeed = speed;
        this.forward = forward;
        repelTrigger = true;
    }
    /// <summary>
    /// 判断是否可以执行这个特效
    /// </summary>
    /// <param name="skill"></param>
    public bool GetSkillResultTrigger(int skillResultId)
    {
        return skillResultTrigger[skillResultId - 1];
    }
    #region 虚方法 

    /// <summary>
    /// 血量发生变化,负数为扣血正为加血
    /// </summary>
    /// <param name="hpNum"></param>
    /// <param name="skillId">技能id数组，用于判断技能属性,0表示是不需要判断的属性</param>
    public virtual void HpChange(int hpNum, int[] skillId)
    {
        if (skillId != null)
        {
            for (int i = 0; i < skillId.Length; i++)
            {
                if (skillId[i] == (int)attributeType)
                {
                    return;
                }
            }
        }
        if (hp <= 0)
        {
            OnDeath();
            return;
        }
        if (hpNum < 0)
        {
            hpNum += (int)(hpNum * ((extraHarm+ BasePlayerAttribute.instance.GetBuffValueForId(4)) / 100f));//易伤与玩家强壮加持
            if (assimilateTrigger)
            {
                hp -= hpNum;
            }
            else
            {
                hp += hpNum;
            }
            if (beHart!=null)
            {
                beHart.OnHurtColor();
            }
        }
        HpUI.UpdateHp(hp, maxHp);//血条变化
        if (hp <= 0)
        {
            hp = 0;
            OnDeath();
        }
    }

    /// <summary>
    /// 额外受伤比例发生变化
    /// </summary>
    /// <param name="extraNum"></param>
    public virtual void ExtraHarmChange(int extraNum)
    {
        extraHarm += extraNum;
        if (extraHarm < 0)
        {
            //Debug.Log("易伤特效出问题啦！变成负数了！");
            //EventManager.AllEvent.OnMesShowEventUse("易伤特效出问题啦！变成负数了！");
            extraHarm = 0;
        }else if (extraHarm > 20)
        {
            extraHarm = 20;
        }
    }
    /// <summary>
    /// 速度发生变化
    /// </summary>
    /// <param name="extraNum"></param>
    public virtual void SpeedChange(float speedNum)
    {
        moveSpeed += speedNum;
        Agent.speed = moveSpeed;
    }
    /// <summary>
    /// 攻击技能
    /// </summary>
    /// <param name="target"></param>
    public virtual void Attack(GameObject target)
    {

    }
    protected float random;
    GameObject tempGo;
    /// <summary>
    /// 死亡时调用
    /// </summary>
    public virtual void OnDeath()
    {
        BasePlayerAttribute.instance.killNum++;//增加1个击杀数量
        isDeathTrigger = true;
        //在原地生成一个血球
        random = Random.Range(0, 1);
        if (random <= hpBallDropRate / 100f)
        {
            if (attackType == EnemyAttackType.Boss)
            {
                for (int i = 0; i < 5; i++)
                {
                    tempGo = Instantiate(hpBall, transform.position + new Vector3(Random.Range(-2, 2), 0.5f, Random.Range(-2, 2)), Quaternion.identity);
                    //tempGo.transform.localPosition = new Vector3(Random.Range(-2,2),0, Random.Range(-2, 2)) + transform.localPosition;
                }
            }
            else
            {
                tempGo = Instantiate(hpBall, transform.position + new Vector3(Random.Range(-2, 2), 0.5f, Random.Range(-2, 2)), Quaternion.identity);
                //tempGo.transform.localPosition = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2))+ transform.localPosition;
            }
        }
        //在原地生成碎片
        for (int i = 0; i < fragmentNum; i++)
        {
            tempGo = Instantiate(FragmentBall, transform.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2)),Quaternion.identity);
            //tempGo.transform.position = ;
        }
        if (attackType != EnemyAttackType.Explosion)//不是自爆则加死亡特效
        {
            tempGo = ObjPoolManager.objpoolmanager.GetPoolsForName(deathEffect.name).Active();//死亡特效
            tempGo.transform.position = transform.position;
        }
        if (IsLearnEnemy)//是否时新手教程怪，是就执行死亡事件
        {
            SendMessage("OnLearnEnemyDeath");
        }
        if (attackType == EnemyAttackType.Boss||BasePlayerAttribute.instance.inInfinite)
            Destroy(gameObject);
        else
        {
            EventManager.AllEvent.OnCameraShake(2, 0.1f, 60);//摇晃镜头
            ObjPoolManager.objpoolmanager.GetPoolsForName(attackType.ToString() + "Enemy").Deactive(gameObject);
        }
    }
    /// <summary>
    /// 设置吸收特性开关，true为打开
    /// </summary>
    /// <param name="b"></param>
    public virtual void SetAssimilate(bool b)
    {

    }
    /// <summary>
    /// 回收到缓冲池被调用后再次初始化
    /// </summary>
    public virtual void ReStart()
    {
        hp = maxHp;attackCoolTimer = 0;
        for (int i = 0; i < skillList.Count; i++)
        {
            skillList[i].ReSkill();
        }
        skillList.Clear();
        extraHarm = 0;//额外伤害
        buffTrigger =false;//身上有无buff的开关
        repelTimeCounter = 0;//被击退时间计时器
        assimilateTrigger = false;//吸收属性
        HpUI.UpdateHp(hp,maxHp);
        isDeathTrigger = false;//是否时死亡
        mutatationProbability = 0.1f;//精英怪的概率
        AI.SetAi(false);//恢复自动寻路
    }

    public virtual void OnAttackAnimFinish()
    {

    }
    #endregion
}
