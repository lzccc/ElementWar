using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 土木系组合技能
/// </summary>
public class SoilWoodSkill : Skill {

    [Header("是否是一次性击退")]
    public bool isOnceRepel;
    [Tooltip("击退速度")]
    public float speed;
    [Tooltip("击退时间")]
    public float time;

    //[Tooltip("二级减速特效")]
    //public GameObject soilEffect;
    //[Tooltip("二级减速百分比")]
    //public int speedCut;//伤害值


    //[Tooltip("土系buff持续时间(不变的标准999为无)")]
    //public float skillKeepTime_Static_Soil;

    [Tooltip("额外伤害百分比")]
    public int extraHarm;
    [Tooltip("木系buff持续时间(不变的标准999为无)")]
    public float skillKeepTime_Static_Wood;


    RepelSkillResult rsr;
    //SpeedCutSkillResult scsr;

    EasyToHurtSkillResult etsr;
    private void Awake()
    {
        Init();
    }
    /// <summary>
    /// 初始化融合技能的基础技能等级
    /// </summary>
    /// <param name="fireLevel">土系等级</param>
    /// <param name="woodLevel">木系等级</param>
    public override void InitSkillResult(int soilLevel, int woodLevel)
    {
        if (soilLevel > 1)
        {
            //scsr.IsInUse = true;
        }
        if (woodLevel > 1)
        {
            SkillUseKind suk = transform.parent.GetComponent<SkillUseKind>();
            suk.ballNum = 6;
        }
    }

    public void Init()
    {

        //土系技能效果初始化
        rsr = transform.parent.gameObject.AddComponent<RepelSkillResult>();
        //scsr = transform.parent.gameObject.AddComponent<SpeedCutSkillResult>();
        rsr.isOnceRepel = isOnceRepel;
        rsr.repelSpeed = speed;//击退速度
        rsr.repelTime = time;//击退时间
        rsr.skillId = skillId;//得到技能元素的数组给效果

        //scsr.skillKeepTime_Static = skillKeepTime_Static_Soil;//减速持续时间
        //scsr.skillKeepTime = skillKeepTime_Static_Soil;//减速持续时间
        //scsr.effect = soilEffect;//减速效果
        //scsr.speedCut = speedCut;//减速百分比
        //scsr.skillId = skillId;//得到技能元素的数组给效果
        //scsr.isOnceRepel = isOnceRepel;
        //scsr.IsInUse = false;//未开放

        resultList.Add(rsr);
        //resultList.Add(scsr);
        //木系技能初始化
        noFollow = true;
        //EasyToHurtSkillResult etsr = gameObject.AddComponent<EasyToHurtSkillResult>();
        etsr = transform.parent.gameObject.AddComponent<EasyToHurtSkillResult>();
        etsr.skillKeepTime_Static = skillKeepTime_Static_Wood;
        etsr.skillKeepTime = skillKeepTime_Static_Wood;
        etsr.extraHarm = extraHarm;
        etsr.skillId = skillId;//得到技能元素的数组给效果
        resultList.Add(etsr);
    }
}
