using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 木系技能
/// </summary>
public class woodSkill : Skill {

    [Tooltip("二级易伤buff持续时间(不变的标准999为无)")]
    public float skillKeepTime_Static;
    [Tooltip("额外伤害百分比")]
    public int extraHarm;
    private void Awake()
    {
        noFollow = true;
        //EasyToHurtSkillResult etsr = gameObject.AddComponent<EasyToHurtSkillResult>();
        EasyToHurtSkillResult etsr = transform.parent.gameObject.AddComponent<EasyToHurtSkillResult>();
        etsr.skillKeepTime_Static = skillKeepTime_Static;
        etsr.skillKeepTime = skillKeepTime_Static;
        etsr.extraHarm = extraHarm;
        etsr.skillId = skillId;//得到技能元素的数组给效果
        resultList.Add(etsr);
    }
    /// <summary>
    /// 初始化技能等级
    /// </summary>
    /// <param name="lv"></param>
    public override void InitLv(int lv)
    {
        Level = lv;
        if (Level > 1)//如果等级为1则无法开启二级特效
        {
            SkillUseKind suk = transform.parent.GetComponent<SkillUseKind>();
            suk.ballNum = 6;
        }
    }
}
