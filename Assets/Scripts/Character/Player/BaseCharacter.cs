using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class BaseCharacter : MonoBehaviour {

    //这个类用来驱动角色
    public static BaseCharacter player;
    [Header("当前生命")]
    [SerializeField]private int health;
    [Header("当前碎片数")]
    [SerializeField]private int elementFragment;
    [Header("当前愤怒值")]
    [SerializeField]private float angerValue;
    [Header("愤怒增加速度（每秒）")]
    public float angerIncrease = 5;
    [Header("愤怒觉醒持续时间")]
    public float angerIncreaseTime=15;
    [Header("移动速度")]
    public float speed = 3;
    [Header("大小，用于判断撞墙")]
    public float radiusSize = 3;
    [Header("buff状态列表")]
    public List<Buff> buffList = new List<Buff>();
    [Header("操作轮盘所在对象")]
    public GameObject Joystick;
    /// <summary>
    /// 处于眩晕状态
    /// </summary>
    public bool isVertigo = false;
    /// <summary>
    /// 无敌状态
    /// </summary>
    public bool isInvincible = false;

    public bool isInMarsh=false;//已经被沼泽减速
    private TurnRed beHart;
    public TurnRed BeHart
    {
        get
        {
            if (beHart == null)
            {
                beHart = GetComponentInChildren<TurnRed>();
            }
            return beHart;
        }
    }
    private float repelTimeCounter = 0;//被击退时间计数
    private float repelSpeed;//被击退速度
    private bool repelTrigger = false;//被击退开关
    private Vector3 forward;//被击退方向
    
    private bool angerIncreaseTrigger=true;//愤怒值使用中的参数，为false不增加怒气
    [SerializeField]private float angerIncreaseTimer;

    #region 玩家的属性
    private Animator anim;
    public Animator Anim
    {
        get
        {
            if (anim == null)
            {
                anim = GetComponentInChildren<Animator>();
            }
            return anim;
        }
    }
    /// <summary>
    /// 生命值
    /// </summary>
    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            if (!isInvincible)
            {
                if (health <= 0)
                {
                    //触发死亡方法
                    OnDeath();
                }
                else
                {
                    if (value > BasePlayerAttribute.instance.maxHealth)
                    {
                        health = BasePlayerAttribute.instance.maxHealth;
                    }
                    else if (value <= 0)
                    {
                        health = 0;
                    }
                    else
                    {
                        health = value;
                        BeHart.OnHurtColor();
                    }
                    EventManager.AllEvent.OnPlayerDataChange(PlayerDataType.Hp);
                }
            }
        }
    }
    /// <summary>
    /// 碎片数
    /// </summary>
    public int ElementFragment
    {
        get
        {
            return elementFragment;
        }

        set
        {
            elementFragment = value;
            if (elementFragment >= BasePlayerAttribute.instance.GetSkillUpFragment())
            {
                BattlePanel.Instance.SetHand(true);//显示升级元素的提示
            }
            else
            {
                BattlePanel.Instance.SetHand(false);
            }
        }
    }
    /// <summary>
    /// 愤怒值
    /// </summary>
    public float AngerValue
    {
        get
        {
            return angerValue;
        }

        set
        {
            if (value > BasePlayerAttribute.instance.maxAngerValue)
            {
                angerValue = BasePlayerAttribute.instance.maxAngerValue;
            }
            else {
                angerValue = value;
            }
            if (angerValue <= 0) angerValue = 0;
        }
    }
    /// <summary>
    /// 增加生命
    /// </summary>
    /// <param name="add"></param>
    public void AddHealth(int add) {
        Health += add;
        EventManager.AllEvent.OnPlayerDataChange(PlayerDataType.Hp);
    }
    
    /// <summary>
    /// 减少生命
    /// </summary>
    /// <param name="remove"></param>
    public void removeHealth(int remove) {
        Health -= remove;
        EventManager.AllEvent.OnPlayerDataChange(PlayerDataType.Hp);
    }
    /// <summary>
    /// 增加愤怒值
    /// </summary>
    /// <param name="add"></param>
    public void AddAngerValue(float add)
    {
        add +=add * (BasePlayerAttribute.instance.GetBuffValueForId(2)/100f);
        AngerValue += add;
        EventManager.AllEvent.OnPlayerDataChange(PlayerDataType.Anger);
    }
    /// <summary>
    /// 重置愤怒值
    /// </summary>
    /// <param name="reset"></param>
    public void ResetAngerValue() {
        AngerValue = 0;
        EventManager.AllEvent.OnPlayerDataChange(PlayerDataType.Anger);
    }
    /// <summary>
    /// 增加碎片数
    /// </summary>
    /// <param name="add"></param>
    public void AddFragment(int add) {
        ElementFragment += add;
        EventManager.AllEvent.OnPlayerDataChange(PlayerDataType.Fragment);
    }
    
    /// <summary>
    /// 升级技能减少碎片数量
    /// </summary>
    /// <param name="remove"></param>
    public void RemoveFragment(int remove) {
        ElementFragment -= remove;
        BasePlayerAttribute.instance.AddSkillNum();//升级技能后增加2级技能个数
        EventManager.AllEvent.OnPlayerDataChange(PlayerDataType.Fragment);
    }
    
    ///// <summary>
    ///// 攻击回复生命值
    ///// </summary>
    ///// <param name="absorb"></param>
    //public void Absorb(int absorb) {
    //    Health += (int)(absorb * BasePlayerAttribute.instance.absorbRate);
    //    EventManager.AllEvent.OnPlayerDataChange(PlayerDataType.Hp);
    //}

    #endregion


    protected float buffTimer;//buff的计时器
    protected bool buffTrigger;//buff的启动开关

    private void Awake()
    {
        player = this;
        angerIncreaseTimer = 0;
        elementFragment = 0;//碎片
        angerValue = 0;//愤怒归零
    }
    private void Start()
    {
        //后期在这里读取数据
        //读取数据
        if (!Global.newGame&&!BasePlayerAttribute.instance.inInfinite)
        {
            Debug.Log("进了无尽模式加载数据");
            SaveAndLoad.LoadGameData(this);
        }
        else
        {
            Global.newGame = false;
        }
        health = BasePlayerAttribute.instance.maxHealth;
    }
    private void Update()
    {
        if (transform.position.y <= -10)//掉出去死亡
        {
            OnDeath();
            gameObject.SetActive(false);
        }
        OnUpdate();
    }
    public void OnUpdate()
    {
        if (angerIncreaseTrigger&&angerValue < BasePlayerAttribute.instance.maxAngerValue)//自动增长愤怒值
        {
            AddAngerValue(Time.deltaTime * angerIncrease);//增长愤怒
            if(angerValue>= BasePlayerAttribute.instance.maxAngerValue)
            {
                BattlePanel.Instance.AwakeAppearOrNot(true);
                angerValue = BasePlayerAttribute.instance.maxAngerValue;
            }
        }
        if (angerIncreaseTimer > 0)//愤怒觉醒持续时间还未结束，不开启愤怒增长
        {
            angerIncreaseTimer -= Time.deltaTime;
            if (angerIncreaseTimer <= 0)
            {
                decrease = false;//停止减少能量条
                angerIncreaseTrigger = true;
                //调用结束觉醒状态方法
                BattlePanel.Instance.AwakeAppearOrNot(false);
                if (!BasePlayerAttribute.instance.inInfinite && BasePlayerAttribute.instance.nowScene == 1)//新手村背景音乐
                {
                    AudioManager.Instance.ChangeMusic(AudioType.NewVillage);
                }
                else//正常背景音乐
                {
                    AudioManager.Instance.ChangeMusic(AudioType.PlayingGame);
                }
                PlayerController.player.InputSkillId(BattlePanel.Instance.queue.Peek());
            }
        }
        if (decrease)
        {
            angerImg.fillAmount -= Time.deltaTime / angerIncreaseTime;//慢慢减少能量条
            AngerValue -= (100 / angerIncreaseTime) * Time.deltaTime;//减少愤怒
        }
        if (buffTrigger)
        {
            buffTimer += Time.deltaTime;
        }
        if (buffList.Count != 0)//判断身上的buff是否到期
        {
            if (buffTimer >= 1)//1秒计时器，执行一次buff伤害
            {
                for (int i = 0; i < buffList.Count; i++)
                {
                    buffList[i].HarmBuffSkill();
                }
                buffTimer = 0;
            }
            buffTrigger = true;
            for (int i = 0; i < buffList.Count; i++)//扣除流去的时间
            {
                if (buffList[i].SubtractTime(Time.deltaTime))
                {
                    buffList[i].ReBuff();
                    buffList.Remove(buffList[i]);
                }
            }
        }
        else
        {
            buffTrigger = false; buffTimer = 0;
        }
        if (repelTrigger)//这里是被击退
        {
            RaycastHit hit;
            bool isHit = Physics.Raycast(transform.position, forward, out hit,radiusSize);
            if (isHit)
            {
                if (hit.collider.CompareTag(CharacterType.Wall.ToString())|| hit.collider.CompareTag(CharacterType.SmallWall.ToString()))
                {
                    repelTrigger = false;
                }
            }
            repelTimeCounter -= Time.deltaTime;
            transform.position += forward * Time.deltaTime * repelSpeed;
            if (repelTimeCounter <= 0)
            {
                repelTrigger = false;
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
    bool decrease=false;
    private Image angerImg;
    /// <summary>
    /// 使用愤怒觉醒
    /// </summary>
    public void UseAnger(Image anger)
    {
        AudioManager.Instance.ChangeMusic(AudioType.Awake);//播放觉醒时间
        angerImg = anger;
        //ResetAngerValue();//怒气归零
        //打开开关开始慢慢减少怒气条
        decrease = true;
        angerIncreaseTrigger = false;//关闭增长怒气开关
        angerIncreaseTimer = angerIncreaseTime;//开始持续计时
    }

    public void OnDeath()
    {
        Joystick.SetActive(false);//关闭操作
        Anim.SetTrigger(AnimType.Death.ToString());
    }

    public void OnDeathAnimFinish()
    {
        Destroy(PlayerController.player.bloodMask);
        GameObject.Find("Main Camera").GetComponent<Follow>().isGameSuccess = true;
        GameObject.Find("Main Camera").GetComponent<LightingChange>().isGameSuccess = true;
        EventManager.AllEvent.OnGameFinish(GameFinishType.Fail);
    }
    /// <summary>
    /// 继续开始游戏
    /// </summary>
    public void RePlay()
    {
        //加载场景
    }
}
