using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 土系技能
/// </summary>
public class SoilSkill : Skill {

    [Header("击退速度")]
    public float speed;
    [Header("击退时间")]
    public float time;

    //[Header("二级减速buff持续时间(不变的标准999为无)")]
    //public float skillKeepTime_Static;
    //[Header("二级减速特效")]
    //public GameObject effect;
    //[Header("二级减速百分比")]
    //public int speedCut;//伤害值
    [Header("是否是一次性击退")]
    public bool isOnceRepel;

    RepelSkillResult rsr;
    //SpeedCutSkillResult scsr;
    private void Awake()
    {
        //RepelSkillResult rsr = gameObject.AddComponent<RepelSkillResult>();
        //SpeedCutSkillResult scsr = gameObject.AddComponent<SpeedCutSkillResult>();

        rsr = transform.parent.gameObject.AddComponent<RepelSkillResult>();
        //scsr = transform.parent.gameObject.AddComponent<SpeedCutSkillResult>();

        rsr.repelSpeed = speed;rsr.repelTime = time;
        rsr.isOnceRepel = isOnceRepel;
        rsr.skillId = skillId;//得到技能元素的数组给效果

        //scsr.skillKeepTime_Static = skillKeepTime_Static;
        //scsr.skillKeepTime = skillKeepTime_Static;
        //scsr.effect = effect;
        //scsr.speedCut = speedCut;
        //scsr.isOnceRepel = isOnceRepel;
        //scsr.skillId = skillId;//得到技能元素的数组给效果

        resultList.Add(rsr);
        //resultList.Add(scsr);
    }
    /// <summary>
    /// 初始化技能等级
    /// </summary>
    /// <param name="lv"></param>
    public override void InitLv(int lv)
    {
        Level = lv;
        if (Level <= 1)//如果等级为1则无法开启二级特效
        {
            //scsr.IsInUse = false;
        }
    }
}
