using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodWindSkill : Skill {

    [Tooltip("buff持续时间(不变的标准999为无)")]
    public float skillKeepTime_Static;

    [Tooltip("额外伤害百分比")]
    public int extraHarm;

    EasyToHurtSkillResult etsr;

    private void Awake()
    {
        Init();
    }
    /// <summary>
    /// 初始化融合技能的基础技能等级
    /// </summary>
    /// <param name="fireLevel">木系等级</param>
    /// <param name="woodLevel">风系等级</param>
    public override void InitSkillResult(int woodLevel, int windLevel)
    {
        if (woodLevel > 1)
        {
            SkillUseKind suk = transform.parent.GetComponent<SkillUseKind>();
            suk.ballNum = 6;
        }
        if (windLevel > 1)
        {
            canAttackArrow = true;//追踪子弹开关
        }
    }

    public void Init()
    {
        //木系技能初始化
        noFollow = true;
        //EasyToHurtSkillResult etsr = gameObject.AddComponent<EasyToHurtSkillResult>();
        etsr = transform.parent.gameObject.AddComponent<EasyToHurtSkillResult>();
        etsr.skillKeepTime_Static = skillKeepTime_Static;
        etsr.skillKeepTime = skillKeepTime_Static;
        etsr.extraHarm = extraHarm;
        etsr.skillId = skillId;//得到技能元素的数组给效果
        resultList.Add(etsr);
        //风系技能初始化
        isFollow = true;
    }
}
