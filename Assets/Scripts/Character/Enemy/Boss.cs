using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy {
    
    [Header("4种属性的光环，按火冰木风顺序")]
    public GameObject[] areaEffect_Boss;
    [Header("释放技能间隔")]
    public float skillCoolTime=6;
    private float skillCoolTimer;
    [Header("Boss普通攻击的特效")]
    public GameObject attackEffect;

    [Header("以下是火系技能数据")]
    [Header("召唤的怪物的数量")]
    public int SummonNum=4;
    [Header("召唤物出现位置的最小偏移，为负值")]
    public int minOffset=-10;
    [Header("召唤物出现位置的最小偏移，为正值")]
    public int maxOffset=10;

    [Header("以下是风系技能数据")]
    [Header("风系技能的攻击球")]
    public GameObject attackBall;
    [Header("风系技能旋转速度，角度单位")]
    public float rotateSpeed;
    [Header("风系技能旋转释放能量球的间隔")]
    public float ballCool=0.1f;
    [Header("风系技能的释放特效")]
    public GameObject windEffect;
    [Header("以下是木系技能数据")]
    [Header("召唤的毒雾")]
    public GameObject poisonousFog;//召唤的毒雾

    [Header("以下是土系技能数据")]
    [Header("冲撞判断范围")]
    public float radius=4;//冲撞判断范围
    [Header("冲撞伤害")]
    public int repelHarm = 100;//冲撞判断范围
    [Header("冲刺时间")]
    public float SprintTime = 2;
    [Header("眩晕和击退时间")]
    public float vertigoTime=2;
    [Header("冲刺速度")]
    public float sprintSpeed=15;
    [Header("击退玩家的速度")]
    public float repelPlayerSpeed=10;
    private float SprintTimer;
    bool attackTrigger = false;//冲刺标志位
    Vector3 playerForward;

    [Header("-------以下是被动技能-吸收的数据-----------")]
    [Header("吸收技能前摇特效")]
    public GameObject assimilateUseSkillEffect;
    [Header("吸收技能发动的比率分母值")]
    public float assimilateNum=4;
    [Header("吸收技能冷却时间")]
    public float assimilateCoolTime;
    private float assimilateCoolTimer;
    [Header("吸收技能持续时间")]
    public float assimilateKeepTime;
    [Header("吸收技能释放中特效")]
    public GameObject assimilateEffect;
    [Header("Boss死亡时出现的传送门(注意判断小boss不会出)")]
    public GameObject bossPortal;
    /// <summary>
    /// boss释放技能的无敌状态
    /// </summary>
    private bool invincible=false;

    private GameObject playerTarget;//目标玩家
    GameObject enemy2;//召唤的enemy
    /// <summary>
    /// 使用过一次吸收并且还没回到八分之一血为true
    /// </summary>
    //private bool isAssimilate=false;
    private bool skillTrigger = true;//释放其他技能时暂停计数
    private bool rotateTrigger = false;//旋转技能：风系的开关
    /// <summary>
    /// 对应元素的12秒内的伤害
    /// </summary>
    [SerializeField]private int[] elementHarm = new int[]
    {
        0,0,0,0
    };
    private float elementTimer = 12;//元素计时器,12秒更新一次并且变换属性
    private float timer=0.1f;//风系技能球计时器
    private BossAnimController anim;
    private void Awake()
    {
        enemy2 = Resources.Load<GameObject>("Enemy/ExplosionEnemy");
        playerTarget = GameObject.FindGameObjectWithTag(CharacterType.Player.ToString());
        anim = GetComponent<BossAnimController>();
    }
    public override void Start()
    {
        assimilateCoolTimer = assimilateCoolTime;
        SprintTimer = SprintTime;
        skillCoolTimer = skillCoolTime;//重置技能释放的冷却时间，0时释放技能
        attributeType = ElementAttributeType.Soil;
        base.Start();
        ShowAreaEffect();
        if(!IsLearnEnemy)
            AudioManager.Instance.ChangeMusic(AudioType.Battle);//boss战
    }

    private void Update()
    {
        if (isDeathTrigger) return;
        //吸收技能冷却时间的判断
        if (assimilateCoolTimer > 0)
        {
            assimilateCoolTimer -= Time.deltaTime;
        }
        //这个用来判断6秒一次的使用技能
        if (skillTrigger)
        {
            skillCoolTimer -= Time.deltaTime;
            if (skillCoolTimer <= 0)
            {
                UseSkill(); 
            }
        }
        //这个if用来判断12秒一次的状态切换
        if (elementTimer > 0)
        {
            elementTimer -= Time.deltaTime;
            if (elementTimer <= 0)
            {
                elementTimer = 0;
                ClearElementHarm();
            }
        }
        //这里是判断吸收技能的
        if ((float)hp / maxHp < 1f / assimilateNum)
        {
            if (assimilateCoolTimer<=0)
            {
                UseSkill_Assimilate();
                assimilateCoolTimer = assimilateCoolTime + assimilateKeepTime;//冷却时间加上持续时间是总冷却
            }
        }
        //这是旋转：风系技能的判断
        if (rotateTrigger)
        {
            transform.Rotate(Vector3.up * rotateSpeed*Time.deltaTime);
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                UseSkill_BarrageBall();
                timer = ballCool;
            }
        }
        //这是冲撞：土系技能的判断
        SprintTimer -= Time.deltaTime;
        if (attackTrigger)
        {
            transform.position += playerForward * sprintSpeed* Time.deltaTime;
            transform.forward = playerForward;
            RaycastHit hit;
            bool noNull = Physics.SphereCast(transform.position, 3, playerForward, out hit, 1);
            if (noNull)//碰撞到物体
            {
                if (hit.collider.CompareTag(CharacterType.Player.ToString()))
                {
                    BaseCharacter.player.Health -= repelHarm;
                    //眩晕
                    VertigoBuff vb = playerTarget.AddComponent<VertigoBuff>();
                    vb.buffTime = vertigoTime;
                    vb.UseBuff(playerTarget);
                    //击退玩家
                    RepelBuff rb = playerTarget.AddComponent<RepelBuff>();
                    rb.buffTime = vertigoTime;
                    rb.repelSpeed = repelPlayerSpeed;
                    rb.UseBuff(gameObject);
                    anim.StopAnim(AnimType.Sprint);//停止冲刺
                    attackTrigger = false;
                }
                else if (hit.collider.CompareTag(CharacterType.Wall.ToString()))
                {
                    anim.StopAnim(AnimType.Sprint);//停止冲刺
                    attackTrigger = false;
                    //播放撞墙的特效
                    EventManager.AllEvent.OnCameraShake(5, 1, 60);//摇晃镜头
                    AudioManager.Instance.PlaySound(AudioType.BossHitWall);//boss撞墙音效
                }
                else if (hit.collider.CompareTag(CharacterType.AttackWall.ToString()))
                {
                    EventManager.AllEvent.OnCameraShake(3, 0.3f, 60);//摇晃镜头
                    Destroy(hit.collider.gameObject);
                }
            }
        }
        if (SprintTimer <= 0)
        {
            anim.StopAnim(AnimType.Sprint);//停止冲刺
            attackTrigger = false;
        }
        OnUpdate();
    }


    #region 技能相关
    /// <summary>
    /// 清除元素伤害计数，进行状态转换,并且计数器清零
    /// </summary>
    public void ClearElementHarm()
    {
        int lastIndex = (int)attributeType -1;
        int maxIndex = 0;
        for (int i = 1; i < elementHarm.Length; i++)
        {
            if (elementHarm[i] > elementHarm[maxIndex])
            {
                maxIndex = i;
            }
        }
        elementHarm[0] = 0; elementHarm[1] = 0; elementHarm[2] = 0; elementHarm[3] = 0;//伤害计数清零
        while (maxIndex == lastIndex)//防止重复的boss属性
        {
            maxIndex = Random.Range(0, 4);
        }
        attributeType = (ElementAttributeType)(maxIndex + 1);
        elementTimer = 12;
        //调用显示光环方法
        ShowAreaEffect();
    }
    GameObject areaGo;
    public void ShowAreaEffect()
    {
        if (areaGo != null)
            Destroy(areaGo);
        areaGo = Instantiate(areaEffect_Boss[(int)attributeType-1], transform);
        areaGo.transform.localPosition = Vector3.zero;
    }
    

    /// <summary>
    /// 使用一个技能
    /// </summary>
    public void UseSkill()
    {
        if (IsLearnEnemy) return;//新手boss不使用技能
        AudioManager.Instance.PlaySound(AudioType.BossNoseSound);//播放使用技能音效
        skillCoolTimer = 6;//归为冷却时间
        skillTrigger = false;//关闭计数开关
        AI.SetAi(true);
        int id = (int)attributeType;
        if (id == 1)
        {
            UseSkill_Summon();//火系召唤
            PBar.startToShow(1.5f, "召唤南瓜怪");
            EventManager.AllEvent.OnMesShowEventUse("牛魔王正在召唤南瓜怪-自爆特性");
            anim.SetAnimator(AnimType.UseSkill);//释放技能使用动画
        }
        else if (id == 2)
        {
            UseSkill_Collision();//土系冲撞
            PBar.startToShow(1.5f, "无脑撞击");
            //不需要前摇动画，因为和冲撞单独绑定在一起
            EventManager.AllEvent.OnMesShowEventUse("牛魔王正在使用无脑撞击");
            anim.SetAnimator(AnimType.UseSprint);//使用冲刺动画
        }
        else if (id == 3)
        {
            UseSkill_PoisonousFog();//木系毒雾
            PBar.startToShow(1.5f, "毒气沼泽");
            EventManager.AllEvent.OnMesShowEventUse("牛魔王正在使用毒气沼泽");
            anim.SetAnimator(AnimType.UseSkill);//释放技能使用动画
        }
        else if (id == 4)
        {
            UseSkill_Barrage();//风系弹幕
            PBar.startToShow(1.5f, "风之弹幕");
            EventManager.AllEvent.OnMesShowEventUse("牛魔王正在使用风之弹幕");
            anim.SetAnimator(AnimType.UseSkill);//释放技能使用动画
        }
    }


    /// <summary>
    /// 使用吸收技能
    /// </summary>
    public void UseSkill_Assimilate()
    {
        if (IsLearnEnemy) return;//新手boss不使用技能
        //加载特效
        Instantiate(assimilateUseSkillEffect, transform);
        skillTrigger = false;//关闭计数开关
        PBar.startToShow(1.5f, "吸收护盾");
        StartCoroutine(beginAssimilate());

    }
    IEnumerator beginAssimilate()
    {
        anim.SetAnimator(AnimType.UseSkill);//释放技能使用动画
        invincible = true;
        //释放前摇
        yield return new WaitForSeconds(1.5f);
        invincible = false;
        AssimilateSkillResult asr = gameObject.AddComponent<AssimilateSkillResult>();
        asr.skillKeepTime = assimilateKeepTime;
        asr.skillKeepTime_Static = assimilateKeepTime;
        asr.assimilateEffect = assimilateEffect;
        asr.UseSkill(null, this, Vector3.zero);
        skillTrigger = true;//打开计数开关
    }

    /// <summary>
    /// 使用毒雾技能
    /// </summary>
    public void UseSkill_PoisonousFog()
    {
        //播放特效
        //Instantiate(playSkillEffect[3], transform);
        StartCoroutine(beginPoisonousFog());
    }
    IEnumerator beginPoisonousFog()
    {
        //前摇预警
        yield return new WaitForSeconds(1.5f);
        //加载到一个毒雾
        //随机好毒雾的位置（建议以boss为中心或随机搜索指定距离的地块)
        //在对应位置生成
        //float posX; float posZ; float posY;
        //GameObject go;
        //posX = Random.Range(-10, 10) + transform.position.x;
        //posZ = Random.Range(-10, 10) + transform.position.z;
        //posY = transform.position.y;
        /*go = */Instantiate(poisonousFog,transform.position,Quaternion.identity);
        //go.transform.position = new Vector3(posX, posY, posZ);
        skillTrigger = true;//开启技能计数开关
        AI.SetAi(false);//恢复ai寻路
    }

    /// <summary>
    /// 使用冲撞技能
    /// </summary>
    public void UseSkill_Collision()
    {
        //播放特效
        //Instantiate(playSkillEffect[2], transform);
        StartCoroutine(beginCollision());
    }
    IEnumerator beginCollision()
    {
        playerForward = (playerTarget.transform.position - transform.position).normalized;//前摇时固定方向
        //前摇预警
        yield return new WaitForSeconds(1.5f);
        anim.SetAnimator(AnimType.Sprint);//使用冲刺动画
        attackCoolTimer = attackCool;
        attackTrigger = true;
        SprintTimer = SprintTime;
        skillTrigger = true;//开启技能计数开关
        yield return new WaitForSeconds(SprintTimer);
        AI.SetAi(false);//恢复寻路
    }

    /// <summary>
    /// 使用弹幕技能
    /// </summary>
    public void UseSkill_Barrage()
    {
        //播放特效
        //Instantiate(playSkillEffect[4], transform);
        StartCoroutine(beginBarrage());
    }
    IEnumerator beginBarrage()
    {
        //前摇预警
        yield return new WaitForSeconds(1.5f);
        rotateTrigger = true;
        windEffectGo = Instantiate(windEffect, transform);//释放旋转特效
        yield return new WaitForSeconds(3);
        Destroy(windEffectGo);
        rotateTrigger = false;
        skillTrigger = true;//开启技能计数开关
        AI.SetAi(false);//恢复ai寻路
    }
    Vector3 offset = new Vector3(0, 0.5f, 1);
    GameObject ball;
    /// <summary>
    /// 发射风系技能球
    /// </summary>
    public void UseSkill_BarrageBall()
    {
        ball = Instantiate(attackBall, transform.position+ offset, Quaternion.identity);
        ball.GetComponent<AttackBallAI_Boss>().InitForward(transform.forward);
    }
    GameObject windEffectGo;

    /// <summary>
    /// 使用召唤技能
    /// </summary>
    public void UseSkill_Summon()
    {
        //播放特效
        //Instantiate(playSkillEffect[1], transform);
        StartCoroutine(beginSummon());
    }
    IEnumerator beginSummon()
    {
        //前摇预警
        yield return new WaitForSeconds(1.5f);
        float posX; float posZ; float posY;
        GameObject go;
        for (int i = 0; i < SummonNum; i++)
        {
            posX = Random.Range(minOffset, maxOffset) + playerTarget.transform.position.x;
            posZ = Random.Range(minOffset, maxOffset) + playerTarget.transform.position.z;
            posY = playerTarget.transform.position.y;
            go = ObjPoolManager.objpoolmanager.GetPoolsForName(PoolType.ExplosionEnemy.ToString()).Active(); //Instantiate(enemy2, transform.parent);
            go.GetComponent<NavMeshAgent>().Warp(new Vector3(posX, posY, posZ));
            go.transform.SetParent(transform.parent);
        }
        skillTrigger = true;//开启技能计数开关
        AI.SetAi(false);//恢复ai寻路
    }

    #endregion


    #region 其他

    /// <summary>
    /// 动画结束调用伤害
    /// </summary>
    public void OnAnimEndHarm()
    {
        BaseCharacter.player.Health -= attack;//扣血
    }
    /// <summary>
    /// 设置吸收状态
    /// </summary>
    /// <param name="b"></param>
    public override void SetAssimilate(bool b)
    {
        assimilateTrigger = b;
    }

    public override void Attack(GameObject target)
    {
        playerTarget = target;
        //播放boss动画,在动画的末尾调用伤害方法
        skillTrigger = false;//关闭计数开关
        transform.forward = target.transform.position - transform.position;//面朝玩家
        anim.SetAnimator(AnimType.Attack);
    }

    Vector3 normalAttackOffset = new Vector3(0, 0, -3);
    /// <summary>
    /// 攻击动画完成时调用
    /// </summary>
    public override void OnAttackAnimFinish()
    {
        //播放攻击特效
        Instantiate(attackEffect, transform.position, attackEffect.transform.rotation);
        EventManager.AllEvent.OnCameraShake(4, 0.2f, 60);//摇晃镜头
        skillTrigger = true;//开启计数开关
        attackCoolTimer = attackCool;//攻击冷却开启
        float range = Vector3.Distance(playerTarget.transform.position, transform.position);
        if (range <= attackRange)
        {
            BaseCharacter.player.Health -= attack;
        }
    }
    public override void HpChange(int hpNum, int[] skillId)
    {
        if (invincible) return;
        if (skillId != null&&!assimilateTrigger)//如果处于吸收技能状态，则直接跳过属性抵抗
        {
            for (int i = 0; i < skillId.Length; i++)
            {
                if (skillId[i] == (int)attributeType)
                {
                    //触发抵抗提示
                    EventManager.AllEvent.OnBossMsgShow("免疫", attributeType);
                    return;
                }
            }
            //后期可以改为id是一个数组，组合技能的id是复数
            if (skillId.Length == 1)//表示是基本四元素
            {
                elementHarm[skillId[0] - 1] += Mathf.Abs(hpNum);
            }
            else//表示是二级组合技能
            {
                int num = skillId.Length;
                for (int i = 1; i < skillId.Length; i++)
                {
                    elementHarm[skillId[i] - 1] += Mathf.Abs(hpNum / num);
                }
            }
        }
        base.HpChange(hpNum, skillId);
    }

    public override void OnDeath()
    {
        isDeathTrigger = true;
        //停止身上的AI脚本
        AI.OnDeathStopAI();
        StopAllCoroutines();
        anim.SetAnimator(AnimType.Death);
    }
    public void Death()
    {
        AudioManager.Instance.PlaySound(AudioType.BossDead);
        AudioManager.Instance.ChangeMusic(AudioType.PlayingGame);
        if (!IsLearnEnemy&&!BasePlayerAttribute.instance.inInfinite)//不是新手难度的小boss
        {
            //保存数据
            SaveAndLoad.SaveGameData(BaseCharacter.player);
            SaveAndLoad.saveCurrentLevel(BasePlayerAttribute.instance.nowScene+1);
            //Instantiate(bossPortal, transform.position+new Vector3(0,2,0), bossPortal.transform.rotation);//生成传送阵
            AudioManager.Instance.ChangeMusic(AudioType.FinalWin);
            BaseCharacter.player.isInvincible = true;
            Destroy(PlayerController.player.bloodMask);//销毁红色血雾
            EventManager.AllEvent.OnGameFinish(GameFinishType.Win);//游戏结束
        }
        base.OnDeath();
    }
    #endregion

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
