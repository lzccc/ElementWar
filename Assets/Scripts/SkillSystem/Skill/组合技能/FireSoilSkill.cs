using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 火土系组合技能
/// </summary>
public class FireSoilSkill : Skill {

    [Header("一个圈形的范围特效")]
    public GameObject explosionEffect;
    [Header("爆炸半径")]
    public float radius;
    [Header("爆炸伤害值")]
    public int explosionHarmNum;//伤害值


    [Header("火系buff持续时间(不变的标准999为无)")]
    public float skillKeepTime_Static_Fire;
    [Header("二级燃烧特效")]
    public GameObject burnEffect;
    [Header("二级燃烧伤害")]
    public int burnHarmNum;//伤害值

    [Header("击退速度")]
    public float speed;
    [Header("击退时间")]
    public float time;

    [Header("是否是一次性击退")]
    public bool isOnceRepel;
    //[Header("二级减速特效")]
    //public GameObject soilEffect;
    //[Header("二级减速百分比")]
    //public int speedCut;//伤害值
    //[Header("土系buff持续时间(不变的标准999为无)")]
    //public float skillKeepTime_Static_Soil;

    ExplosionSkillResult esr;
    BurningSkillResult bsr;
    RangeRepelSkillResult rsr;
    //SpeedCutSkillResult scsr;
    private void Awake()
    {
        Init();
    }
    /// <summary>
    /// 初始化融合技能的基础技能等级
    /// </summary>
    /// <param name="fireLevel">火系等级</param>
    /// <param name="soilLevel">土系等级</param>
    public override void InitSkillResult(int fireLevel,int soilLevel)
    {
        if (fireLevel > 1)
        {
            bsr.IsInUse = true;
        }
        if (soilLevel > 1)
        {
            //scsr.IsInUse = true;
        }
    }

    public void Init()
    {

        //火系技能效果初始化
        //esr = gameObject.AddComponent<ExplosionSkillResult>();
        //bsr = gameObject.AddComponent<BurningSkillResult>();

        esr = transform.parent.gameObject.AddComponent<ExplosionSkillResult>();
        bsr = transform.parent.gameObject.AddComponent<BurningSkillResult>();

        esr.explosionEffect = explosionEffect;
        esr.radius = radius;//爆炸半径
        esr.harmNum = explosionHarmNum;//爆炸伤害
        esr.pos = transform;//爆炸位置
        esr.skillId = skillId;//得到技能元素的数组给效果
        bsr.effect = burnEffect;//燃烧特效
        bsr.skillKeepTime_Static = skillKeepTime_Static_Fire;//燃烧持续时间
        bsr.skillKeepTime = skillKeepTime_Static_Fire;//燃烧持续时间
        bsr.harmNum = burnHarmNum;//燃烧伤害
        bsr.skillId = skillId;//得到技能元素的数组给效果
        bsr.IsInUse = false;//未开放

        resultList.Add(esr);
        resultList.Add(bsr);
        //土系技能效果初始化
        //rsr = gameObject.AddComponent<RepelSkillResult>();
        //scsr = gameObject.AddComponent<SpeedCutSkillResult>();

        rsr = transform.parent.gameObject.AddComponent<RangeRepelSkillResult>();
        //scsr = transform.parent.gameObject.AddComponent<SpeedCutSkillResult>();

        rsr.repelSpeed = speed;//击退速度
        rsr.repelTime = time;//击退时间
        rsr.radius = radius;//击退范围
        rsr.pos = transform.position;//爆炸中心
        rsr.skillId = skillId;//得到技能元素的数组给效果

        //scsr.skillKeepTime_Static = skillKeepTime_Static_Soil;//减速持续时间
        //scsr.skillKeepTime = skillKeepTime_Static_Soil;//减速持续时间
        //scsr.effect = soilEffect;//减速效果
        //scsr.speedCut = speedCut;//减速百分比
        //scsr.skillId = skillId;//得到技能元素的数组给效果
        //scsr.IsInUse = false;//未开放

        resultList.Add(rsr);
        //resultList.Add(scsr);
    }
}
