using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 火系技能
/// </summary>
public class FireSkill : Skill {

    [Tooltip("一个圈形的范围特效")]
    public GameObject explosionEffect;
    [Tooltip("爆炸半径")]
    public float radius;
    [Tooltip("爆炸伤害值")]
    public int explosionHarmNum;//伤害值


    [Tooltip("二级燃烧buff持续时间(不变的标准999为无)")]
    public float skillKeepTime_Static;
    [Tooltip("二级燃烧特效")]
    public GameObject burnEffect;
    [Tooltip("二级燃烧伤害")]
    public int burnHarmNum;//伤害值
    ExplosionSkillResult esr;
    BurningSkillResult bsr;
    private void Awake()
    {
        //ExplosionSkillResult esr = gameObject.AddComponent<ExplosionSkillResult>();
        //BurningSkillResult bsr = gameObject.AddComponent<BurningSkillResult>();
        esr = transform.parent.gameObject.AddComponent<ExplosionSkillResult>();
        bsr = transform.parent.gameObject.AddComponent<BurningSkillResult>();
        esr.explosionEffect = explosionEffect;
        esr.radius = radius;
        esr.harmNum = explosionHarmNum;
        esr.pos = transform;
        esr.skillId = skillId;//得到技能元素的数组给效果

        bsr.effect = burnEffect;
        bsr.skillKeepTime_Static = skillKeepTime_Static;
        bsr.skillKeepTime = skillKeepTime_Static;
        bsr.harmNum = burnHarmNum;
        bsr.skillId = skillId;//得到技能元素的数组给效果


        resultList.Add(esr);
        resultList.Add(bsr);
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
            bsr.IsInUse = false;
        }
    }
}
