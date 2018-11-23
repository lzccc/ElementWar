using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWoodSkill : Skill {

    [Tooltip("一个圈形的范围特效")]
    public GameObject explosionEffect;
    [Tooltip("爆炸半径")]
    public float radius;
    [Tooltip("爆炸伤害值")]
    public int explosionHarmNum;//伤害值


    [Tooltip("火系buff持续时间(不变的标准999为无)")]
    public float skillKeepTime_Static_Fire;
    [Tooltip("二级燃烧特效")]
    public GameObject burnEffect;
    [Tooltip("二级燃烧伤害")]
    public int burnHarmNum;//伤害值
    
    [Tooltip("额外伤害百分比")]
    public int extraHarm;
    [Tooltip("木系buff持续时间(不变的标准999为无)")]
    public float skillKeepTime_Static_Wood;

    ExplosionSkillResult esr;
    BurningSkillResult bsr;

    EasyToHurtSkillResult etsr;
    private void Awake()
    {
        Init();
    }
    /// <summary>
    /// 初始化融合技能的基础技能等级
    /// </summary>
    /// <param name="fireLevel">火系等级</param>
    /// <param name="woodLevel">木系等级</param>
    public override void InitSkillResult(int fireLevel, int woodLevel)
    {
        if (fireLevel > 1)
        {
            bsr.IsInUse = true;
        }
        if (woodLevel > 1)
        {
            SkillUseKind suk = transform.parent.GetComponent<SkillUseKind>();
            suk.ballNum = 6;
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
