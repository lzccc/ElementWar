using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 易伤效果
/// </summary>
public class EasyToHurtSkillResult : SkillResult {

    [Tooltip("命中敌人的脚本")]
    public Enemy enemy;
    [Tooltip("额外伤害百分比")]
    public int extraHarm;
    private void Start()
    {
        skillResultId = 1;
        skillKeepTime = skillKeepTime_Static;
    }
    public override void UseSkill(GameObject target, Enemy e, Vector3 forward)
    {
        base.UseSkill(target, e, forward);//基类的方法带有冷却判断
        enemy = e;
        for (int i = 0; i < skillId.Length; i++)//当前不是木系免疫
        {
            if (enemy.attributeType == (ElementAttributeType)(skillId[i]))
            {
                return;
            }
        }
        enemy.GetSkillById(skillResultId);
        enemy.ExtraHarmChange(extraHarm);
        enemy.skillList.Add(this);
    }

    public override void ReSkill()
    {
        enemy.ExtraHarmChange(-extraHarm);
        base.ReSkill();
    }
}
