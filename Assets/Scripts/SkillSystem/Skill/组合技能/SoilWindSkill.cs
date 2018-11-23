using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 土风系组合技能
/// </summary>
public class SoilWindSkill : Skill {


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

    //[Tooltip("buff持续时间(不变的标准999为无)")]
    //public float skillKeepTime_Static;


    RepelSkillResult rsr;
    //SpeedCutSkillResult scsr;
    
    private void Awake()
    {
        Init();
    }
    /// <summary>
    /// 初始化融合技能的基础技能等级
    /// </summary>
    /// <param name="fireLevel">土系等级</param>
    /// <param name="woodLevel">风系等级</param>
    public override void InitSkillResult(int soilLevel, int windLevel)
    {
        if (soilLevel > 1)
        {
            //scsr.IsInUse = true;
        }
        if (windLevel > 1)
        {
            canAttackArrow = true;//追踪子弹开关
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

        //scsr.skillKeepTime_Static = skillKeepTime_Static;//减速持续时间
        //scsr.skillKeepTime = skillKeepTime_Static;//减速持续时间
        //scsr.effect = soilEffect;//减速效果
        //scsr.speedCut = speedCut;//减速百分比
        //scsr.skillId = skillId;//得到技能元素的数组给效果
        //scsr.isOnceRepel = isOnceRepel;
        //scsr.IsInUse = false;//未开放

        resultList.Add(rsr);
        //resultList.Add(scsr);
        //风系技能初始化
        isFollow = true;
    }
}
